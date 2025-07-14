using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePaises.Queries.GetOpePaisesList
{
    public class GetOpePaisesListQuery : IRequest<IReadOnlyList<OpePais>>
    {
        public bool? Extranjero { get; set; }
        public bool? OpePuertos { get; set; }
        public bool? OpeDatosAsistencias { get; set; }
        public bool? Borrado { get; set; }
    }
}
