using System.Data;
using System.Text;
using System.Text.Json;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using DGPCE.Sigemad.Application.Record;
using DGPCE.Sigemad.Identity.Services.Interfaces;

//EXCEL
using ClosedXML.Excel;
using System.Dynamic;
using DocumentFormat.OpenXml.Drawing.Diagrams;

namespace DGPCE.Sigemad.API.Controllers;

[ApiController]
[Route("api/v1/catalog")] // e.g. api/catalog, api/catalog/1/items 
public class CatalogController : ControllerBase
{
    private readonly string _connectionString;
    private readonly IExcelExportService _excelExportService;

    public CatalogController(IConfiguration configuration, IExcelExportService excelExportService)
    {
        _connectionString = configuration.GetConnectionString("ConnectionString")!
                           ?? throw new ArgumentException("Connection string 'ConnectionString' not found");
        _excelExportService = excelExportService;
    }

    //------------------------------------------------------------------
    // 1. TablaMaestra list
    //------------------------------------------------------------------
    [HttpGet("tables")]
    public async Task<IActionResult> GetTables([FromQuery] int? idTablaMaestraGrupo)
    {
        var sql = "SELECT Id, Tabla, Etiqueta, Listable, Editable FROM TablasMaestras WHERE Listable = 1";
        if (idTablaMaestraGrupo.HasValue)
            sql += " AND IdTablaMaestraGrupo = @IdTablaMaestraGrupo";

        await using var con = new SqlConnection(_connectionString);
        var rows = await con.QueryAsync(sql, new { IdTablaMaestraGrupo = idTablaMaestraGrupo });
        return Ok(rows);
    }
    //------------------------------------------------------------------
    // 2. Column metadata for UI (both for list and edit)
    //------------------------------------------------------------------
    [HttpGet("{tableId:int}/columns")]
    public async Task<IActionResult> GetColumns(int tableId, [FromQuery] bool forEdit = false)
    {
        // if forEdit=true, return columns marked ApareceEnEdicion; else, only Busqueda columns
        var filterColumn = forEdit ? "ApareceEnEdicion = 1" : "Busqueda = 1";
        var sql = $@"
            SELECT
                Id, IdTablasMaestras, Columna, Etiqueta, Orden,
                Busqueda, Defecto, Nulo, Duplicado, Tipo, Longitud,
                TablaRelacionada, CampoRelacionado,
                ApareceEnListado, ApareceEnEdicion
            FROM ColumnasTM
            WHERE IdTablasMaestras = @tableId
              AND {filterColumn}
            ORDER BY Orden";

        await using var con = new SqlConnection(_connectionString);
        var cols = await con.QueryAsync(sql, new { tableId });
        return Ok(cols);
    }

