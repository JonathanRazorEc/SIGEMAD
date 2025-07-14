namespace DGPCE.Sigemad.Application.Dtos.Ope.Datos;
public class CreateOpeDatoAsistenciaSocialDto : IEquatable<CreateOpeDatoAsistenciaSocialDto>
{
    public int? Id { get; set; }
    public int IdOpeDatoAsistencia { get; set; }
    public int IdOpeAsistenciaSocialTipo { get; set; }
    public int Numero { get; set; }
    public string? Observaciones { get; set; }

    //
    public Boolean OpeDatosAsistenciasSocialesTareasModificado { get; set; }
    public List<CreateOpeDatoAsistenciaSocialTareaDto>? OpeDatosAsistenciasSocialesTareas { get; set; }
    public Boolean OpeDatosAsistenciasSocialesOrganismosModificado { get; set; }
    public List<CreateOpeDatoAsistenciaSocialOrganismoDto>? OpeDatosAsistenciasSocialesOrganismos { get; set; }

    public Boolean OpeDatosAsistenciasSocialesUsuariosModificado { get; set; }
    public List<CreateOpeDatoAsistenciaSocialUsuarioDto>? OpeDatosAsistenciasSocialesUsuarios { get; set; }
    //

    public bool Equals(CreateOpeDatoAsistenciaSocialDto? other)
    {
        if (other is null)
        {
            return false;
        }
        return Id == other.Id &&
            IdOpeDatoAsistencia == other.IdOpeDatoAsistencia &&
            IdOpeAsistenciaSocialTipo == other.IdOpeAsistenciaSocialTipo &&
            Numero == other.Numero &&
            string.Equals(Observaciones, other.Observaciones
            );
    }


    public override bool Equals(object? obj)
    {
        if (obj is CreateOpeDatoAsistenciaSocialDto other)
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
            IdOpeAsistenciaSocialTipo,
            Numero,
            Observaciones);
    }
}
