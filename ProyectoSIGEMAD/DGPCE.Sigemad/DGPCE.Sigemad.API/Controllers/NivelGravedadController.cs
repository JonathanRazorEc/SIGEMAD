using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.NivelesGravedad.Queries.GetNivelesGravedadList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class NivelGravedadController : ControllerBase
{
    private readonly IMediator _mediator;
    public NivelGravedadController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todos los niveles de gravedad")]
    public async Task<ActionResult<IReadOnlyList<NivelGravedad>>> GetAll()
    {
        var query = new GetNivelesGravedadListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }

}
