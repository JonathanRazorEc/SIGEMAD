

using DGPCE.Sigemad.Application.Features.EvolucionProcedenciaDestinos.Vms;
namespace DGPCE.Sigemad.Application.Features.Evoluciones.Vms
{
    public class EvolucionVm
    {
        public int Id { get; set; }
        public int IdIncendio { get; set; }
        public ICollection<RegistroProcedenciaDestinoVm>? RegistroProcedenciasDestinos { get; set; } = null;
    }
}
