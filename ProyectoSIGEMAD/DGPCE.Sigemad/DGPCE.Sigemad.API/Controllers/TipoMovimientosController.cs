using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.TipoMovimientos.Quereis.GetTipoMovimientosList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/tipos-movimientos")]
public class TipoMovimientosController : ControllerBase
{
    private readonly IMediator _mediator;

    public TipoMovimientosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todos los tipos de movimientos")]
    public async Task<ActionResult<IReadOnlyList<TipoMovimiento>>> GetAll()
    {
        var query = new GetTipoMovimientosLisQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }

}



