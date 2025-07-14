using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeLogs;

public class OpeLogsSpecification : BaseSpecification<OpeLog>
{
    public OpeLogsSpecification(OpeLogsSpecificationParams request)
        : base(opeLog =>
        (!request.Id.HasValue || opeLog.Id == request.Id) 
        // && opeLog.Borrado != true
        )
    {
        //AddInclude(i => i.OpeLogTipo);

        ApplyPaging(request);

        // Aplicar la ordenación
        if (!string.IsNullOrEmpty(request.Sort?.ToLower()))
        {
            switch (request.Sort)
            {
                case "fechainicioasc":
                    AddOrderBy(i => i.FechaRegistro);
                    break;
                case "fechaIniciodesc":
                    AddOrderByDescending(i => i.FechaRegistro);
                    break;
                case "denominacionasc":
                    //AddOrderBy(i => i.Nombre);
                    break;
                case "denominaciondesc":
                    //AddOrderByDescending(i => i.Nombre);
                    break;
                default:
                    AddOrderBy(i => i.FechaRegistro); // Orden por defecto
                    break;
            }
        }
    }
}
