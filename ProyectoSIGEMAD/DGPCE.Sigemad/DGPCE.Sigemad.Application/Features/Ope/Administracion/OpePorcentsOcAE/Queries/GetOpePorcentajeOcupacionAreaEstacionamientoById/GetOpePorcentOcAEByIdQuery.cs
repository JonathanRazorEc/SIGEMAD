using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePorcentsOcAE.Queries.GetOpePorcentajeOcupacionAreaEstacionamientoById
{
    public class GetOpePorcentOcAEByIdQuery : IRequest<OpePorcentajeOcupacionAreaEstacionamiento>
    {
        public int Id { get; set; }

        public GetOpePorcentOcAEByIdQuery(int id)
        {
            Id = id;
        }
    }
}
