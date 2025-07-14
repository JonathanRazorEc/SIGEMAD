using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.TipoIntervencionMedios.Quereis.GetTipoIntervencionMediosList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/tipo-intervencion-medios")]
[ApiController]
public class TipoIntervencionMediosController : ControllerBase
{
    private readonly IMediator _mediator;

    public TipoIntervencionMediosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene la lista general de tipos de impactos")]
    public async Task<ActionResult<IReadOnlyList<CaracterMedio>>> GetAll()
    {
        var query = new GetTipoIntervencionMediosListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }
}