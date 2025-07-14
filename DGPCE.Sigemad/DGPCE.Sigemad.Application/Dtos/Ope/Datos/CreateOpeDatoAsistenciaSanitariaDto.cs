namespace DGPCE.Sigemad.Application.Dtos.Ope.Datos;
public class CreateOpeDatoAsistenciaSanitariaDto : IEquatable<CreateOpeDatoAsistenciaSanitariaDto>
{
    public int? Id { get; set; }
    public int IdOpeDatoAsistencia { get; set; }
    public int IdOpeAsistenciaSanitariaTipo { get; set; }
    public int Numero { get; set; }
    public string? Observaciones { get; set; }


    public bool Equals(CreateOpeDatoAsistenciaSanitariaDto? other)
    {
        if (other is null)
        {
            return false;
        }
        return Id == other.Id &&
            IdOpeDatoAsistencia == other.IdOpeDatoAsistencia &&
            IdOpeAsistenciaSanitariaTipo == other.IdOpeAsistenciaSanitariaTipo &&
            Numero == other.Numero &&
            string.Equals(Observaciones, other.Observaciones
            );
    }


    public override bool Equals(object? obj)
    {
        if (obj is CreateOpeDatoAsistenciaSanitariaDto other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Id,
            IdOpeDatoAsistencia,
            IdOpeAsistenciaSanitariaTipo,
            Numero,
            Observaciones);
    }
}
