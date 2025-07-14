using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class AuditoriaSolicitudMedio : BaseDomainModel<int>
{
    public AuditoriaEjecucionPaso AuditoriaEjecucionPaso { get; set; }
    public int IdEjecucionPaso { get; set; }


    public ProcedenciaMedio ProcedenciaMedio { get; set; }
    public int IdProcedenciaMedio { get; set; }


    public string AutoridadSolicitante { get; set; }
    public DateTime FechaHoraSolicitud { get; set; }
    public string? Descripcion { get; set; }
    public string? Observaciones { get; set; }


    public Archivo? Archivo { get; set; }
    public Guid? IdArchivo { get; set; }
}
