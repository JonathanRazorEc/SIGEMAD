using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class Documentacion : BaseDomainModel<int>
{
    public int IdSuceso { get; set; }

    public Suceso Suceso { get; set; }
    public List<DetalleDocumentacion> DetallesDocumentacion { get; set; } = new();
}