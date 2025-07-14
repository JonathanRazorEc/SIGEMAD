using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.AspNetUsers.Queries.GetAspNetUsers;
using DGPCE.Sigemad.Application.Features.AspNetUsers.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Auditoria;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using DGPCE.Sigemad.Application.Specifications.AspNetRoles;


namespace DGPCE.Sigemad.Application.Features.AspNetUsers.Queries
{
    public class GetAspNetUsersListQueryHandler : IRequestHandler<GetAspNetUsersListQuery, PaginationVm<AspNetUserVm>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAspNetUsersListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationVm<AspNetUserVm>> Handle(GetAspNetUsersListQuery request, CancellationToken cancellationToken)
        {
            // 1️⃣ Traer página de usuarios
            var spec = new AspNetUsersSpec(
                skip: (request.PageIndex - 1) * request.PageSize,
                take: request.PageSize);
            var pagedUsers = await _unitOfWork.Repository<AspNetUser>().GetAllWithSpec(spec);

            // 2️⃣ Mapear a VM
            var userVMs = _mapper.Map<List<AspNetUserVm>>(pagedUsers);

            // 3️⃣ Total para paginación
            var totalItems = await _unitOfWork.Repository<AspNetUser>().CountAsync(new AspNetUsersSpec());
            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

            // 4️⃣ Roles asignados a estos usuarios
            var userIds = pagedUsers.Select(u => u.Id);
            var userRoles = await _unitOfWork
                .Repository<AspNetUserRol>()
                .GetAllWithSpec(new AspNetUsersRolesByUserIdsSpec(userIds));

            // 5️⃣ Agrupar por UserId → List<RoleId>
            var rolesByUser = userRoles
            .GroupBy(r => r.UserId)
            .ToDictionary(
               g => g.Key,
               g => g.Select(r => r.RoleId!).ToList()
            );

            var allRoleIds = rolesByUser.Values.SelectMany(x => x).Distinct().ToList();

            // 7️⃣ Consultar la tabla AspNetRoles una sola vez  ➜  { Id, Name }
            var roleEntities = await _unitOfWork
                    .Repository<AspNetRole>()
                    .GetAllWithSpec(new AspNetRolesByIdsSpec(allRoleIds));

            var roleNamesById = roleEntities
                    .ToDictionary(r => r.Id, r => r.Name!);

            // 8️⃣ Devolver paginación
            return new PaginationVm<AspNetUserVm>
            {
                Count = totalItems,
                Data = userVMs,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
        }
    }
}