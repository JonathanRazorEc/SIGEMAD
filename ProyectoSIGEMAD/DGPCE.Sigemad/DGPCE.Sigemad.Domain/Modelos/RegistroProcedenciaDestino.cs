
using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class RegistroProcedenciaDestino : BaseEntity
{
    public int IdRegistro { get; set; }
    public int IdProcedenciaDestino { get; set; }

    public virtual Registro Registro { get; set; }
    public ProcedenciaDestino ProcedenciaDestino { get; set; }
}
