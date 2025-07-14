using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuntosControlCarreteras.Queries.GetOpePuntoControlCarreteraById
{
    public class GetOpePuntoControlCarreteraByIdQuery : IRequest<OpePuntoControlCarretera>
    {
        public int Id { get; set; }

        public GetOpePuntoControlCarreteraByIdQuery(int id)
        {
            Id = id;
        }
    }
}
