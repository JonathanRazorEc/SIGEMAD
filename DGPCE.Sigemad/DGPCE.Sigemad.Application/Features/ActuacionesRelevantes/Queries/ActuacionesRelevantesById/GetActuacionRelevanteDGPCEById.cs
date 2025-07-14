using DGPCE.Sigemad.Application.Dtos.ActuacionesRelevantes;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ActuacionesRelevantes.Queries.ActuacionesRelevantesById;
public class GetActuacionRelevanteDGPCEById : IRequest<ActuacionRelevanteDGPCEDto>
{
    public int Id { get; set; }

    public GetActuacionRelevanteDGPCEById(int id)
    {
        Id = id;
    }
}
