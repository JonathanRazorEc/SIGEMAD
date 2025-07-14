-- ELIMINAR TABLAS CON DEPENDENCIAS

-- ELIMINAR TABLAS DE CATÁLOGO

DROP TABLE IF EXISTS ColumnasTM
GO

DROP TABLE IF EXISTS TablasMaestras
GO

DROP TABLE IF EXISTS TablasMaestrasGrupos
GO

--ELIMINAR TABLAS DE AUDITORIA
DROP TRIGGER IF EXISTS trg_Auditoria_Incendio
GO

DROP TABLE IF EXISTS Auditoria_Incendio
GO

DROP TRIGGER IF EXISTS trg_Auditoria_Suceso
GO

DROP TABLE IF EXISTS Auditoria_Suceso
GO

DROP TRIGGER IF EXISTS trg_Auditoria_Evolucion
GO

DROP TABLE IF EXISTS Auditoria_Evolucion
GO

DROP TRIGGER IF EXISTS trg_Auditoria_Parametro
GO

DROP TABLE IF EXISTS Auditoria_Parametro
GO

DROP TRIGGER IF EXISTS trg_Auditoria_Registro
GO

DROP TABLE IF EXISTS Auditoria_Registro  
GO

DROP TRIGGER IF EXISTS trg_Auditoria_ActuacionRelevanteDGPCE
GO

DROP TABLE IF EXISTS Auditoria_ActuacionRelevanteDGPCE  
GO

DROP TRIGGER IF EXISTS trg_Auditoria_EmergenciaNacional
GO

DROP TABLE IF EXISTS Auditoria_EmergenciaNacional  
GO

DROP TRIGGER IF EXISTS trg_Auditoria_Activacion_ActivacionPlanEmergencia
GO

DROP TABLE IF EXISTS Auditoria_ActivacionPlanEmergencia  
GO

DROP TRIGGER IF EXISTS trg_Auditoria_DeclaracionZAGEP
GO

DROP TABLE IF EXISTS Auditoria_DeclaracionZAGEP  
GO

DROP TRIGGER IF EXISTS trg_Auditoria_ActivacionSistema
GO

DROP TABLE IF EXISTS Auditoria_ActivacionSistema  
GO

DROP TRIGGER IF EXISTS trg_Auditoria_ConvocatoriaCECOD
GO

DROP TABLE IF EXISTS Auditoria_ConvocatoriaCECOD  
GO

DROP TRIGGER IF EXISTS trg_Auditoria_NotificacionEmergencia
GO

DROP TABLE IF EXISTS Auditoria_NotificacionEmergencia  
GO

DROP TRIGGER IF EXISTS trg_Auditoria_Archivo
GO

DROP TABLE IF EXISTS Auditoria_Archivo  
GO

DROP TRIGGER IF EXISTS trg_Auditoria_MovilizacionMedio
GO

DROP TABLE IF EXISTS Auditoria_MovilizacionMedio  
GO

DROP TRIGGER IF EXISTS trg_Auditoria_EjecucionPaso
GO

DROP TABLE IF EXISTS Auditoria_EjecucionPaso  
GO

DROP TRIGGER IF EXISTS trg_Auditoria_SolicitudMedio
GO

DROP TABLE IF EXISTS Auditoria_SolicitudMedio  
GO

DROP TRIGGER IF EXISTS trg_Auditoria_TramitacionMedio
GO

DROP TABLE IF EXISTS Auditoria_TramitacionMedio  
GO

DROP TRIGGER IF EXISTS trg_Auditoria_CancelacionMedio
GO

DROP TABLE IF EXISTS Auditoria_CancelacionMedio  
GO

DROP TRIGGER IF EXISTS trg_Auditoria_OfrecimientoMedio
GO

DROP TABLE IF EXISTS Auditoria_OfrecimientoMedio  
GO

DROP TRIGGER IF EXISTS trg_Auditoria_AportacionMedio
GO

DROP TABLE IF EXISTS Auditoria_AportacionMedio  
GO

DROP TRIGGER IF EXISTS trg_Auditoria_DespliegueMedio
GO

DROP TABLE IF EXISTS Auditoria_DespliegueMedio  
GO

DROP TRIGGER IF EXISTS trg_Auditoria_FinIntervencionMedio
GO

DROP TABLE IF EXISTS Auditoria_FinIntervencionMedio  
GO

DROP TRIGGER IF EXISTS trg_Auditoria_LlegadaBaseMedio
GO

DROP TABLE IF EXISTS Auditoria_LlegadaBaseMedio  
GO

