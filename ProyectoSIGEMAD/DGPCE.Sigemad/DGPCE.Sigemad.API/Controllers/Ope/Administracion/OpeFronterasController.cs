using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Commands.DeleteOpeFronteras;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Queries.GetOpeFronteraById;
using DGPCE.Sigemad.Application.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Commands.CreateOpeFronteras;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Commands.UpdateOpeFronteras;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Queries.GetOpeFronterasList;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Vms;
using DGPCE.Sigemad.Identity.Services.Interfaces;
using Microsoft.Data.SqlClient;
using DGPCE.Sigemad.Application.Record;
using Dapper;

namespace DGPCE.Sigemad.API.Controllers.Ope.Administracion;

[Authorize]
[Route("api/v1/ope-fronteras")]
[ApiController]
public class OpeFronterasController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpeFronterasController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost(Name = "CreateOpeFrontera")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateOpeFronteraResponse>> Create([FromBody] CreateOpeFronteraCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginationVm<OpeFrontera>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationVm<OpeFronteraVm>>> GetOpeFronteras(
        [FromQuery] GetOpeFronterasListQuery query)
    {
        var pagination = await _mediator.Send(query);
        return Ok(pagination);
    }


    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Summary = "Busqueda de frontera de OPE por id")]
    public async Task<ActionResult<OpeFrontera>> GetById(int id)
    {
        var query = new GetOpeFronteraByIdQuery(id);
        var opeFrontera = await _mediator.Send(query);

        if (opeFrontera == null)
            return NotFound();

        return Ok(opeFrontera);
    }

    [HttpPut(Name = "UpdateOpeFrontera")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update([FromBody] UpdateOpeFronteraCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteOpeFrontera")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteOpeFronteraCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet("exportExcel")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [SwaggerOperation("Exportar a Excel OPE Fronteras activas con filtros")]
    public async Task<IActionResult> ExportExcel(
    [FromServices] IExcelExportService excelExportService,
    [FromServices] IConfiguration configuration,
    [FromQuery] string? nombre = null
)
    {
        var cs = configuration.GetConnectionString("ConnectionString")!;
        await using var con = new SqlConnection(cs);

        // 1️⃣ Filtros dinámicos: siempre activos (Borrado = 0)
        var filtros = new List<string> { "f.Borrado = 0" };
        if (!string.IsNullOrWhiteSpace(nombre))
            filtros.Add("f.Nombre LIKE '%' + @nombre + '%'");

        var where = filtros.Any()
            ? "WHERE " + string.Join(" AND ", filtros)
            : "";

        // 2️⃣ Consulta principal con joins
        var sql = $@"
SELECT
    f.Nombre                             AS Nombre,
    cc.Descripcion                       AS CCAA,
    p.Descripcion                        AS Provincia,
    m.Descripcion                        AS Municipio,
    f.Carretera                          AS Carretera,
    f.CoordenadaUTM_X                    AS UtmX,
    f.CoordenadaUTM_Y                    AS UtmY,
    f.TransitoMedioVehiculos             AS TransitoMedioVehiculos,
    f.TransitoAltoVehiculos              AS TransitoAltoVehiculos,
    CONVERT(date, f.FechaCreacion)       AS Fecha
FROM OPE_Frontera f
LEFT JOIN CCAA      cc ON f.IdCcaa      = cc.Id
LEFT JOIN Provincia p  ON f.IdProvincia = p.Id
LEFT JOIN Municipio m  ON f.IdMunicipio = m.Id
{where}
ORDER BY f.Nombre ASC";

        var rows = (await con.QueryAsync(sql, new { nombre }))
            .Select(r => (IDictionary<string, object>)r)
            .ToList();

        // 3️⃣ Convertimos a List<Dictionary<...>> y eliminamos la hora de “Fecha”
        var data = rows
            .Select(d => new Dictionary<string, object>(d, StringComparer.OrdinalIgnoreCase))
            .ToList();

        foreach (var dict in data)
        {
            //if (dict.TryGetValue("Fecha", out var o) && o is DateTime dt)
                //dict["Fecha"] = dt.Date;

            if (dict.TryGetValue("Fecha", out var f) && f is DateTime dt)
                dict["Fecha"] = dt.ToString("yyyy-MM-dd");
        }

        // 4️⃣ Definición de columnas para el Excel
        var columns = new List<ColumnInfo>
    {
        new("Nombre",                  "Nombre",                  false, false, "text",   null, null),
        new("CCAA",                    "Comunidad Autónoma",      false, false, "text",   null, null),
        new("Provincia",               "Provincia",               false, false, "text",   null, null),
        new("Municipio",               "Municipio",               false, false, "text",   null, null),
        new("Carretera", "Carretera PK",                          false, false, "text",   null, null),
        new("UtmX",                    "Utm X",                   false, false, "number", null, null),
        new("UtmY",                    "Utm Y",                   false, false, "number", null, null),
        new("TransitoMedioVehiculos",  "Tránsito Medio Vehículos",false, false, "number", null, null),
        new("TransitoAltoVehiculos",   "Tránsito Alto Vehículos", false, false, "number", null, null),
        new("Fecha",                   "Fecha",                   false, false, "date",   null, null),
    };

        // 5️⃣ Construcción de la lista de filtros para el encabezado
        var filtrosTexto = new List<string> { "Filtros aplicados:" };
        if (!string.IsNullOrWhiteSpace(nombre))
            filtrosTexto.Add($"Nombre contiene: \"{nombre}\"");

        // 6️⃣ Exportación con cabecera de filtros
        return await excelExportService.ExportAsync(
            "OPE_Fronteras_Activas",
            columns,
            data,
            filtrosTexto
        );
    }



}
