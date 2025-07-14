using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Dtos.CaracterMedios;
using DGPCE.Sigemad.Application.Features.CaracterMedios.Quereis.GetCaracterMediosList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/caracter-medios")]
[ApiController]
public class CaracterMediosController : ControllerBase
{
    private readonly IMediator _mediator;

    public CaracterMediosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene la lista general de caracter de medios")]
    public async Task<ActionResult<IReadOnlyList<CaracterMedioDto>>> GetAll()
    {
        var query = new GetCaracterMediosListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }
}

