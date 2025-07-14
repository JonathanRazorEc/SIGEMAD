using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.ProcedenciasDestinos.Queries.GetProcedenciasDestinosList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[ApiController]
[Route("/api/v1/procedencias-destinos")]
public class ProcedenciasDestinosController : Controller
{
    private readonly IMediator _mediator;

    public ProcedenciasDestinosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todas las procedencias y destinos")]
    public async Task<ActionResult<IReadOnlyList<ProcedenciaDestino>>> GetAll()
    {
        var query = new GetProcedenciasDestinosListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }
}
