using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.TiposRegistros.Queries.GetTiposRegistrosList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/tipos-registros")]
public class TipoRegistrosController : ControllerBase
{
    private readonly IMediator _mediator;

    public TipoRegistrosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene los tipos de registros")]
    public async Task<ActionResult<IReadOnlyList<TipoRegistro>>> GetTiposRegistros()
    {
        var query = new GetTiposRegistrosListQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
