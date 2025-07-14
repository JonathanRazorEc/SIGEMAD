using DGPCE.Sigemad.Application.Dtos.ActuacionesRelevantes;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.ActuacionesRelevantes.Queries;
public class GetActuacionRelevanteQuery : IRequest<ActuacionRelevanteDGPCEDto>
{
    public int? IdRegistroActualizacion { get; set; }
    public int IdSuceso { get; set; }
}
