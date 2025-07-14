using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasDescansoTipos.Queries.GetOpeAreasDescansoTiposList;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers.Ope.Administracion;

[Authorize]
[Route("api/v1/ope-areas-descanso-tipos")]
[ApiController]
public class OpeAreasDescansoTiposController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpeAreasDescansoTiposController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de tipos de áreas de descanso para la OPE")]
    public async Task<ActionResult<IReadOnlyList<OpeAreaDescansoTipo>>> GetAll()
    {
        var query = new GetOpeAreasDescansoTiposListQuery { };
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }

}
