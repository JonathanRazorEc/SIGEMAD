using DGPCE.Sigemad.Application.Features.Utilitarios.Queries.GetTestResponseTime;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/[controller]")]
[ApiController]
public class UtilitariosController : ControllerBase
{
    private readonly IMediator _mediator;

    public UtilitariosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("delay")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetDelay([FromQuery] GetTestResponseTimeQuery query)
    {
        var response = await _mediator.Send(query);
        return Ok(response);
    }
}
