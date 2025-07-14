using DGPCE.Sigemad.Domain.Modelos;
using DGPCE.Sigemad.Application.Specifications;

namespace DGPCE.Sigemad.Application.Specifications.Auditoria
{
    public class AuditoriaParametroBySucesoSpec : BaseSpecification<Auditoria_Parametro>
    {
        public AuditoriaParametroBySucesoSpec(int idEstadoIncendio)
            : base(p => p.Borrado != true
                        // y si Auditoria_Parametro no tiene un IdSuceso directo,
                        // quizás tengas que filtrar por IdEvolucion => Auditoria_Evolucion => IdSuceso
                        // Este ejemplo asume p.IdSuceso existe. Ajusta según tu DB.
                        && p.IdEstadoIncendio== idEstadoIncendio
                    )
        {
            // no includes unless you need more
        }
    }
}
