using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class OtraInformacion : BaseDomainModel<int>
{
    public OtraInformacion()
    {
        DetallesOtraInformacion = new List<DetalleOtraInformacion>();
    }

    public int IdSuceso { get; set; }

    public List<DetalleOtraInformacion> DetallesOtraInformacion { get; set; }

    public virtual Suceso Suceso { get; set; }
}