-- ELIMINAR TABLAS OPE
-- Auditorias OPE
DROP TRIGGER IF EXISTS trg_Auditoria_OPE_Periodo
GO
DROP TABLE IF EXISTS Auditoria_OPE_Periodo
GO

DROP TRIGGER IF EXISTS trg_Auditoria_OPE_Puerto
GO
DROP TABLE IF EXISTS Auditoria_OPE_Puerto
GO

DROP TRIGGER IF EXISTS trg_Auditoria_OPE_LineaMaritima
GO
DROP TABLE IF EXISTS Auditoria_OPE_LineaMaritima
GO

DROP TRIGGER IF EXISTS trg_Auditoria_OPE_Frontera
GO
DROP TABLE IF EXISTS Auditoria_OPE_Frontera
GO

DROP TRIGGER IF EXISTS trg_Auditoria_OPE_PuntoControlCarretera
GO
DROP TABLE IF EXISTS Auditoria_OPE_PuntoControlCarretera
GO

DROP TRIGGER IF EXISTS trg_Auditoria_OPE_AreaDescanso
GO
DROP TABLE IF EXISTS Auditoria_OPE_AreaDescanso
GO

DROP TRIGGER IF EXISTS trg_Auditoria_OPE_AreaEstacionamiento
GO
DROP TABLE IF EXISTS Auditoria_OPE_AreaEstacionamiento
GO

DROP TRIGGER IF EXISTS trg_Auditoria_OPE_PorcentajeOcupacionAreaEstacionamiento
GO
DROP TABLE IF EXISTS Auditoria_OPE_PorcentajeOcupacionAreaEstacionamiento
GO

DROP TRIGGER IF EXISTS trg_Auditoria_OPE_DatoEmbarqueDiario
GO
DROP TABLE IF EXISTS Auditoria_OPE_DatoEmbarqueDiario
GO

DROP TRIGGER IF EXISTS trg_Auditoria_OPE_DatoAsistencia
GO
DROP TABLE IF EXISTS Auditoria_OPE_DatoAsistencia
GO

DROP TRIGGER IF EXISTS trg_Auditoria_OPE_DatoFrontera
GO
DROP TABLE IF EXISTS Auditoria_OPE_DatoFrontera
GO
-- Fin auditorias OPE

----------------------------
---    HISTORICO SIGE 2 ----
----------------------------

DROP TABLE IF EXISTS OPE_HIST_AsistenciaSocialDemandante
GO

DROP TABLE IF EXISTS OPE_HIST_TipoSexoDemandante
GO

DROP TABLE IF EXISTS OPE_HIST_TipoPaisResidencia
GO

DROP TABLE IF EXISTS OPE_HIST_TipoNacionalidadDemandante
GO

DROP TABLE IF EXISTS OPE_HIST_TipoEdadDemandante
GO

DROP TABLE IF EXISTS OPE_HIST_AsistenciaSocialTarea
GO

DROP TABLE IF EXISTS OPE_HIST_TipoAsistenciaTarea
GO

DROP TABLE IF EXISTS OPE_HIST_AsistenciaSocialOrganismo
GO

DROP TABLE IF EXISTS OPE_HIST_TipoOrganismo
GO

DROP TABLE IF EXISTS OPE_HIST_AsistenciaSocial
GO

DROP TABLE IF EXISTS OPE_HIST_TipoAsistenciaSocial
GO

DROP TABLE IF EXISTS OPE_HIST_AsistenciaSanitaria
GO

DROP TABLE IF EXISTS OPE_HIST_TipoAsistenciaSanitaria
GO

DROP TABLE IF EXISTS OPE_HIST_AsistenciaTraduccion
GO

DROP TABLE IF EXISTS OPE_HIST_Asistencia
GO

DROP TABLE IF EXISTS OPE_HIST_AfluenciaPuntoControlDestino
GO

DROP TABLE IF EXISTS OPE_HIST_AfluenciaPuntoControl
GO

DROP TABLE IF EXISTS OPE_HIST_PuntoControl
GO

DROP TABLE IF EXISTS OPE_HIST_CapacidadAparcamiento
GO

DROP TABLE IF EXISTS OPE_HIST_OcupacionAreaEstacionamientoDestino
GO

DROP TABLE IF EXISTS OPE_HIST_OcupacionAreaEstacionamiento
GO

DROP TABLE IF EXISTS OPE_HIST_TipoOcupacion
GO

DROP TABLE IF EXISTS OPE_HIST_AreaEstacionamiento
GO

DROP TABLE IF EXISTS OPE_HIST_EmbarqueIntervaloHorarioDestino
GO

