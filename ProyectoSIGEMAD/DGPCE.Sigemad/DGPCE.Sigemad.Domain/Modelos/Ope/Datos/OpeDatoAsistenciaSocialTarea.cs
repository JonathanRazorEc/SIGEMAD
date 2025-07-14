using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
public class OpeDatoAsistenciaSocialTarea : BaseDomainModel<int>
{
    public int IdOpeDatoAsistenciaSocial { get; set; }
    public int IdOpeAsistenciaSocialTareaTipo { get; set; }
    public int Numero { get; set; }
    public string Observaciones { get; set; }


    public virtual OpeDatoAsistenciaSocial OpeDatoAsistenciaSocial { get; set; } = null!;
    public virtual OpeAsistenciaSocialTareaTipo OpeAsistenciaSocialTareaTipo { get; set; } = null!;
}
