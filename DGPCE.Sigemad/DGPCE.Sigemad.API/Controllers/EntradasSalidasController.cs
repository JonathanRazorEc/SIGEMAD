using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.EntradasSalidas.Quereis.GetEntradaSalidaList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/entradas-salidas")]
[ApiController]
public class EntradasSalidasController : ControllerBase
{
    private readonly IMediator _mediator;

    public EntradasSalidasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todas la lista general de tipos de entrada y salida")]
    public async Task<ActionResult<IReadOnlyList<ClaseSuceso>>> GetAll()
    {
        var query = new GetEntradaSalidaListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }
}