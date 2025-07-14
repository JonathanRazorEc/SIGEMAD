using DGPCE.Sigemad.Application.Dtos.Archivos;

namespace DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios.Pasos;
public class SolicitudMedioDto
{
    public int Id { get; set; }
    public ProcedenciaMedioDto ProcedenciaMedio { get; set; }
    public string AutoridadSolicitante { get; set; }
    public DateTime FechaHoraSolicitud { get; set; }
    public string? Descripcion { get; set; }
    public string? Observaciones { get; set; }


    public ArchivoDto? Archivo { get; set; }
}
