using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.Territorios.Queries.GetTerritoriosCrearList;
using DGPCE.Sigemad.Application.Features.Territorios.Queries.GetTerritoriosList;
using DGPCE.Sigemad.Application.Features.Territorios.Vms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/")]
[ApiController]
public class TerritoriosController : ControllerBase
{
    private readonly IMediator _mediator;

    public TerritoriosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("territorios")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todos los tipos de territorios")]
    public async Task<ActionResult<IReadOnlyList<TerritorioVm>>> GetTerritorio()
    {
        var query = new GetTerritoriosListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }

    [HttpGet("territorios-crear")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todos los tipos de territorios para pantalla Crear incendio")]
    public async Task<ActionResult<IReadOnlyList<TerritorioVm>>> GetTerritorioCrear()
    {
        var query = new GetTerritorioCrearListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }

}