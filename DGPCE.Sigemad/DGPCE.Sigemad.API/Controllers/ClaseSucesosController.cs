using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.ClasesSucesos.Quereis.GetClaseSucesosList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/clase-sucesos")]
[ApiController]
public class ClaseSucesosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClaseSucesosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todas las clases de sucesos")]
    public async Task<ActionResult<IReadOnlyList<ClaseSuceso>>> GetAll()
    {
        var query = new GetClaseSucesosListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }
}


