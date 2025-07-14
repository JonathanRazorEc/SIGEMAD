using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.TiposSistemasEmergencias.Queries.GetTiposSistemasEmergenciasList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;


[Authorize]
[Route("api/v1/tipos-sistemas-emergencia")]
[ApiController]
public class TiposFasesEmergenciasController : ControllerBase
{
    private readonly IMediator _mediator;

    public TiposFasesEmergenciasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todos los tipos de sistemas de emergencias")]
    public async Task<ActionResult<IReadOnlyList<TipoSistemaEmergencia>>> GetAll()
    {
        var query = new GetTiposSistemasEmergenciasListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }
}