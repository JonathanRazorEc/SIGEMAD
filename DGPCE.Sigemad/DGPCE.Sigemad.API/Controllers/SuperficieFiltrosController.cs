using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.Superficies.Queries.GetSuperficiesList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/superficies-filtros")]
[ApiController]
public class SuperficieFiltrosController : ControllerBase
{
    private readonly IMediator _mediator;

    public SuperficieFiltrosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todos los filtros de superficie")]
    public async Task<ActionResult<IReadOnlyList<SuperficieFiltro>>> GetAll()
    {
        var query = new GetSuperficieListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }
}
