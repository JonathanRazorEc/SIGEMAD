SET IDENTITY_INSERT TablasMaestras ON;

-- SIGEMAD - CATÁLOGO
INSERT INTO TablasMaestras (id, tabla, etiqueta, IdTablaMaestraGrupo, Listable, Editable) VALUES
(1, N'SituacionEquivalente',N'Situación operativa equivalente',1,1,1),
(2, N'TipoDocumento',N'Tipo de documento',1,1,1),
(3, N'ProcedenciaDestino',N'Procedencia y destino',1,1,1),
(4, N'Capacidad',N'Capacidades',1,1,1),
(5, N'EstadoIncendio',N'Estados de un incendio',1,1,1);

-- OPE - CATÁLOGO
INSERT INTO TablasMaestras (id, tabla, etiqueta, IdTablaMaestraGrupo, Listable, Editable) VALUES
(7, N'OPE_Fase',N'Fases',3,1,1),
(8, N'OPE_PeriodoTipo',N'Tipos de periodos',3,1,1),
(9, N'OPE_EstadoOcupacion',N'Estados de ocupación',3,1,1),
(10, N'OPE_AreaDescansoTipo',N'Tipos área descanso',3,1,1),
(11, N'OPE_AsistenciaSanitariaTipo',N'Tipos de asistencia sanitaria',3,1,1),
(12,N'OPE_AsistenciaSocialTipo',N'Tipos de asistencia social',3,1,1),
(13,N'OPE_AsistenciaSocialTareaTipo',N'Tipos de tareas asistencia social',3,1,1),
(14,N'OPE_AsistenciaSocialOrganismoTipo',N'Tipos organismo asistencia social',3,1,1),
(15,N'OPE_AsistenciaSocialNacionalidad',N'Nacionalidades asistencia social',3,1,1),
(16,N'OPE_AsistenciaSocialSexo',N'Sexos asistencia social',3,1,1),
(17,N'OPE_AsistenciaSocialEdad',N'Edades asistencia social',3,1,1),
(18,N'OPE_DatoFronteraIntervaloHorario',N'Intervalos horarios datos frontera',3,1,1),
(19,N'OPE_Ocupacion',N'Ocupación',3,1,1),
(32,N'OPE_Pais',N'Países OPE',3,1,1);

-- OPE - LOGS
INSERT INTO TablasMaestras (id, tabla, etiqueta, IdTablaMaestraGrupo, Listable, Editable) VALUES
(20, N'Auditoria_OPE_Frontera',N'Fronteras',4,1,0),
(22, N'Auditoria_OPE_Puerto',N'Puertos',4,1,0),
(23, N'Auditoria_OPE_LineaMaritima',N'Líneas marítimas',4,1,0),
(24, N'Auditoria_OPE_Periodo',N'Periodos',4,1,0),
(25, N'Auditoria_OPE_DatoFrontera',N'Datos de fronteras',4,1,0),
(26, N'Auditoria_OPE_DatoAsistencia',N'Datos de asistencias',4,1,0),
(27, N'Auditoria_OPE_DatoEmbarqueDiario',N'Datos de embarques',4,1,0),
(28, N'Auditoria_OPE_PuntoControlCarretera',N'Puntos de control en carreteras',4,1,0),
(29, N'Auditoria_OPE_AreaDescanso',N'Áreas de descanso',4,1,0),
(30, N'Auditoria_OPE_AreaEstacionamiento',N'Áreas de estacionamiento',4,1,0),
(31, N'Auditoria_OPE_PorcentajeOcupacionAreaEstacionamiento',N'Porcentaje ocupación área estacionamiento',4,1,0);

-- OPE - HISTÓRICO SIGE 2
INSERT INTO TablasMaestras (id, tabla, etiqueta, IdTablaMaestraGrupo, Listable, Editable) VALUES
(1039, N'OPE_HIST_PeriodoDetalle',N'Periodos (detalle)',5,1,0),
(1021, N'OPE_HIST_DatoFrontera',N'Datos de fronteras',5,1,0),
(1040, N'OPE_HIST_DatoEmbarque',N'Datos de embarques',5,1,0),
(1041, N'OPE_HIST_EmbarqueIntervaloHorario',N'Embarque intervalo horario',5,1,0),
(1042, N'OPE_HIST_EmbarqueIntervaloHorarioDestino',N'Embarque intervalo horario destino',5,1,0),
(1043, N'OPE_HIST_OcupacionAreaEstacionamiento',N'Ocupación área estacionamiento',5,1,0),
(1044, N'OPE_HIST_OcupacionAreaEstacionamientoDestino',N'Ocupación área estacionamiento - destino',5,1,0),
(1045, N'OPE_HIST_CapacidadAparcamiento',N'Capacidad aparcamiento',5,1,0),
(1046, N'OPE_HIST_AfluenciaPuntoControl',N'Afluencia punto control',5,1,0),
(1047, N'OPE_HIST_AfluenciaPuntoControlDestino',N'Afluencia punto control (destino)',5,1,0),
(1048, N'OPE_HIST_Asistencia',N'Asistencias',5,1,0),
(1049, N'OPE_HIST_AsistenciaTraduccion',N'Asistencias - traducciones',5,1,0),
(1050, N'OPE_HIST_AsistenciaSanitaria',N'Asistencias sanitarias',5,1,0),
(1051, N'OPE_HIST_AsistenciaSocial',N'Asistencias sociales',5,1,0),
(1052, N'OPE_HIST_AsistenciaSocialOrganismo',N'Asistencias sociales - organismos',5,1,0),
(1053, N'OPE_HIST_AsistenciaSocialTarea',N'Asistencias sociales - tareas',5,1,0),
(1054, N'OPE_HIST_AsistenciaSocialDemandante',N'Asistencias sociales - demandantes',5,1,0);

-- RESTO SIGEMAD
INSERT INTO TablasMaestras (Id,tabla,etiqueta,IdTablaMaestraGrupo,Listable,Editable) VALUES
(6,N'Entidad',N'Entidades',1,1,1),
(33,N'Organismo',N'Organismos',1,1,1),
(34,N'Administracion',N'Administraciones',1,1,1),
(35,N'TipoAdministracion',N'Tipo de administración',1,1,1),
(36,N'TipoCapacidad',N'Tipo de capacidad',1,1,1),
(37,N'ClaseSuceso',N'Clase de suceso',1,1,1),
(38,N'EstadoSuceso',N'Estado del suceso',1,1,1),
(39,N'TipoImpacto',N'Tipo de impacto',1,1,1),
(40,N'AlteracionInterrupcion',N'Alteracion Interrupcion',1,1,1);

SET IDENTITY_INSERT TablasMaestras OFF;

ALTER TABLE AspNetUsers 
ADD Apellidos NVARCHAR(100) NULL;

ALTER TABLE AspNetUsers 
ADD Nombre NVARCHAR(100) NULL;

ALTER TABLE AspNetUsers ADD Activo bit NOT NULL DEFAULT 1;









