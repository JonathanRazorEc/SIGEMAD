using DGPCE.Sigemad.Application.Dtos.SucesoRelacionados;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.ManageSucesoRelacionados;
public class ManageSucesoRelacionadosCommand: IRequest<ManageSucesoRelacionadoResponse>, IEquatable<ManageSucesoRelacionadosCommand>
{
    public int? IdRegistroActualizacion { get; set; }
    public int IdSuceso { get; set; }
    public List<int> IdsSucesosAsociados { get; set; }

    public bool Equals(ManageSucesoRelacionadosCommand? other)
    {
        if (other is null)
        {
            return false;
        }
        return IdRegistroActualizacion == other.IdRegistroActualizacion &&
            IdSuceso == other.IdSuceso &&
            (IdsSucesosAsociados == null && other.IdsSucesosAsociados == null ||
            IdsSucesosAsociados != null && other.IdsSucesosAsociados != null &&
            IdsSucesosAsociados.SequenceEqual(other.IdsSucesosAsociados));
    }

    public override bool Equals(object? obj)
    {
        if (obj is ManageSucesoRelacionadosCommand other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            IdRegistroActualizacion,
            IdSuceso);
    }
}
