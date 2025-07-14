using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Paises.Queries.GetPaisesList;

public class GetPaisesListQuery : IRequest<IReadOnlyList<Pais>>
{
    public bool MostrarNacional { get; set; }
}
