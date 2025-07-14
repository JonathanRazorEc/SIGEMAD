using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePuntosControlCarreteras;

public class OpePuntosControlCarreterasSpecification : BaseSpecification<OpePuntoControlCarretera>
{
    public OpePuntosControlCarreterasSpecification(OpePuntosControlCarreterasSpecificationParams request)
        : base(opePuntoControlCarretera =>
        (string.IsNullOrEmpty(request.Nombre) || opePuntoControlCarretera.Nombre.Contains(request.Nombre)) &&
        (!request.Id.HasValue || opePuntoControlCarretera.Id == request.Id) &&
        opePuntoControlCarretera.Borrado != true
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
