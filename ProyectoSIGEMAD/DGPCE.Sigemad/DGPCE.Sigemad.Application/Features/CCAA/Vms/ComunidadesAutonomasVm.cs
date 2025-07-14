using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.CCAA.Vms
{
    public class ComunidadesAutonomasVm
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = null!;
        public ICollection<ProvinciaSinMunicipiosVm> Provincia { get; set; } = new List<ProvinciaSinMunicipiosVm>();
    }
}
