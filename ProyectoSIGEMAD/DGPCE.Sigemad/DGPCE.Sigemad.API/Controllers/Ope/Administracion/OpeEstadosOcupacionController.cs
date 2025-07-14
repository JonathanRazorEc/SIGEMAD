using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeEstadosOcupacion.Queries.GetOpeEstadosOcupacionList;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers.Ope.Administracion;

[Authorize]
[Route("api/v1/ope-estados-ocupacion")]
[ApiController]
public class OpeEstadosOcupacionController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpeEstadosOcupacionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de estados de ocupación para la OPE")]
    public async Task<ActionResult<IReadOnlyList<OpeEstadoOcupacion>>> GetAll()
    {
        var query = new GetOpeEstadosOcupacionListQuery { };
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }

}
