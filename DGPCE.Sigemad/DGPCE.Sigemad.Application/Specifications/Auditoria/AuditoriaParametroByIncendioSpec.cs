using DGPCE.Sigemad.Domain.Modelos;
using DGPCE.Sigemad.Application.Specifications;

namespace DGPCE.Sigemad.Application.Specifications.Auditoria
{
    public class AuditoriaParametroByIncendioSpec : BaseSpecification<Auditoria_Parametro>
    {
        public AuditoriaParametroByIncendioSpec(int idIncendio)
            : base(p => p.Borrado != true && p.IdEvolucion == idIncendio)
        {
        }
    }
}
