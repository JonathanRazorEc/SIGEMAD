using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePorcentsOcAE.Commands.UpdateOpePorcentajesOcupacionAreasEstacionamiento;

public class UpdateOpePorcentOcAECommand : IRequest
{
    public int Id { get; set; }
    public int IdOpeOcupacion { get; set; }
    public int PorcentajeInferior { get; set; }
    public int PorcentajeSuperior { get; set; }

}
