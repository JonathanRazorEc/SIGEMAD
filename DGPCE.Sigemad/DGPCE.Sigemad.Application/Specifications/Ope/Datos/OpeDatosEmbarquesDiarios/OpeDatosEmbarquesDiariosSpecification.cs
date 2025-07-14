using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosAsistencias.Vms;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosEmbarquesDiarios;

public class OpeDatosEmbarquesDiariosSpecification : BaseSpecification<OpeDatoEmbarqueDiario>
{
    public OpeDatosEmbarquesDiariosSpecification(OpeDatosEmbarquesDiariosSpecificationParams request)
        : base(opeDatoEmbarqueDiario =>
        (!request.Id.HasValue || opeDatoEmbarqueDiario.Id == request.Id) &&
        //(!request.IdOpeLineaMaritima.HasValue || opeDatoEmbarqueDiario.IdOpeLineaMaritima == request.IdOpeLineaMaritima) &&
        (request.IdsOpeLineasMaritimas == null || request.IdsOpeLineasMaritimas.Contains(opeDatoEmbarqueDiario.IdOpeLineaMaritima)) &&
        //
        (!request.IdOpeFase.HasValue || opeDatoEmbarqueDiario.OpeLineaMaritima.OpeFase.Id == request.IdOpeFase) &&
        //

        (!request.FechaInicio.HasValue || DateOnly.FromDateTime(opeDatoEmbarqueDiario.Fecha) >= request.FechaInicio) &&
        (!request.FechaFin.HasValue || DateOnly.FromDateTime(opeDatoEmbarqueDiario.Fecha) <= request.FechaFin) &&
        opeDatoEmbarqueDiario.Borrado != true
    )
    {
        AddInclude(i => i.OpeLineaMaritima);
        AddInclude(i => i.OpeLineaMaritima.OpeFase);

        ApplyPaging(request);

        // Aplicar la ordenación
        if (!string.IsNullOrEmpty(request.Sort?.ToLower()))
        {
            switch (request.Sort)
            {
                case "fechainicioasc":
                    AddOrderBy(i => i.Fecha);
                    break;
                case "fechaIniciodesc":
                    AddOrderByDescending(i => i.Fecha);
                    break;
                case "denominacionasc":
                    //AddOrderBy(i => i.Nombre);
                    break;
                case "denominaciondesc":
                    //AddOrderByDescending(i => i.Nombre);
                    break;
                default:
                    AddOrderByDescending(i => i.Fecha); // Orden por defecto
                    break;
            }
        }
    }
}
