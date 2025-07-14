using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.CCAA.Queries.GetCCAABySearch;
using DGPCE.Sigemad.Application.Features.CCAA.Vms;
using DGPCE.Sigemad.Application.Features.Paises.Queries.GetPaisesBySearch;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers.v2;

[Authorize]
[Route("api/v1/v2-paises")]
[ApiController]
public class PaisesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaisesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [HttpGet("busqueda")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de paises y permite filtrar por busqueda")]
    public async Task<ActionResult<IReadOnlyList<Pais>>> GetAll([FromQuery] string? busqueda = null,[FromQuery] bool mostrarNacional = false)
    {
        var query = new GetPaisesBySearchQuery { MostrarNacional = mostrarNacional,busqueda = busqueda };
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }

    [HttpGet("busqueda/comunidades")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de comunidades de un pais por busqueda o por id de pais")]
    public async Task<ActionResult<IReadOnlyList<ComunidadesAutonomasConPaisVm>>> GetComunidades([FromQuery] string? busqueda = null, [FromQuery] int? idPais = null)
    {
        var query = new GetCCAABySearchListQuery(idPais, busqueda);
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }
}
