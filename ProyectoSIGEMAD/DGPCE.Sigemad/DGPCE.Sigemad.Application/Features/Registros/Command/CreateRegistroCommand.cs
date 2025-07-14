using DGPCE.Sigemad.Application.Features.Registros.CreateRegistros;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Registros.Command.CreateRegistros;
public class CreateRegistroCommand : IEquatable<CreateRegistroCommand>,IRequest<ManageRegistroResponse>
{
    public int IdSuceso { get; set; }
    public int? IdRegistroActualizacion { get; set; }
    public DateTimeOffset? FechaHoraEvolucion { get; set; }
    public int? IdEntradaSalida { get; set; }
    public int? IdMedio { get; set; }
    public List<int> RegistroProcedenciasDestinos { get; set; } = new();

    public bool Equals(CreateRegistroCommand? other)
    {
        if (other is null) return false;

        return FechaHoraEvolucion == other.FechaHoraEvolucion &&
               IdEntradaSalida == other.IdEntradaSalida &&
               IdMedio == other.IdMedio &&
               RegistroProcedenciasDestinos.SequenceEqual(other.RegistroProcedenciasDestinos);
    }

    public override bool Equals(object? obj)
    {
        if (obj is CreateRegistroCommand other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        int hash = HashCode.Combine(
            FechaHoraEvolucion, 
            IdEntradaSalida, 
            IdMedio,
            RegistroProcedenciasDestinos.Count);

        return hash;
    }
}
