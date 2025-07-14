using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosEmbarquesDiarios.Queries.GetOpeDatoEmbarqueDiarioById
{
    public class GetOpeDatoEmbarqueDiarioByIdQuery : IRequest<OpeDatoEmbarqueDiario>
    {
        public int Id { get; set; }

        public GetOpeDatoEmbarqueDiarioByIdQuery(int id)
        {
            Id = id;
        }
    }
}
