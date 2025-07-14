using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePaises.Queries.GetOpePaisesList;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers.Ope.Administracion;

[Authorize]
[Route("api/v1/ope-paises")]
[ApiController]
public class OpePaisesController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpePaisesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de paises para la OPE")]
    public async Task<ActionResult<IReadOnlyList<OpePais>>> GetAll([FromQuery] GetOpePaisesListQuery query)
    {
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }

}
