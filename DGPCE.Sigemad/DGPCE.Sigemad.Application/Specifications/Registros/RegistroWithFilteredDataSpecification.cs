using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Registros;
public class RegistroWithFilteredDataSpecification : BaseSpecification<Registro>
{
    public RegistroWithFilteredDataSpecification(
        int id,
        List<int>? idsAreaAfectada = null,
        List<int>? idsParametro = null,
        List<int>? idsConsecuenciaActuacion = null,
        List<int>? idsIntervencionMedio = null,
        List<int>? idsDirecciones = null,
        List<int>? idsCecopi = null,
        List<int>? idsPma = null,
        List<int>? idsActivacionPlanEmergencias = null,
        List<int>? idsActivacionSistemas = null,
        int? ambito = null)
        : base(e => e.Id == id && e.Borrado == false)
    {
        AddInclude(r => r.Medio);
        AddInclude(r => r.EntradaSalida);
        AddInclude(r => r.ProcedenciaDestinos);
        AddInclude(e => e.ProcedenciaDestinos.Where(parametro => !parametro.Borrado));
        AddInclude(r => r.CreadoPorNavigation);
        AddInclude(r => r.ModificadoPorNavigation);
        AddInclude(r => r.EliminadoPorNavigation);
        AddInclude("ProcedenciaDestinos.ProcedenciaDestino");

        if (idsAreaAfectada != null && idsAreaAfectada.Any())
        {
            AddInclude(e => e.AreaAfectadas.Where(area => idsAreaAfectada.Contains(area.Id) && !area.Borrado));
            AddInclude("AreaAfectadas.Municipio");
            AddInclude("AreaAfectadas.Provincia");
            AddInclude("AreaAfectadas.EntidadMenor");
        }


        if (idsParametro != null  && idsParametro.Any())
        {
            AddInclude(e => e.Parametros.Where(parametro => idsParametro.Contains(parametro.Id) && !parametro.Borrado));
            AddInclude("Parametros.PlanEmergencia");
            AddInclude("Parametros.FaseEmergencia");
            AddInclude("Parametros.PlanSituacion");
            AddInclude("Parametros.SituacionEquivalente");
            AddInclude("Parametros.EstadoIncendio");
        }

        if (idsConsecuenciaActuacion != null && idsConsecuenciaActuacion.Any())
        {
            AddInclude(e => e.TipoImpactosEvoluciones.Where(impacto => idsConsecuenciaActuacion.Contains(impacto.Id) && !impacto.Borrado));

            AddInclude("TipoImpactosEvoluciones.TipoImpacto");
            AddInclude("TipoImpactosEvoluciones.ImpactosEvoluciones");
            AddInclude("TipoImpactosEvoluciones.ImpactosEvoluciones.AlteracionInterrupcion");
            AddInclude("TipoImpactosEvoluciones.ImpactosEvoluciones.TipoDanio");
            AddInclude("TipoImpactosEvoluciones.ImpactosEvoluciones.ZonaPlanificacion");
            AddInclude("TipoImpactosEvoluciones.ImpactosEvoluciones.ImpactoClasificado");
            AddInclude("TipoImpactosEvoluciones.ImpactosEvoluciones.Provincia");
            AddInclude("TipoImpactosEvoluciones.ImpactosEvoluciones.Municipio");

        }

        if (idsIntervencionMedio != null && idsIntervencionMedio.Any())
        {
            AddInclude(e => e.IntervencionMedios.Where(intervencion => idsIntervencionMedio.Contains(intervencion.Id) && !intervencion.Borrado));
            AddInclude("IntervencionMedios.CaracterMedio");
            AddInclude("IntervencionMedios.TitularidadMedio");
            AddInclude("IntervencionMedios.Municipio");
            AddInclude("IntervencionMedios.Provincia");
            AddInclude("IntervencionMedios.Capacidad");
            AddInclude("IntervencionMedios.DetalleIntervencionMedios");
            AddInclude("IntervencionMedios.DetalleIntervencionMedios.MediosCapacidad");
        }

        if (idsDirecciones != null && idsDirecciones.Any())
        {
            AddInclude(d => d.Direcciones.Where(dir => idsDirecciones.Contains(dir.Id) && !dir.Borrado));
            AddInclude("Direcciones.TipoDireccionEmergencia");
            AddInclude("Direcciones.Archivo");
        }

        if (idsCecopi != null && idsCecopi.Any())
        {
            AddInclude(d => d.CoordinacionesCecopi.Where(cecopi => idsCecopi.Contains(cecopi.Id) && !cecopi.Borrado));
            AddInclude("CoordinacionesCecopi.Provincia");
            AddInclude("CoordinacionesCecopi.Municipio");
            AddInclude("CoordinacionesCecopi.Archivo");
        }

        if (idsPma != null && idsPma.Any())
        {
            AddInclude(d => d.CoordinacionesPMA.Where(pma => idsPma.Contains(pma.Id) && !pma.Borrado));
            AddInclude("CoordinacionesPMA.Provincia");
            AddInclude("CoordinacionesPMA.Municipio");
            AddInclude("CoordinacionesPMA.Archivo");
        }


        if (idsActivacionPlanEmergencias != null && idsActivacionPlanEmergencias.Any())
        {
            AddInclude(d => d.ActivacionPlanEmergencias
                .Where(planEmergencia =>
                    idsActivacionPlanEmergencias.Contains(planEmergencia.Id) &&
                    !planEmergencia.Borrado &&
                    (ambito == null || planEmergencia.PlanEmergencia.IdAmbitoPlan == ambito)
                ));

            AddInclude("ActivacionPlanEmergencias.TipoPlan");
            AddInclude("ActivacionPlanEmergencias.PlanEmergencia");
            AddInclude("ActivacionPlanEmergencias.Archivo");
        }

        if (idsActivacionSistemas != null && idsActivacionSistemas.Any())
        {
            AddInclude(d => d.ActivacionSistemas.Where(activacionSistemas => idsActivacionSistemas.Contains(activacionSistemas.Id) && !activacionSistemas.Borrado));
            AddInclude("ActivacionSistemas.TipoSistemaEmergencia");
            AddInclude("ActivacionSistemas.ModoActivacion");
        }
    }
}
