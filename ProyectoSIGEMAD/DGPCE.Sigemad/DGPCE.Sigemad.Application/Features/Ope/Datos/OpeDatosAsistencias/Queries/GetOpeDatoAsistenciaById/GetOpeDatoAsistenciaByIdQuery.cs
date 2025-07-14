using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosAsistencias.Queries.GetOpeDatoAsistenciaById
{
    public class GetOpeDatoAsistenciaByIdQuery : IRequest<OpeDatoAsistencia>
    {
        public int Id { get; set; }

        public GetOpeDatoAsistenciaByIdQuery(int id)
        {
            Id = id;
        }
    }
}
