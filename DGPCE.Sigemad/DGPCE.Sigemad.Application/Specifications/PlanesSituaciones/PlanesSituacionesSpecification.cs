using DGPCE.Sigemad.Domain.Modelos;


namespace DGPCE.Sigemad.Application.Specifications.PlanesSituaciones;
public class PlanesSituacionesSpecification : BaseSpecification<PlanSituacion>
{
    public PlanesSituacionesSpecification(PlanesSituacionesParams request,bool obtenerUltimoRegistro =false)
       : base(PlanSituacion =>
        (string.IsNullOrEmpty(request.Descripcion) || PlanSituacion.Descripcion.Contains(request.Descripcion)) &&
        (string.IsNullOrEmpty(request.Nivel) || PlanSituacion.Nivel.Contains(request.Nivel)) &&
        (string.IsNullOrEmpty(request.Situacion) || PlanSituacion.Situacion.Contains(request.Situacion)) &&
        (string.IsNullOrEmpty(request.SituacionEquivalente) || PlanSituacion.SituacionEquivalente.Contains(request.SituacionEquivalente)) &&
       (!request.Id.HasValue || PlanSituacion.Id == request.Id) &&
       (!request.IdPlanEmergencia.HasValue || PlanSituacion.IdPlanEmergencia == request.IdPlanEmergencia) &&
       (!request.IdFaseEmergencia.HasValue || PlanSituacion.IdFaseEmergencia == request.IdFaseEmergencia) 
       )
    {
        AddOrderBy(p => p.Orden);

        if (obtenerUltimoRegistro)
        {
            AddOrderByDescending(p => p.Id);
            AddInclude(p => p.FaseEmergencia);
        }
    }
}
