using System.Data;
using System.Text;
using System.Text.Json;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Sigemad.Api.Controllers;

[ApiController]
[Route("api/catalog")] // e.g. api/catalog, api/catalog/1/items
public class CatalogController : ControllerBase
{
    private readonly string _connectionString;

    public CatalogController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SigemadDB")!
                           ?? throw new ArgumentException("Connection string 'DefaultConnection' not found");
    }

    //------------------------------------------------------------------
    // 1. TablaMaestra list  -------------------------------------------------------------
    //------------------------------------------------------------------
    [HttpGet("tables")]
    public async Task<IActionResult> GetTables()
    {
        const string sql = "SELECT Id, Tabla, Etiqueta FROM TablasMaestras";
        await using var con = new SqlConnection(_connectionString);
        var rows = await con.QueryAsync(sql);
        return Ok(rows);
    }

    //------------------------------------------------------------------
    // 2. Column metadata for UI  --------------------------------------------
    //------------------------------------------------------------------
    [HttpGet("{tableId:int}/columns")]
    public async Task<IActionResult> GetColumns(int tableId)
    {
        const string sql = @"SELECT Id, IdTablasMaestras, Columna, Etiqueta, Orden, Busqueda, Defecto, Nulo, Duplicado, Tipo, Longitud, TablaRelacionada, CampoRelacionado
                             FROM ColumnasTM WHERE IdTablasMaestras = @tableId ORDER BY Orden";
        await using var con = new SqlConnection(_connectionString);
        var rows = await con.QueryAsync(sql, new { tableId });
        return Ok(rows);
    }

    //------------------------------------------------------------------
    // 3. Obtener items con filtro, borrado lógico y paginación --------
    //------------------------------------------------------------------
    [HttpGet("{tableId:int}/items")]
    public async Task<IActionResult> GetItems(
        int tableId,
        [FromQuery] string? column = null,
        [FromQuery] string? value = null,
        [FromQuery] bool showDeleted = false,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0 || pageSize > 200) pageSize = 20;

        var metaNullable = await GetTableMeta(tableId);
        if (metaNullable is null)
            return NotFound("Tabla maestra no encontrada");
        var meta = metaNullable.Value;

        // ---------------- Condiciones dinámicas ----------------------
        var filter = new StringBuilder("WHERE 1 = 1");
        var p = new DynamicParameters();

        if (!showDeleted && await HasColumn(meta.Tabla, "Borrado"))
            filter.Append(" AND Borrado = 0");

        if (!string.IsNullOrWhiteSpace(column) && !string.IsNullOrWhiteSpace(value))
        {
            filter.Append($" AND {column} LIKE @value");
            p.Add("value", $"%{value}%");
        }

        // ---------------- Total --------------------------------------
        string countSql = $"SELECT COUNT(*) FROM {meta.Tabla} {filter}";

        // ---------------- Datos paginados ----------------------------
        int skip = (page - 1) * pageSize;
        p.Add("skip", skip);
        p.Add("take", pageSize);

        string dataSql = $"SELECT * FROM {meta.Tabla} {filter} ORDER BY Id OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";

        await using var con = new SqlConnection(_connectionString);
        var total = await con.ExecuteScalarAsync<int>(countSql, p);
        var items = await con.QueryAsync(dataSql, p);

        return Ok(new
        {
            page,
            pageSize,
            total,
            totalPages = (int)Math.Ceiling(total / (double)pageSize),
            items
        });
    }
    //------------------------------------------------------------------
    // 4. Create item -------------------------------------------------
    //------------------------------------------------------------------
    [HttpPost("{tableId:int}/items")]
    public async Task<IActionResult> CreateItem(int tableId, [FromBody] Dictionary<string, JsonElement> jsonBody)
    {
        var meta = await GetTableMeta(tableId, includeColumns: true);
        if (meta is null) return NotFound();

        var body = NormalizeJson(jsonBody);
        var (ok, msg) = ValidateBody(body, meta.Value.Columns, false);
        if (!ok) return BadRequest(msg);

        var sql = BuildInsert(meta.Value, body);
        await using var con = new SqlConnection(_connectionString);
        var id = await con.ExecuteScalarAsync<int>(sql, body);
        return Created($"{Request.Scheme}://{Request.Host}/api/catalog/{tableId}/items/{id}", new { id });
    }

    //------------------------------------------------------------------
    // 5. Update item -------------------------------------------------
    //------------------------------------------------------------------
    [HttpPut("{tableId:int}/items/{id:int}")]
    public async Task<IActionResult> UpdateItem(int tableId, int id, [FromBody] Dictionary<string, JsonElement> jsonBody)
    {
        var meta = await GetTableMeta(tableId, includeColumns: true);
        if (meta is null) return NotFound();

        var body = NormalizeJson(jsonBody);
        var (ok, msg) = ValidateBody(body, meta.Value.Columns, true);
        if (!ok) return BadRequest(msg);

        var sql = BuildUpdate(meta.Value, id, body);
        await using var con = new SqlConnection(_connectionString);
        await con.ExecuteAsync(sql, body);
        return NoContent();
    }

    //------------------------------------------------------------------
    // 6. Borrado lógico ---------------------------------------------
    //------------------------------------------------------------------
    [HttpDelete("{tableId:int}/items/{id:int}")]
    public async Task<IActionResult> DeleteItem(int tableId, int id)
    {
        var metaNullable = await GetTableMeta(tableId);
        if (metaNullable is null) return NotFound();
        var meta = metaNullable.Value;

        string sql = await HasColumn(meta.Tabla, "Borrado")
            ? $"UPDATE {meta.Tabla} SET Borrado = 1 WHERE Id = @id"
            : $"DELETE FROM {meta.Tabla} WHERE Id = @id";

        await using var con = new SqlConnection(_connectionString);
        await con.ExecuteAsync(sql, new { id });
        return NoContent();
    }

    #region Helpers ---------------------------------------------------

    private static Dictionary<string, object> NormalizeJson(Dictionary<string, JsonElement> raw)
    {
        var dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        foreach (var (k, v) in raw)
        {
            dict[k] = v.ValueKind switch
            {
                JsonValueKind.String => v.GetString()!,
                JsonValueKind.Number => v.TryGetInt64(out var l) ? l : v.GetDecimal(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null!,
                _ => v.GetRawText()
            };
        }
        return dict;
    }

    private async Task<(string Tabla, IEnumerable<ColumnInfo> Columns)?> GetTableMeta(int tableId, bool includeColumns = false)
    {
        await using var con = new SqlConnection(_connectionString);
        var tabla = await con.QuerySingleOrDefaultAsync<string>("SELECT Tabla FROM TablasMaestras WHERE Id = @tableId", new { tableId });
        if (tabla == null) return null;

        if (!includeColumns)
            return (tabla, Enumerable.Empty<ColumnInfo>());

        var columns = await con.QueryAsync<ColumnInfo>("SELECT Columna, Nulo, Duplicado FROM ColumnasTM WHERE IdTablasMaestras = @tableId", new { tableId });
        return (tabla, columns);
    }

    private async Task<bool> HasColumn(string table, string column)
    {
        const string sql = @"SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @table AND COLUMN_NAME = @column";
        await using var con = new SqlConnection(_connectionString);
        var count = await con.ExecuteScalarAsync<int>(sql, new { table, column });
        return count > 0;
    }

    private static (bool ok, string msg) ValidateBody(Dictionary<string, object> body,
                                                      IEnumerable<ColumnInfo> cols,
                                                      bool isUpdate)
    {
        foreach (var c in cols)
        {
            if (!c.Nulo && !body.ContainsKey(c.Columna) && !isUpdate)
                return (false, $"El campo {c.Columna} es obligatorio");
        }
        // TODO: duplicados / types
        return (true, string.Empty);
    }

    private static string BuildInsert((string Tabla, IEnumerable<ColumnInfo> Columns) meta,
                                      Dictionary<string, object> body)
    {
        var cols = string.Join(',', body.Keys);
        var parms = string.Join(',', body.Keys.Select(k => '@' + k));
        return $"INSERT INTO {meta.Tabla} ({cols}) OUTPUT INSERTED.Id VALUES ({parms})";
    }

    private static string BuildUpdate((string Tabla, IEnumerable<ColumnInfo> Columns) meta,
                                      int id,
                                      Dictionary<string, object> body)
    {
        var sets = string.Join(',', body.Keys.Select(k => $"{k} = @{k}"));
        body["id"] = id; // param for WHERE
        return $"UPDATE {meta.Tabla} SET {sets} WHERE Id = @id";
    }

    public record ColumnInfo(string Columna, bool Nulo, bool Duplicado);

    #endregion
}
