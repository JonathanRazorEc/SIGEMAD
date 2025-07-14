using DGPCE.Sigemad.Application.Specifications.Registros;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Evoluciones;
public class EvolucionWithRegistroSpecification : BaseSpecification<Evolucion>
{
    public EvolucionWithRegistroSpecification(RegistroSpecificationParams @params)
        : base(e =>
        (!@params.Id.HasValue || e.Id == @params.Id.Value) &&
        (!@params.IdSuceso.HasValue || e.IdSuceso == @params.IdSuceso.Value) &&
        e.Borrado == false)
    {
        //AddInclude(e => e.Registros.Where(r => r.Borrado == false));
        //AddInclude("Registros.ProcedenciaDestinos");
        //AddInclude(e => e.DatosPrincipales.Where(d => d.Borrado == false));
        //AddInclude(e => e.Parametros);
    }
}
