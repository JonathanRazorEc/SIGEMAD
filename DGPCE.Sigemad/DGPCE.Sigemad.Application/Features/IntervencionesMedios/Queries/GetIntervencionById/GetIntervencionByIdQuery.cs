using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.IntervencionesMedios.Queries.GetIntervencionById;
public class GetIntervencionByIdQuery : IRequest<IntervencionMedio>
{
    public int Id { get; set; }

    public GetIntervencionByIdQuery(int id)
    {
        Id = id;
    }
}
