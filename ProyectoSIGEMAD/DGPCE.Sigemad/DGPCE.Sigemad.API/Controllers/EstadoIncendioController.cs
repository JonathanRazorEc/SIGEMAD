using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.EstadosIncendio.Queries.GetEstadosIncendioList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/estados-incendios")]
public class EstadoIncendioController : ControllerBase
{
    private readonly IMediator _mediator;

    public EstadoIncendioController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todos los estados de incendio")]
    public async Task<ActionResult<IReadOnlyList<EstadoIncendio>>> GetAll()
    {
        var query = new GetEstadosIncendioListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }

}
