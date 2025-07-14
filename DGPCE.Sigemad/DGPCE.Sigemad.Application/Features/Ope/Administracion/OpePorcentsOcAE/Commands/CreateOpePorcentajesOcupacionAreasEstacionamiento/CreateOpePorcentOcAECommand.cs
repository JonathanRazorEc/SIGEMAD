using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePorcentsOcAE.Commands.CreateOpePorcentajesOcupacionAreasEstacionamiento;

public class CreateOpePorcentOcAECommand : IRequest<CreateOpePorcentOcAEResponse>
{

    public int IdOpeOcupacion { get; set; }

    public int PorcentajeInferior { get; set; }
    public int PorcentajeSuperior { get; set; }

}
