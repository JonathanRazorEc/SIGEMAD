using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.EstadosSucesos.Queries.GetEstadosSucesosList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[ApiController]
[Route("/api/v1/estados-sucesos")]
public class EstadosSucesosController : Controller
{
    private readonly IMediator _mediator;

    public EstadosSucesosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todos los estados de sucesos")]
    public async Task<ActionResult<IReadOnlyList<EstadoSuceso>>> GetAll()
    {
        var query = new GetEstadosSucesosListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }
}
