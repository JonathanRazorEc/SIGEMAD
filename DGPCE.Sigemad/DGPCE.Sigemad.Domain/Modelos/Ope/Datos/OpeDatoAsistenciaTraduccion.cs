using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
public class OpeDatoAsistenciaTraduccion : BaseDomainModel<int>
{
    public int IdOpeDatoAsistencia { get; set; }
    public int Numero { get; set; }
    public string Observaciones { get; set; }


    public virtual OpeDatoAsistencia OpeDatoAsistencia { get; set; } = null!;
}
