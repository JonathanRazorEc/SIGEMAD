using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Paises;
public class PaisesSpecification : BaseSpecification<Pais>
{
    public PaisesSpecification(bool mostrarNacional)
        : base(p => (mostrarNacional && p.Id == (int)PaisesEnum.Espana) ||
                    (!mostrarNacional && p.Id != (int)PaisesEnum.Espana))
    {
        if (!mostrarNacional)
        {
            AddOrderBy(p => p.Id == (int)PaisesEnum.Portugal ? 0 :
                             p.Id == (int)PaisesEnum.Francia ? 1 : 2);
            AddOrderBy(p => p.Id != (int)PaisesEnum.Portugal && p.Id != (int)PaisesEnum.Francia ? p.Descripcion : null);
        }
          
    }

}
