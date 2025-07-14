using DGPCE.Sigemad.Application.Features.AspNetUsers.Vms;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.AspNetUsers.Queries.GetAspNetRoles
{
    public record GetAspNetRolesListQuery : IRequest<IEnumerable<RoleVm>>;
}
