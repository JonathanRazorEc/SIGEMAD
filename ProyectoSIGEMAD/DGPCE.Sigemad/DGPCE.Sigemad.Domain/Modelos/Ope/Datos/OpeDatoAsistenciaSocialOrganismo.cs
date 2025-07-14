using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
public class OpeDatoAsistenciaSocialOrganismo : BaseDomainModel<int>
{
    public int IdOpeDatoAsistenciaSocial { get; set; }
    public int IdOpeAsistenciaSocialOrganismoTipo { get; set; }
    public int Numero { get; set; }
    public string Observaciones { get; set; }


    public virtual OpeDatoAsistenciaSocial OpeDatoAsistenciaSocial { get; set; } = null!;
    public virtual OpeAsistenciaSocialOrganismoTipo OpeAsistenciaSocialOrganismoTipo { get; set; } = null!;
}
