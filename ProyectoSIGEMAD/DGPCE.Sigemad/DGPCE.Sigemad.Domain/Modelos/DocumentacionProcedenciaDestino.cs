using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class DocumentacionProcedenciaDestino : BaseEntity
{
    public int IdDetalleDocumentacion { get; set; }
    public int IdProcedenciaDestino { get; set; }

    public DetalleDocumentacion DetalleDocumentacion { get; set; }
    public ProcedenciaDestino ProcedenciaDestino { get; set; }
}