using DGPCE.Sigemad.Application.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeLogs.Queries.GetOpeLogsList;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLogs.Vms;


namespace DGPCE.Sigemad.API.Controllers.Ope.Administracion;

[Authorize]
[Route("api/v1/ope-logs")]
[ApiController]
public class OpeLogsController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpeLogsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginationVm<OpeLog>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationVm<OpeLogVm>>> GetOpeLogs(
        [FromQuery] GetOpeLogsListQuery query)
    {
        var pagination = await _mediator.Send(query);
        return Ok(pagination);
    }
}
