using DGPCE.Sigemad.Application.Features.Incendios.Commands.UpdateAspNetUser;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.AspNetUsers.Commands.UpdateAspNetUser
{
    public class UpdateAspNetUserCommandValidator : AbstractValidator<UpdateAspNetUserCommand>
    {
    public UpdateAspNetUserCommandValidator()
        {
            // 0️⃣ Id
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El identificador de usuario es obligatorio.");

            // 1️⃣ UserName (opcional pero si viene, aplicar mismas reglas que en Create)
            When(x => !string.IsNullOrWhiteSpace(x.UserName), () =>
            {
                RuleFor(x => x.UserName)
                    .MinimumLength(4).WithMessage("El nombre de usuario debe tener al menos 4 caracteres.")
                    .MaximumLength(50).WithMessage("El nombre de usuario no puede exceder 50 caracteres.");
            });

            // 2️⃣ Email (opcional pero si viene, mismo formato)
            When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
            {
                RuleFor(x => x.Email)
                    .EmailAddress().WithMessage("El correo no tiene un formato válido.")
                    .MaximumLength(100).WithMessage("El correo no puede exceder 100 caracteres.");
            });

            // 3️⃣ PhoneNumber (opcional)
            When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber), () =>
            {
                RuleFor(x => x.PhoneNumber)
                    .Matches(@"^\+?[0-9]{7,15}$")
                    .WithMessage("El teléfono no tiene un formato válido.");
            });

            // 4️⃣ Password y confirmación (solo si uno de los dos viene)
            When(x => !string.IsNullOrWhiteSpace(x.Password)
                   || !string.IsNullOrWhiteSpace(x.PasswordConfirmed), () =>
                   {
                       RuleFor(x => x.Password)
                       .NotEmpty().WithMessage("La contraseña es obligatoria.")
                       .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");

                       RuleFor(x => x.PasswordConfirmed)
                       .NotEmpty().WithMessage("La confirmación de contraseña es obligatoria.");

                       RuleFor(x => x)
                       .Must(x => x.Password == x.PasswordConfirmed)
                       .WithMessage("Las contraseñas no coinciden.");
                   });

            // 5️⃣ Nombre y Apellidos (opcionales)
            When(x => !string.IsNullOrWhiteSpace(x.Nombre), () =>
            {
                RuleFor(x => x.Nombre)
                    .MaximumLength(50)
                    .WithMessage("El nombre no puede exceder 50 caracteres.");
            });

            When(x => !string.IsNullOrWhiteSpace(x.Apellidos), () =>
            {
                RuleFor(x => x.Apellidos)
                    .MaximumLength(100)
                    .WithMessage("Los apellidos no pueden exceder 100 caracteres.");
            });

            // 6️⃣ Roles
            RuleFor(x => x.RoleIds)
                .NotNull().WithMessage("Debe especificar la lista de roles.")
                .Must(r => r.Any()).WithMessage("Debe asignar al menos un rol.");

            // 7️⃣ Activo
            // bool siempre tiene valor; no requiere regla
        }
    }
}
