using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePeriodos.Queries.GetOpePeriodoById
{
    public class GetOpePeriodoByIdQuery : IRequest<OpePeriodo>
    {
        public int Id { get; set; }

        public GetOpePeriodoByIdQuery(int id)
        {
            Id = id;
        }
    }
}
