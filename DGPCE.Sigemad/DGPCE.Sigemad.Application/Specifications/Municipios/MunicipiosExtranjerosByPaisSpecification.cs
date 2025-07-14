using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Municipios;
public class MunicipiosExtranjerosByPaisSpecification : BaseSpecification<MunicipioExtranjero>
{
    public MunicipiosExtranjerosByPaisSpecification(int idPais)
    {
        AddInclude(m => m.Distrito);
        AddCriteria(m => m.Distrito.IdPais == idPais);
    }
}
