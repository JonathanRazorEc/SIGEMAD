using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class DetalleOtraInformacion_ProcedenciaDestino : BaseEntity
{
    public DetalleOtraInformacion_ProcedenciaDestino() { }

    public int IdDetalleOtraInformacion { get; set; }
    public int IdProcedenciaDestino { get; set; }

    public virtual DetalleOtraInformacion DetalleOtraInformacion { get; set; }
    public virtual ProcedenciaDestino ProcedenciaDestino { get; set; }
}
