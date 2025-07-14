using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeAsistenciasSocialesTipos.Queries.GetOpeAsistenciasSocialesTiposList;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers.Ope.Datos;

[Authorize]
[Route("api/v1/ope-asistencias-sociales-tipos")]
[ApiController]
public class OpeAsistenciasSocialesTiposController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpeAsistenciasSocialesTiposController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de tipos de asistencias sociales para la OPE")]
    public async Task<ActionResult<IReadOnlyList<OpeAsistenciaSocialTipo>>> GetAll()
    {
        var query = new GetOpeAsistenciasSocialesTiposListQuery { };
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }

}