    //------------------------------------------------------------------
    // 3. Obtener items con filtro, borrado lógico y paginación
    //------------------------------------------------------------------
    [HttpGet("{tableId:int}/items")]
    public async Task<IActionResult> GetItems(
        int tableId,
        [FromQuery] string? column = null,
        [FromQuery] string? value = null,
        [FromQuery] bool showDeleted = false,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string orderDirection = "asc")
    {
        //
        var direction = orderDirection?.ToLower() == "desc" ? "DESC" : "ASC";
        //

        if (page <= 0) page = 1;
        //Opcion especial para mostrar todos los registros sin paginar (se usa en combos de formularios que no se sabe cuantos registros hay)
        if (pageSize <= 0 || pageSize > 9999) pageSize = 20;

        // 1️⃣ Metadatos
        var metaNullable = await GetTableMeta(tableId, includeColumns: true);
        if (metaNullable is null)
            return NotFound("Tabla maestra no encontrada");
        var (tabla, columns) = metaNullable.Value;

        // 2️⃣ WHERE dinámico
        var filter = new StringBuilder("WHERE 1=1");
        var p = new DynamicParameters();

        if (!showDeleted && await HasColumn(tabla, "Borrado"))
            filter.Append(" AND Borrado = 0");

        if (!string.IsNullOrWhiteSpace(column) && !string.IsNullOrWhiteSpace(value))
        {
            filter.Append($" AND {column} LIKE @value");
            p.Add("value", $"%{value}%");
        }

        // 3️⃣ Total
        var countSql = $"SELECT COUNT(*) FROM {tabla} {filter}";
        await using var con = new SqlConnection(_connectionString);
        var total = await con.ExecuteScalarAsync<int>(countSql, p);

        // 4️⃣ Paginación
        int skip = (page - 1) * pageSize;
        p.Add("skip", skip);
        p.Add("take", pageSize);
        var dataSql = $@"
            SELECT * FROM {tabla} {filter}
            ORDER BY Id {direction}
            OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";
        var items = (await con.QueryAsync(dataSql, p)).ToList();

        // 5️⃣ Lookups para selects
        var selectCols = columns
            .Where(c => string.Equals(c.Tipo, "select", StringComparison.OrdinalIgnoreCase)
                     && !string.IsNullOrWhiteSpace(c.TablaRelacionada))
            .ToList();

        var lookups = new Dictionary<string, Dictionary<int, string>>();
        foreach (var col in selectCols)
        {
            var campos = col.CampoRelacionado!.Split(';', StringSplitOptions.RemoveEmptyEntries);
            var textoExpr = campos.Length == 1
                ? campos[0]
                : $"CONCAT({string.Join(", ' - ', ", campos)})";

            var sb = new StringBuilder($@"SELECT Id, {textoExpr} AS Texto FROM {col.TablaRelacionada}");
            if (await HasColumn(col.TablaRelacionada!, "Borrado"))
                sb.Append(" WHERE Borrado = 0");

            var rows = await con.QueryAsync(sb.ToString());
            lookups[col.Columna] = rows
                .ToDictionary(r => (int)r.Id, r => (string)r.Texto);
        }

        // 6️⃣ Enriquecer
        var enriched = new List<Dictionary<string, object>>();
        foreach (IDictionary<string, object> row in items)
        {
            var dict = new Dictionary<string, object>(row, StringComparer.OrdinalIgnoreCase);
            foreach (var col in selectCols)
            {
                if (dict.TryGetValue(col.Columna, out var idObj)
                    && idObj is int id
                    && lookups[col.Columna].TryGetValue(id, out var text))
                {
                    dict[$"{col.Columna}__texto"] = text;
                }
            }
            enriched.Add(dict);
        }

        return Ok(new
        {
            page,
            pageSize,
            total,
            totalPages = (int)Math.Ceiling(total / (double)pageSize),
            items = enriched
        });
    }

    //------------------------------------------------------------------
    // 3b. Obtener un solo item (detalle)
    //------------------------------------------------------------------
    [HttpGet("{tableId:int}/items/{id:int}")]
    public async Task<IActionResult> GetItem(int tableId, int id)
    {
        var metaNullable = await GetTableMeta(tableId, includeColumns: false);
        if (metaNullable is null)
            return NotFound("Tabla maestra no encontrada");
        var (tabla, _) = metaNullable.Value;

        var sql = $"SELECT * FROM {tabla} WHERE Id = @id";
        await using var con = new SqlConnection(_connectionString);
        var item = await con.QuerySingleOrDefaultAsync(sql, new { id });
        if (item == null)
            return NotFound();
        return Ok(item);
    }

    //------------------------------------------------------------------
    // 4. Create item
    //------------------------------------------------------------------
    [HttpPost("{tableId:int}/items")]
    [Consumes("application/json")]
    public async Task<IActionResult> CreateItem(int tableId)
    {
        using var sr = new StreamReader(Request.Body, Encoding.UTF8);
        var jsonText = await sr.ReadToEndAsync();
        using var doc = JsonDocument.Parse(jsonText);
        if (doc.RootElement.ValueKind != JsonValueKind.Object)
            return BadRequest("Se esperaba un objeto JSON.");

        var jsonDict = doc.RootElement
            .EnumerateObject()
            .ToDictionary(x => x.Name, x => x.Value, StringComparer.OrdinalIgnoreCase);

        var body = NormalizeJson(jsonDict);

        var meta = await GetTableMeta(tableId, includeColumns: true);
        if (meta is null)
            return NotFound($"Tabla maestra {tableId} no encontrada.");

        var (ok, msg) = await ValidateBody(body, meta.Value.Columns, false, meta.Value.Tabla, null, _connectionString);
        if (!ok)
            return BadRequest(msg);

        var sql = BuildInsert(meta.Value, body);
        await using var con = new SqlConnection(_connectionString);
        var newId = await con.ExecuteScalarAsync<int>(sql, body);

        return CreatedAtAction(nameof(GetItem),
                               new { tableId, id = newId },
                               new { id = newId });
    }

