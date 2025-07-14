using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;
using DGPCE.Sigemad.Application.Features.Capacidades.Queries.GetCapacidadesList;
using DGPCE.Sigemad.Application.Features.DestinosMedios.Queries.GetDestinoMediosList;
using DGPCE.Sigemad.Application.Features.MovilizacionMedios.Queries.GetTipoGestion;
using DGPCE.Sigemad.Application.Features.ProcedenciasMedios.Queries.GetProcedenciasMediosList;
using DGPCE.Sigemad.Application.Features.TiposAdministraciones.Queries.GetTiposAdministracionList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/movilizaciones-medios")]
[ApiController]
public class MovilizacionMediosController : ControllerBase
{
    private readonly IMediator _mediator;

    public MovilizacionMediosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("tipos-gestion")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene los tipos de gestion")]
    public async Task<ActionResult<IReadOnlyList<TipoGestionDto>>> GetTipoGestion([FromQuery] GetTipoGestionQuery query)
    {
        var tipoGestiones = await _mediator.Send(query);
        return Ok(tipoGestiones);
    }

    [HttpGet("procedencias-medios", Name = "GetAllProcedenciasMedios")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todas las procedencias de medios")]
    public async Task<ActionResult<IReadOnlyList<ProcedenciaMedioDto>>> GetAllProcedenciasMedios()
    {
        var dtos = await _mediator.Send(new GetProcedenciasMediosListQuery());
        return Ok(dtos);
    }

    [HttpGet("destinos-medios", Name = "GetAllDestinosMedios")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todos los destinos de medios")]
    public async Task<ActionResult<IReadOnlyList<DestinoMedioDto>>> GetAllDestinosMedios()
    {
        var dtos = await _mediator.Send(new GetDestinoMediosListQuery());
        return Ok(dtos);
    }

    [HttpGet("capacidades", Name = "GetAllCapacidades")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todas las capacidades de medios")]
    public async Task<ActionResult<IReadOnlyList<DestinoMedioDto>>> GetAllCapacidades()
    {
        var dtos = await _mediator.Send(new GetCapacidadesListQuery());
        return Ok(dtos);
    }

    [HttpGet("tipos-administracion", Name = "GetAllTiposAdministracion")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todos los tipos de administracion")]
    public async Task<ActionResult<IReadOnlyList<DestinoMedioDto>>> GetAllTiposAdministracion()
    {
        var dtos = await _mediator.Send(new GetTiposAdministracionListQuery());
        return Ok(dtos);
    }
}
