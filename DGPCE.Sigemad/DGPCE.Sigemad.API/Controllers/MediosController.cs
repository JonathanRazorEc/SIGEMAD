using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.Medios.Quereis.GetMediosList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/medios")]
public class MediosController : ControllerBase
{
    private readonly IMediator _mediator;

    public MediosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de medios completo")]
    public async Task<ActionResult<IReadOnlyList<Medio>>> GetAll()
    {
        var query = new GetMediosListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }

}


