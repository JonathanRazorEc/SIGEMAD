using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpePeriodos.Vms
{
    public class OpePeriodoVm : BaseDomainModel<int>
    {
        public string Nombre { get; set; } = null!;
        public int IdOpePeriodoTipo { get; set; }
        public DateTime FechaInicioFaseSalida { get; set; }
        public DateTime FechaFinFaseSalida { get; set; }

        public DateTime FechaInicioFaseRetorno { get; set; }
        public DateTime FechaFinFaseRetorno { get; set; }

        public virtual OpePeriodoTipo OpePeriodoTipo { get; set; } = null!;
    }
}
