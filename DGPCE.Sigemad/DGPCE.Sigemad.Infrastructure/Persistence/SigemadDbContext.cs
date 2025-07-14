using DGPCE.Sigemad.Application.Contracts.Identity;
using DGPCE.Sigemad.Application.Features.AspNetUsers.Vms;
using DGPCE.Sigemad.Domain;
using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Modelos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Menu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace DGPCE.Sigemad.Infrastructure.Persistence
{
    public class SigemadDbContext : DbContext
    {
        private readonly IAuthService _authService;

        public SigemadDbContext(DbContextOptions<SigemadDbContext> options, IAuthService authService) : base(options)
        {
            _authService = authService;
        }

        
        public DbSet<AspNetRole> AspNetRoles { get; set; }
        

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.FechaCreacion = DateTime.Now;
                        entry.Entity.CreadoPor = _authService.GetCurrentUserId().ToString();
                        break;

                    case EntityState.Modified:
                        entry.Entity.FechaModificacion = DateTime.Now;
                        entry.Entity.ModificadoPor = _authService.GetCurrentUserId().ToString();
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.Borrado = true;
                        entry.Entity.FechaEliminacion = DateTime.Now;
                        entry.Entity.EliminadoPor = _authService.GetCurrentUserId().ToString();
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Automatically apply HasTrigger to all entities
            modelBuilder.ApplyHasTrigger();

            modelBuilder.Entity<Incendio>()
            .Property(i => i.FechaInicio)
            .HasColumnType("datetimeoffset");
            base.OnModelCreating(modelBuilder);


            /***********************************************************************************************************************/


            modelBuilder.Entity<AspNetUserRol>(b =>
            {
                b.HasKey(ur => new { ur.UserId, ur.RoleId });
            });





















            /***********************************************************************************************************************/



            modelBuilder.Entity<AspNetRole>().ToTable("AspNetRoles");
            modelBuilder.Entity<TipoSuceso>().ToTable("TipoSuceso");
            modelBuilder.Entity<ClaseSuceso>().ToTable("ClaseSuceso");
            modelBuilder.Entity<Menu>().ToTable("Menu");
            modelBuilder.Entity<Territorio>().ToTable("Territorio");
            modelBuilder.Entity<NivelGravedad>().ToTable("NivelGravedad");
            modelBuilder.Entity<EstadoIncendio>().ToTable("EstadoIncendio");
            modelBuilder.Entity<TipoMovimiento>().ToTable("TipoMovimiento");
            modelBuilder.Entity<ComparativaFecha>().ToTable("ComparativaFecha");
            modelBuilder.Entity<Medio>().ToTable("Medio");
            modelBuilder.Entity<EntradaSalida>().ToTable("EntradaSalida");
            modelBuilder.Entity<ProcedenciaDestino>().ToTable("ProcedenciaDestino");
            modelBuilder.Entity<Pais>().ToTable("Pais");
            modelBuilder.Entity<EstadoSuceso>().ToTable("EstadoSuceso");
            modelBuilder.Entity<TipoRegistro>().ToTable("TipoRegistro");
            modelBuilder.Entity<ImpactoClasificado>().ToTable("ImpactoClasificado");
            modelBuilder.Entity<CaracterMedio>().ToTable("CaracterMedio");
            modelBuilder.Entity<ClasificacionMedio>().ToTable("ClasificacionMedio");
            modelBuilder.Entity<TitularidadMedio>().ToTable("TitularidadMedio");
            modelBuilder.Entity<TipoEntidadTitularidadMedio>().ToTable("TipoEntidadTitularidadMedio");
            modelBuilder.Entity<TipoPlan>().ToTable("TipoPlan");
            modelBuilder.Entity<ValidacionImpactoClasificado>().ToTable("ValidacionImpactoClasificado");
            modelBuilder.Entity<TipoDanio>().ToTable("TipoDanio");
            modelBuilder.Entity<SuperficieFiltro>().ToTable(nameof(SuperficieFiltro));
            modelBuilder.Entity<SituacionOperativa>().ToTable(nameof(SituacionOperativa));
            modelBuilder.Entity<SucesoRelacionado>().ToTable(nameof(SucesoRelacionado));
            modelBuilder.Entity<Archivo>().ToTable(nameof(Archivo));
            modelBuilder.Entity<TipoDocumento>().ToTable(nameof(TipoDocumento));
            modelBuilder.Entity<AmbitoPlan>().ToTable(nameof(AmbitoPlan));
            modelBuilder.Entity<SituacionEquivalente>().ToTable(nameof(SituacionEquivalente));
            modelBuilder.Entity<EstadoMovilizacion>().ToTable(nameof(EstadoMovilizacion));
            modelBuilder.Entity<TipoCapacidad>().ToTable(nameof(TipoCapacidad));
            modelBuilder.Entity<GrupoMedio>().ToTable(nameof(GrupoMedio));
            modelBuilder.Entity<TipoFiltro>().ToTable(nameof(TipoFiltro));
            modelBuilder.Entity<TipoGestionDireccion>().ToTable(nameof(TipoGestionDireccion));
            modelBuilder.Entity<GrupoImpacto>().ToTable(nameof(GrupoImpacto));
            modelBuilder.Entity<TipoImpacto>().ToTable(nameof(TipoImpacto));
            modelBuilder.Entity<ClaseImpacto>().ToTable(nameof(ClaseImpacto));
            modelBuilder.Entity<TipoActuacion>().ToTable(nameof(TipoActuacion));

            // PCD
            // OPE - MENU
            modelBuilder.Entity<OpeMenu>().ToTable("OPE_Menu");
            // OPE - ADMINISTRACION
            modelBuilder.Entity<OpePeriodo>().ToTable("OPE_Periodo");
            modelBuilder.Entity<OpePeriodoTipo>().ToTable("OPE_PeriodoTipo");
            modelBuilder.Entity<OpePuerto>().ToTable("OPE_Puerto");
            modelBuilder.Entity<OpeFase>().ToTable("OPE_Fase");
            modelBuilder.Entity<OpePais>().ToTable("OPE_Pais");
            modelBuilder.Entity<OpeLineaMaritima>().ToTable("OPE_LineaMaritima");
            modelBuilder.Entity<OpeFrontera>().ToTable("OPE_Frontera");
            modelBuilder.Entity<OpeAreaDescanso>().ToTable("OPE_AreaDescanso");
            modelBuilder.Entity<OpeAreaDescansoTipo>().ToTable("OPE_AreaDescansoTipo");
            modelBuilder.Entity<OpeEstadoOcupacion>().ToTable("OPE_EstadoOcupacion");
            modelBuilder.Entity<OpePuntoControlCarretera>().ToTable("OPE_PuntoControlCarretera");
            modelBuilder.Entity<OpeOcupacion>().ToTable("OPE_Ocupacion");
            modelBuilder.Entity<OpePorcentajeOcupacionAreaEstacionamiento>().ToTable("OPE_PorcentajeOcupacionAreaEstacionamiento");
            modelBuilder.Entity<OpeAreaEstacionamiento>().ToTable("OPE_AreaEstacionamiento");
            // OPE - DATOS
            modelBuilder.Entity<OpeDatoFrontera>().ToTable("OPE_DatoFrontera");
            modelBuilder.Entity<OpeDatoFronteraIntervaloHorario>().ToTable("OPE_DatoFronteraIntervaloHorario");
            modelBuilder.Entity<OpeDatoEmbarqueDiario>().ToTable("OPE_DatoEmbarqueDiario");
            /* asistencias */
            modelBuilder.Entity<OpeDatoAsistencia>().ToTable("OPE_DatoAsistencia");
            modelBuilder.Entity<OpeAsistenciaSanitariaTipo>().ToTable("OPE_AsistenciaSanitariaTipo");
            modelBuilder.Entity<OpeAsistenciaSocialTipo>().ToTable("OPE_AsistenciaSocialTipo");
            modelBuilder.Entity<OpeDatoAsistenciaSanitaria>().ToTable("OPE_DatoAsistenciaSanitaria");
            modelBuilder.Entity<OpeDatoAsistenciaSocial>().ToTable("OPE_DatoAsistenciaSocial");
            modelBuilder.Entity<OpeDatoAsistenciaTraduccion>().ToTable("OPE_DatoAsistenciaTraduccion");
            modelBuilder.Entity<OpeAsistenciaSocialTareaTipo>().ToTable("OPE_AsistenciaSocialTareaTipo");
            modelBuilder.Entity<OpeAsistenciaSocialOrganismoTipo>().ToTable("OPE_AsistenciaSocialOrganismoTipo");
            modelBuilder.Entity<OpeAsistenciaSocialNacionalidad>().ToTable("OPE_AsistenciaSocialNacionalidad");
            modelBuilder.Entity<OpeAsistenciaSocialSexo>().ToTable("OPE_AsistenciaSocialSexo");
            modelBuilder.Entity<OpeDatoAsistenciaSocialTarea>().ToTable("OPE_DatoAsistenciaSocialTarea");
            modelBuilder.Entity<OpeDatoAsistenciaSocialOrganismo>().ToTable("OPE_DatoAsistenciaSocialOrganismo");
            //modelBuilder.Entity<OpeDatoAsistenciaSocialUsuario>().ToTable("OPE_DatoAsistenciaSocialUsuario");
            modelBuilder.Entity<OpeAsistenciaSocialEdad>().ToTable("OPE_AsistenciaSocialEdad");
            // FIN PCD

            modelBuilder.Entity<ZonaPlanificacion>().ToTable("ZonaPlanificacion");
            modelBuilder.Entity<AlteracionInterrupcion>().ToTable("AlteracionInterrupcion");
            modelBuilder.Entity<Auditoria_Incendio>()
                .ToTable("Auditoria_Incendio");

            //modelBuilder.Entity<Auditoria_Parametro>()
            //    .ToTable("Auditoria_Parametro");

            modelBuilder.Entity<AuditoriaActivacionPlanEmergencia>()
                .ToTable("Auditoria_ActivacionPlanEmergencia");
            
            modelBuilder.Entity<AuditoriaActivacionSistema>()
                .ToTable("Auditoria_ActivacionSistema");

            modelBuilder.Entity<AuditoriaConvocatoriaCECOD>()
                .ToTable("Auditoria_ConvocatoriaCECOD");

            modelBuilder.Entity<AuditoriaDeclaracionZAGEP>()
                .ToTable("Auditoria_DeclaracionZAGEP");

            modelBuilder.Entity<AuditoriaEmergenciaNacional>()
              .ToTable("Auditoria_EmergenciaNacional");

            modelBuilder.Entity<AuditoriaMovilizacionMedio>()
              .ToTable("Auditoria_MovilizacionMedio");

            modelBuilder.Entity<AuditoriaSolicitudMedio>()
              .ToTable("Auditoria_SolicitudMedio");

            modelBuilder.Entity<AuditoriaEjecucionPaso>()
              .ToTable("Auditoria_EjecucionPaso");

            modelBuilder.Entity<AuditoriaTramitacionMedio>()
    .ToTable("Auditoria_TramitacionMedio");

            modelBuilder.Entity<AuditoriaCancelacionMedio>()
                .ToTable("Auditoria_CancelacionMedio");

            modelBuilder.Entity<AuditoriaOfrecimientoMedio>()
                .ToTable("Auditoria_OfrecimientoMedio");

            modelBuilder.Entity<AuditoriaAportacionMedio>()
                .ToTable("Auditoria_AportacionMedio");

            modelBuilder.Entity<AuditoriaDespliegueMedio>()
                .ToTable("Auditoria_DespliegueMedio");

            modelBuilder.Entity<AuditoriaFinIntervencionMedio>()
                .ToTable("Auditoria_FinIntervencionMedio");

            modelBuilder.Entity<AuditoriaLlegadaBaseMedio>()
                .ToTable("Auditoria_LlegadaBaseMedio");


            modelBuilder.Entity<AuditoriaNotificacionEmergencia>()
              .ToTable("Auditoria_notificacionEmergencia");

            modelBuilder.Entity<AuditoriaSuceso>()
              .ToTable("Auditoria_Suceso");

            modelBuilder.Entity<AspNetUserRol>();

        }

        public DbSet<TipoSuceso> TiposSuceso { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Ccaa>? CCAA { get; set; }
        public DbSet<Territorio>? Territorios { get; set; }
        public DbSet<Provincia>? Provincias { get; set; }
        public DbSet<Municipio>? Municipios { get; set; }
        public DbSet<Suceso> Sucesos { get; set; }

        public DbSet<ClaseSuceso> ClasesSucesos { get; set; }
        public DbSet<Incendio> Incendios { get; set; }
        public DbSet<NivelGravedad> NivelesGravedad { get; set; }
        public DbSet<EstadoIncendio> EstadosIncendio { get; set; }
        public DbSet<TipoMovimiento> TipoMovimientos { get; set; }
        public DbSet<ComparativaFecha> ComparativaFechas { get; set; }
        public DbSet<Medio> Medios { get; set; }
        public DbSet<Pais> Paises { get; set; }
        public DbSet<ProcedenciaDestino> ProcedenciaDestinos { get; set; }
        public DbSet<EstadoSuceso> EstadosSucesos { get; set; }

        public DbSet<EntradaSalida> EntradasSalidas { get; set; }
        public DbSet<ImpactoClasificado> ImpactosClasificados { get; set; }
        public DbSet<ImpactoEvolucion> ImpactosEvoluciones {  get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<TipoRegistro> TiposRegistros { get; set; }
        public DbSet<Evolucion> Evoluciones { get; set; }
        public DbSet<AreaAfectada> AreaAfectadas { get; set; }
        public DbSet<IntervencionMedio> IntervencionMedios { get; set; }
        public DbSet<RegistroProcedenciaDestino> RegistroProcedenciasDestinos { get; set; }
        public DbSet<CaracterMedio> CaracterMedios { get; set; }
        public DbSet<ClasificacionMedio> ClasificacionMedios { get; set; }
        public DbSet<TitularidadMedio> TitularidadMedios { get; set; }
        public DbSet<TipoEntidadTitularidadMedio> tipoEntidadTitularidadMedios { get; set; }
        public DbSet<TipoIntervencionMedio> TipoIntervencionMedios { get; set; }
        public DbSet<TipoDireccionEmergencia> TipoDireccionEmergencias { get; set; }

        public DbSet<TipoPlan> TipoPlanes { get; set; }
        public DbSet<ActivacionPlanEmergencia> ActivacionPlanesEmergencias { get; set; }
        public DbSet<Direccion> Direccions { get; set; }
        public DbSet<CoordinacionCecopi> CoordinacionCecopis { get; set; }
        public DbSet<CoordinacionPMA> CoordinacionPMAs { get; set; }

        public DbSet<ValidacionImpactoClasificado> ValidacionImpactoClasificados { get; set; }

        public DbSet<TipoDanio> TipoDanios { get; set; }

        public DbSet<OtraInformacion> OtrasInformaciones { get; set; }
        public DbSet<DetalleOtraInformacion> DetallesOtraInformacion { get; set; }
        public DbSet<DetalleOtraInformacion_ProcedenciaDestino> DetallesOtraInformacion_ProcedenciaDestinos { get; set; }

        public DbSet<SucesoRelacionado> SucesosRelacionados { get; set; }
        public DbSet<FaseEmergencia> FasesEmergencia { get; set; }
        public DbSet<Registro> Registros { get; set; }

        public DbSet<Parametro> Parametro { get; set; }
        public DbSet<DatoPrincipal> DatoPrincipal { get; set; }

        public DbSet<TipoDocumento> TipoDocumentos { get; set; }
        public DbSet<Documentacion> Documentaciones { get; set; }
        public DbSet<DetalleDocumentacion> DetalleDocumentaciones { get; set; }
        public DbSet<DocumentacionProcedenciaDestino> DocumentacionProcedenciaDestinos { get; set; }
        public DbSet<AmbitoPlan> AmbitoPlanes { get; set; }
        public DbSet<TipoRiesgo> TipoRiesgos { get; set; }
        public DbSet<PlanEmergencia> PlanesEmergencias { get; set; }
        public DbSet<ModoActivacion> ModosActivacion { get; set; }

        public DbSet<TipoSistemaEmergencia> TiposSistemasEmergencias { get; set; }

        public DbSet<PlanSituacion> PlanesSituaciones { get; set; }

        public DbSet<ActuacionRelevanteDGPCE> ActuacionesRelevantesDGPCE { get; set; }

        public DbSet<EmergenciaNacional> EmergenciasNacionales { get; set; }
        public DbSet<SituacionEquivalente> SituacionEquivalentes { get; set; }

        public DbSet<DeclaracionZAGEP> DeclaracionesZAGEP { get; set; }

        public DbSet<ActivacionSistema> ActivacionesSistemas { get; set; }

        public DbSet<ConvocatoriaCECOD> ConvocatoriasCECOD { get; set; }

        public DbSet<NotificacionEmergencia> NotificacionesEmergencias { get; set; }

        public DbSet<TipoNotificacion> TiposNotificaciones { get; set; }
        public DbSet<TipoRegistroActualizacion> TipoRegistroActualizaciones { get; set; }
        public DbSet<ApartadoRegistro> ApartadosRegistro { get; set; }
        public DbSet<RegistroActualizacion> RegistrosActualizacion { get; set; }
        public DbSet<RegistroApartado> RegistrosApartados { get; set; }
        public DbSet<DetalleRegistroActualizacion> DetallesRegistroActualizacion { get; set; }
        public DbSet<HistorialCambios> HistorialCambios { get; set; }
        public DbSet<MediosCapacidad> MediosCapacidads { get; set; }



        // PCD
        // OPE - MENU
        public DbSet<OpeMenu> OpeMenus { get; set; }
        // OPE - ADMINISTRACIÓN
        public DbSet<OpePeriodo> OpePeriodos { get; set; }
        public DbSet<OpePeriodoTipo> OpePeriodosTipos { get; set; }
        public DbSet<OpePuerto> OpePuertos { get; set; }
        public DbSet<OpeFase> OpeFases { get; set; }
        public DbSet<OpePais> OpePaises { get; set; }
        public DbSet<OpeLineaMaritima> OpeLineasMaritimas { get; set; }
        public DbSet<OpeFrontera> OpeFronteras { get; set; }
        public DbSet<OpeAreaDescanso> OpeAreasDescanso { get; set; }
        public DbSet<OpeAreaDescansoTipo> OpeAreasDescansoTipos { get; set; }
        public DbSet<OpeEstadoOcupacion> OpeEstadosOcupacion { get; set; }
        public DbSet<OpePuntoControlCarretera> OpePuntosControlCarreteras { get; set; }
        public DbSet<OpeOcupacion> OpeOcupaciones { get; set; }
        public DbSet<OpePorcentajeOcupacionAreaEstacionamiento> OpePorcentajesOcupacionAreasEstacionamiento { get; set; }
        public DbSet<OpeAreaEstacionamiento> OpeAreasEstacionamiento { get; set; }
        // OPE - DATOS
        public DbSet<OpeDatoFrontera> OpeDatosFronteras { get; set; }
        public DbSet<OpeDatoFronteraIntervaloHorario> OpeDatosFronterasIntervalosHorarios { get; set; }
        public DbSet<OpeDatoEmbarqueDiario> OpeDatosEmbarquesDiarios { get; set; }
        public DbSet<OpeDatoAsistencia> OpeDatosAsistencias { get; set; }

        public DbSet<OpeDatoAsistenciaSanitaria> OpeDatosAsistenciasSanitarias { get; set; }
        public DbSet<OpeDatoAsistenciaSocial> OpeDatosAsistenciasSociales { get; set; }
        public DbSet<OpeDatoAsistenciaTraduccion> OpeDatosAsistenciasTraducciones { get; set; }
        // FIN PCD

        public DbSet<ZonaPlanificacion> ZonaPlanificacions { get; set; }
        public DbSet<AlteracionInterrupcion> AlteracionInterrupcions { get; set; }
        //Auditoria
        public DbSet<Auditoria_Incendio> AuditoriaIncendios { get; set; }
        public DbSet<Auditoria_Parametro> AuditoriaParametros { get; set; }
        public DbSet<AuditoriaActivacionPlanEmergencia> AuditoriaActivacionPlanesEmergencia { get; set; }
        public DbSet<AuditoriaActivacionSistema> AuditoriaActivacionSistema { get; set; }
        public DbSet<AuditoriaConvocatoriaCECOD> AuditoriaConvocatoriaCECOD { get; set; }
        public DbSet<AuditoriaDeclaracionZAGEP> AuditoriaDeclaracionZAGEP { get; set; }
        public DbSet<AuditoriaEmergenciaNacional> AuditoriaEmergenciaNacional { get; set; }
        public DbSet<AuditoriaMovilizacionMedio> AuditoriaMovilizacionMedio { get; set; }
        public DbSet<AuditoriaNotificacionEmergencia> AuditoriaNotificacionEmergencia { get; set; }

        public DbSet<AuditoriaEjecucionPaso> AuditoriaEjecucionPaso { get; set; }
        public DbSet<AuditoriaSolicitudMedio> AuditoriaSolicitudMedio { get; set; }
       public DbSet<AuditoriaTramitacionMedio> AuditoriaTramitacionMedio { get; set; }
        public DbSet<AuditoriaCancelacionMedio> AuditoriaCancelacionMedio { get; set; }
        public DbSet<AuditoriaOfrecimientoMedio> AuditoriaOfrecimientoMedio { get; set; }
        public DbSet<AuditoriaAportacionMedio> AuditoriaAportacionMedio { get; set; }
        public DbSet<AuditoriaDespliegueMedio> AuditoriaDespliegueMedio { get; set; }
        public DbSet<AuditoriaFinIntervencionMedio> AuditoriaFinIntervencionMedio { get; set; }
        public DbSet<AuditoriaLlegadaBaseMedio> AuditoriaLlegadaBaseMedio { get; set; }

        public DbSet<AuditoriaSuceso> AuditoriaSuceso { get; set; }

        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<AspNetUserRol> AspNetUserRoles { get; set; }

        public DbSet<TipoFiltro> TipoFiltros { get; set; }

        public DbSet<GrupoImpacto> GrupoImpactos { get; set; }

        public DbSet<GrIconoImpacto> GrIconoImpactos { get; set; }

        public DbSet<SubgrupoImpacto> SubgrupoImpactos { get; set; }
        public DbSet<TipoImpacto> TipoImpactos { get; set; }
        public DbSet<ClaseImpacto> ClasesImpactos { get; set; }

        public DbSet<TipoActuacion> TiposActuaciones { get; set; }

        public DbSet<TipoImpactoEvolucion> TipoImpactoEvoluciones { get; set; }


    }


}
