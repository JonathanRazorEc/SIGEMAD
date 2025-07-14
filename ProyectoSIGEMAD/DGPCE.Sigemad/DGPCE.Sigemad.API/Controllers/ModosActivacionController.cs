using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.ModosActivacion.Queries.GetModosActivacionList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/modos-activacion")]
[ApiController]
public class ModosActivacionController : ControllerBase
{
    private readonly IMediator _mediator;

    public ModosActivacionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todos los modos de activacion")]
    public async Task<ActionResult<IReadOnlyList<ModoActivacion>>> GetAll()
    {
        var query = new GetModosActivacionListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }
}