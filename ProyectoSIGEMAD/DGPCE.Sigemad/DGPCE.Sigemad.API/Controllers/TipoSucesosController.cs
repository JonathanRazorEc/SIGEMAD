using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.TipoSucesos.Queries.GetTipoSucesosList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/tipo-sucesos")]
[ApiController]
public class TipoSucesosController : ControllerBase
{
    private readonly IMediator _mediator;

    public TipoSucesosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todos los tipos de sucesos")]
    public async Task<ActionResult<IReadOnlyList<TipoSuceso>>> GetAll()
    {
        var query = new GetTipoSucesosListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }
}
