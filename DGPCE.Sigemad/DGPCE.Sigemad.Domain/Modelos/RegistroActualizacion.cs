using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class RegistroActualizacion: BaseDomainModel<int>
{
    public RegistroActualizacion()
    {
        RegistrosApartados = new();
        DetallesRegistro = new();
    }

    public int IdSuceso { get; set; }
    public Suceso Suceso { get; set; }

    public int IdTipoRegistroActualizacion { get; set; }
    public int IdReferencia { get; set; } // ID de la cabecera (DireccionCoordinacionEmergencia, etc.)
    public string TipoEntidad { get; set; } // 'Direccion', 'CoordinacionCecopi', 'CoordinacionPMA'

    public TipoRegistroActualizacion TipoRegistroActualizacion { get; set; }
    public List<RegistroApartado> RegistrosApartados { get; set; }
    public List<DetalleRegistroActualizacion> DetallesRegistro { get; set; }
}
