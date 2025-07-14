namespace DGPCE.Sigemad.Application.Dtos.Ope.Datos;
public class CreateOpeDatoAsistenciaTraduccionDto : IEquatable<CreateOpeDatoAsistenciaTraduccionDto>
{
    public int? Id { get; set; }
    public int IdOpeDatoAsistencia { get; set; }
    public int Numero { get; set; }
    public string? Observaciones { get; set; }


    public bool Equals(CreateOpeDatoAsistenciaTraduccionDto? other)
    {
        if (other is null)
        {
            return false;
        }
        return Id == other.Id &&
            IdOpeDatoAsistencia == other.IdOpeDatoAsistencia &&
            Numero == other.Numero &&
            string.Equals(Observaciones, other.Observaciones
            );
    }


    public override bool Equals(object? obj)
    {
        if (obj is CreateOpeDatoAsistenciaTraduccionDto other)
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
            Numero,
            Observaciones);
    }
}