DROP TABLE IF EXISTS OPE_HIST_EmbarqueIntervaloHorario
GO

DROP TABLE IF EXISTS OPE_HIST_TipoHoraPuntoControl
GO


DROP TABLE IF EXISTS OPE_HIST_DatoEmbarque
GO

DROP TABLE IF EXISTS OPE_HIST_Linea
GO

DROP TABLE IF EXISTS OPE_HIST_Puerto
GO

DROP TABLE IF EXISTS OPE_HIST_TipoPaisPuerto
GO

DROP TABLE IF EXISTS OPE_HIST_TipoOperacion
GO

DROP TABLE IF EXISTS OPE_HIST_DatoFrontera
GO

DROP TABLE IF EXISTS OPE_HIST_TipoHoraFrontera
GO

DROP TABLE IF EXISTS OPE_HIST_Frontera
GO


DROP TABLE IF EXISTS OPE_HIST_PeriodoDetalle
GO

DROP TABLE IF EXISTS OPE_HIST_Periodo
GO













DROP TABLE IF EXISTS OPE_HIST_Frontera
GO

DROP TABLE IF EXISTS OPE_HIST_LineaMaritima
GO

-----------------------------
---  FINHISTORICO SIGE 2 ----
-----------------------------

DROP TABLE IF EXISTS OPE_AreaEstacionamiento
GO


DROP TABLE IF EXISTS OPE_Menu
GO

DROP TABLE IF EXISTS OPE_DatoEmbarqueDiario
GO

DROP TABLE IF EXISTS OPE_DatoFrontera
GO

DROP TABLE IF EXISTS OPE_DatoAsistenciaSocialTarea
GO

DROP TABLE IF EXISTS OPE_DatoAsistenciaSocialOrganismo
GO

DROP TABLE IF EXISTS OPE_DatoAsistenciaSocialUsuario
GO

DROP TABLE IF EXISTS OPE_DatoAsistenciaSanitaria
GO

DROP TABLE IF EXISTS OPE_DatoAsistenciaSocial
GO

DROP TABLE IF EXISTS OPE_DatoAsistenciaTraduccion
GO

DROP TABLE IF EXISTS OPE_DatoAsistencia
GO

DROP TABLE IF EXISTS OPE_AsistenciaSanitariaTipo
GO

DROP TABLE IF EXISTS OPE_AsistenciaSocialTipo
GO

DROP TABLE IF EXISTS OPE_AsistenciaSocialTareaTipo
GO

DROP TABLE IF EXISTS OPE_AsistenciaSocialOrganismoTipo
GO


DROP TABLE IF EXISTS OPE_AreaDescanso
GO

DROP TABLE IF EXISTS OPE_PorcentajeOcupacionAreaEstacionamiento
GO

DROP TABLE IF EXISTS OPE_PuntoControlCarretera
GO

DROP TABLE IF EXISTS OPE_LineaMaritima
GO

DROP TABLE IF EXISTS OPE_Puerto
GO

DROP TABLE IF EXISTS OPE_Periodo
GO

DROP TABLE IF EXISTS OPE_EstadoOcupacion
GO

DROP TABLE IF EXISTS OPE_AreaDescansoTipo
GO

DROP TABLE IF EXISTS OPE_Ocupacion
GO

DROP TABLE IF EXISTS OPE_Fase
GO

DROP TABLE IF EXISTS OPE_PeriodoTipo
GO


DROP TABLE IF EXISTS OPE_AsistenciaSocialNacionalidad
GO

DROP TABLE IF EXISTS OPE_AsistenciaSocialEdad
GO

DROP TABLE IF EXISTS OPE_AsistenciaSocialSexo
GO

DROP TABLE IF EXISTS OPE_Pais
GO


DROP TABLE IF EXISTS OPE_DatoFronteraIntervaloHorario
GO

DROP TABLE IF EXISTS OPE_Frontera
GO

-- FIN ELIMINAR TABLAS OPE


-- ---------------------------------------------------
DROP TABLE IF EXISTS DetalleIntervencionMedio
GO

DROP TABLE IF EXISTS IntervencionMedio
GO

DROP TABLE IF EXISTS LlegadaBaseMedio
GO

DROP TABLE IF EXISTS FinIntervencionMedio
GO

DROP TABLE IF EXISTS DespliegueMedio
GO

DROP TABLE IF EXISTS AportacionMedio
GO

DROP TABLE IF EXISTS OfrecimientoMedio
GO

DROP TABLE IF EXISTS CancelacionMedio
GO

DROP TABLE IF EXISTS TramitacionMedio
GO

