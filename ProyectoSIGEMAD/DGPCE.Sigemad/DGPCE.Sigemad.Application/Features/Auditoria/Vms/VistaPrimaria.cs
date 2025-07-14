namespace DGPCE.Sigemad.Application.Features.Auditoria.Vms
{
    public class VistaPrimariaVm
    {
        public int Id { get; set; }
        public DateTime Inicio { get; set; }
        public string? EstadoIncendio { get; set; }
        public string? Denominacion { get; set; }
        public string? Provincia { get; set; }
        public string? NotaGeneral { get; set; }
        public int? SituacionOperativa { get; set; }
        public string? Municipio { get; set; }
        public decimal? SuperficieAfectadaHa { get; set; }
        public string? Seguimiento { get; set; }
        public string? EstadoSuceso { get; set; }
        public string? ClaseSuceso { get; set; }
    }
}
