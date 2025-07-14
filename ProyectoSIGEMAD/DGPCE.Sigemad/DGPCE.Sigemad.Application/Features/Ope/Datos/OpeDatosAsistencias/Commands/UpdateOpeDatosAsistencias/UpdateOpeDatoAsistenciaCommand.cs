using DGPCE.Sigemad.Application.Dtos.Ope.Datos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosAsistencias.Commands.UpdateOpeDatosAsistencias;

public class UpdateOpeDatoAsistenciaCommand : IRequest
{
    public int Id { get; set; }
    public int IdOpePuerto { get; set; }
    public DateTime Fecha { get; set; }


    public Boolean OpeDatosAsistenciasSanitariasModificado { get; set; }
    public List<CreateOpeDatoAsistenciaSanitariaDto>? OpeDatosAsistenciasSanitarias { get; set; }
    public Boolean OpeDatosAsistenciasSocialesModificado { get; set; }
    public List<CreateOpeDatoAsistenciaSocialDto>? OpeDatosAsistenciasSociales { get; set; }
    public Boolean OpeDatosAsistenciasTraduccionesModificado { get; set; }
    public List<CreateOpeDatoAsistenciaTraduccionDto>? OpeDatosAsistenciasTraducciones { get; set; }
}