DROP TABLE IF EXISTS SolicitudMedio
GO

DROP TABLE IF EXISTS EjecucionPaso
GO

DROP TABLE IF EXISTS MovilizacionMedio
GO

DROP TABLE IF EXISTS Capacidad
GO

DROP TABLE IF EXISTS Entidad
GO

DROP TABLE IF EXISTS Organismo
GO

DROP TABLE IF EXISTS Administracion
GO

DROP TABLE IF EXISTS TipoAdministracion
GO

DROP TABLE IF EXISTS FlujoPasoMovilizacion
GO

DROP TABLE IF EXISTS PasoMovilizacion
GO

DROP TABLE IF EXISTS EstadoMovilizacion
GO

DROP TABLE IF EXISTS DestinoMedio
GO

DROP TABLE IF EXISTS ProcedenciaMedio
GO

DROP TABLE IF EXISTS EmergenciaNacional
GO

DROP TABLE IF EXISTS DeclaracionZAGEP
GO

DROP TABLE IF EXISTS ActivacionSistema
GO

DROP TABLE IF EXISTS NotificacionEmergencia
GO

DROP TABLE IF EXISTS ActivacionPlanEmergencia
GO

DROP TABLE IF EXISTS ConvocatoriaCECOD
GO

DROP TABLE IF EXISTS ActuacionRelevanteDGPCE
GO

DROP TABLE IF EXISTS ModoActivacion
GO


DROP TABLE IF EXISTS ActivacionPlanEmergencia
GO

DROP TABLE IF EXISTS ActivacionPlanesEmergencia
GO

DROP TABLE IF EXISTS TipoSistemaEmergenciaTipoSuceso
GO

DROP TABLE IF EXISTS TipoSistemaEmergencia
GO

DROP TABLE IF EXISTS TIpoSistemaEmergencia
GO

DROP TABLE IF EXISTS Documentacion_ProcedenciaDestino
GO

DROP TABLE IF EXISTS DetalleDocumentacion
GO

DROP TABLE IF EXISTS Documentacion
GO

DROP TABLE IF EXISTS TipoDocumento
GO

DROP TABLE IF EXISTS DetalleSucesoRelacionado
GO

DROP TABLE IF EXISTS SucesoRelacionado
GO

DROP TABLE IF EXISTS OtraInformacion_ProcedenciaDestino
GO

DROP TABLE IF EXISTS DetalleOtraInformacion_ProcedenciaDestino
GO

DROP TABLE IF EXISTS DetalleOtraInformacion
GO

DROP TABLE IF EXISTS OtraInformacion
GO

DROP TABLE IF EXISTS SuperficieFiltro
GO

DROP TABLE IF EXISTS TipoFiltro
GO

DROP TABLE IF EXISTS CoordinacionCecopi
GO

DROP TABLE IF EXISTS CoordinacionPMA
GO

DROP TABLE IF EXISTS Direccion
GO

DROP TABLE IF EXISTS Archivo
GO


DROP TABLE IF EXISTS DireccionCoordinacionEmergencia
GO

DROP TABLE IF EXISTS TipoDireccionEmergencia
GO

DROP TABLE IF EXISTS TipoGestionDireccion
GO

DROP TABLE IF EXISTS TipoIntervencionMedio
GO

DROP TABLE IF EXISTS TipoEntidadTitularidadMedio
GO

DROP TABLE IF EXISTS ClasificacionMedio
GO

DROP TABLE IF EXISTS TitularidadMedio
GO

DROP TABLE IF EXISTS CaracterMedio
GO

DROP TABLE IF EXISTS MediosCapacidad
GO

DROP TABLE IF EXISTS TipoMedio
GO

DROP TABLE IF EXISTS SubgrupoMedio
GO

DROP TABLE IF EXISTS GrupoMedio
GO

DROP TABLE IF EXISTS TipoCapacidad
GO

DROP TABLE IF EXISTS ImpactoEvolucion
GO

DROP TABLE IF EXISTS TipoImpactoEvolucion
GO

DROP TABLE IF EXISTS AlteracionInterrupcion
GO

DROP TABLE IF EXISTS ZonaPlanificacion
GO

DROP TABLE IF EXISTS TipoDanio
GO


DROP TABLE IF EXISTS ValidacionImpactoClasificado
GO

DROP TABLE IF EXISTS ImpactoClasificado
GO

DROP TABLE IF EXISTS TipoActuacion
GO

DROP TABLE IF EXISTS ClaseImpacto
GO

DROP TABLE IF EXISTS CategoriaImpacto
GO

DROP TABLE IF EXISTS TipoImpacto
GO

