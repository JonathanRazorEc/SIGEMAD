using MediatR;

namespace DGPCE.Sigemad.Application.Features.AspNetUsers.Commands.DeleteAspNetUser
{
    public class DeleteAspNetUserCommand : IRequest
    {
        public string Id { get; set; }
    }
}
