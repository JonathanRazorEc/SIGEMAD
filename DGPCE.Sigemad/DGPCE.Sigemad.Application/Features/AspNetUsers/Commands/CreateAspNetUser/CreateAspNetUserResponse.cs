using DGPCE.Sigemad.Application.Features.AspNetUsers.Vms;
using System.Collections;

namespace DGPCE.Sigemad.Application.Features.Incendios.Commands.CreateAspNetUser
{
    public class CreateAspNetUserResponse
    {
        public string Id { get; set; }
        public List<RoleVm> AssignedRoles { get; set; } = new();
    }
}
