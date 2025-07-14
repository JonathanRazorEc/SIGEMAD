namespace DGPCE.Sigemad.Application.Dtos.Ope.Datos;
public class CreateOpeDatoAsistenciaSocialTareaDto : IEquatable<CreateOpeDatoAsistenciaSocialTareaDto>
{
    public int? Id { get; set; }
    public int IdOpeDatoAsistenciaSocial { get; set; }
    public int IdOpeAsistenciaSocialTareaTipo { get; set; }
    public int Numero { get; set; }
    public string? Observaciones { get; set; }


    public bool Equals(CreateOpeDatoAsistenciaSocialTareaDto? other)
    {
        if (other is null)
        {
            return false;
        }
        return Id == other.Id &&
            IdOpeDatoAsistenciaSocial == other.IdOpeDatoAsistenciaSocial &&
            IdOpeAsistenciaSocialTareaTipo == other.IdOpeAsistenciaSocialTareaTipo &&
            Numero == other.Numero &&
            string.Equals(Observaciones, other.Observaciones
            );
    }


    public override bool Equals(object? obj)
    {
        if (obj is CreateOpeDatoAsistenciaSocialTareaDto other)
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
            IdOpeAsistenciaSocialTareaTipo,
            Numero,
            Observaciones);
    }
}
