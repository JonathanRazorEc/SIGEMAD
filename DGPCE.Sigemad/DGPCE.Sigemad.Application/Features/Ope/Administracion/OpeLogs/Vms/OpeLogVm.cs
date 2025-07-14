using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLogs.Vms
{
    public class OpeLogVm : BaseDomainModel<int>
    {
        public DateTime FechaRegistro { get; set; }
        public string TipoMovimiento { get; set; } = null!;

    }
}
