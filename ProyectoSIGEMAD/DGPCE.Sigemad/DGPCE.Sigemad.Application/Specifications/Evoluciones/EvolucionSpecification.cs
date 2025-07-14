
using DGPCE.Sigemad.Application.Specifications.Registros;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Evoluciones
{
    public class EvolucionSpecification : BaseSpecification<Evolucion>
    {
        public EvolucionSpecification(RegistroSpecificationParams request)
         : base(Evolucion =>
        (!request.Id.HasValue || Evolucion.Id == request.Id) &&
        (!request.IdSuceso.HasValue || Evolucion.IdSuceso == request.IdSuceso) &&
        (Evolucion.Borrado == false)
       )
        {
            //AddInclude(i => i.Registros.Where(r => !r.Borrado));
            //AddInclude("Registros.Medio");
            //AddInclude("Registros.EntradaSalida");
            //AddInclude("Registros.ProcedenciaDestinos");
            //AddInclude("Registros.ProcedenciaDestinos.ProcedenciaDestino");

            //AddInclude(i => i.DatosPrincipales.Where(d => !d.Borrado));

            //AddInclude(i => i.Parametros.Where(parametro => !parametro.Borrado));
            //AddInclude("Parametros.PlanEmergencia");
            //AddInclude("Parametros.FaseEmergencia");
            //AddInclude("Parametros.PlanSituacion");
            //AddInclude("Parametros.SituacionEquivalente");
            //AddInclude("Parametros.EstadoIncendio");

            //AddInclude(e => e.AreaAfectadas.Where(area => !area.Borrado));
            //AddInclude("AreaAfectadas.Municipio");
            //AddInclude("AreaAfectadas.Provincia");
            //AddInclude("AreaAfectadas.EntidadMenor");

            //AddInclude(e => e.Impactos.Where(consecuencia => !consecuencia.Borrado));
            //AddInclude("Impactos.ImpactoClasificado");
            //AddInclude("Impactos.TipoDanio");
            //AddInclude("Impactos.AlteracionInterrupcion");
            //AddInclude("Impactos.ZonaPlanificacion");

            //AddInclude(e => e.IntervencionMedios.Where(intervencion => !intervencion.Borrado));
            //AddInclude("IntervencionMedios.CaracterMedio");
            //AddInclude("IntervencionMedios.TitularidadMedio");
            //AddInclude("IntervencionMedios.Municipio");
            //AddInclude("IntervencionMedios.Provincia");
            //AddInclude("IntervencionMedios.Capacidad");
            //AddInclude("IntervencionMedios.DetalleIntervencionMedios");
            //AddInclude("IntervencionMedios.DetalleIntervencionMedios.MediosCapacidad");
        }
}
}