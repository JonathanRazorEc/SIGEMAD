using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeAreasDescanso;

public class OpeAreasDescansoSpecification : BaseSpecification<OpeAreaDescanso>
{
    public OpeAreasDescansoSpecification(OpeAreasDescansoSpecificationParams request)
        : base(opeAreaDescanso =>
        (string.IsNullOrEmpty(request.Nombre) || opeAreaDescanso.Nombre.Contains(request.Nombre)) &&
        (!request.Id.HasValue || opeAreaDescanso.Id == request.Id) &&
        opeAreaDescanso.Borrado != true
        )
    {
        AddInclude(i => i.OpeAreaDescansoTipo);
        AddInclude(i => i.Ccaa);
        AddInclude(i => i.Provincia);
        AddInclude(i => i.Municipio);
        //AddInclude(i => i.OpeEstadoOcupacion);


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
