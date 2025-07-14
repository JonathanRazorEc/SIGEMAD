using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.Municipios.Queries.GetMunicipiosBySearch;
using DGPCE.Sigemad.Application.Features.Municipios.Vms;
using DGPCE.Sigemad.Application.Features.MunicipiosExtranjeros.Queries.GetMunicipiosExtranjerosBySearch;
using DGPCE.Sigemad.Application.Features.MunicipiosExtranjeros.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers.v2;

[Authorize]
[ApiController]
[Route("api/v1/v2-[controller]")]
public class MunicipiosController : ControllerBase
{
    private readonly IMediator _mediator;
    public MunicipiosController(IMediator mediator)
    {
        _mediator = mediator;

    }

    [HttpGet]
    [Route("Busqueda")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de los municipios para una determinada provincia o por busqueda de texto")]
    public async Task<ActionResult<IReadOnlyList<MunicipiosConProvinciaVM>>> GetMunicipiosBySearch([FromQuery] string? busqueda = null, [FromQuery] int? idProvincia = null)
    {
        var query = new GetMunicipiosBySearchListQuery(idProvincia, busqueda);
        var listado = await _mediator.Send(query);

        return Ok(listado);
    }

    [HttpGet("busqueda/extranjeros")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de municipios de un pais extranjero")]
    public async Task<ActionResult<IReadOnlyList<MunicipioExtranjeroVm>>> GetMunicipioExtranjero([FromQuery] string? busqueda = null, [FromQuery] int? idDistrito = null, [FromQuery] int? idPais = null)
    {
        var query = new GetMunicipiosExtranjerosBySearchListQuery(idDistrito, idPais, busqueda);
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }
}
