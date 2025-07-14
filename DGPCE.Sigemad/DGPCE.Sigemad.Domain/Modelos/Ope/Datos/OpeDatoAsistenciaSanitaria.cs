using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
public class OpeDatoAsistenciaSanitaria : BaseDomainModel<int>
{
    public int IdOpeDatoAsistencia { get; set; }
    public int IdOpeAsistenciaSanitariaTipo { get; set; }
    public int Numero { get; set; }
    public string Observaciones { get; set; }


    public virtual OpeDatoAsistencia OpeDatoAsistencia { get; set; } = null!;
    public virtual OpeAsistenciaSanitariaTipo OpeAsistenciaSanitariaTipo { get; set; } = null!;
}
