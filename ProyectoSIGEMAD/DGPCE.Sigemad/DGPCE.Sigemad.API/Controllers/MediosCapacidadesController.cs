using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Dtos.IntervencionMedios;
using DGPCE.Sigemad.Application.Features.MediosCapacidades.Queries.GetMediosCapacidadesByIdTipoCapacidad;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/Medios-capacidades")]
public class MediosCapacidadesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MediosCapacidadesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{idTipoCapacidad}")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todos los MediosCapacidad por id TipoCapacidad")]
    public async Task<ActionResult<IReadOnlyList<MediosCapacidadDto>>> GetByIdTipoCapacidad(int idTipoCapacidad)
    {
        var query = new GetMediosCapacidadesByIdTipoCapacidadListQuery(idTipoCapacidad);
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }

}