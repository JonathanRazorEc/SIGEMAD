using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasDescanso.Queries.GetOpeAreaDescansoById
{
    public class GetOpeAreaDescansoByIdQuery : IRequest<OpeAreaDescanso>
    {
        public int Id { get; set; }

        public GetOpeAreaDescansoByIdQuery(int id)
        {
            Id = id;
        }
    }
}
