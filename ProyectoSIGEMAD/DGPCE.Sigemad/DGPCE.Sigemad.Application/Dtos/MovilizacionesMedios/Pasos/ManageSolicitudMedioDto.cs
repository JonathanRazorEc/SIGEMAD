using DGPCE.Sigemad.Application.Dtos.Common;

namespace DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios.Pasos;
public class ManageSolicitudMedioDto : DatosPasoBase
{
    public int IdProcedenciaMedio { get; set; }
    public string AutoridadSolicitante { get; set; }
    public DateTime FechaHoraSolicitud { get; set; }
    public string? Descripcion { get; set; }
    public string? Observaciones { get; set; }
    public FileDto? Archivo { get; set; }
}
