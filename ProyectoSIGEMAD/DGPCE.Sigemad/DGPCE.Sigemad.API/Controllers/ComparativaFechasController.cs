using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.ComparativaFechas.Quereis.GetComparativaFechasList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/comparativa-fechas")]
public class ComparativaFechasController : ControllerBase
{
    private readonly IMediator _mediator;

    public ComparativaFechasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todos las opciones de comparación entre fechas")]
    public async Task<ActionResult<IReadOnlyList<ComparativaFecha>>> GetAll()
    {
        var query = new GetComparativaFechasListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }

}

