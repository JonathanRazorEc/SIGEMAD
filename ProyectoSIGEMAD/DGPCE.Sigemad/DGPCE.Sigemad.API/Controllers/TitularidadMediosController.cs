using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.TitularidadMedios.Quereis.GetTitularidadMediosList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/titular-medios")]
[ApiController]
public class TitularidadMediosController : ControllerBase
{
    private readonly IMediator _mediator;

    public TitularidadMediosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene la lista general de titular de medios")]
    public async Task<ActionResult<IReadOnlyList<CaracterMedio>>> GetAll()
    {
        var query = new GetTitularidadMediosListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }
}