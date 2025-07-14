namespace DGPCE.Sigemad.Application.Dtos.Ope.Datos;
public class CreateOpeDatoAsistenciaSocialUsuarioDto : IEquatable<CreateOpeDatoAsistenciaSocialUsuarioDto>
{
    public int? Id { get; set; }
    public int IdOpeDatoAsistenciaSocial { get; set; }
    public int IdOpeAsistenciaSocialEdad { get; set; }
    public int IdOpeAsistenciaSocialSexo { get; set; }
    public int IdOpeAsistenciaSocialNacionalidad { get; set; }
    public int IdPaisResidencia { get; set; }
    public int Numero { get; set; }
    public string? Observaciones { get; set; }


    public bool Equals(CreateOpeDatoAsistenciaSocialUsuarioDto? other)
    {
        if (other is null)
        {
            return false;
        }
        return Id == other.Id &&
            IdOpeDatoAsistenciaSocial == other.IdOpeDatoAsistenciaSocial &&
            IdOpeAsistenciaSocialEdad == other.IdOpeAsistenciaSocialEdad &&
            IdOpeAsistenciaSocialSexo == other.IdOpeAsistenciaSocialSexo &&
            IdOpeAsistenciaSocialNacionalidad == other.IdOpeAsistenciaSocialNacionalidad &&
            IdPaisResidencia == other.IdPaisResidencia &&
            Numero == other.Numero &&
            string.Equals(Observaciones, other.Observaciones
            );
    }


    public override bool Equals(object? obj)
    {
        if (obj is CreateOpeDatoAsistenciaSocialUsuarioDto other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Id,
            IdOpeDatoAsistenciaSocial,
            IdOpeAsistenciaSocialEdad,
            IdOpeAsistenciaSocialSexo,
            IdOpeAsistenciaSocialNacionalidad,
            IdPaisResidencia,
            Numero,
            Observaciones);
    }
}
