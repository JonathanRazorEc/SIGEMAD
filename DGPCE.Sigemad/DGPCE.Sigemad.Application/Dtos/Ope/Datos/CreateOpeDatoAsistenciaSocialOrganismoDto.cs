namespace DGPCE.Sigemad.Application.Dtos.Ope.Datos;
public class CreateOpeDatoAsistenciaSocialOrganismoDto : IEquatable<CreateOpeDatoAsistenciaSocialOrganismoDto>
{
    public int? Id { get; set; }
    public int IdOpeDatoAsistenciaSocial { get; set; }
    public int IdOpeAsistenciaSocialOrganismoTipo { get; set; }
    public int Numero { get; set; }
    public string? Observaciones { get; set; }


    public bool Equals(CreateOpeDatoAsistenciaSocialOrganismoDto? other)
    {
        if (other is null)
        {
            return false;
        }
        return Id == other.Id &&
            IdOpeDatoAsistenciaSocial == other.IdOpeDatoAsistenciaSocial &&
            IdOpeAsistenciaSocialOrganismoTipo == other.IdOpeAsistenciaSocialOrganismoTipo &&
            Numero == other.Numero &&
            string.Equals(Observaciones, other.Observaciones
            );
    }


    public override bool Equals(object? obj)
    {
        if (obj is CreateOpeDatoAsistenciaSocialOrganismoDto other)
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
            IdOpeAsistenciaSocialOrganismoTipo,
            Numero,
            Observaciones);
    }
}
