using DGPCE.Sigemad.Application.Dtos.Ope.Datos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosAsistencias.Commands.CreateOpeDatosAsistencias;

public class CreateOpeDatoAsistenciaCommand : IRequest<CreateOpeDatoAsistenciaResponse>
{
    public int IdOpePuerto { get; set; }
    public DateTime Fecha { get; set; }

    public Boolean opeDatosAsistenciasSanitariasModificado { get; set; }
    public List<CreateOpeDatoAsistenciaSanitariaDto>? OpeDatosAsistenciasSanitarias { get; set; }
    public Boolean opeDatosAsistenciasSocialesModificado { get; set; }
    public List<CreateOpeDatoAsistenciaSocialDto>? OpeDatosAsistenciasSociales { get; set; }
    public Boolean opeDatosAsistenciasTraduccionesModificado { get; set; }
    public List<CreateOpeDatoAsistenciaTraduccionDto>? OpeDatosAsistenciasTraducciones { get; set; }

}
