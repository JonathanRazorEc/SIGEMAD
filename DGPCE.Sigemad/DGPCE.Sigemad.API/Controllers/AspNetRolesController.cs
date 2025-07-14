using Microsoft.AspNetCore.Mvc;
using MediatR;

using DGPCE.Sigemad.Application.Features.AspNetUsers.Vms;
using DGPCE.Sigemad.Application.Features.AspNetUsers.Queries.GetAspNetRoles;

namespace DGPCE.Sigemad.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AspNetRolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AspNetRolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Devuelve la lista de roles disponibles (Id + Name).
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RoleVm>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RoleVm>>> GetRoles()
        {
            var query = new GetAspNetRolesListQuery();
            var roles = await _mediator.Send(query);
            return Ok(roles);
        }
    }
}