DROP TABLE IF EXISTS SubgrupoImpacto
GO

DROP TABLE IF EXISTS GrIconoImpacto
GO

DROP TABLE IF EXISTS GrupoImpacto
GO


DROP TABLE IF EXISTS AreaAfectada
GO

DROP TABLE IF EXISTS Parametro
GO

DROP TABLE IF EXISTS DatoPrincipal
GO

DROP TABLE IF EXISTS RegistroEvolucion_ProcedenciaDestino
GO

DROP TABLE IF EXISTS Registro_ProcedenciaDestino
GO

DROP TABLE IF EXISTS Datoprincipal
GO

DROP TABLE IF EXISTS Registro
GO

DROP TABLE IF EXISTS Evolucion_ProcedenciaDestino
GO

DROP TABLE IF EXISTS DatoPrincipal
GO

DROP TABLE IF EXISTS Parametro
GO

DROP TABLE IF EXISTS Evolucion
GO

DROP TABLE IF EXISTS PlanSituacion
GO

DROP TABLE IF EXISTS FaseEmergencia
GO

DROP TABLE IF EXISTS PlanEmergencia
GO

DROP TABLE IF EXISTS TipoNotificacion
GO

DROP TABLE IF EXISTS TipoPlanMapeo
GO

DROP TABLE IF EXISTS AmbitoPlan
GO

DROP TABLE IF EXISTS TipoRiesgo
GO

DROP TABLE IF EXISTS TipoPlanMapeo
GO

DROP TABLE IF EXISTS TipoPlan
GO

DROP TABLE IF EXISTS SituacionOperativa
GO

DROP TABLE IF EXISTS Fase
GO

DROP TABLE IF EXISTS SituacionEquivalente
GO

DROP TABLE IF EXISTS TipoRegistro_
GO

DROP TABLE IF EXISTS TipoRegistro
GO

DROP TABLE IF EXISTS EstadoEvolucion
GO

DROP TABLE IF EXISTS ProcedenciaDestino
GO

DROP TABLE IF EXISTS Medio
GO

DROP TABLE IF EXISTS EntradaSalida
GO

DROP TABLE IF EXISTS ComparativaFecha
GO

DROP TABLE IF EXISTS TipoMovimiento
GO

DROP TABLE IF EXISTS IncendioExtranjero
GO

DROP TABLE IF EXISTS IncendioNacional
GO

DROP TABLE IF EXISTS Incendio
GO

DROP TABLE IF EXISTS EstadoIncendio
GO

DROP TABLE IF EXISTS ClaseSuceso
GO

DROP TABLE IF EXISTS Menu
GO

DROP TABLE IF EXISTS EstadoSuceso
GO

DROP TABLE IF EXISTS TipoRiesgo
GO

DROP TABLE IF EXISTS HistorialCambios
GO

DROP TABLE IF EXISTS DetalleRegistroActualizacion
GO

DROP TABLE IF EXISTS RegistroApartado
GO

DROP TABLE IF EXISTS RegistroActualizacion
GO

DROP TABLE IF EXISTS EstadoRegistro
GO

DROP TABLE IF EXISTS ApartadoRegistro
GO

DROP TABLE IF EXISTS TipoRegistroActualizacion
GO

DROP TABLE IF EXISTS Suceso
GO

DROP TABLE IF EXISTS TipoSuceso
GO

DROP TABLE IF EXISTS NivelGravedad
GO

DROP TABLE IF EXISTS MunicipioExtranjero
GO

DROP TABLE IF EXISTS EntidadMenor
GO

DROP TABLE IF EXISTS Municipio
GO

DROP TABLE IF EXISTS Provincia
GO

DROP TABLE IF EXISTS CCAA
GO

DROP TABLE IF EXISTS Distrito
GO

DROP TABLE IF EXISTS Pais
GO

DROP TABLE IF EXISTS Territorio
GO



-- ELIMINAR TABLAS SEGURIDAD

DROP TABLE IF EXISTS RefreshTokens
GO

DROP TABLE IF EXISTS AspNetUserTokens
GO

DROP TABLE IF EXISTS AspNetUserRoles
GO

DROP TABLE IF EXISTS AspNetUserLogins
GO

DROP TABLE IF EXISTS AspNetUserClaims
GO

DROP TABLE IF EXISTS AspNetRoleClaims
GO

DROP TABLE IF EXISTS AspNetUsers
GO

DROP TABLE IF EXISTS AspNetRoles
GO

DROP TABLE IF EXISTS ApplicationUsers
GO

DROP TABLE IF EXISTS __EFMigrationsHistory
GO



