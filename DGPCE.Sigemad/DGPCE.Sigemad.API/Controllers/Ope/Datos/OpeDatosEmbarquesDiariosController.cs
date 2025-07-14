
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosEmbarquesDiarios.Queries.GetOpeDatoEmbarqueDiarioById;
using DGPCE.Sigemad.Application.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosEmbarquesDiarios.Vms;

using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosEmbarquesDiarios.Commands.CreateOpeDatosEmbarquesDiarios;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosEmbarquesDiarios.Commands.UpdateOpeDatosEmbarquesDiarios;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosEmbarquesDiarios.Commands.DeleteOpeDatosEmbarquesDiarios;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosEmbarquesDiarios.Queries.GetOpeDatosEmbarquesDiariosList;

using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using DGPCE.Sigemad.Application.Record;
using DGPCE.Sigemad.Identity.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Dapper;

namespace DGPCE.Sigemad.API.Controllers.Ope.Datos;

[Authorize]
[Route("api/v1/ope-datos-embarques-diarios")]
[ApiController]
public class OpeDatosEmbarquesDiariosController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpeDatosEmbarquesDiariosController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost(Name = "CreateOpeDatoEmbarqueDiario")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateOpeDatoEmbarqueDiarioResponse>> Create([FromBody] CreateOpeDatoEmbarqueDiarioCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginationVm<OpeDatoEmbarqueDiario>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationVm<OpeDatoEmbarqueDiarioVm>>> GetOpeDatosEmbarquesDiarios(
        [FromQuery] GetOpeDatosEmbarquesDiariosListQuery query)
    {
        var pagination = await _mediator.Send(query);
        return Ok(pagination);
    }


    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Summary = "Busqueda de dato de embarque diario de OPE por id")]
    public async Task<ActionResult<OpeDatoEmbarqueDiario>> GetById(int id)
    {
        var query = new GetOpeDatoEmbarqueDiarioByIdQuery(id);
        var opeDatoEmbarqueDiario = await _mediator.Send(query);

        if (opeDatoEmbarqueDiario == null)
            return NotFound();

        return Ok(opeDatoEmbarqueDiario);
    }

    [HttpPut(Name = "UpdateOpeDatoEmbarqueDiario")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OpeDatoEmbarqueDiarioVm))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update([FromBody] UpdateOpeDatoEmbarqueDiarioCommand command)
    {
        //await _mediator.Send(command);
        //return NoContent();
        var updatedObject = await _mediator.Send(command);
        return Ok(updatedObject);
    }

    [HttpDelete("{id:int}", Name = "DeleteOpeDatoEmbarqueDiario")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteOpeDatoEmbarqueDiarioCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet("exportExcel")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> ExportExcel(
    [FromServices] IExcelExportService excelExportService,
    [FromServices] IConfiguration configuration,
    [FromQuery] string? estadoEliminado = null,
    [FromQuery] DateTime? fechaDesde = null,
    [FromQuery] DateTime? fechaHasta = null,
    [FromQuery] int? idLinea = null,
    [FromQuery] string? fase = null,
    [FromQuery] string? periodo = null,
    [FromQuery] string? criterioNumerico = null,
    [FromQuery] string? criterioNumericoRadio = null,
    [FromQuery] string? criterioNumericoCondicion = null,
    [FromQuery] int? criterioNumericoCantidad = null
)
    {
        var connectionString = configuration.GetConnectionString("ConnectionString")!;
        await using var con = new SqlConnection(connectionString);

        var filtros = new List<string>();

        switch (estadoEliminado?.ToLowerInvariant())
        {
            case "activo":
            case "no":
                filtros.Add("e.Borrado = 0");
                break;
            case "eliminado":
            case "sí":
            case "si":
                filtros.Add("e.Borrado = 1");
                break;
        }

        if (fechaDesde.HasValue)
            filtros.Add("e.Fecha >= @fechaDesde");
        if (fechaHasta.HasValue)
            filtros.Add("e.Fecha < DATEADD(DAY, 1, @fechaHasta)");
        if (idLinea.HasValue)
            filtros.Add("e.IdOpeLineaMaritima = @idLinea");
        if (!string.IsNullOrWhiteSpace(fase))
            filtros.Add("l.IdOpeFase = @fase");
        if (!string.IsNullOrWhiteSpace(periodo))
            filtros.Add("e.Periodo = @periodo");

        if (!string.IsNullOrWhiteSpace(criterioNumericoCondicion) && criterioNumericoCantidad.HasValue)
        {
            var campo = criterioNumerico switch
            {
                "rotaciones" => "e.NumeroRotaciones",
                "pasajeros" => "e.NumeroPasajeros",
                "turismos" => "e.NumeroTurismos",
                _ => null
            };

            var operador = criterioNumericoCondicion switch
            {
                "mayor" => ">=",
                "menor" => "<=",
                "igual" => "=",
                _ => "="
            };

            if (!string.IsNullOrWhiteSpace(campo))
                filtros.Add($"{campo} {operador} @criterioNumericoCantidad");
        }

        var whereClause = filtros.Any() ? "WHERE " + string.Join(" AND ", filtros) : "";

        var sql = $@"
        SELECT e.*, l.IdOpeFase
        FROM OPE_DatoEmbarqueDiario e
        LEFT JOIN OPE_LineaMaritima l ON e.IdOpeLineaMaritima = l.Id
        {whereClause}
        ORDER BY e.Fecha DESC";

        var rows = (await con.QueryAsync(sql, new
        {
            fechaDesde,
            fechaHasta,
            idLinea,
            fase,
            periodo,
            criterioNumericoCantidad
        })).Select(r => (IDictionary<string, object>)r).ToList();

        var columns = new List<ColumnInfo>
    {
        new("Fecha", "Fecha", false, false, "date", null, null),
        new("IdOpeLineaMaritima", "Línea Marítima", false, false, "select", "OPE_LineaMaritima", "Nombre"),
        new("IdOpeFase", "Fase", false, false, "select", "OPE_Fase", "Nombre"),
        new("NumeroRotaciones", "Rotaciones", false, false, "number", null, null),
        new("NumeroPasajeros", "Pasajeros", false, false, "number", null, null),
        new("NumeroTurismos", "Turismos", false, false, "number", null, null),
        new("NumeroAutocares", "Autocares", false, false, "number", null, null),
        new("NumeroCamiones", "Camiones", false, false, "number", null, null),
        new("NumeroTotalVehiculos", "Total Vehículos", false, false, "number", null, null)
    };

        var lookups = new Dictionary<string, Dictionary<int, string>>();
        foreach (var col in columns.Where(c => c.Tipo == "select"))
        {
            var campo = col.CampoRelacionado ?? "Nombre";
            var tabla = col.TablaRelacionada ?? throw new InvalidOperationException($"Falta TablaRelacionada en {col.Columna}");
            var lookupSql = $"SELECT Id, {campo} AS Texto FROM {tabla}";
            var data = await con.QueryAsync(lookupSql);

            var dic = new Dictionary<int, string>();
            foreach (var item in data)
            {
                int id = (int)item.Id;
                string texto = (string)item.Texto;
                if (!dic.ContainsKey(id)) dic[id] = texto;
            }
            lookups[col.Columna] = dic;
        }

        var enriched = rows.Select(row =>
        {
            var dict = new Dictionary<string, object>(row, StringComparer.OrdinalIgnoreCase);
            foreach (var col in columns.Where(c => c.Tipo == "select"))
            {
                if (dict.TryGetValue(col.Columna, out var val)
                    && val != null
                    && int.TryParse(val.ToString(), out var id)
                    && lookups[col.Columna].TryGetValue(id, out var texto))
                {
                    dict[$"{col.Columna}__texto"] = texto;
                }

                if (dict.TryGetValue("Fecha", out var f) && f is DateTime dt)
                    dict["Fecha"] = dt.ToString("yyyy-MM-dd");
            }
            
            return dict;
        }).ToList();

        // 🔍 Obtener nombre de la Fase
        string? nombreFase = null;
        if (!string.IsNullOrWhiteSpace(fase))
        {
            nombreFase = await con.ExecuteScalarAsync<string?>(
                "SELECT Nombre FROM OPE_Fase WHERE Id = @idFase",
                new { idFase = fase });
        }

        // 🔍 Obtener nombre de la Línea
        string? nombreLinea = null;
        if (idLinea.HasValue)
        {
            nombreLinea = await con.ExecuteScalarAsync<string?>(
                "SELECT Nombre FROM OPE_LineaMaritima WHERE Id = @idLinea",
                new { idLinea });
        }

        // 📝 Filtros aplicados
        var filtrosTexto = new List<string> { "Filtros aplicados:" };
        if (fechaDesde.HasValue) filtrosTexto.Add($"Fecha desde: {fechaDesde:dd/MM/yyyy}");
        if (fechaHasta.HasValue) filtrosTexto.Add($"Fecha hasta: {fechaHasta:dd/MM/yyyy}");
        if (!string.IsNullOrWhiteSpace(nombreLinea)) filtrosTexto.Add($"Línea: {nombreLinea}");
        if (!string.IsNullOrWhiteSpace(nombreFase)) filtrosTexto.Add($"Fase: {nombreFase}");
        if (!string.IsNullOrWhiteSpace(periodo)) filtrosTexto.Add($"Periodo: {periodo}");
        if (!string.IsNullOrWhiteSpace(criterioNumerico))
        filtrosTexto.Add($"Criterio: {criterioNumerico} {criterioNumericoCondicion} {criterioNumericoCantidad}");

        var estado = (estadoEliminado ?? "no").Trim().ToLower();
        filtrosTexto.Add($"Mostrar eliminados: {estado switch
        {
            "no" or "activo" => "Solo activos",
            "sí" or "si" or "eliminado" => "Solo eliminados",
            _ => "Todos"
        }}");

        return await excelExportService.ExportAsync("OPE_DatoEmbarqueDiario", columns, enriched, filtrosTexto);
    }
}
