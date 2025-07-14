using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePorcentajesOcupacionAreasEstacionamiento;

public class OpePorcentajesOcupacionAreasEstacionamientoSpecification : BaseSpecification<OpePorcentajeOcupacionAreaEstacionamiento>
{
    public OpePorcentajesOcupacionAreasEstacionamientoSpecification(OpePorcentajesOcupacionAreasEstacionamientoSpecificationParams request)
        : base(opePorcentajeOcupacionAreaEstacionamiento =>
        (!request.Id.HasValue || opePorcentajeOcupacionAreaEstacionamiento.Id == request.Id) &&
        (!request.IdOpeOcupacion.HasValue || opePorcentajeOcupacionAreaEstacionamiento.IdOpeOcupacion == request.IdOpeOcupacion) &&
        opePorcentajeOcupacionAreaEstacionamiento.Borrado != true
        )
    {
        AddInclude(i => i.OpeOcupacion);

        ApplyPaging(request);

        // Aplicar la ordenación
        if (!string.IsNullOrEmpty(request.Sort?.ToLower()))
        {
            switch (request.Sort)
            {
                case "fechainicioasc":
                    //AddOrderBy(i => i.FechaValidezDesde);
                    break;
                case "fechaIniciodesc":
                    //AddOrderByDescending(i => i.FechaValidezDesde);
                    break;
                case "denominacionasc":
                    //AddOrderBy(i => i.Nombre);
                    break;
                case "denominaciondesc":
                    //AddOrderByDescending(i => i.Nombre);
                    break;
                default:
                    //AddOrderBy(i => i.Nombre); // Orden por defecto
                    break;
            }
        }
    }
}
