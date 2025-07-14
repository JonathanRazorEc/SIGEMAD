using DGPCE.Sigemad.Application.Features.Ope.Datos.OpePuertos.Commands.DeleteOpePuertos;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpePuertos.Queries.GetOpePuertoById;
using DGPCE.Sigemad.Application.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpePuertos.Vms;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using DGPCE.Sigemad.Identity.Services.Interfaces;
using Microsoft.Data.SqlClient;
using DGPCE.Sigemad.Application.Record;
using Dapper;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuertos.Commands.UpdateOpePuertos;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuertos.Commands.CreateOpePuertos;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpePuertos.Queries.GetOpePuertosList;

namespace DGPCE.Sigemad.API.Controllers.Ope.Administracion;

[Authorize]
[Route("api/v1/ope-puertos")]
[ApiController]
public class OpePuertosController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpePuertosController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost(Name = "CreateOpePuerto")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateOpePuertoResponse>> Create([FromBody] CreateOpePuertoCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginationVm<OpePuerto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationVm<OpePuertoVm>>> GetOpePuertos(
        [FromQuery] GetOpePuertosListQuery query)
    {
        var pagination = await _mediator.Send(query);
        return Ok(pagination);
    }


    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Summary = "Busqueda de puerto de OPE por id")]
    public async Task<ActionResult<OpePuerto>> GetById(int id)
    {
        var query = new GetOpePuertoByIdQuery(id);
        var opePuerto = await _mediator.Send(query);

        if (opePuerto == null)
            return NotFound();

        return Ok(opePuerto);
    }

    [HttpPut(Name = "UpdateOpePuerto")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update([FromBody] UpdateOpePuertoCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteOpePuerto")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteOpePuertoCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet("exportExcel")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [SwaggerOperation("Exportar a Excel OPE Puertos activos con filtros")]
    public async Task<IActionResult> ExportExcel(
    [FromServices] IExcelExportService excelExportService,
    [FromServices] IConfiguration configuration,
    [FromQuery] string? nombre = null,
    [FromQuery] int? idPais = null,
    [FromQuery] int? idOpeFase = null
)
    {
        var connStr = configuration.GetConnectionString("ConnectionString")!;
        await using var con = new SqlConnection(connStr);

        // 1️⃣ Sólo activos y demás filtros dinámicos
        var filtros = new List<string> { "p.Borrado = 0" };
        if (!string.IsNullOrWhiteSpace(nombre)) filtros.Add("p.Nombre LIKE '%' + @nombre + '%'");
        if (idPais.HasValue) filtros.Add("p.IdPais = @idPais");
        if (idOpeFase.HasValue) filtros.Add("p.IdOpeFase = @idOpeFase");

        var whereClause = filtros.Any()
            ? "WHERE " + string.Join(" AND ", filtros)
            : "";

        // 2️⃣ Consulta principal
        var sql = $@"
SELECT
    p.Nombre        AS PuertoNombre,
    f.Nombre        AS FaseNombre,
    pa.Descripcion  AS PaisNombre,
    CONVERT(date, p.FechaValidezDesde) AS FechaValidezDesde,
    CONVERT(date, p.FechaValidezHasta) AS FechaValidezHasta,
    p.CoordenadaUTM_X,
    p.CoordenadaUTM_Y,
    p.Capacidad
FROM OPE_Puerto p
LEFT JOIN OPE_Fase f ON p.IdOpeFase = f.Id
LEFT JOIN Pais   pa ON p.IdPais    = pa.Id
{whereClause}
ORDER BY p.FechaValidezDesde ASC";

        var rows = (await con.QueryAsync(sql, new { nombre, idPais, idOpeFase }))
            .Select(r => (IDictionary<string, object>)r)
            .ToList();

        // 3️⃣ Preparamos el diccionario y eliminamos la hora de las fechas
        var data = rows
            .Select(d => new Dictionary<string, object>(d, StringComparer.OrdinalIgnoreCase))
            .ToList();

        var dateCols = new[] { "FechaValidezDesde", "FechaValidezHasta" };
        foreach (var dict in data)
        {
            foreach (var col in dateCols)
            {
                if (dict.TryGetValue(col, out var val) && val is DateTime dt)
                    dict[col] = dt.ToString("yyyy-MM-dd");
            }
        }

        // 4️⃣ Definición de columnas para Excel
        var columns = new List<ColumnInfo>
    {
        new("PuertoNombre",      "Nombre",             false, false, "text",   null, null),
        new("FaseNombre",        "Operación",          false, false, "text",   null, null),
        new("PaisNombre",        "País",               false, false, "text",   null, null),
        new("FechaValidezDesde", "Operatividad Desde", false, false, "date",   null, null),
        new("FechaValidezHasta", "Operatividad Hasta", false, false, "date",   null, null),
        new("CoordenadaUTM_X",   "UTM X",              false, false, "number", null, null),
        new("CoordenadaUTM_Y",   "UTM Y",              false, false, "number", null, null),
        new("Capacidad",         "Capacidad",          false, false, "number", null, null),
    };

        // 5️⃣ Cabecera de filtros
        var filtrosTexto = new List<string> { "Filtros aplicados:" };
        if (!string.IsNullOrWhiteSpace(nombre))
            filtrosTexto.Add($"Nombre contiene: \"{nombre}\"");
        if (idPais.HasValue)
        {
            var paisTexto = await con.ExecuteScalarAsync<string>(
                "SELECT Descripcion FROM Pais WHERE Id = @idPais", new { idPais });
            filtrosTexto.Add($"País: {paisTexto}");
        }
        if (idOpeFase.HasValue)
        {
            var faseTexto = await con.ExecuteScalarAsync<string>(
                "SELECT Nombre FROM OPE_Fase WHERE Id = @idOpeFase", new { idOpeFase });
            filtrosTexto.Add($"Operación: {faseTexto}");
        }

        // 6️⃣ Exportar a Excel
        return await excelExportService.ExportAsync(
            "OPE_Puertos_Activos",
            columns,
            data,
            filtrosTexto
        );
    }







}
