using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Features.AspNetUsers.Vms
{
    public class AspNetUserVm
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public string? NormalizedUserName { get; set; }

        public string? Email { get; set; }

        public string? NormalizedEmail { get; set; }

        public bool? EmailConfirmed { get; set; }

        public string? PhoneNumber { get; set; }

        public bool? PhoneNumberConfirmed { get; set; }

        public bool? TwoFactorEnabled { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        public bool? LockoutEnabled { get; set; }
        public List<string> Roles { get; set; } = new();

        //nombres y apellidos

        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }

        /// <summary>Fecha de creación del usuario.</summary>
        public DateTime FechaCreacion { get; set; }

        /// <summary>Indica si el usuario está activo.</summary>
        public bool Activo { get; set; }

    }

}
