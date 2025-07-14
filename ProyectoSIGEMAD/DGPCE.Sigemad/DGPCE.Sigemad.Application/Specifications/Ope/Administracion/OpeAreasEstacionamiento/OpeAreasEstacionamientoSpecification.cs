using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeAreasEstacionamiento;

public class OpeAreasEstacionamientoSpecification : BaseSpecification<OpeAreaEstacionamiento>
{
    public OpeAreasEstacionamientoSpecification(OpeAreasEstacionamientoSpecificationParams request)
        : base(opeAreaEstacionamiento =>
        (string.IsNullOrEmpty(request.Nombre) || opeAreaEstacionamiento.Nombre.Contains(request.Nombre)) &&
        (!request.Id.HasValue || opeAreaEstacionamiento.Id == request.Id) &&
        opeAreaEstacionamiento.Borrado != true
        )
    {
        AddInclude(i => i.Ccaa);
        AddInclude(i => i.Provincia);
        AddInclude(i => i.Municipio);
        AddInclude(i => i.OpePuerto);


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
