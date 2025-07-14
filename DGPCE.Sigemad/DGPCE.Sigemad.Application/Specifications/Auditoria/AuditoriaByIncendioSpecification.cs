using DGPCE.Sigemad.Application.Specifications; // donde tengas tu BaseSpecification<T>
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Auditoria
{
    public class AuditoriaIncendioByIncendioIdSpec : BaseSpecification<Auditoria_Incendio>
    {
        public AuditoriaIncendioByIncendioIdSpec(int idIncendio)
            : base(ai => ai.IdIncendio == idIncendio && ai.Borrado != true)
        {
            
            //AddInclude(ai => ai.Incendio);
            AddInclude(ai => ai.EstadoSuceso);
            AddInclude(ai => ai.ClaseSuceso);
            AddInclude(ai => ai.Municipio);
            AddInclude(ai => ai.Provincia);
            //AddInclude(ai => ai.Suceso);
            //AddInclude(ai => ai.Parametro);
            //AddInclude("AuditoriaSuceso.TipoSuceso");
        }
    }
}
