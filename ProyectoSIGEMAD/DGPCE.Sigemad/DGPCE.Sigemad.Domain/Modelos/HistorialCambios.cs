namespace DGPCE.Sigemad.Domain.Modelos;
public class HistorialCambios
{
    public int Id { get; set; }
    public int IdDetalleRegistroActualizacion { get; set; }
    public int IdReferencia { get; set; } // ID del registro modificado
    public string TablaModificada { get; set; }
    public string CampoModificado { get; set; }
    public string ValorAnterior { get; set; }
    public string ValorNuevo { get; set; }
    public DateTime FechaModificacion { get; set; }
    public Guid? ModificadoPor { get; set; }

    public DetalleRegistroActualizacion DetalleRegistroActualizacion { get; set; }
}

