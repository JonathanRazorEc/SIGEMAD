using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronterasIntervalosHorarios.Queries.GetOpeDatosFronterasIntervalosHorariosList;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers.Ope.Datos;

[Authorize]
[Route("api/v1/ope-datos-fronteras-intervalos-horarios")]
[ApiController]
public class OpeDatosFronterasIntervalosHorariosController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpeDatosFronterasIntervalosHorariosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de intervalos horarios de datos de fronteras para la OPE")]
    public async Task<ActionResult<IReadOnlyList<OpeDatoFronteraIntervaloHorario>>> GetAll()
    {
        var query = new GetOpeDatosFronterasIntervalosHorariosListQuery { };
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }

}
