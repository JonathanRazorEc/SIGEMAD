using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeAsistenciasSocialesTareasTipos.Queries.GetOpeAsistenciasSocialesTareasTiposList;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers.Ope.Datos;

[Authorize]
[Route("api/v1/ope-asistencias-sociales-tareas-tipos")]
[ApiController]
public class OpeAsistenciasSocialesTareasTiposController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpeAsistenciasSocialesTareasTiposController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de tipos de asistencias sociales tareas para la OPE")]
    public async Task<ActionResult<IReadOnlyList<OpeAsistenciaSocialTareaTipo>>> GetAll()
    {
        var query = new GetOpeAsistenciasSocialesTareasTiposListQuery { };
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }

}
