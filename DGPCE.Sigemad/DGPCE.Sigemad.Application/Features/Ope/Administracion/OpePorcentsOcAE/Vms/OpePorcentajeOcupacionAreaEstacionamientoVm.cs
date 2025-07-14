using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpePorcentajesOcupacionAreasEstacionamiento.Vms
{
    public class OpePorcentajeOcupacionAreaEstacionamientoVm : BaseDomainModel<int>
    {
        public int IdOpeOcupacion { get; set; }
        public int porcentajeInferior { get; set; }
        public int porcentajeSuperior { get; set; }

        public virtual OpeOcupacion OpeOcupacion { get; set; } = null!;
    }
}
