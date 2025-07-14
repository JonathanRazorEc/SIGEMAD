using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Queries.GetOpeFronteraById
{
    public class GetOpeFronteraByIdQuery : IRequest<OpeFrontera>
    {
        public int Id { get; set; }

        public GetOpeFronteraByIdQuery(int id)
        {
            Id = id;
        }
    }
}
