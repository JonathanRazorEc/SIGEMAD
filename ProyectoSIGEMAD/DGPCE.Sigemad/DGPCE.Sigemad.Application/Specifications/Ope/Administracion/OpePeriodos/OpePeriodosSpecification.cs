using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePeriodos;

public class OpePeriodosSpecification : BaseSpecification<OpePeriodo>
{
    public OpePeriodosSpecification(OpePeriodosSpecificationParams request)
        : base(opePeriodo =>
        (string.IsNullOrEmpty(request.Nombre) || opePeriodo.Nombre.Contains(request.Nombre)) &&
        (!request.Id.HasValue || opePeriodo.Id == request.Id) &&
        opePeriodo.Borrado != true
        )
    {
        AddInclude(i => i.OpePeriodoTipo);

        ApplyPaging(request);

        // Aplicar la ordenación
        if (!string.IsNullOrEmpty(request.Sort?.ToLower()))
        {
            switch (request.Sort)
            {
                case "fechainicioasc":
                    AddOrderBy(i => i.FechaInicioFaseSalida);
                    break;
                case "fechaIniciodesc":
                    AddOrderByDescending(i => i.FechaInicioFaseSalida);
                    break;
                case "denominacionasc":
                    AddOrderBy(i => i.Nombre);
                    break;
                case "denominaciondesc":
                    AddOrderByDescending(i => i.Nombre);
                    break;
                default:
                    //AddOrderBy(i => i.FechaInicioFaseSalida); // Orden por defecto
                    //AddOrderByDescending(i => i.FechaCreacion); // Orden por defecto
                    AddOrderByDescending(i => i.FechaInicioFaseSalida); // Orden por defecto
                    break;
            }
        }
    }
}
