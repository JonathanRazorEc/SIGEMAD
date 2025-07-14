using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.Provincias.Queries.GetProvinciasByIdCCAAList;
using DGPCE.Sigemad.Application.Features.Provincias.Queries.GetProvinciasList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class ProvinciasController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProvinciasController(IMediator mediator)
    {
        _mediator = mediator;

    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de las provincias")]
    public async Task<ActionResult<Provincia>> GetProvincias()
    {
        var query = new GetProvinciasListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }


    [HttpGet]
    [Route("{idCcaa}")]
    [Obsolete]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de las provincias para una determinada comunidad autonoma")]
    public async Task<ActionResult<IReadOnlyList<Provincia>>> GetProvinciasByIdCcaa(int idCcaa)
    {
        var query = new GetProvinciasByIdCCAAListQuery(idCcaa);
        var listado = await _mediator.Send(query);

        if (listado.Count == 0)
            return NotFound();

        return Ok(listado);
    }

}
