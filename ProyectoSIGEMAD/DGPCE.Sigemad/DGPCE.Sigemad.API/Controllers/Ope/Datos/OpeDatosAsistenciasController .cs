using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosAsistencias.Commands.DeleteOpeDatosAsistencias;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosAsistencias.Queries.GetOpeDatoAsistenciaById;
using DGPCE.Sigemad.Application.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosAsistencias.Vms;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosAsistencias.Commands.CreateOpeDatosAsistencias;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosAsistencias.Commands.UpdateOpeDatosAsistencias;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosAsistencias.Queries.GetOpeDatosAsistenciasList;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using DGPCE.Sigemad.Identity.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Dapper;
using DGPCE.Sigemad.Application.Record;

namespace DGPCE.Sigemad.API.Controllers.Ope.Datos;

[Authorize]
[Route("api/v1/ope-datos-asistencias")]
[ApiController]
public class OpeDatosAsistenciasController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<OpeDatosAsistenciasController> _logger;

    public OpeDatosAsistenciasController(IMediator mediator, ILogger<OpeDatosAsistenciasController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }


    [HttpPost(Name = "CreateOpeDatoAsistencia")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateOpeDatoAsistenciaResponse>> Create([FromBody] CreateOpeDatoAsistenciaCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginationVm<OpeDatoAsistencia>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationVm<OpeDatoAsistenciaVm>>> GetOpeDatosAsistencias(
        [FromQuery] GetOpeDatosAsistenciasListQuery query)
    {
        var pagination = await _mediator.Send(query);
        return Ok(pagination);
    }


    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Summary = "Busqueda de dato de asistencia de OPE por id")]
    public async Task<ActionResult<OpeDatoAsistencia>> GetById(int id)
    {
        var query = new GetOpeDatoAsistenciaByIdQuery(id);
        var opeDatoAsistencia = await _mediator.Send(query);

        if (opeDatoAsistencia == null)
            return NotFound();

        return Ok(opeDatoAsistencia);
    }

    [HttpPut(Name = "UpdateOpeDatoAsistencia")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update([FromBody] UpdateOpeDatoAsistenciaCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteOpeDatoAsistencia")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteOpeDatoAsistenciaCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }


    [HttpGet("exportExcel")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [SwaggerOperation("Exportar a Excel OPE Datos de Asistencia con filtros")]
    public async Task<IActionResult> ExportExcel(
    [FromServices] IExcelExportService excelExportService,
    [FromServices] IConfiguration configuration,
    [FromQuery] DateTime? fechaInicio = null,
    [FromQuery] DateTime? fechaFin = null,
    [FromQuery] int? IdOpePeriodo = null,
    [FromQuery] int? IdOpeFase = null,
    [FromQuery] int[]? IdsOpePuertos = null,
    [FromQuery] string? criterioNumerico = null,
    [FromQuery] string? criterioNumericoRadio = null,
    [FromQuery] string? criterioNumericoCriterioCantidad = null,
    [FromQuery] int? criterioNumericoCriterioCantidadCantidad = null
)
    {
        var connStr = configuration.GetConnectionString("ConnectionString")!;
        await using var con = new SqlConnection(connStr);

        var filtros = new List<string> { "df.Borrado = 0" };

        if (fechaInicio.HasValue)
            filtros.Add("CONVERT(date, df.Fecha) >= @fechaInicio");
        if (fechaFin.HasValue)
            filtros.Add("CONVERT(date, df.Fecha) <= @fechaFin");
        if (IdOpePeriodo.HasValue)
            filtros.Add("df.IdOpePeriodo = @IdOpePeriodo");
        if (IdOpeFase.HasValue)
            filtros.Add("f.Id = @IdOpeFase");
        if (IdsOpePuertos?.Any() == true)
            filtros.Add("df.IdOpePuerto IN @IdsOpePuertos");

        // 🩺 Lógica de filtro numérico
        if (!string.IsNullOrEmpty(criterioNumericoRadio) && 
            !string.IsNullOrEmpty(criterioNumerico))
        {
            var radio = criterioNumericoRadio.Trim().ToLower().Replace("que", "").Trim();
            string? op = radio switch
            {
                "maximo" => ">=",
                "mínimo" => "<=",
                "cantidad" => "=",
                "mayor" => ">",
                "menor" => "<",
                _ => null
            };

            if (!string.IsNullOrEmpty(op))
            {
                string columna = criterioNumerico.Trim().ToLowerInvariant() switch
                {
                    "total" => "(COALESCE(s.Sanitarias,0) + COALESCE(o.Sociales,0) + COALESCE(t.Traducciones,0))",
                    "sanitarias" => "COALESCE(s.Sanitarias,0)",
                    "sociales" => "COALESCE(o.Sociales,0)",
                    "traducciones" => "COALESCE(t.Traducciones,0)",
                    _ => throw new ArgumentException("Columna no válida")
                };

                // Solo aplicar filtro SQL si es 'cantidad'
                if (radio == "cantidad" || radio == "mayor" || radio == "menor")
                {
                    filtros.Add($"{columna} {op} @criterioNumericoCantidad");
                }
                // Para 'maximo' y 'minimo', NO agregar nada aquí (ya que se filtra en el handler)
            }
        }

        var where = filtros.Any() ? "WHERE " + string.Join(" AND ", filtros) : "";

        var sql = $@"
SELECT
    CONVERT(date, df.Fecha) AS Fecha,
    p.Nombre AS Puerto,
    f.Nombre AS Fase,
    COALESCE(s.Sanitarias, 0) AS Sanitarias,
    COALESCE(o.Sociales, 0) AS Sociales,
    COALESCE(t.Traducciones, 0) AS Traducciones,
    COALESCE(s.Sanitarias, 0) + COALESCE(o.Sociales, 0) + COALESCE(t.Traducciones, 0) AS Total
FROM OPE_DatoAsistencia df
INNER JOIN OPE_Puerto p ON df.IdOpePuerto = p.Id
INNER JOIN OPE_Fase f ON p.IdOpeFase = f.Id
LEFT JOIN (
    SELECT IdOpeDatoAsistencia, SUM(Numero) AS Sanitarias
    FROM OPE_DatoAsistenciaSanitaria
    WHERE Borrado = 0
    GROUP BY IdOpeDatoAsistencia
) s ON s.IdOpeDatoAsistencia = df.Id
LEFT JOIN (
    SELECT IdOpeDatoAsistencia, SUM(Numero) AS Sociales
    FROM OPE_DatoAsistenciaSocial
    WHERE Borrado = 0
    GROUP BY IdOpeDatoAsistencia
) o ON o.IdOpeDatoAsistencia = df.Id
LEFT JOIN (
    SELECT IdOpeDatoAsistencia, SUM(Numero) AS Traducciones
    FROM OPE_DatoAsistenciaTraduccion
    WHERE Borrado = 0
    GROUP BY IdOpeDatoAsistencia
) t ON t.IdOpeDatoAsistencia = df.Id
{where}
ORDER BY Fecha DESC;";

        var parametros = new
        {
            fechaInicio,
            fechaFin,
            IdOpePeriodo,
            IdOpeFase,
            IdsOpePuertos,
            criterioNumericoCantidad = (criterioNumericoRadio == "cantidad" || criterioNumericoRadio == "mayor" || criterioNumericoRadio == "menor")
            ? criterioNumericoCriterioCantidadCantidad
            : null
        };

        // 3️⃣ Ejecutar y mapear resultados
        var rows = await con.QueryAsync(sql, parametros);
        var data = rows
            .Select(r => (IDictionary<string, object>)r)
            .Select(d => new Dictionary<string, object>(d, StringComparer.OrdinalIgnoreCase))
            .ToList();

        // 🔧 Formatear las fechas sin hora
        foreach (var row in data)
        {
            if (row.ContainsKey("Fecha") && row["Fecha"] is DateTime fecha)
            {
                row["Fecha"] = fecha.ToString("dd/MM/yyyy");
            }
        }

        // 4️⃣ Lógica para 'maximo' y 'minimo'
        if (!string.IsNullOrEmpty(criterioNumericoRadio) && !string.IsNullOrEmpty(criterioNumerico))
{
    var radio = criterioNumericoRadio.Trim().ToLower();
    string columna = criterioNumerico.Trim().ToLowerInvariant() switch
    {
        "total" => "Total",
        "sanitarias" => "Sanitarias",
        "sociales" => "Sociales",
        "traducciones" => "Traducciones",
        _ => throw new ArgumentException("Columna no válida")
    };



    if (radio == "maximo")
    {
        var maxValue = data.Max(d => Convert.ToInt32(d[columna]));
        data = data.Where(d => Convert.ToInt32(d[columna]) == maxValue).ToList();
    }
    else if (radio == "mínimo")
    {
        var minValue = data.Min(d => Convert.ToInt32(d[columna]));
        data = data.Where(d => Convert.ToInt32(d[columna]) == minValue).ToList();
    }
}

        var columns = new List<ColumnInfo>
    {
        new("Fecha", "Fecha", false, false, "date", null, null),
        new("Puerto", "Puerto", false, false, "text", null, null),
        new("Fase", "Fase", false, false, "text", null, null),
        new("Sanitarias", "Sanitarias", false, false, "number", null, null),
        new("Sociales", "Sociales", false, false, "number", null, null),
        new("Traducciones", "Traducciones", false, false, "number", null, null),
        new("Total", "Total", false, false, "number", null, null),
    };

        string nombreFase = "";
        if (IdOpeFase.HasValue)
        {
            nombreFase = await con.QueryFirstOrDefaultAsync<string>(
                "SELECT Nombre FROM OPE_Fase WHERE Id = @id",
                new { id = IdOpeFase });
        }

        List<string> nombresPuertos = new();
        if (IdsOpePuertos?.Any() == true)
        {
            nombresPuertos = (await con.QueryAsync<string>(
                "SELECT Nombre FROM OPE_Puerto WHERE Id IN @ids",
                new { ids = IdsOpePuertos }
            )).ToList();
        }

        var filtrosTexto = new List<string> { "Filtros aplicados:" };
        if (fechaInicio.HasValue)
            filtrosTexto.Add($"Fecha desde ≥ {fechaInicio:dd/MM/yyyy}");
        if (fechaFin.HasValue)
            filtrosTexto.Add($"Fecha hasta ≤ {fechaFin:dd/MM/yyyy}");
        if (IdOpePeriodo.HasValue)
            filtrosTexto.Add($"Período: {IdOpePeriodo}");
        if (IdOpeFase.HasValue)
            filtrosTexto.Add($"Fase: {nombreFase}");
        if (IdsOpePuertos?.Any() == true)
            filtrosTexto.Add($"Puertos: {string.Join(", ", nombresPuertos)}");


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


        return await excelExportService.ExportAsync("OPE_Asistencias", columns, data, filtrosTexto);
    }


}
