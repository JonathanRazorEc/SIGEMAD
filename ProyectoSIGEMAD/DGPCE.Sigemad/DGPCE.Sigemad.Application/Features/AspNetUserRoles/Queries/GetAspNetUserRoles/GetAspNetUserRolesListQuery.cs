using DGPCE.Sigemad.Application.Features.AspNetUsers.Vms;
using DGPCE.Sigemad.Application.Features.Incendios.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.AspNetUsersRoles.Queries.GetAspNetUsersRoles;

public class GetAspNetUsersRolesListQuery : IRequest<PaginationVm<AspNetUserRolVm>>
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
