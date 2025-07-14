using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosAsistencias.Commands.DeleteOpeDatosAsistencias
{
    public class DeleteOpeDatoAsistenciaCommand : IRequest
    {
        public int Id { get; set; }
    }
}
