using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.MovilizacionMedios;
public class FlujoPasoMovilizacionSpecification : BaseSpecification<FlujoPasoMovilizacion>
{
    public FlujoPasoMovilizacionSpecification(FlujoPasoMovilizacionParams @params)
        : base(x => x.IdPasoActual == @params.IdPasoActual)
    {
        AddInclude(x => x.PasoActual);
        AddInclude(x => x.PasoSiguiente);
        AddOrderBy(x => x.Orden);
    }
}

