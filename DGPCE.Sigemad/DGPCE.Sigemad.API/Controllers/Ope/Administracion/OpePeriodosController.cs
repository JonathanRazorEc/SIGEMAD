using DGPCE.Sigemad.Application.Features.Ope.Datos.OpePeriodos.Commands.DeleteOpePeriodos;
using DGPCE.Sigemad.Application.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpePeriodos.Vms;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpePeriodos.Commands.CreateOpePeriodos;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpePeriodos.Commands.UpdateOpePeriodos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePeriodos.Queries.GetOpePeriodosList;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePeriodos.Queries.GetOpePeriodoById;
using DGPCE.Sigemad.Identity.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Dapper;
using DGPCE.Sigemad.Application.Record;

namespace DGPCE.Sigemad.API.Controllers.Ope.Administracion;

[Authorize]
[Route("api/v1/ope-periodos")]
[ApiController]
public class OpePeriodosController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpePeriodosController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost(Name = "CreateOpePeriodo")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateOpePeriodoResponse>> Create([FromBody] CreateOpePeriodoCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginationVm<OpePeriodo>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationVm<OpePeriodoVm>>> GetOpePeriodos(
        [FromQuery] GetOpePeriodosListQuery query)
    {
        var pagination = await _mediator.Send(query);
        return Ok(pagination);
    }


    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Summary = "Busqueda de periodo de OPE por id")]
    public async Task<ActionResult<OpePeriodo>> GetById(int id)
    {
        var query = new GetOpePeriodoByIdQuery(id);
        var opePeriodo = await _mediator.Send(query);

        if (opePeriodo == null)
            return NotFound();

        return Ok(opePeriodo);
    }

    [HttpPut(Name = "UpdateOpePeriodo")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update([FromBody] UpdateOpePeriodoCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteOpePeriodo")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteOpePeriodoCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }


    [HttpGet("exportExcel")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [SwaggerOperation("Exportar a Excel OPE Periodos con filtros")]
    public async Task<IActionResult> ExportExcel(
    [FromServices] IExcelExportService excelExportService,
    [FromServices] IConfiguration configuration,
    [FromQuery] string? nombre = null,
    [FromQuery] int? idOpePeriodoTipo = null,
    [FromQuery] DateTime? fechaInicioFaseSalida = null,
    [FromQuery] DateTime? fechaFinFaseSalida = null,
    [FromQuery] DateTime? fechaInicioFaseRetorno = null,
    [FromQuery] DateTime? fechaFinFaseRetorno = null
)
    {
        var connStr = configuration.GetConnectionString("ConnectionString")!;
        await using var con = new SqlConnection(connStr);

        // 1️⃣ Sólo activos + filtros dinámicos
        var filtros = new List<string> { "p.Borrado = 0" };
        if (!string.IsNullOrWhiteSpace(nombre)) filtros.Add("p.Nombre LIKE '%' + @nombre + '%'");
        if (idOpePeriodoTipo.HasValue) filtros.Add("p.IdOpePeriodoTipo = @idOpePeriodoTipo");
        if (fechaInicioFaseSalida.HasValue) filtros.Add("p.FechaInicioFaseSalida >= @fechaInicioFaseSalida");
        if (fechaFinFaseSalida.HasValue) filtros.Add("p.FechaFinFaseSalida <= @fechaFinFaseSalida");
        if (fechaInicioFaseRetorno.HasValue) filtros.Add("p.FechaInicioFaseRetorno >= @fechaInicioFaseRetorno");
        if (fechaFinFaseRetorno.HasValue) filtros.Add("p.FechaFinFaseRetorno <= @fechaFinFaseRetorno");

        var whereClause = filtros.Any()
            ? "WHERE " + string.Join(" AND ", filtros)
            : string.Empty;

        // 2️⃣ Consulta con CTE
        var sql = $@"
WITH PeriodosCTE AS (
    SELECT
        p.Nombre                                AS Nombre,
        t.Nombre                                AS Periodo,
        CONVERT(date, p.FechaInicioFaseSalida)  AS FechaInicioFaseSalida,
        CONVERT(date, p.FechaFinFaseSalida)     AS FechaFinFaseSalida,
        CONVERT(date, p.FechaInicioFaseRetorno) AS FechaInicioFaseRetorno,
        CONVERT(date, p.FechaFinFaseRetorno)    AS FechaFinFaseRetorno
    FROM OPE_Periodo p
    LEFT JOIN OPE_PeriodoTipo t 
        ON p.IdOpePeriodoTipo = t.Id
    {whereClause}
)
SELECT *
FROM PeriodosCTE
ORDER BY FechaInicioFaseSalida DESC;";

        var parametros = new
        {
            nombre,
            idOpePeriodoTipo,
            fechaInicioFaseSalida,
            fechaFinFaseSalida,
            fechaInicioFaseRetorno,
            fechaFinFaseRetorno
        };

        var rows = await con.QueryAsync(sql, parametros);
        var data = rows
            .Select(r => (IDictionary<string, object>)r)
            .Select(dict => new Dictionary<string, object>(dict, StringComparer.OrdinalIgnoreCase))
            .ToList();

        // 3️⃣ Eliminamos la hora de cada DateTime antes de exportar
        var dateCols = new[]
        {
        "FechaInicioFaseSalida",
        "FechaFinFaseSalida",
        "FechaInicioFaseRetorno",
        "FechaFinFaseRetorno"
    };
        foreach (var dict in data)
        {
            foreach (var col in dateCols)
            {
                if (dict.TryGetValue(col, out var val) && val is DateTime dt)
                    dict[col] = dt.ToString("yyyy-MM-dd");

            }
        }

        // 4️⃣ Columnas para el Excel (sin “Borrado”)
        var columns = new List<ColumnInfo>
    {
        new("Nombre",                 "Nombre",               false, false, "text",  null, null),
        new("Periodo",                "Periodo",              false, false, "text",  null, null),
        new("FechaInicioFaseSalida",  "Inicio fase salida",   false, false, "date",  null, null),
        new("FechaFinFaseSalida",     "Fin fase salida",      false, false, "date",  null, null),
        new("FechaInicioFaseRetorno","Inicio fase retorno",   false, false, "date",  null, null),
        new("FechaFinFaseRetorno",    "Fin fase retorno",     false, false, "date",  null, null),
    };

        // 5️⃣ Filtros para la cabecera del Excel
        var filtrosTexto = new List<string> { "Filtros aplicados:" };
        if (!string.IsNullOrWhiteSpace(nombre))
            filtrosTexto.Add($"Nombre contiene: \"{nombre}\"");
        if (idOpePeriodoTipo.HasValue)
        {
            var tipoDesc = await con.ExecuteScalarAsync<string>(
                "SELECT Nombre FROM OPE_PeriodoTipo WHERE Id = @id",
                new { id = idOpePeriodoTipo });
            filtrosTexto.Add($"Tipo de periodo: {tipoDesc}");
        }
        if (fechaInicioFaseSalida.HasValue)
            filtrosTexto.Add($"Inicio fase salida ≥ {fechaInicioFaseSalida:dd/MM/yyyy}");
        if (fechaFinFaseSalida.HasValue)
            filtrosTexto.Add($"Fin fase salida ≤ {fechaFinFaseSalida:dd/MM/yyyy}");
        if (fechaInicioFaseRetorno.HasValue)
            filtrosTexto.Add($"Inicio fase retorno ≥ {fechaInicioFaseRetorno:dd/MM/yyyy}");
        if (fechaFinFaseRetorno.HasValue)
            filtrosTexto.Add($"Fin fase retorno ≤ {fechaFinFaseRetorno:dd/MM/yyyy}");

        // 6️⃣ ¡Exportamos sin rastro de hora!
        return await excelExportService.ExportAsync(
            "OPE_Periodos_Activos",
            columns,
            data,
            filtrosTexto
        );
    }



}
