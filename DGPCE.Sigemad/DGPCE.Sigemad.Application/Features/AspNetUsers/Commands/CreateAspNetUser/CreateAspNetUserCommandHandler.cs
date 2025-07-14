using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.AspNetUsers.Vms;
using DGPCE.Sigemad.Application.Specifications.AspNetRoles;
using DGPCE.Sigemad.Application.Specifications.AspNetUsers;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.Incendios.Commands.CreateAspNetUser
{
    public class CreateAspNetUserCommandHandler :
        IRequestHandler<CreateAspNetUserCommand, CreateAspNetUserResponse>
    {
        private readonly ILogger<CreateAspNetUserCommandHandler> _logger;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateAspNetUserCommandHandler(
            ILogger<CreateAspNetUserCommandHandler> logger,
            IUnitOfWork uow,
            IMapper mapper)
        {
            _logger = logger;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<CreateAspNetUserResponse> Handle(
            CreateAspNetUserCommand request,
            CancellationToken ct)
        {
            // 0️⃣ Unicidad de usuario y e-mail
            var normalizedUser = request.UserName.ToUpperInvariant();
            if (await _uow.Repository<AspNetUser>()
                          .CountAsync(new UserByNormalizedUserNameSpec(normalizedUser)) > 0)
                throw new BadRequestException($"Ya existe el usuario '{request.UserName}'.");

            var normalizedMail = (request.Email ?? string.Empty).ToUpperInvariant();
            if (!string.IsNullOrWhiteSpace(request.Email) &&
                await _uow.Repository<AspNetUser>()
                          .CountAsync(new UserByNormalizedEmailSpec(normalizedMail)) > 0)
                throw new BadRequestException($"El correo '{request.Email}' ya está registrado.");

            if (request.Password != request.PasswordConfirmed)
                throw new BadRequestException("Las contraseñas no coinciden.");

            // 1️⃣  Creación de AspNetUser
            var userEntity = _mapper.Map<AspNetUser>(request);
            userEntity.Id = Guid.NewGuid().ToString();
            userEntity.UserName = request.UserName;
            userEntity.NormalizedUserName = normalizedUser;
            userEntity.Email = request.Email;
            userEntity.NormalizedEmail = normalizedMail;
            userEntity.PhoneNumber = request.PhoneNumber;
            userEntity.PasswordHash = new PasswordHasher<object?>()
                                                .HashPassword(null, request.Password);
            userEntity.AccessFailedCount = 0;
            userEntity.EmailConfirmed = false;
            userEntity.PhoneNumberConfirmed = false;
            userEntity.TwoFactorEnabled = false;
            userEntity.LockoutEnabled = false;
            userEntity.Activo = request.Activo;

            // Campos de dominio en AspNetUsers
            userEntity.Nombre = request.Nombre;
            userEntity.Apellidos = request.Apellidos;

            userEntity.SecurityStamp = Guid.NewGuid().ToString();
            userEntity.ConcurrencyStamp = Guid.NewGuid().ToString();

            _uow.Repository<AspNetUser>().AddEntity(userEntity);

            // 2️⃣  Creación de ApplicationUser “espejo”
            var appUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),   // ← ¡IMPORTANTE!
                IdentityId = userEntity.Id,
                Nombre = request.Nombre,
                Apellidos = request.Apellidos,
                Email = request.Email,
                Telefono = request.PhoneNumber
            };
            _uow.Repository<ApplicationUser>().AddEntity(appUser);

            // 3️⃣  Persistimos ambas inserciones en la misma transacción
            await _uow.Complete();

            // 4️⃣  Asignación de roles (si procede)
            var wantedRoleIds = (request.RoleIds ?? Enumerable.Empty<string>())
                                .Distinct(StringComparer.OrdinalIgnoreCase)
                                .ToList();

            var rolesAdded = new List<RoleVm>();
            if (wantedRoleIds.Any())
            {
                var existingRoles = await _uow.Repository<AspNetRole>()
                                           .GetAllWithSpec(new AspNetRolesByIdsSpec(wantedRoleIds));

                var userRoles = existingRoles
                                .Select(r => new AspNetUserRol
                                {
                                    UserId = userEntity.Id,
                                    RoleId = r.Id
                                })
                                .ToList();

                if (userRoles.Any())
                {
                    await _uow.Repository<AspNetUserRol>()
                              .AddRangeAsync(userRoles, ct);
                    await _uow.Complete();

                    rolesAdded = existingRoles
                                 .Select(r => new RoleVm { Id = r.Id, Name = r.Name })
                                 .ToList();
                }

                var missing = wantedRoleIds
                              .Except(existingRoles.Select(r => r.Id),
                                      StringComparer.OrdinalIgnoreCase)
                              .ToList();
                if (missing.Any())
                    _logger.LogWarning(
                        "Se ignoraron RoleIds inexistentes: {Missing}",
                        string.Join(", ", missing));
            }

            // 5️⃣  Respuesta
            return new CreateAspNetUserResponse
            {
                Id = userEntity.Id,
                AssignedRoles = rolesAdded
            };
        }
    }
}
