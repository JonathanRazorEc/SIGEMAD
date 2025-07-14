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

namespace DGPCE.Sigemad.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AspNetUsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AspNetUsersController(IMediator mediator)
        {
            _mediator = mediator;
        }




        [HttpGet]
        [ProducesResponseType(typeof(PaginationVm<AspNetUser>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<AspNetUserVm>>> GetAspNetUsers(
            [FromQuery] GetAspNetUsersListQuery query)
        {
            var pagination = await _mediator.Send(query);
            return Ok(pagination);
        }

        [HttpPost(Name = "CreateAspNetUser")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<CreateAspNetUserResponse>> Create([FromBody] CreateAspNetUserCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(new CreateAspNetUserResponse { Id = response.Id });
        }

        [HttpPut(Name = "UpdateAspNetUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update([FromBody] UpdateAspNetUserCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteDeleteAspNetUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(string id)
        {
            var command = new DeleteAspNetUserCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
