using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Paises.Queries.GetPaisesBySearch;

public class GetPaisesBySearchQuery : IRequest<IReadOnlyList<Pais>>
{
    public bool MostrarNacional { get; set; }


    public string? busqueda { get; set; }
}
