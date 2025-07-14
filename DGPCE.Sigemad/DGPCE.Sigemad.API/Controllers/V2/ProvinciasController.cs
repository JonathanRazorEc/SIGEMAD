using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.Provincias.Queries.GetProvinciasBySearch;
using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers.v2;

[Authorize]
[ApiController]
[Route("api/v1/v2-[controller]")]
public class ProvinciasController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProvinciasController(IMediator mediator)
    {
        _mediator = mediator;

    }

    [HttpGet]
    [Route("Busqueda")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de las provincias para una determinada comunidad autonoma o por busqueda de texto, en caso contrario devuelve todos las provincias")]
    public async Task<ActionResult<IReadOnlyList<ProvinciasConCCAAVm>>> GetProvinciasByIdCcaa([FromQuery] string? busqueda = null, [FromQuery] int? idCCaa = null)
    {
        var query = new GetProvinciasBySearchListQuery(idCCaa, busqueda);
        var listado = await _mediator.Send(query);

        return Ok(listado);
    }

}
