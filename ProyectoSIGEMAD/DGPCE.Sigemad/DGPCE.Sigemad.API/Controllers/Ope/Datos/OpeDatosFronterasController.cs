using Dapper;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Commands.CreateOpeDatosFronteras;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Commands.DeleteOpeDatosFronteras;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Commands.UpdateOpeDatosFronteras;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Queries.GetOpeDatoFronteraById;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Queries.GetOpeDatosFronterasList;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Record;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using DGPCE.Sigemad.Identity.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers.Ope.Datos;

[Authorize]
[Route("api/v1/ope-datos-fronteras")]
[ApiController]
public class OpeDatosFronterasController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpeDatosFronterasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(Name = "CreateOpeDatoFrontera")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateOpeDatoFronteraResponse>> Create([FromBody] CreateOpeDatoFronteraCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginationVm<OpeDatoFrontera>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationVm<OpeDatoFronteraVm>>> GetOpeDatosFronteras(
        [FromQuery] GetOpeDatosFronterasListQuery query)
    {
        var pagination = await _mediator.Send(query);
        return Ok(pagination);
    }

    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Summary = "Busqueda de dato de frontera de OPE por id")]
    public async Task<ActionResult<OpeDatoFrontera>> GetById(int id)
    {
        var query = new GetOpeDatoFronteraByIdQuery(id);
        var opeDatoFrontera = await _mediator.Send(query);

        if (opeDatoFrontera == null)
            return NotFound();

        return Ok(opeDatoFrontera);
    }

    [HttpPut(Name = "UpdateOpeDatoFrontera")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update([FromBody] UpdateOpeDatoFronteraCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteOpeDatoFrontera")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteOpeDatoFronteraCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet("exportExcel")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [SwaggerOperation("Exportar a Excel OPE Datos Fronteras con filtros")]
    public async Task<IActionResult> ExportExcel(
    [FromServices] IExcelExportService excelExportService,
    [FromServices] IConfiguration configuration,
    [FromQuery] DateTime? fechaInicio = null,
    [FromQuery] DateTime? fechaFin = null,
    [FromQuery] int[]? IdsOpeFronteras = null,
    [FromQuery] string? criterioNumerico = null,
    [FromQuery] string? criterioNumericoRadio = null,
    [FromQuery] int? criterioNumericoCriterioCantidadCantidad = null,
    [FromQuery] string? criterioNumericoCriterioCantidad = null

)
    {
        var connStr = configuration.GetConnectionString("ConnectionString")!;
        await using var con = new SqlConnection(connStr);

        var filtros = new List<string> { "df.Borrado = 0" };

        if (fechaInicio.HasValue)
            filtros.Add("CONVERT(date, df.Fecha) >= @fechaInicio");
        if (fechaFin.HasValue)
            filtros.Add("CONVERT(date, df.Fecha) <= @fechaFin");
        if (IdsOpeFronteras?.Any() == true)
            filtros.Add("df.IdOpeFrontera IN @IdsOpeFronteras");

        // Lógica corregida: usar criterioNumericoRadio como operador directamente
        if (!string.IsNullOrEmpty(criterioNumericoRadio) &&
            !string.IsNullOrEmpty(criterioNumerico) &&
            criterioNumericoRadio is "cantidad" or "mayor" or "menor")
        {
            var op = criterioNumericoRadio.Trim().ToLower() switch
            {
                "cantidad" => "=",
                "mayor" => ">",
                "menor" => "<",

                _ => null
            };

            if (!string.IsNullOrEmpty(op))
            {
                string columna = criterioNumerico.Trim().ToLowerInvariant() switch
                {
                    "vehiculos" => "df.NumeroVehiculos",
                    "numerovehiculos" => "df.NumeroVehiculos",
                    "total" => "df.NumeroVehiculos",
                    _ => throw new ArgumentException("Columna no válida")
                };

                filtros.Add($"{columna} {op} @criterioNumericoCantidad");
                
                
            }
        }

        var sql = $@"
SELECT
    CONVERT(date, df.Fecha) AS Fecha,
    CONCAT(CONVERT(varchar(10), df.Fecha, 103), ' ',
           CONVERT(varchar(5), df.InicioIntervaloHorarioPersonalizado, 108)) AS FechaInicio,
    CONCAT(CONVERT(varchar(10), df.Fecha, 103), ' ',
           CONVERT(varchar(5), df.FinIntervaloHorarioPersonalizado, 108)) AS FechaFin,
    f.Nombre AS Frontera,
    SUM(df.NumeroVehiculos) AS TotalVehiculos
FROM dbo.OPE_DatoFrontera df
INNER JOIN dbo.OPE_Frontera f ON df.IdOpeFrontera = f.Id
{(filtros.Any() ? "WHERE " + string.Join(" AND ", filtros) : "")}
GROUP BY
    CONVERT(date, df.Fecha),
    df.Fecha,
    df.InicioIntervaloHorarioPersonalizado,
    df.FinIntervaloHorarioPersonalizado,
    f.Nombre
ORDER BY
    Fecha DESC,
    f.Nombre;
";

        var parametros = new
        {
            fechaInicio,
            fechaFin,
            IdsOpeFronteras,
            criterioNumericoCantidad = (criterioNumericoRadio == "cantidad" || criterioNumericoRadio == "mayor" || criterioNumericoRadio == "menor")
                ? criterioNumericoCriterioCantidadCantidad
                : null
        };

        var rows = await con.QueryAsync(sql, parametros);
        var data = rows
            .Select(r => (IDictionary<string, object>)r)
            .Select(d => new Dictionary<string, object>(d, StringComparer.OrdinalIgnoreCase))
            .ToList();

        // 🩺 Filtrado post-procesado para 'maximo' y 'mínimo'
        if (!string.IsNullOrEmpty(criterioNumericoRadio) && !string.IsNullOrEmpty(criterioNumerico))
        {
            var radio = criterioNumericoRadio.Trim().ToLower();
            string columna = "TotalVehiculos";  // Usar el alias del resultado agregado

            if (radio == "maximo")
            {
                var maxValue = data.Max(d => Convert.ToInt32(d[columna]));
                data = data.Where(d => Convert.ToInt32(d[columna]) == maxValue).ToList();
            }
            else if (radio == "minimo")//con tilde
            {
                var minValue = data.Min(d => Convert.ToInt32(d[columna]));
                data = data.Where(d => Convert.ToInt32(d[columna]) == minValue).ToList();
            }
        }

        var columns = new List<ColumnInfo>
    {
        new("Fecha", "Fecha", false, false, "date", null, null),
        new("FechaInicio", "Fecha Inicio", false, false, "text", null, null),
        new("FechaFin", "Fecha Fin", false, false, "text", null, null),
        new("Frontera", "Frontera", false, false, "text", null, null),
        new("TotalVehiculos", "Total Vehículos", false, false, "number", null, null),
    };

        var filtrosTexto = new List<string> { "Filtros aplicados:" };
        if (fechaInicio.HasValue)
            filtrosTexto.Add($"Fecha desde ≥ {fechaInicio:dd/MM/yyyy}");
        if (fechaFin.HasValue)
            filtrosTexto.Add($"Fecha hasta ≤ {fechaFin:dd/MM/yyyy}");
        if (IdsOpeFronteras?.Any() == true)
        {
            var fronterasSql = "SELECT Id, Nombre FROM OPE_Frontera WHERE Id IN @IdsOpeFronteras";
            var fronteras = await con.QueryAsync(fronterasSql, new { IdsOpeFronteras });
            var nombresFronteras = fronteras.Select(f => f.Nombre?.ToString()).ToList();
            filtrosTexto.Add($"Fronteras: {string.Join(", ", nombresFronteras)}");
        }

        //if (!string.IsNullOrEmpty(criterioNumericoRadio) && !string.IsNullOrEmpty(criterioNumerico))
        //{
        //    var radio = criterioNumericoRadio.Trim().ToLower();
        //    string columna = "TotalVehiculos";  // usar el alias de resultado

        //    if (radio == "maximo")
        //    {
        //        var maxValue = data.Max(d => Convert.ToInt32(d[columna]));
        //        data = data.Where(d => Convert.ToInt32(d[columna]) == maxValue).ToList();
        //    }
        //    else if (radio == "mínimo")
        //    {
        //        var minValue = data.Min(d => Convert.ToInt32(d[columna]));
        //        data = data.Where(d => Convert.ToInt32(d[columna]) == minValue).ToList();
        //    }
        //}

        if (!string.IsNullOrEmpty(criterioNumericoRadio) && !string.IsNullOrEmpty(criterioNumerico))
        {
            // Para "cantidad", "mayor" y "menor" queremos que aparezca el número
            if ((criterioNumericoRadio.Equals("cantidad", StringComparison.OrdinalIgnoreCase)
                 || criterioNumericoRadio.Equals("mayor", StringComparison.OrdinalIgnoreCase)
                 || criterioNumericoRadio.Equals("menor", StringComparison.OrdinalIgnoreCase))
                && criterioNumericoCriterioCantidadCantidad.HasValue)
            {
                filtrosTexto.Add($"{criterioNumerico} {criterioNumericoRadio} {criterioNumericoCriterioCantidadCantidad}");
            }
            else
            {
                filtrosTexto.Add($"{criterioNumerico} {criterioNumericoRadio}");
            }
        }



        return await excelExportService.ExportAsync("OPE_Datos_Fronteras", columns, data, filtrosTexto);
    }
}