    //------------------------------------------------------------------
    // 5. Update item
    //------------------------------------------------------------------
    [HttpPut("{tableId:int}/items/{id:int}")]
    [Consumes("application/json")]
    public async Task<IActionResult> UpdateItem(int tableId, int id)
    {
        using var sr = new StreamReader(Request.Body, Encoding.UTF8);
        var jsonText = await sr.ReadToEndAsync();
        using var doc = JsonDocument.Parse(jsonText);
        if (doc.RootElement.ValueKind != JsonValueKind.Object)
            return BadRequest("Se esperaba un objeto JSON.");

        var jsonDict = doc.RootElement
            .EnumerateObject()
            .ToDictionary(x => x.Name, x => x.Value, StringComparer.OrdinalIgnoreCase);

        var body = NormalizeJson(jsonDict);

        var metaNullable = await GetTableMeta(tableId, includeColumns: true);
        if (metaNullable is null)
            return NotFound("Tabla maestra no encontrada");

        var (tabla, cols) = metaNullable.Value;
        var (ok, msg) = await ValidateBody(body, cols, true, tabla, id, _connectionString);
        if (!ok)
            return BadRequest(msg);

        var sql = BuildUpdate(metaNullable.Value, id, body);
        await using var con = new SqlConnection(_connectionString);
        await con.ExecuteAsync(sql, body);

        return NoContent();
    }

    //------------------------------------------------------------------
    // 6. Borrado lógico / recuperación
    //------------------------------------------------------------------
    [HttpDelete("{tableId:int}/items/{id:int}")]
    public async Task<IActionResult> DeleteItem(int tableId, int id)
    {
        var metaNullable = await GetTableMeta(tableId, includeColumns: false);
        if (metaNullable is null)
            return NotFound("Tabla maestra no encontrada");
        var (tabla, _) = metaNullable.Value;

        await using var con = new SqlConnection(_connectionString);
        var currentDeleted = await con.ExecuteScalarAsync<bool?>(
            $"SELECT Borrado FROM {tabla} WHERE Id = @id", new { id });

        if (currentDeleted is null)
            return NotFound();

        if (currentDeleted == true)
        {
            try
            {
                await con.ExecuteAsync(
                    $"UPDATE {tabla} SET Borrado = 0 WHERE Id = @id", new { id });
                return Ok(new { recovered = true, message = "Registro recuperado correctamente." });
            }
            catch (SqlException ex)
            {
                return BadRequest(new { recovered = false, message = $"No se pudo recuperar: {ex.Message}" });
            }
        }

        try
        {
            await con.ExecuteAsync(
                $"UPDATE {tabla} SET Borrado = 1 WHERE Id = @id", new { id });
            return NoContent();
        }
        catch (SqlException ex)
        {
            return BadRequest(new { message = $"No se pudo borrar: {ex.Message}" });
        }
    }

    //EXCEL

