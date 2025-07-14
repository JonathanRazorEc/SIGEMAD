using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeFronteras;

public class OpeFronterasSpecification : BaseSpecification<OpeFrontera>
{
    public OpeFronterasSpecification(OpeFronterasSpecificationParams request)
        : base(opeFrontera =>
        (string.IsNullOrEmpty(request.Nombre) || opeFrontera.Nombre.Contains(request.Nombre)) &&
        (!request.Id.HasValue || opeFrontera.Id == request.Id) &&
        opeFrontera.Borrado != true
        )
    {
        AddInclude(i => i.Ccaa);
        AddInclude(i => i.Provincia);
        AddInclude(i => i.Municipio);


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
                    AddOrderBy(i => i.Nombre);
                    break;
                case "denominaciondesc":
                    AddOrderByDescending(i => i.Nombre);
                    break;
                default:
                    //AddOrderBy(i => i.FechaValidezDesde); // Orden por defecto
                    break;
            }
        }
    }
}
