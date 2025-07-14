using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.AspNetUsersRoles.Queries.GetAspNetUsersRoles;
using DGPCE.Sigemad.Application.Features.AspNetUsers.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Auditoria;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using DGPCE.Sigemad.Application.Specifications.AspNetUsers;

namespace DGPCE.Sigemad.Application.Features.AspNetUsersRoles.Queries
{
    public class GetAspNetUsersRolesListQueryHandler : IRequestHandler<GetAspNetUsersRolesListQuery, PaginationVm<AspNetUserRolVm>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAspNetUsersRolesListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationVm<AspNetUserRolVm>> Handle(GetAspNetUsersRolesListQuery request, CancellationToken cancellationToken)
        {
            // 1️⃣ Spec de paginación
            var spec = new AspNetUsersRolesSpec(
                skip: (request.PageIndex - 1) * request.PageSize,
                take: request.PageSize);

            // 2️⃣ Traer la página
            var pagedItems = await _unitOfWork
                .Repository<AspNetUserRol>()
                .GetAllWithSpec(spec);

            var roleVMs = _mapper.Map<List<AspNetUserRolVm>>(pagedItems);

            // 3️⃣ Total para la paginación
            var totalItems = await _unitOfWork
                .Repository<AspNetUserRol>()
                .CountAsync(new AspNetUsersRolesSpec());   // ← ctor sin parámetros

            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

            return new PaginationVm<AspNetUserRolVm>
            {
                Count = totalItems,
                Data = roleVMs,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

        }
    }
}