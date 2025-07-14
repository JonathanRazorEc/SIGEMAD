using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLineasMaritimas.Commands.DeleteOpeLineasMaritimas;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLineasMaritimas.Queries.GetOpeLineaMaritimaById;
using DGPCE.Sigemad.Application.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLineasMaritimas.Vms;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLineasMaritimas.Commands.CreateOpeLineasMaritimas;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLineasMaritimas.Commands.UpdateOpeLineasMaritimas;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLineasMaritimas.Queries.GetOpeLineasMaritimasList;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using DGPCE.Sigemad.Identity.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Dapper;
using DGPCE.Sigemad.Application.Record;

namespace DGPCE.Sigemad.API.Controllers.Ope.Administracion;

[Authorize]
[Route("api/v1/ope-lineas-maritimas")]
[ApiController]
public class OpeLineasMaritimasController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpeLineasMaritimasController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost(Name = "CreateOpeLineaMaritima")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateOpeLineaMaritimaResponse>> Create([FromBody] CreateOpeLineaMaritimaCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginationVm<OpeLineaMaritima>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationVm<OpeLineaMaritimaVm>>> GetOpeLineasMaritimas(
        [FromQuery] GetOpeLineasMaritimasListQuery query)
    {
        var pagination = await _mediator.Send(query);
        return Ok(pagination);
    }


    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Summary = "Busqueda de línea marítima de OPE por id")]
    public async Task<ActionResult<OpeLineaMaritima>> GetById(int id)
    {
        var query = new GetOpeLineaMaritimaByIdQuery(id);
        var opeLineaMaritima = await _mediator.Send(query);

        if (opeLineaMaritima == null)
            return NotFound();

        return Ok(opeLineaMaritima);
    }

    [HttpPut(Name = "UpdateOpeLineaMaritima")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update([FromBody] UpdateOpeLineaMaritimaCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteOpeLineaMaritima")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteOpeLineaMaritimaCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }

    /////////////////////EXCEL//////////////////////////////////////////
    ///

    [HttpGet("exportExcel")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [SwaggerOperation("Exportar a Excel OPE Línea Marítima con filtros")]
    public async Task<IActionResult> ExportExcel(
    [FromServices] IExcelExportService excelExportService,
    [FromServices] IConfiguration configuration,
    [FromQuery] string? nombre = null,
    [FromQuery] DateTime? fechaInicioFaseSalida = null,
    [FromQuery] DateTime? fechaFinFaseSalida = null,
    [FromQuery] DateTime? fechaInicioFaseRetorno = null,
    [FromQuery] DateTime? fechaFinFaseRetorno = null,
    [FromQuery] int? idPuertoOrigen = null,
    [FromQuery] int? idPuertoDestino = null,
    [FromQuery] int? idPaisOrigen = null,
    [FromQuery] int? idPaisDestino = null,
    [FromQuery] int? idOpeFase = null
)
    {
        var connStr = configuration.GetConnectionString("ConnectionString")!;
        await using var con = new SqlConnection(connStr);

        string? nombrePuertoOrigen = null;
        string? nombrePuertoDestino = null;
        string? nombrePaisOrigen = null;
        string? nombrePaisDestino = null;
        string? nombreFase = null;

        if (idPuertoOrigen.HasValue)
            nombrePuertoOrigen = await con.ExecuteScalarAsync<string?>(
                "SELECT Nombre FROM OPE_Puerto WHERE Id = @id", new { id = idPuertoOrigen });

        if (idPuertoDestino.HasValue)
            nombrePuertoDestino = await con.ExecuteScalarAsync<string?>(
                "SELECT Nombre FROM OPE_Puerto WHERE Id = @id", new { id = idPuertoDestino });

        nombrePaisOrigen = await con.ExecuteScalarAsync<string?>(
        "SELECT Descripcion FROM Pais WHERE Id = @id", new { id = idPaisOrigen });

        nombrePaisDestino = await con.ExecuteScalarAsync<string?>(
    "SELECT Descripcion FROM Pais WHERE Id = @id", new { id = idPaisDestino });

        if (idOpeFase.HasValue)
            nombreFase = await con.ExecuteScalarAsync<string?>(
                "SELECT Nombre FROM OPE_Fase WHERE Id = @id", new { id = idOpeFase });

        var filtros = new List<string> { "p.Borrado = 0" };
        if (!string.IsNullOrWhiteSpace(nombre))
            filtros.Add("p.Nombre LIKE '%' + @nombre + '%'");
        if (fechaInicioFaseSalida.HasValue)
            filtros.Add("p.FechaValidezDesde >= @fechaInicioFaseSalida");
        if (fechaFinFaseSalida.HasValue)
            filtros.Add("p.FechaValidezDesde <= @fechaFinFaseSalida");
        if (fechaInicioFaseRetorno.HasValue)
            filtros.Add("p.FechaValidezHasta >= @fechaInicioFaseRetorno");
        if (fechaFinFaseRetorno.HasValue)
            filtros.Add("p.FechaValidezHasta <= @fechaFinFaseRetorno");
        if (idPuertoOrigen.HasValue)
            filtros.Add("p.IdOpePuertoOrigen = @idPuertoOrigen");
        if (idPuertoDestino.HasValue)
            filtros.Add("p.IdOpePuertoDestino = @idPuertoDestino");
        if (idPaisOrigen.HasValue)
            filtros.Add("por.IdPais = @idPaisOrigen");
        if (idPaisDestino.HasValue)
            filtros.Add("pdest.IdPais = @idPaisDestino");
        if (idOpeFase.HasValue)
            filtros.Add("p.IdOpeFase = @idOpeFase");

        var whereClause = filtros.Any()
            ? "WHERE " + string.Join(" AND ", filtros)
            : string.Empty;

        var sql = $@"
SELECT
    p.Nombre,
    p.IdOpePuertoOrigen,
    p.IdOpePuertoDestino,
    p.IdOpeFase,
    CONVERT(date, p.FechaValidezDesde) AS FechaValidezDesde,
    CONVERT(date, p.FechaValidezHasta) AS FechaValidezHasta
FROM OPE_LineaMaritima p
LEFT JOIN OPE_Puerto por ON p.IdOpePuertoOrigen = por.Id
LEFT JOIN OPE_Puerto pdest ON p.IdOpePuertoDestino = pdest.Id
{whereClause}
ORDER BY p.FechaValidezDesde DESC";

        var rows = (await con.QueryAsync(sql, new
        {
            nombre,
            fechaInicioFaseSalida,
            fechaFinFaseSalida,
            fechaInicioFaseRetorno,
            fechaFinFaseRetorno,
            idPuertoOrigen,
            idPuertoDestino,
            idPaisOrigen,
            idPaisDestino,
            idOpeFase
        }))
        .Select(r => (IDictionary<string, object>)r)
        .ToList();

        var lookupCols = new[]
        {
        new { Col = "IdOpePuertoOrigen", Tabla = "OPE_Puerto", Campo = "Nombre" },
        new { Col = "IdOpePuertoDestino", Tabla = "OPE_Puerto", Campo = "Nombre" },
        new { Col = "IdOpeFase", Tabla = "OPE_Fase", Campo = "Nombre" }
    };

        var lookups = new Dictionary<string, Dictionary<int, string>>();
        foreach (var lc in lookupCols)
        {
            var lookupSql = $"SELECT Id, {lc.Campo} AS Texto FROM {lc.Tabla} WHERE Borrado = 0";
            var data = await con.QueryAsync(lookupSql);
            lookups[lc.Col] = data.ToDictionary(d => (int)d.Id, d => (string)d.Texto);
        }

        var enriched = rows.Select(row =>
        {
            var dict = new Dictionary<string, object>(row, StringComparer.OrdinalIgnoreCase);

            foreach (var lc in lookupCols)
            {
                if (dict.TryGetValue(lc.Col, out var val)
                    && val != null
                    && int.TryParse(val.ToString(), out var id)
                    && lookups[lc.Col].TryGetValue(id, out var texto))
                {
                    dict[$"{lc.Col}__texto"] = texto;
                }
            }

            if (dict.TryGetValue("FechaValidezDesde", out var f1) && f1 is DateTime d1)
                dict["FechaValidezDesde"] = d1.ToString("dd/MM/yyyy");
            if (dict.TryGetValue("FechaValidezHasta", out var f2) && f2 is DateTime d2)
                dict["FechaValidezHasta"] = d2.ToString("dd/MM/yyyy");

            return dict;
        }).ToList();

        var columns = new List<ColumnInfo>
    {
        new("Nombre", "Nombre", false, false, "text", null, null),
        new("IdOpePuertoOrigen__texto", "Puerto Origen", false, false, "text", null, null),
        new("IdOpePuertoDestino__texto", "Puerto Destino", false, false, "text", null, null),
        new("IdOpeFase__texto", "Fase", false, false, "text", null, null),
        new("FechaValidezDesde", "Fecha operatividad OPE desde", false, false, "text", null, null),
        new("FechaValidezHasta", "Fecha operatividad OPE hasta", false, false, "text", null, null)
    };

        var filtrosTexto = new List<string> { "Filtros aplicados:" };
        if (!string.IsNullOrWhiteSpace(nombre))
            filtrosTexto.Add($"Nombre contiene: \"{nombre}\"");
        if (fechaInicioFaseSalida.HasValue)
            filtrosTexto.Add($"Fecha operatividad OPE desde ≥ {fechaInicioFaseSalida:dd/MM/yyyy}");
        if (fechaFinFaseSalida.HasValue)
            filtrosTexto.Add($"Fecha operatividad OPE desde ≤ {fechaFinFaseSalida:dd/MM/yyyy}");
        if (fechaInicioFaseRetorno.HasValue)
            filtrosTexto.Add($"Fecha operatividad OPE hasta ≥ {fechaInicioFaseRetorno:dd/MM/yyyy}");
        if (fechaFinFaseRetorno.HasValue)
            filtrosTexto.Add($"Fecha operatividad OPE hasta ≤ {fechaFinFaseRetorno:dd/MM/yyyy}");
        if (!string.IsNullOrWhiteSpace(nombrePuertoOrigen))
            filtrosTexto.Add($"Puerto origen: {nombrePuertoOrigen}");
        if (!string.IsNullOrWhiteSpace(nombrePuertoDestino))
            filtrosTexto.Add($"Puerto destino: {nombrePuertoDestino}");
        if (!string.IsNullOrWhiteSpace(nombrePaisOrigen))
            filtrosTexto.Add($"País puerto origen: {nombrePaisOrigen}");
        if (!string.IsNullOrWhiteSpace(nombrePaisDestino))
            filtrosTexto.Add($"País puerto destino: {nombrePaisDestino}");
        if (!string.IsNullOrWhiteSpace(nombreFase))
            filtrosTexto.Add($"Fase: {nombreFase}");


        return await excelExportService.ExportAsync("OPE_LineaMaritima", columns, enriched, filtrosTexto);
    }




}
