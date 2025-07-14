using DGPCE.Sigemad.Application.Dtos.Evoluciones;
using DGPCE.Sigemad.Application.Dtos.RegistrosAnteriores;
using DGPCE.Sigemad.Application.Features.RegistrosAnteriores.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace DGPCE.Sigemad.API.Controllers
{

    [Authorize]
    [Route("api/v1/registros-anteriores")]
    [ApiController]
    public class RegistrosAnterioresController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RegistrosAnterioresController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        public async Task<ActionResult<RegistrosAnterioresDto>> GetRegistro([FromQuery] int idSuceso, int? idRegistro)
        {
            var query = new GetRegistrosAnterioresByIdSucesoListQuery
            {
                IdSuceso = idSuceso,
                IdRegistro = idRegistro
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

}

}