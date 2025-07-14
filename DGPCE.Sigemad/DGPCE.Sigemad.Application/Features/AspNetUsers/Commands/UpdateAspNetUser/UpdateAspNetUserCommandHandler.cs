// UpdateAspNetUserCommandHandler.cs
using AutoMapper;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Incendios.Commands.UpdateAspNetUser;
using DGPCE.Sigemad.Application.Specifications.AspNetUsers;
using DGPCE.Sigemad.Application.Specifications.AspNetUsersRoles;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Incendios.Commands.UpdateAspNetUser
{

    

    public class UpdateAspNetUserCommandHandler : IRequestHandler<UpdateAspNetUserCommand>
    {
        private readonly ILogger<UpdateAspNetUserCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateAspNetUserCommandHandler(
            ILogger<UpdateAspNetUserCommandHandler> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateAspNetUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("BEGIN UpdateAspNetUser");

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // 1️⃣ Recupero la entidad
            var user = await _unitOfWork.Repository<AspNetUser>()
                .GetByIdWithSpec(new AspNetUserByIdSpec(request.Id));

            if (user == null)
            {
                _logger.LogWarning("Usuario no encontrado Id={UserId}", request.Id);
                throw new NotFoundException(nameof(AspNetUser), request.Id);
            }

            // 2️⃣ Validaciones de unicidad si cambian UserName o Email
            if (!string.IsNullOrWhiteSpace(request.UserName)
                && !request.UserName.Equals(user.UserName, StringComparison.OrdinalIgnoreCase))
            {
                var countName = await _unitOfWork.Repository<AspNetUser>()
                    .CountAsync(new UserByNormalizedUserNameSpec(request.UserName.ToUpperInvariant()));
                if (countName > 0)
                    throw new BadRequestException($"Ya existe un usuario con el nombre '{request.UserName}'.");
                user.UserName = request.UserName;
                user.NormalizedUserName = request.UserName.ToUpperInvariant();
            }

            if (!string.IsNullOrWhiteSpace(request.Email)
                && !request.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase))
            {
                var countEmail = await _unitOfWork.Repository<AspNetUser>()
                    .CountAsync(new UserByNormalizedEmailSpec(request.Email.ToUpperInvariant()));
                if (countEmail > 0)
                    throw new BadRequestException($"El correo '{request.Email}' ya está en uso.");
                user.Email = request.Email;
                user.NormalizedEmail = request.Email.ToUpperInvariant();
            }

            // 3️⃣ Mapeo del resto de campos (telefono, nombre, apellidos…).
            _mapper.Map(request, user, typeof(UpdateAspNetUserCommand), typeof(AspNetUser));

            user.Activo = request.Activo;
            // 4️⃣ Si el password viene y coincide con el confirmado, lo hasheo
            if (!string.IsNullOrWhiteSpace(request.Password) || !string.IsNullOrWhiteSpace(request.PasswordConfirmed))
            {
                if (request.Password != request.PasswordConfirmed)
                    throw new BadRequestException("Las contraseñas deben coincidir.");
                var hasher = new PasswordHasher<object>();
                user.PasswordHash = hasher.HashPassword(null, request.Password!);
            }

            // 5️⃣ Actualizo la entidad
            _unitOfWork.Repository<AspNetUser>().UpdateEntity(user);
            _logger.LogWarning("Guardando Activo = {Activo}", user.Activo);
            await _unitOfWork.Complete();

            // 6️⃣ Ahora gestiono sus roles: primero borro los que ya tenía
            var existingRoles = await _unitOfWork.Repository<AspNetUserRol>()
                .GetAllWithSpec(new AspNetUserRolesByUserSpec(request.Id));

            if (existingRoles.Any())
            {
                var repo = _unitOfWork.Repository<AspNetUserRol>();
                foreach (var rol in existingRoles)
                    repo.DeleteEntity(rol);
                await _unitOfWork.Complete();
            }

            // 7️⃣ Añado los que vienen en el request
            var newRoles = request.RoleIds
                .Select(rid => new AspNetUserRol { UserId = request.Id, RoleId = rid })
                .ToList();

            if (newRoles.Any())
            {
                await _unitOfWork.Repository<AspNetUserRol>().AddRangeAsync(newRoles, cancellationToken);
                try
                {
                    await _unitOfWork.Complete();
                }
                catch (DbUpdateException dbEx)
                {
                    var sqlEx = dbEx.GetBaseException() as SqlException;
                    if (sqlEx != null && sqlEx.Number == 2627)
                        throw new BadRequestException("Ya existe esa asignación de rol para el usuario.", dbEx);
                    _logger.LogError(dbEx, "Error al asignar roles a usuario Id={UserId}", request.Id);
                    throw new Exception($"Error al guardar roles en BD: {dbEx.GetBaseException().Message}", dbEx);
                }
            }

            _logger.LogInformation("END UpdateAspNetUser Id={UserId}", request.Id);
            return Unit.Value;
        }
    }
}
