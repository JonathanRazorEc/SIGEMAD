using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.CCAA.Vms
{
    public class ComunidadesAutonomasConPaisVm
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = null!;
        public Pais Pais { get; set; } = null!;
    }
}
