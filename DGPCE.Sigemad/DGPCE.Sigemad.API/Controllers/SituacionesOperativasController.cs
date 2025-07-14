using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.SituacionesOperativas.Queries.GetSituacionesOperativasList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/situaciones-operativas")]
[ApiController]
public class SituacionesOperativasController : ControllerBase
{
    private readonly IMediator _mediator;
    public SituacionesOperativasController(IMediator mediator)
    {
        _mediator = mediator;

    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de las situaciones operativas")]
    public async Task<ActionResult<IReadOnlyList<SituacionOperativa>>> GetAll()
    {
        var query = new GetSituacionesOperativasListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }

}
