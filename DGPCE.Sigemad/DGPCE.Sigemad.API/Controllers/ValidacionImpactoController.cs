using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.ValidacionesImpacto.Queries.GetCamposImpactosById;
using DGPCE.Sigemad.Application.Features.ValidacionesImpacto.Vms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1")]
public class ValidacionImpactoController : ControllerBase
{
    private readonly IMediator _mediator;

    public ValidacionImpactoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("campos-impactos/{idImpacto}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene los campos de validación de un impacto clasificado")]
    public async Task<ActionResult<IReadOnlyList<ValidacionImpactoClasificadoVm>>> GetCamposImpactos(int idImpacto)
    {
        var query = new GetCamposImpactosByIdQuery(idImpacto);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
