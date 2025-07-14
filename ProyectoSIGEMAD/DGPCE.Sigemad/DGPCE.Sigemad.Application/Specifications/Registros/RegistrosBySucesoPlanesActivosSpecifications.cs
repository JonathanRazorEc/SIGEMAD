using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.Registros;
public class RegistrosBySucesoPlanesActivosSpecifications : BaseSpecification<Registro>
{
    public RegistrosBySucesoPlanesActivosSpecifications(RegistroSpecificationParams request)
        : base(registro =>
            (!request.Id.HasValue || registro.Id == request.Id) &&
            (!request.IdSuceso.HasValue || registro.IdSuceso == request.IdSuceso) &&
            registro.Borrado == false &&
            registro.ActivacionPlanEmergencias.Any(a => a.FechaHoraFin == null)
        )
    {
        AddInclude(r => r.ActivacionPlanEmergencias
            .Where(a => a.FechaHoraFin == null)
        );

        AddInclude("ActivacionPlanEmergencias.TipoPlan");
        AddInclude("ActivacionPlanEmergencias.PlanEmergencia");
        //AddInclude("ActivacionPlanEmergencias.Archivo");
    }
}

