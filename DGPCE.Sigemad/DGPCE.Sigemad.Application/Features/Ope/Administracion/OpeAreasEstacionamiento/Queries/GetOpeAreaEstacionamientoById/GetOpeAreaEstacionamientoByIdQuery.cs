using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasEstacionamiento.Queries.GetOpeAreaEstacionamientoById
{
    public class GetOpeAreaEstacionamientoByIdQuery : IRequest<OpeAreaEstacionamiento>
    {
        public int Id { get; set; }

        public GetOpeAreaEstacionamientoByIdQuery(int id)
        {
            Id = id;
        }
    }
}