    [HttpGet("{tableId:int}/items/exportExcel")]
    public async Task<IActionResult> ExportExcel(int tableId, [FromQuery] bool showDeleted = false)
    {
        // 1️⃣ Obtén la metadata de la tabla
        var metaNullable = await GetTableMeta(tableId, includeColumns: true);
        if (metaNullable is null)
            return NotFound("Tabla maestra no encontrada");

        var (tabla, originalColumns) = metaNullable.Value;
        var columns = originalColumns.ToList();

        // 2️⃣ Construye la consulta con o sin filtro de borrado
        var whereClause = showDeleted ? "" : " WHERE Borrado = 0";
        var sql = $"SELECT * FROM {tabla}{whereClause}";

        await using var con = new SqlConnection(_connectionString);
        var rows = (await con.QueryAsync(sql))
            .Select(r => (IDictionary<string, object>)r)
            .ToList();

        // 3️⃣ Prepara los lookups para columnas "select"
        var selectCols = columns
            .Where(c =>
                c.Tipo?.Equals("select", StringComparison.OrdinalIgnoreCase) == true &&
                !string.IsNullOrWhiteSpace(c.TablaRelacionada))
            .ToList();

        var lookups = new Dictionary<string, Dictionary<int, string>>();
        foreach (var col in selectCols)
        {
            var campos = col.CampoRelacionado!
                .Split(';', StringSplitOptions.RemoveEmptyEntries);
            var expr = campos.Length == 1
                ? campos[0]
                : $"CONCAT({string.Join(", ' - ', ", campos)})";

            var sb = new StringBuilder($@"
SELECT Id, {expr} AS Texto
  FROM {col.TablaRelacionada}");

            var lookupRows = await con.QueryAsync(sb.ToString());
            lookups[col.Columna] = lookupRows
                .ToDictionary(r => (int)r.Id, r => (string)r.Texto);
        }

        // 4️⃣ Enriquecer cada fila con "{columna}__texto"
        var enriched = new List<Dictionary<string, object>>(rows.Count);
        foreach (var rawRow in rows)
        {
            var dict = new Dictionary<string, object>(rawRow, StringComparer.OrdinalIgnoreCase);

            // ❌ Eliminar la columna "Borrado" si existe
            dict.Remove("Borrado");

            foreach (var col in selectCols)
            {
                if (dict.TryGetValue(col.Columna, out var idObj)
                 && idObj != null
                 && int.TryParse(idObj.ToString(), out var id)
                 && lookups[col.Columna].TryGetValue(id, out var text))
                {
                    dict[$"{col.Columna}__texto"] = text;
                }
            }

            enriched.Add(dict);
        }

        // 5️⃣ Eliminar "Borrado" de las columnas también
        columns = columns
            .Where(c => !string.Equals(c.Columna, "Borrado", StringComparison.OrdinalIgnoreCase))
            .ToList();

        // 6️⃣ Recuperar Etiqueta como título de hoja si existe
        var etiqueta = await con.QuerySingleOrDefaultAsync<string>(
            "SELECT Etiqueta FROM TablasMaestras WHERE Id = @tableId", new { tableId });

        var sheetTitle = string.IsNullOrWhiteSpace(etiqueta) ? tabla : etiqueta;

        // 7️⃣ Construir lista de filtros aplicados
        var filtrosTexto = new List<string>();
        if (!showDeleted)
            filtrosTexto.Add("Mostrar eliminados: Solo activos");
        else
            filtrosTexto.Add("Mostrar eliminados: Todos");

        // 8️⃣ Exportar
        return await _excelExportService.ExportAsync(sheetTitle, columns, enriched, filtrosTexto);
    }

    //prueba


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

        var tabla = await con.QuerySingleOrDefaultAsync<string>(
            "SELECT Tabla FROM TablasMaestras WHERE Id = @tableId",
            new { tableId });

        if (tabla == null) return null;

        if (!includeColumns)
            return (tabla, Enumerable.Empty<ColumnInfo>());

        var columns = await con.QueryAsync<ColumnInfo>(
            @"SELECT Columna, Etiqueta, Nulo, Duplicado, Tipo, TablaRelacionada, CampoRelacionado 
          FROM ColumnasTM 
          WHERE IdTablasMaestras = @tableId 
          ORDER BY Orden",
            new { tableId });

        return (tabla, columns);
    }

    private async Task<bool> HasColumn(string tableName, string columnName)
    {
        const string sql = @"
        SELECT COUNT(*) 
        FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_SCHEMA = 'dbo'
          AND TABLE_NAME   = @table
          AND COLUMN_NAME  = @column";
        await using var con = new SqlConnection(_connectionString);
        var count = await con.ExecuteScalarAsync<int>(sql,
                        new { table = tableName, column = columnName });
        return count > 0;
    }

