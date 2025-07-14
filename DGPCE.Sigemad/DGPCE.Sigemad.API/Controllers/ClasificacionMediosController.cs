using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.ClasificacionMedios.Quereis.GetClasificacionMediosList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/clasificacion-medios")]
[ApiController]
public class ClasificacionMediosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClasificacionMediosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene la lista general de clasificación de medios")]
    public async Task<ActionResult<IReadOnlyList<CaracterMedio>>> GetAll()
    {
        var query = new GetClasificacionMediosListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }
}