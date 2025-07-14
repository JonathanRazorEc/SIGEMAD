using DGPCE.Sigemad.Application.Features.AspNetUsers.Vms;
using DGPCE.Sigemad.Application.Features.Incendios.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.AspNetUsers.Queries.GetAspNetUsers;

public class GetAspNetUsersListQuery : IRequest<PaginationVm<AspNetUserVm>>
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
