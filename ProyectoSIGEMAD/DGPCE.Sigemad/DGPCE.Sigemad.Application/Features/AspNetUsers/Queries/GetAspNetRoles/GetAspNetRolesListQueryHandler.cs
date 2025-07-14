using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.AspNetUsers.Vms;
using DGPCE.Sigemad.Application.Specifications.AspNetRoles;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.AspNetUsers.Queries.GetAspNetRoles
{
    public class GetAspNetRolesListQueryHandler
        : IRequestHandler<GetAspNetRolesListQuery, IEnumerable<RoleVm>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetAspNetRolesListQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleVm>> Handle(GetAspNetRolesListQuery request,
                                                      CancellationToken ct)
        {
            var roles = await _uow
            .Repository<AspNetRole>()
            .GetAllWithSpec(new AspNetRolesSpec());
            return _mapper.Map<IEnumerable<RoleVm>>(roles);
        }
    }
}