using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Queries.GetOpeDatoFronteraById
{
    public class GetOpeDatoFronteraByIdQuery : IRequest<OpeDatoFrontera>
    {
        public int Id { get; set; }

        public GetOpeDatoFronteraByIdQuery(int id)
        {
            Id = id;
        }
    }
}
