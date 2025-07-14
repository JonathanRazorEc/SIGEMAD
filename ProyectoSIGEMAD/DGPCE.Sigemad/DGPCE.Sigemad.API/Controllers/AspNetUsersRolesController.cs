using Microsoft.AspNetCore.Mvc;
using MediatR;
using DGPCE.Sigemad.Application.Features.Auditoria.Queries.GetAuditoriaIncendio;
using DGPCE.Sigemad.Application.Features.Auditoria.Vms;
using DGPCE.Sigemad.Application.Features.Incendios.Queries.GetIncendiosList;
using DGPCE.Sigemad.Application.Features.Incendios.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Domain.Modelos;
using System.Net;
using DGPCE.Sigemad.Application.Features.AspNetUsers.Vms;
using DGPCE.Sigemad.Application.Features.AspNetUsers.Queries.GetAspNetUsers;
using DGPCE.Sigemad.Application.Features.Incendios.Commands.CreateIncendios;
using DGPCE.Sigemad.Application.Features.Incendios.Commands.CreateAspNetUser;
using DGPCE.Sigemad.Application.Features.Incendios.Commands.UpdateIncendios;
using DGPCE.Sigemad.Application.Features.Incendios.Commands.UpdateAspNetUser;
using DGPCE.Sigemad.Application.Features.Incendios.Commands.DeleteIncendios;
using DGPCE.Sigemad.Application.Features.AspNetUsers.Commands.DeleteAspNetUser;
using DGPCE.Sigemad.Application.Features.AspNetUsersRoles.Queries.GetAspNetUsersRoles;

namespace DGPCE.Sigemad.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AspNetUsersRolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AspNetUsersRolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginationVm<AspNetUserRol>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<AspNetUserRolVm>>> GetAspNetUsersRoles(
            [FromQuery] GetAspNetUsersRolesListQuery query)
        {
            var pagination = await _mediator.Send(query);
            return Ok(pagination);
        }
    }
}
