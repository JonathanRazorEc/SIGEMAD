using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.Registros;
public class RegistroSpecification : BaseSpecification<Registro>
{
    public RegistroSpecification(RegistroSpecificationParams request)
      : base(registro =>
     (!request.Id.HasValue || registro.Id == request.Id) &&
     (!request.IdSuceso.HasValue || registro.IdSuceso == request.IdSuceso) &&
    (!request.FechaRegistro.HasValue || registro.FechaCreacion < request.FechaRegistro.Value.AddTicks(-request.FechaRegistro.Value.Ticks % TimeSpan.TicksPerSecond)) &&
     (registro.Borrado == false)
    )
    {

        AddInclude(e => e.AreaAfectadas.Where(area => !area.Borrado));
        AddInclude("AreaAfectadas.Municipio");
        AddInclude("AreaAfectadas.Provincia");
        AddInclude("AreaAfectadas.EntidadMenor");

        AddInclude(i => i.Parametros.Where(parametro => !parametro.Borrado));
        AddInclude("Parametros.PlanEmergencia");
        AddInclude("Parametros.FaseEmergencia");
        AddInclude("Parametros.PlanSituacion");
        AddInclude("Parametros.SituacionEquivalente");
        AddInclude("Parametros.EstadoIncendio");


        AddInclude(i => i.Parametros.Where(parametro => !parametro.Borrado));
        AddInclude("Parametros.PlanEmergencia");
        AddInclude("Parametros.FaseEmergencia");
        AddInclude("Parametros.PlanSituacion");
        AddInclude("Parametros.SituacionEquivalente");
        AddInclude("Parametros.EstadoIncendio");

        AddInclude(e => e.TipoImpactosEvoluciones.Where(impacto => !impacto.Borrado));

        AddInclude("TipoImpactosEvoluciones.TipoImpacto");
        AddInclude("TipoImpactosEvoluciones.ImpactosEvoluciones");
        AddInclude("TipoImpactosEvoluciones.ImpactosEvoluciones.AlteracionInterrupcion");
        AddInclude("TipoImpactosEvoluciones.ImpactosEvoluciones.TipoDanio");
        AddInclude("TipoImpactosEvoluciones.ImpactosEvoluciones.ZonaPlanificacion");
        AddInclude("TipoImpactosEvoluciones.ImpactosEvoluciones.ImpactoClasificado");
        AddInclude("TipoImpactosEvoluciones.ImpactosEvoluciones.Provincia");
        AddInclude("TipoImpactosEvoluciones.ImpactosEvoluciones.Municipio");

        AddInclude(e => e.IntervencionMedios.Where(intervencion => !intervencion.Borrado));
        AddInclude("IntervencionMedios.CaracterMedio");
        AddInclude("IntervencionMedios.TitularidadMedio");
        AddInclude("IntervencionMedios.Municipio");
        AddInclude("IntervencionMedios.Provincia");
        AddInclude("IntervencionMedios.Capacidad");
        AddInclude("IntervencionMedios.DetalleIntervencionMedios");
        AddInclude("IntervencionMedios.DetalleIntervencionMedios.MediosCapacidad");

        AddInclude(d => d.Direcciones.Where(direccion => !direccion.Borrado));
        AddInclude("Direcciones.TipoDireccionEmergencia");
        AddInclude("Direcciones.Archivo");

        AddInclude(d => d.CoordinacionesCecopi.Where(cecopi => !cecopi.Borrado));
        AddInclude("CoordinacionesCecopi.Provincia");
        AddInclude("CoordinacionesCecopi.Municipio");
        AddInclude("CoordinacionesCecopi.Archivo");

        AddInclude(d => d.CoordinacionesPMA.Where(pma => !pma.Borrado));
        AddInclude("CoordinacionesPMA.Provincia");
        AddInclude("CoordinacionesPMA.Municipio");
        AddInclude("CoordinacionesPMA.Archivo");

        AddInclude(d => d.ActivacionPlanEmergencias.Where(dir => !dir.Borrado));
        AddInclude("ActivacionPlanEmergencias.TipoPlan");
        AddInclude("ActivacionPlanEmergencias.PlanEmergencia");
        AddInclude("ActivacionPlanEmergencias.Archivo");

        AddInclude(d => d.ActivacionSistemas.Where(dir => !dir.Borrado));
        AddInclude("ActivacionSistemas.TipoSistemaEmergencia");
        AddInclude("ActivacionSistemas.ModoActivacion");


    }
}

