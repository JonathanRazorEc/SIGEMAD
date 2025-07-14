using DGPCE.Sigemad.Application.Features.Incendios.Commands.CreateAspNetUser;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.AspNetUsers.Commands.CreateAspNetUser
{
    public class CreateAspNetUserCommandValidator: AbstractValidator<CreateAspNetUserCommand>
    {
        public CreateAspNetUserCommandValidator()
        {
            // 1️⃣ UserName
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("El nombre de usuario es obligatorio.")
                .MinimumLength(4).WithMessage("El nombre de usuario debe tener al menos 4 caracteres.")
                .MaximumLength(50).WithMessage("El nombre de usuario no puede exceder 50 caracteres.");

            // 2️⃣ Email
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo es obligatorio.")
                .EmailAddress().WithMessage("El correo no tiene un formato válido.")
                .MaximumLength(100).WithMessage("El correo no puede exceder 100 caracteres.");

            // 3️⃣ PhoneNumber (opcional)
            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[0-9]{7,15}$")
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
                .WithMessage("El teléfono no tiene un formato válido.");

            // 4️⃣ Password y confirmación
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");

            RuleFor(x => x.PasswordConfirmed)
                .NotEmpty().WithMessage("La confirmación de contraseña es obligatoria.");

            RuleFor(x => x)
                .Must(x => x.Password == x.PasswordConfirmed)
                .WithMessage("Las contraseñas no coinciden.");

            // 5️⃣ Nombre y Apellidos (opcionales)
            RuleFor(x => x.Nombre)
                .MaximumLength(50)
                .WithMessage("El nombre no puede exceder 50 caracteres.")
                .When(x => !string.IsNullOrWhiteSpace(x.Nombre));

            RuleFor(x => x.Apellidos)
                .MaximumLength(100)
                .WithMessage("Los apellidos no pueden exceder 100 caracteres.")
                .When(x => !string.IsNullOrWhiteSpace(x.Apellidos));

            // 6️⃣ Roles
            RuleFor(x => x.RoleIds)
                .NotNull().WithMessage("Debe especificar la lista de roles.")
                .Must(r => r.Any()).WithMessage("Debe asignar al menos un rol.");

            // 7️⃣ Activo
            // bool siempre tiene valor; no requiere regla
        }
    }
}
