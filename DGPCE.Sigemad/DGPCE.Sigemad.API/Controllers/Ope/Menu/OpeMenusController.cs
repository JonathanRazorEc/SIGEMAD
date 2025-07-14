using DGPCE.Sigemad.Application.Features.Ope.Menus.Queries.GetOpeMenusList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/ope-menus")]
[ApiController]
public class OpeMenusController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpeMenusController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Summary = "Obtiene el menu de la OPE")]
    public async Task<IActionResult> GetOpeMenus()
    {
        var query = new GetOpeMenusListQuery();
        var menu = await _mediator.Send(query);
        return Ok(menu);
    }
}
