using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeAsistenciasSocialesSexos.Queries.GetOpeAsistenciasSocialesSexosList;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers.Ope.Datos;

[Authorize]
[Route("api/v1/ope-asistencias-sociales-sexos")]
[ApiController]
public class OpeAsistenciasSocialesSexosController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpeAsistenciasSocialesSexosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de sexos de asistencias sociales para la OPE")]
    public async Task<ActionResult<IReadOnlyList<OpeAsistenciaSocialOrganismoTipo>>> GetAll()
    {
        var query = new GetOpeAsistenciasSocialesSexosListQuery { };
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }

}
