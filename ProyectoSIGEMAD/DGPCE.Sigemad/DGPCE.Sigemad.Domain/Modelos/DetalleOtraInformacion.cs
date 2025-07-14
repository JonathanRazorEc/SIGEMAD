using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class DetalleOtraInformacion : BaseDomainModel<int>
{
    public DetalleOtraInformacion() {
        ProcedenciasDestinos = new List<DetalleOtraInformacion_ProcedenciaDestino>();
    }
    
    public int IdOtraInformacion { get; set; }
    public DateTime FechaHora { get; set; }
    public int IdMedio { get; set; }
    public string Asunto { get; set; }
    public string Observaciones { get; set; }

    public List<DetalleOtraInformacion_ProcedenciaDestino> ProcedenciasDestinos { get; set; }

    public virtual OtraInformacion OtraInformacion { get; set; }
    public virtual Medio Medio { get; set; }
}
