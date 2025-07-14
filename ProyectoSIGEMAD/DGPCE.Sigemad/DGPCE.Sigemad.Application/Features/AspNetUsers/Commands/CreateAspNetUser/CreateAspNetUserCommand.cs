using DGPCE.Sigemad.Domain.Enums;
using MediatR;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Features.Incendios.Commands.CreateAspNetUser;

public class CreateAspNetUserCommand : IRequest<CreateAspNetUserResponse>
{
    public string UserName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }
    public string? Password { get; set; }
    public string? PasswordConfirmed { get; set; }

    //nombres y apellidos
    public string? Nombre { get; set; }
    public string? Apellidos { get; set; }

    //ROL
    public IEnumerable<string> RoleIds { get; set; } = Array.Empty<string>();

    //activo
    public bool Activo { get; set; }


}

