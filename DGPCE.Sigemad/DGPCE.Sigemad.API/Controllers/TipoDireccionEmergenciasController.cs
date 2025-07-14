using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.TipoDireccionEmergencias.Quereis.GetTipoDireccionEmergenciasList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/tipos-direcciones-emergencias")]
[ApiController]
public class TipoDireccionEmergenciasController : ControllerBase
{
    private readonly IMediator _mediator;

    public TipoDireccionEmergenciasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todas la lista general de TipoDireccionEmergencia")]
    public async Task<ActionResult<IReadOnlyList<TipoDireccionEmergencia>>> GetAll()
    {
        var query = new GetTipoDireccionEmergenciasListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }
}
