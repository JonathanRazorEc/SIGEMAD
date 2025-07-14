using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Dtos.SituacionesEquivalentes;
using DGPCE.Sigemad.Application.Features.SituacionesEquivalentes.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/situaciones-equivalentes")]
[ApiController]
public class SituacionesEquivalentesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SituacionesEquivalentesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todas las situaciones equivalentes.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Devuelve todas las situaciones equivalentes.", typeof(IReadOnlyList<SituacionEquivalenteDto>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "No autorizado.")]
    public async Task<ActionResult<IReadOnlyList<SituacionEquivalenteDto>>> GetAllSituacionesEquivalentes()
    {
        var query = new GetAllSituacionesEquivalentesQuery();
        var dtos = await _mediator.Send(query);
        return Ok(dtos);
    }
}
