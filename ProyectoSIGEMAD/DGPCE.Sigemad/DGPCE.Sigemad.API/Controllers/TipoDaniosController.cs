using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.TipoDanios.Queries.GetTipoDaniosList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/tipos-danios")]
[ApiController]
public class TipoDaniosController : ControllerBase
{
    private readonly IMediator _mediator;

    public TipoDaniosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene toda la lista de tipo de daños")]
    public async Task<ActionResult<IReadOnlyList<TipoDanio>>> GetAll()
    {
        var query = new GetTipoDaniosListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }
}
