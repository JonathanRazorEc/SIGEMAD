using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.MunicipiosExtranjeros.Queries.GetMunicipiosExtranjerosByIdPais;
public class GetMunicipiosExtranjerosByIdPaisQuery : IRequest<IReadOnlyList<MunicipioExtranjero>>
{
    public int IdPais { get; set; }

    public GetMunicipiosExtranjerosByIdPaisQuery(int idPais)
    {
        IdPais = idPais;
    }
}
