using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.TipoDocumentos;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers
{

    [Authorize]
    [Route("api/v1/tipo-documentos")]
    [ApiController]
    public class TipoDocumentosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TipoDocumentosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todos los tipos de documentos")]
        public async Task<ActionResult<IReadOnlyList<TipoDocumento>>> GetAll()
        {
            var query = new GetTipoDocumentosListQuery();
            var listado = await _mediator.Send(query);
            return Ok(listado);
        }
    }

}
