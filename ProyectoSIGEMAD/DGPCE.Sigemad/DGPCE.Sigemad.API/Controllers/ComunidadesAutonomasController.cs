using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.CCAA.Queries.GetComunidadesAutonomasList;
using DGPCE.Sigemad.Application.Features.CCAA.Vms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class ComunidadesAutonomasController : ControllerBase
{
    private readonly IMediator _mediator;
    public ComunidadesAutonomasController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
        Tags = new[] { SwaggerTags.Maestros },
        Summary = "Obtiene el listado de las comunidades autonomas y sus provincias")]
    public async Task<ActionResult<IReadOnlyList<ComunidadesAutonomasVm>>> GetComunidadesAutonomas()
    {
        var query = new GetComunidadesAutonomasListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }

}
