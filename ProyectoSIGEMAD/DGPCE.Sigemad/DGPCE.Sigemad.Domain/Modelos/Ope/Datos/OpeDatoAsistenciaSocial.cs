using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
public class OpeDatoAsistenciaSocial : BaseDomainModel<int>
{
    public int IdOpeDatoAsistencia { get; set; }
    public int IdOpeAsistenciaSocialTipo { get; set; }
    public int Numero { get; set; }
    public string Observaciones { get; set; }


    public virtual OpeDatoAsistencia OpeDatoAsistencia { get; set; } = null!;
    public virtual OpeAsistenciaSocialTipo OpeAsistenciaSocialTipo { get; set; } = null!;

    //
    public List<OpeDatoAsistenciaSocialTarea>? OpeDatosAsistenciasSocialesTareas { get; set; }
    public List<OpeDatoAsistenciaSocialOrganismo>? OpeDatosAsistenciasSocialesOrganismos { get; set; }
    public List<OpeDatoAsistenciaSocialUsuario>? OpeDatosAsistenciasSocialesUsuarios { get; set; }
    //
}
