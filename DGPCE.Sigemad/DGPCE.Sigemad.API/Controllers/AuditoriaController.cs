using Microsoft.AspNetCore.Mvc;
using MediatR;
using DGPCE.Sigemad.Application.Features.Auditoria.Queries.GetAuditoriaIncendio;
using DGPCE.Sigemad.Application.Features.Auditoria.Vms;

namespace DGPCE.Sigemad.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuditoriaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuditoriaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("incendio/{idIncendio}")]
        public async Task<ActionResult<AuditoriaIncendioVm>> GetAuditoriaIncendio(int idIncendio)
        {
            var query = new GetAuditoriaIncendioQuery(idIncendio);
            var result = await _mediator.Send(query);
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}
