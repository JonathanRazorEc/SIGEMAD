using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Enums;

namespace DGPCE.Sigemad.Domain.Modelos;
public class DetalleRegistroActualizacion: BaseDomainModel<int>
{
    public int IdRegistroActualizacion { get; set; }
    public int IdApartadoRegistro { get; set; }
    public int IdReferencia { get; set; } // ID del registro en Direccion, CoordinacionCecopi, CoordinacionPMA
    public EstadoRegistroEnum IdEstadoRegistro { get; set; } // 🔹 Ahora es un `enum`

    public string? Ambito { get; set; }
    public string? Descripcion { get; set; }


    public RegistroActualizacion RegistroActualizacion { get; set; }
    public ApartadoRegistro ApartadoRegistro { get; set; }
    public ICollection<HistorialCambios> HistorialCambios { get; set; }
}