    private static async Task<(bool ok, string msg)> ValidateBody(
    Dictionary<string, object> body,
    IEnumerable<ColumnInfo> cols,
    bool isUpdate,
    string tableName,
    int? idToExclude,
    string connectionString)
    {
        await using var con = new SqlConnection(connectionString);

        foreach (var c in cols)
        {
            // 1️⃣ Obligatorios (solo INSERT)
            if (!c.Nulo && !body.ContainsKey(c.Columna) && !isUpdate)
                return (false, $"El campo '{c.Etiqueta}' es obligatorio.");

            // 2️⃣ Duplicados
            if (c.Duplicado == false
                && body.TryGetValue(c.Columna, out var value)
                && value != null)
            {
                // ⛔ Omitir validación para booleanos
                if (value is bool
                    || string.Equals(value.ToString(), "true", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(value.ToString(), "false", StringComparison.OrdinalIgnoreCase))
                    continue;

                // Excepción explícita para tabla Ej04Capacidad
                if (tableName.Equals("Ej04Capacidad", StringComparison.OrdinalIgnoreCase))
                    return (true, null);

                var sql = $"SELECT COUNT(*) FROM {tableName} WHERE {c.Columna} = @value";
                if (isUpdate && idToExclude.HasValue)
                    sql += " AND Id != @id";

                var count = await con.ExecuteScalarAsync<int>(sql, new { value, id = idToExclude });
                if (count > 0)
                    return (false, $"El valor '{value}' ya existe en '{c.Etiqueta}'.");
            }
        }

        return (true, string.Empty);
    }


    private static string BuildInsert(
        (string Tabla, IEnumerable<ColumnInfo> Columns) meta,
        Dictionary<string, object> body)
    {
        var cols = string.Join(",", body.Keys);
        var parms = string.Join(",", body.Keys.Select(k => "@" + k));
        return $"INSERT INTO {meta.Tabla} ({cols}) OUTPUT INSERTED.Id VALUES ({parms})";
    }

    private static string BuildUpdate(
        (string Tabla, IEnumerable<ColumnInfo> Columns) meta,
        int id,
        Dictionary<string, object> body)
    {
        var columnasValidas = meta.Columns.Select(c => c.Columna).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var columnasActualizables = body.Keys.Where(k => columnasValidas.Contains(k)).ToList();

        if (!columnasActualizables.Any())
            throw new InvalidOperationException("No se proporcionaron columnas válidas para actualizar.");

        var sets = string.Join(",", columnasActualizables.Select(k => $"{k} = @{k}"));

        body["id"] = id;
        return $"UPDATE {meta.Tabla} SET {sets} WHERE Id = @id";
    }


    private async Task<bool> HasTable(string tableName)
    {
        const string sql = @"
        SELECT COUNT(*) 
        FROM INFORMATION_SCHEMA.TABLES 
        WHERE TABLE_SCHEMA = 'dbo'
          AND TABLE_NAME = @table";
        await using var con = new SqlConnection(_connectionString);
        var count = await con.ExecuteScalarAsync<int>(sql, new { table = tableName });
        return count > 0;
    }

    #endregion Helpers

    // POCO para leer CampoRelacionado
    private class ColumnRelation
    {
        public string CampoRelacionado { get; set; } = string.Empty;
    }

    /// <summary>
    /// Devuelve Id + text para poblar selects de cualquier tabla relacionada.
    /// </summary>
    [HttpGet("select-options/{tabla}")]
    public async Task<IActionResult> GetSelectOptions(string tabla)
    {
        // 1️⃣ Verificar existencia de la tabla
        if (!await HasTable(tabla))
            return NotFound(new { error = $"La tabla '{tabla}' no existe." });

        await using var con = new SqlConnection(_connectionString);

        // 2️⃣ Obtener el primer CampoRelacionado definido para esa tabla
        var rel = await con.QuerySingleOrDefaultAsync<ColumnRelation>(
            @"SELECT TOP 1 CampoRelacionado
          FROM ColumnasTM
          WHERE TablaRelacionada = @tabla",
            new { tabla });

        if (rel == null || string.IsNullOrWhiteSpace(rel.CampoRelacionado))
            return BadRequest(new { error = "No está definido CampoRelacionado para esta tabla." });

        // 3️⃣ Construir expresión de texto
        var campos = rel.CampoRelacionado.Split(';', StringSplitOptions.RemoveEmptyEntries);
        var textoExpr = campos.Length == 1
            ? campos[0]
            : $"CONCAT({string.Join(", ' - ', ", campos)})";

        // 4️⃣ Filtrar borrados si aplica
        var hasBorrado = await HasColumn(tabla, "Borrado");
        var sql = new StringBuilder($@"SELECT Id, {textoExpr} AS Texto FROM {tabla}");
        if (hasBorrado)
            sql.Append(" WHERE Borrado = 0");

        // 5️⃣ Ejecutar y proyectar
        var rows = await con.QueryAsync(sql.ToString());
        var list = rows
            .Select(r => new { id = (int)r.Id, text = (string)r.Texto })
            .ToList();

        return Ok(list);
    }
}
