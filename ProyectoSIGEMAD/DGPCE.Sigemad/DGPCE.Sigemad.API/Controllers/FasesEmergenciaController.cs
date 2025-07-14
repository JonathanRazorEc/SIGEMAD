using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.Fases.Queries.GetFasesEmergenciaListByIdPlanEmergencia;
using DGPCE.Sigemad.Application.Features.Fases.Vms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[ApiController]
[Route("/api/v1/fases-emergencia")]
public class FasesEmergenciaController : Controller
{
    private readonly IMediator _mediator;

    public FasesEmergenciaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet()]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de las fases de emergencia completas o filtradas por el plan de emergencia")]
    public async Task<ActionResult<IReadOnlyList<FaseEmergenciaVm>>> GetFasesEmergenciasByIdPlanEmergencia([FromQuery] int? idPlanEmergencia)
    {
        var query = new GetFasesEmergenciaByIdPlanEmergenciaListQuery(idPlanEmergencia);
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }
}