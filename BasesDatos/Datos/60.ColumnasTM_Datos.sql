-- OPE - CATÁLOGO
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(7,N'Nombre',2,N'Nombre',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(8,N'Nombre',2,N'Nombre',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(9,N'Nombre',2,N'Nombre',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(9,N'PorcentajeInferior',2,N'Porcentaje inferior',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(9,N'PorcentajeSuperior',2,N'Porcentaje superior',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(10,N'Nombre',2,N'Nombre',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(11,N'Nombre',2,N'Nombre',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(12,N'Nombre',2,N'Nombre',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(13,N'Nombre',2,N'Nombre',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(14,N'Nombre',2,N'Nombre',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(15,N'Nombre',2,N'Nombre',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(16,N'Nombre',2,N'Nombre',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(17,N'Nombre',2,N'Nombre',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(18,N'Inicio',2,N'Inicio',1,NULL,0,0,N'time',7,NULL,NULL,1,1),
(18,N'Fin',2,N'Fin',1,NULL,0,0,N'time',7,NULL,NULL,1,1),
(19,N'Nombre',2,N'Nombre',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(32,N'IdPais',2,N'Pais',1,NULL,0,0,N'select',NULL,N'Pais',N'Descripcion',1,1),
(32,N'Extranjero',3,N'Extranjero',1,NULL,0,0,N'bit',NULL,NULL,NULL,1,1),
(32,N'OpePuertos',4,N'Se lista en puertos',1,NULL,0,0,N'bit',NULL,NULL,NULL,1,1),
(32,N'OpeDatosAsistencias',5,N'Se lista en asistencias',1,NULL,0,0,N'bit',NULL,NULL,NULL,1,1),
(32,N'RutaImagen',6,N'Ruta imagen',1,NULL,0,0,N'nvarchar',255,NULL,NULL,0,1);


-- OPE - LOGS
-- OPE_Frontera
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(20,N'FechaRegistro',2,N'Fecha registro',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1),
(20,N'TipoMovimiento',3,N'Tipo movimiento',1,NULL,0,0,N'char',1,NULL,NULL,1,1),
(20,N'IdOpeFrontera',4,N'Frontera',1,NULL,0,0,N'select',NULL,N'OPE_Frontera',N'Nombre',0,1),
(20,N'IdCcaa',5,N'CCAA',1,NULL,0,0,N'select',NULL,N'CCAA',N'Descripcion',0,1),
(20,N'IdProvincia',6,N'Provincia',1,NULL,0,0,N'select',NULL,N'Provincia',N'Descripcion',0,1),
(20,N'IdMunicipio',7,N'Municipio',1,NULL,0,0,N'select',NULL,N'Municipio',N'Descripcion',0,1),
(20,N'Carretera',8,N'Carretera',1,NULL,0,0,N'nvarchar',255,NULL,NULL,0,1),
(20,N'PK',8,N'PK',1,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(20,N'CoordenadaUTM_X',10,N'Coordenada UTM X',1,NULL,0,0,N'nvarchar',255,NULL,NULL,0,1),
(20,N'CoordenadaUTM_Y',11,N'Coordenada UTM Y',1,NULL,0,0,N'nvarchar',255,NULL,NULL,0,1),
(20,N'TransitoMedioVehiculos',12,N'Transito medio vehículos',1,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(20,N'TransitoAltoVehiculos',13,N'Transito alto vehículos',1,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(20,N'FechaCreacion',14,N'Fecha creación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(20,N'CreadoPor',15,N'Creado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1),
(20,N'FechaModificacion',16,N'Fecha modificación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(20,N'ModificadoPor',17,N'Modificado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1),
(20,N'FechaEliminacion',18,N'Fecha eliminación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(20,N'EliminadoPor',19,N'Eliminado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1);

-- OPE Puerto
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(22,N'FechaRegistro',2,N'Fecha registro',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1),
(22,N'TipoMovimiento',3,N'Tipo movimiento',1,NULL,0,0,N'char',1,NULL,NULL,1,1),
(22,N'IdOpePuerto',4,N'Puerto',1,NULL,0,0,N'select',NULL,N'OPE_Puerto',N'Nombre',0,1),
(22,N'IdOpeFase',5,N'Fase',1,NULL,0,0,N'select',NULL,N'OPE_Fase',N'Nombre',0,1),
(22,N'IdPais',6,N'Pais',1,NULL,0,0,N'select',NULL,N'Pais',N'Descripcion',0,1),
(22,N'IdCcaa',7,N'CCAA',1,NULL,0,0,N'select',NULL,N'CCAA',N'Descripcion',0,1),
(22,N'IdProvincia',8,N'Provincia',1,NULL,0,0,N'select',NULL,N'Provincia',N'Descripcion',0,1),
(22,N'IdMunicipio',9,N'Municipio',1,NULL,0,0,N'select',NULL,N'Municipio',N'Descripcion',0,1),
(22,N'CoordenadaUTM_X',10,N'Coordenada UTM X',1,NULL,0,0,N'nvarchar',255,NULL,NULL,0,1),
(22,N'CoordenadaUTM_Y',11,N'Coordenada UTM Y',1,NULL,0,0,N'nvarchar',255,NULL,NULL,0,1),
(22,N'FechaValidezDesde',12,N'Operatividad desde',1,NULL,0,0,N'date',7,NULL,NULL,0,1),
(22,N'FechaValidezHasta',13,N'Operatividad hasta',1,NULL,0,0,N'date',7,NULL,NULL,0,1),
(22,N'Capacidad',14,N'Capacidad',1,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(22,N'FechaCreacion',15,N'Fecha creación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(22,N'CreadoPor',16,N'Creado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1),
(22,N'FechaModificacion',17,N'Fecha modificación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(22,N'ModificadoPor',18,N'Modificado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1),
(22,N'FechaEliminacion',19,N'Fecha eliminación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(22,N'EliminadoPor',20,N'Eliminado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1);

-- OPE Linea Marítima
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(23,N'FechaRegistro',2,N'Fecha registro',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1),
(23,N'TipoMovimiento',3,N'Tipo movimiento',1,NULL,0,0,N'char',1,NULL,NULL,1,1),
(23,N'IdOpeLineaMaritima',4,N'Frontera',1,NULL,0,0,N'select',NULL,N'OPE_LineaMaritima',N'Nombre',0,1),
(23,N'IdOpePuertoOrigen',5,N'Frontera',1,NULL,0,0,N'select',NULL,N'OPE_Puerto',N'Nombre',0,1),
(23,N'IdOpePuertoDestino',6,N'Frontera',1,NULL,0,0,N'select',NULL,N'OPE_Puerto',N'Nombre',0,1),
(23,N'IdOpeFase',5,N'Fase',7,NULL,0,0,N'select',NULL,N'OPE_Fase',N'Nombre',0,1),
(23,N'FechaValidezDesde',8,N'Operatividad desde',1,NULL,0,0,N'date',7,NULL,NULL,0,1),
(23,N'FechaValidezHasta',9,N'Operatividad hasta',1,NULL,0,0,N'date',7,NULL,NULL,0,1),
(23,N'NumeroRotaciones',10,N'Rotaciones',1,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(23,N'NumeroPasajeros',11,N'Pasajeros',1,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(23,N'NumeroTurismos',12,N'Turismo',1,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(23,N'NumeroAutocares',13,N'Autocares',1,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(23,N'NumeroCamiones',14,N'Camiones',1,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(23,N'NumeroTotalVehiculos',15,N'Total Vehículos',1,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(23,N'FechaCreacion',16,N'Fecha creación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(23,N'CreadoPor',17,N'Creado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1),
(23,N'FechaModificacion',18,N'Fecha modificación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(23,N'ModificadoPor',19,N'Modificado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1),
(23,N'FechaEliminacion',20,N'Fecha eliminación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(23,N'EliminadoPor',21,N'Eliminado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1);

-- OPE Periodo
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(24,N'FechaRegistro',2,N'Fecha registro',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1),
(24,N'TipoMovimiento',3,N'Tipo movimiento',1,NULL,0,0,N'char',1,NULL,NULL,1,1),
(24,N'IdOpePeriodo',4,N'Frontera',1,NULL,0,0,N'select',NULL,N'OPE_Periodo',N'Nombre',0,1),
(24,N'FechaInicioFaseSalida',5,N'Fecha validez desde',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(24,N'FechaFinFaseSalida',6,N'Fecha validez desde',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(24,N'FechaInicioFaseRetornos',7,N'Fecha validez desde',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(24,N'FechaFinFaseRetorno',8,N'Fecha validez desde',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(24,N'FechaCreacion',9,N'Fecha creación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(24,N'CreadoPor',10,N'Creado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1),
(24,N'FechaModificacion',11,N'Fecha modificación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(24,N'ModificadoPor',12,N'Modificado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1),
(24,N'FechaEliminacion',13,N'Fecha eliminación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(24,N'EliminadoPor',14,N'Eliminado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1);

-- OPE Datos fronteras
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(25,N'FechaRegistro',2,N'Fecha registro',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1),
(25,N'TipoMovimiento',3,N'Tipo movimiento',1,NULL,0,0,N'char',1,NULL,NULL,1,1),
(25,N'IdOpeFrontera',4,N'Frontera',1,NULL,0,0,N'select',NULL,N'OPE_Frontera',N'Nombre',0,1),
(25,N'Fecha',5,N'Fecha',1,NULL,0,0,N'date',7,NULL,NULL,0,1),
(25,N'IdOpeDatoFronteraIntervaloHorario',6,N'Intervalo Horario',1,NULL,0,0,N'select',NULL,N'OPE_DatoFronteraIntervaloHorario',N'Nombre',0,1),
(25,N'IntervaloHorarioPersonalizado',7,N'Intervalo personalizado',1,NULL,0,0,N'bit',NULL,NULL,NULL,0,1),
(25,N'InicioIntervaloHorarioPersonalizado',8,N'Inicio int personalizado',1,NULL,0,0,N'time',7,NULL,NULL,0,1),
(25,N'FinIntervaloHorarioPersonalizado',9,N'Fin int personalizado',1,NULL,0,0,N'time',7,NULL,NULL,0,1),
(25,N'NumeroVehiculos',10,N'Nº Vehículos',1,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(25,N'Afluencia',11,N'Afluencia',1,NULL,0,0,N'nvarchar',255,NULL,NULL,0,1),
(25,N'FechaCreacion',12,N'Fecha creación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(25,N'CreadoPor',13,N'Creado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1),
(25,N'FechaModificacion',14,N'Fecha modificación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(25,N'ModificadoPor',15,N'Modificado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1),
(25,N'FechaEliminacion',16,N'Fecha eliminación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(25,N'EliminadoPor',17,N'Eliminado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1);


-- OPE Datos asistencias
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(26,N'FechaRegistro',2,N'Fecha registro',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1),
(26,N'TipoMovimiento',3,N'Tipo movimiento',1,NULL,0,0,N'char',1,NULL,NULL,1,1),
(26,N'IdOpePuerto',4,N'Frontera',1,NULL,0,0,N'select',NULL,N'OPE_Puerto',N'Nombre',0,1),
(26,N'Fecha',5,N'Fecha',1,NULL,0,0,N'date',7,NULL,NULL,0,1),
(26,N'FechaCreacion',6,N'Fecha creación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(26,N'CreadoPor',7,N'Creado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1),
(26,N'FechaModificacion',8,N'Fecha modificación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(26,N'ModificadoPor',9,N'Modificado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1),
(26,N'FechaEliminacion',10,N'Fecha eliminación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(26,N'EliminadoPor',11,N'Eliminado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1);


-- OPE Datos embarques diarios
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(27,N'FechaRegistro',2,N'Fecha registro',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1),
(27,N'TipoMovimiento',3,N'Tipo movimiento',1,NULL,0,0,N'char',1,NULL,NULL,1,1),
(27,N'IdOpeLineaMaritima',4,N'Frontera',1,NULL,0,0,N'select',NULL,N'OPE_LineaMaritima',N'Nombre',0,1),
(27,N'Fecha',5,N'Fecha',1,NULL,0,0,N'date',7,NULL,NULL,0,1),
(27,N'NumeroRotaciones',6,N'Rotaciones',1,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(27,N'NumeroPasajeros',7,N'Pasajeros',1,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(27,N'NumeroTurismos',8,N'Turismo',1,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(27,N'NumeroAutocares',9,N'Autocares',1,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(27,N'NumeroCamiones',10,N'Camiones',1,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(27,N'NumeroTotalVehiculos',11,N'Total Vehículos',1,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(27,N'FechaCreacion',12,N'Fecha creación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(27,N'CreadoPor',13,N'Creado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1),
(27,N'FechaModificacion',14,N'Fecha modificación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(27,N'ModificadoPor',15,N'Modificado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1),
(27,N'FechaEliminacion',16,N'Fecha eliminación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(27,N'EliminadoPor',17,N'Eliminado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1);


-- OPE Puntos control carretera
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(28,N'FechaRegistro',2,N'Fecha registro',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1),
(28,N'TipoMovimiento',3,N'Tipo movimiento',1,NULL,0,0,N'char',1,NULL,NULL,1,1),
(28,N'IdOpePuntoControlCarretera',4,N'Punto control carretera',1,NULL,0,0,N'select',NULL,N'OPE_PuntoControlCarretera',N'Nombre',0,1),
(28,N'IdCcaa',5,N'CCAA',1,NULL,0,0,N'select',NULL,N'CCAA',N'Descripcion',0,1),
(28,N'IdProvincia',6,N'Provincia',1,NULL,0,0,N'select',NULL,N'Provincia',N'Descripcion',0,1),
(28,N'IdMunicipio',7,N'Municipio',1,NULL,0,0,N'select',NULL,N'Municipio',N'Descripcion',0,1),
(28,N'Carretera',8,N'Carretera',1,NULL,0,0,N'nvarchar',255,NULL,NULL,0,1),
(28,N'PK',9,N'PK',1,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(28,N'CoordenadaUTM_X',10,N'Coordenada UTM X',1,NULL,0,0,N'nvarchar',255,NULL,NULL,0,1),
(28,N'CoordenadaUTM_Y',11,N'Coordenada UTM Y',1,NULL,0,0,N'nvarchar',255,NULL,NULL,0,1),
(28,N'TransitoMedioVehiculos',12,N'Tránsito medio vehículos',9,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(28,N'TransitoAltoVehiculos',13,N'Tránsito alto vehículos',9,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(28,N'FechaCreacion',14,N'Fecha creación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(28,N'CreadoPor',15,N'Creado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1),
(28,N'FechaModificacion',16,N'Fecha modificación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(28,N'ModificadoPor',17,N'Modificado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1),
(28,N'FechaEliminacion',18,N'Fecha eliminación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(28,N'EliminadoPor',19,N'Eliminado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1);

-- OPE Áreas descanso
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(29,N'FechaRegistro',2,N'Fecha registro',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1),
(29,N'TipoMovimiento',3,N'Tipo movimiento',1,NULL,0,0,N'char',1,NULL,NULL,1,1),
(29,N'IdOpeAreaDescanso',4,N'Área descanso',1,NULL,0,0,N'select',NULL,N'OPE_AreaDescanso',N'Nombre',0,1),
(29,N'IdOpeAreaDescansoTipo',5,N'Tipo de área descanso',1,NULL,0,0,N'select',NULL,N'OPE_AreaDescansoTipo',N'Nombre',0,1),
(29,N'IdCcaa',6,N'CCAA',1,NULL,0,0,N'select',NULL,N'CCAA',N'Descripcion',0,1),
(29,N'IdProvincia',7,N'Provincia',1,NULL,0,0,N'select',NULL,N'Provincia',N'Descripcion',0,1),
(29,N'IdMunicipio',8,N'Municipio',1,NULL,0,0,N'select',NULL,N'Municipio',N'Descripcion',0,1),
(29,N'Carretera',9,N'Carretera',1,NULL,0,0,N'nvarchar',255,NULL,NULL,0,1),
(29,N'PK',10,N'PK',1,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(29,N'CoordenadaUTM_X',11,N'Coordenada UTM X',1,NULL,0,0,N'nvarchar',255,NULL,NULL,0,1),
(29,N'CoordenadaUTM_Y',12,N'Coordenada UTM Y',1,NULL,0,0,N'nvarchar',255,NULL,NULL,0,1),
(29,N'Capacidad',13,N'Capacidad',9,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(29,N'IdOpeEstadoOcupacion',14,N'Estado de ocupación',1,NULL,0,0,N'select',NULL,N'OPE_EstadoOcupacion',N'Nombre',0,1),
(29,N'FechaCreacion',15,N'Fecha creación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(29,N'CreadoPor',16,N'Creado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1),
(29,N'FechaModificacion',17,N'Fecha modificación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(29,N'ModificadoPor',18,N'Modificado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1),
(29,N'FechaEliminacion',19,N'Fecha eliminación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(29,N'EliminadoPor',20,N'Eliminado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1);


-- OPE Áreas estacionamiento
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(30,N'FechaRegistro',2,N'Fecha registro',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1),
(30,N'TipoMovimiento',3,N'Tipo movimiento',1,NULL,0,0,N'char',1,NULL,NULL,1,1),
(30,N'IdOpeAreaEstacionamiento',4,N'Área estacionamiento',1,NULL,0,0,N'select',NULL,N'OPE_AreaEstacionamiento',N'Nombre',0,1),
(30,N'IdCcaa',5,N'CCAA',1,NULL,0,0,N'select',NULL,N'CCAA',N'Descripcion',0,1),
(30,N'IdProvincia',6,N'Provincia',1,NULL,0,0,N'select',NULL,N'Provincia',N'Descripcion',0,1),
(30,N'IdMunicipio',7,N'Municipio',1,NULL,0,0,N'select',NULL,N'Municipio',N'Descripcion',0,1),
(30,N'Carretera',8,N'Carretera',1,NULL,0,0,N'nvarchar',255,NULL,NULL,0,1),
(30,N'PK',9,N'PK',1,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(30,N'CoordenadaUTM_X',10,N'Coordenada UTM X',1,NULL,0,0,N'nvarchar',255,NULL,NULL,0,1),
(30,N'CoordenadaUTM_Y',11,N'Coordenada UTM Y',1,NULL,0,0,N'nvarchar',255,NULL,NULL,0,1),
(30,N'InstalacionPortuaria',12,N'Instalación portuaria',1,NULL,0,0,N'bit',NULL,NULL,NULL,0,1),
(30,N'IdOpePuerto',13,N'Puerto',1,NULL,0,0,N'select',NULL,N'OPE_Puerto',N'Nombre',0,1),
(30,N'Capacidad',14,N'Capacidad',9,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(30,N'IdOpeEstadoOcupacion',15,N'Estado de ocupación',1,NULL,0,0,N'select',NULL,N'OPE_EstadoOcupacion',N'Nombre',0,1),
(30,N'FechaCreacion',16,N'Fecha creación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(30,N'CreadoPor',17,N'Creado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1),
(30,N'FechaModificacion',18,N'Fecha modificación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(30,N'ModificadoPor',19,N'Modificado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1),
(30,N'FechaEliminacion',20,N'Fecha eliminación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(30,N'EliminadoPor',21,N'Eliminado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1);


-- OPE Porcentaje ocupación áreas estacionamiento
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(31,N'FechaRegistro',2,N'Fecha registro',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1),
(31,N'TipoMovimiento',3,N'Tipo movimiento',1,NULL,0,0,N'char',1,NULL,NULL,1,1),
--(31,N'IdOpePorcentajeOcupacionAreaEstacionamiento',4,N'Porcentaje ocupación área estacionamiento',1,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(31,N'IdOpePorcentajeOcupacionAreaEstacionamiento',4,N'Porcentaje ocupación área estacionamiento',1,NULL,0,0,N'select',NULL,N'OPE_PorcentajeOcupacionAreaEstacionamiento',N'Nombre',0,1),
(31,N'IdOpeOcupacion',5,N'Ocupación',1,NULL,0,0,N'select',NULL,N'OPE_Ocupacion',N'Nombre',0,1),
(31,N'PorcentajeInferior',6,N'Porcentaje inferior',9,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(31,N'PorcentajeSuperior',7,N'Porcentaje superior',9,NULL,0,0,N'int',NULL,NULL,NULL,0,1),
(31,N'FechaCreacion',8,N'Fecha creación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(31,N'CreadoPor',9,N'Creado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1),
(31,N'FechaModificacion',10,N'Fecha modificación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(31,N'ModificadoPor',11,N'Modificado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1),
(31,N'FechaEliminacion',12,N'Fecha eliminación',1,NULL,0,0,N'datetime2',7,NULL,NULL,0,1),
(31,N'EliminadoPor',13,N'Eliminado por',1,NULL,0,0,N'uniqueidentifier',255,NULL,NULL,0,1);

--------------------------------------
----   OPE - HISTÓRICO SIGE 2  -------
--------------------------------------

-- Periodos (detalle)
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(1039,N'IdPeriodo',2,N'Periodo',1,NULL,0,0,N'select',NULL,N'OPE_HIST_Periodo',N'Descripcion',1,1),
(1039, N'Nombre',3,N'Nombre',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(1039,N'FechaInicial',4,N'Fecha inicial',1,NULL,0,0,N'date',7,NULL,NULL,1,1),
(1039,N'FechaFinal',5,N'Fecha final',1,NULL,0,0,N'date',7,NULL,NULL,1,1),
(1039,N'FechaFinSalida',6,N'Fecha fin salida',1,NULL,0,0,N'date',7,NULL,NULL,1,1),
(1039,N'FechaInicioRetorno',7,N'Fecha inicio retorno',1,NULL,0,0,N'date',7,NULL,NULL,1,1),
(1039, N'UsrAltaAuditoria',8,N'Usuario alta auditoria',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(1039,N'FechaAltaAuditoria',9,N'Fecha alta auditoria',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1);


-- Datos de fronteras
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(1021,N'IdFrontera',2,N'Frontera',1,NULL,0,0,N'select',NULL,N'OPE_HIST_Frontera',N'Nombre',1,1),
(1021,N'Turismos',3,N'Turismos',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1),
(1021,N'Autocares',4,N'Autocares',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1),
(1021,N'FechaInicio',5,N'Fecha inicio',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1),
(1021,N'FechaFin',6,N'Fecha fin',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1),
--(1021,N'IdIntervalo',7,N'Intervalo',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1);
(1021,N'IdIntervalo',7,N'Intervalo',1,NULL,0,0,N'select',NULL,N'OPE_DatoFronteraIntervaloHorario',N'Nombre',1,1),
(1021, N'UsrAltaAuditoria',8,N'Usuario alta auditoria',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(1021,N'FechaAltaAuditoria',9,N'Fecha alta auditoria',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1),
(1021, N'UsrAuditoria',10,N'Usuario auditoria',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(1021,N'FechaAuditoria',11,N'Fecha auditoria',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1);

-- Datos de embarques
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(1040,N'IdLinea',2,N'Linea',1,NULL,0,0,N'select',NULL,N'OPE_HIST_Linea',N'Nombre',1,1),
(1040,N'Fecha',3,N'Fecha',1,NULL,0,0,N'date',7,NULL,NULL,1,1),
(1040,N'Rotaciones',4,N'Rotaciones',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1),
(1040,N'Pasajeros',5,N'Pasajeros',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1),
(1040,N'Turismos',6,N'Turismos',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1),
(1040,N'Autocares',7,N'Autocares',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1),
(1040,N'Camiones',8,N'Camiones',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1),
(1040, N'UsrAltaAuditoria',9,N'Usuario alta auditoria',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(1040,N'FechaAltaAuditoria',10,N'Fecha alta auditoria',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1),
(1040, N'UsrAuditoria',11,N'Usuario auditoria',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(1040,N'FechaAuditoria',12,N'Fecha auditoria',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1);

-- Embarques - intervalos horarios
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(1041,N'IdIntervalo',2,N'Intervalo',1,NULL,0,0,N'select',NULL,N'OPE_HIST_TipoHoraPuntoControl',N'Descripcion',1,1),
(1041,N'FechaHoraInicio',3,N'Fecha hora inicio',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1),
(1041,N'FechaHoraFin',4,N'Fecha hora fin',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1);

-- Embarques - intervalos horarios destinos
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(1042,N'IdEmbarqueIntervalo',2,N'Embarque Intervalo',1,NULL,0,0,N'select',NULL,N'OPE_HIST_EmbarqueIntervaloHorario',N'Nombre',1,1),
(1042,N'NumeroVehiculos',3,N'Vehículos',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1),
(1042,N'IdLineaDestino',4,N'Línea destino',1,NULL,0,0,N'select',NULL,N'OPE_LineaMaritima',N'Nombre',1,1);


-- Ocupación área estacionamiento
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(1043,N'IdAreaEstacionamiento',2,N'Área estacionamiento',1,NULL,0,0,N'select',NULL,N'OPE_HIST_AreaEstacionamiento',N'Nombre',1,1),
(1043,N'IdOcupacion',3,N'Ocupación',1,NULL,0,0,N'select',NULL,N'OPE_HIST_TipoOcupacion',N'Descripcion',1,1),
(1043,N'Fecha',4,N'Fecha',1,NULL,0,0,N'date',7,NULL,NULL,1,1),
(1043,N'NumeroVehiculos',5,N'Nº vehículos',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1),
(1043, N'Observaciones',6,N'Observaciones',1,NULL,0,0,N'nvarchar',8000,NULL,NULL,1,1),
(1043, N'UsrAltaAuditoria',7,N'Usuario alta auditoria',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(1043,N'FechaAltaAuditoria',8,N'Fecha alta auditoria',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1),
(1043, N'UsrAuditoria',9,N'Usuario auditoria',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(1043,N'FechaAuditoria',10,N'Fecha auditoria',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1);


-- Ocupación área estacionamiento - destino
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(1044,N'IdOcupacionAreaEstacionamiento',2,N'Ocupación',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1),
(1044,N'IdPuertoDestino',3,N'Puerto',1,NULL,0,0,N'select',NULL,N'OPE_HIST_Puerto',N'Nombre',1,1),
(1044,N'NumeroVehiculos',4,N'Nº vehículos',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1),
(1044,N'EsperaMaxima',5,N'Espera máxima',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1);


-- Capacidad de aparcamiento
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(1045,N'IdAreaEstacionamiento',2,N'Área estacionamiento',1,NULL,0,0,N'select',NULL,N'OPE_HIST_AreaEstacionamiento',N'Nombre',1,1),
(1045,N'IdPeriodoDetalle',3,N'Periodo detalle',1,NULL,0,0,N'select',NULL,N'OPE_HIST_PeriodoDetalle',N'Nombre',1,1),
(1045,N'Capacidad',4,N'Capacidad',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1);


-- Afluencia punto control
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(1046,N'IdPuntoControl',2,N'Punto control',1,NULL,0,0,N'select',NULL,N'OPE_HIST_PuntoControl',N'Nombre',1,1),
(1046,N'VehiculosSinBillete',3,N'Vehiculos sin billete',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1),
(1046,N'TotalVehiculos',4,N'Total vehículos',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1),
(1046,N'FechaHoraInicio',5,N'Fecha hora inicio',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1),
(1046,N'FechaHoraFin',6,N'Fecha hora fin',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1),
(1046,N'Observaciones',7,N'Observaciones',1,NULL,0,0,N'nvarchar',8000,NULL,NULL,1,1),
(1046,N'UsrAltaAuditoria',8,N'Usuario alta auditoria',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(1046,N'FechaAltaAuditoria',9,N'Fecha alta auditoria',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1),
(1046,N'UsrAuditoria',10,N'Usuario auditoria',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(1046,N'FechaAuditoria',11,N'Fecha auditoria',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1);


-- Afluencia punto control (destino)
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(1047,N'IdAfluenciaPuntoControl',2,N'Afluencia punto control',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1),
(1047,N'IdPuertoDestino',3,N'Puerto destino',1,NULL,0,0,N'select',NULL,N'OPE_HIST_Puerto',N'Nombre',1,1),
(1047,N'NumeroVehiculos',4,N'Nº vehículos',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1);


-- Asistencias
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(1048,N'IdPuerto',2,N'Puerto',1,NULL,0,0,N'select',NULL,N'OPE_HIST_Puerto',N'Nombre',1,1),
(1048,N'Fecha',3,N'Fecha',1,NULL,0,0,N'date',7,NULL,NULL,1,1),
(1048,N'UsrAltaAuditoria',4,N'Usuario alta auditoria',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(1048,N'FechaAltaAuditoria',5,N'Fecha alta auditoria',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1),
(1048,N'UsrAuditoria',6,N'Usuario auditoria',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(1048,N'FechaAuditoria',7,N'Fecha auditoria',1,NULL,0,0,N'datetime2',7,NULL,NULL,1,1);


-- Asistencias - traducciones
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(1049,N'IdAsistencia',2,N'Asistencia',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1),
(1049,N'Numero',3,N'Número',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1),
(1049,N'Observaciones',4,N'Observaciones',1,NULL,0,0,N'nvarchar',8000,NULL,NULL,1,1);


-- Asistencias sanitarias
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(1050,N'IdAsistencia',2,N'Asistencia',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1),
(1050,N'IdTipoAsistenciaSanitaria',3,N'Tipo asistencia sanitaria',1,NULL,0,0,N'select',NULL,N'OPE_HIST_TipoAsistenciaSanitaria',N'Descripcion',1,1),
(1050,N'Numero',4,N'Número',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1),
(1050,N'Observaciones',5,N'Observaciones',1,NULL,0,0,N'nvarchar',8000,NULL,NULL,1,1);


-- Asistencias sociales
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(1051,N'IdAsistencia',2,N'Asistencia',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1),
(1051,N'IdTipoAsistenciaSocial',3,N'Tipo asistencia social',1,NULL,0,0,N'select',NULL,N'OPE_HIST_TipoAsistenciaSocial',N'Descripcion',1,1),
(1051,N'Numero',4,N'Número',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1),
(1051,N'Observaciones',5,N'Observaciones',1,NULL,0,0,N'nvarchar',8000,NULL,NULL,1,1);


-- Asistencias sociales - organismos
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(1052,N'IdAsistenciaSocial',2,N'Asistencia social',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1),
(1052,N'IdTipoOrganismo',3,N'Tipo organismo',1,NULL,0,0,N'select',NULL,N'OPE_HIST_TipoOrganismo',N'Descripcion',1,1),
(1052,N'Numero',4,N'Número',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1),
(1052,N'Observaciones',5,N'Observaciones',1,NULL,0,0,N'nvarchar',8000,NULL,NULL,1,1);


-- Asistencias sociales - tareas
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(1053,N'IdAsistenciaSocial',2,N'Asistencia social',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1),
(1053,N'IdTipoAsistenciaTarea',3,N'Tipo tarea',1,NULL,0,0,N'select',NULL,N'OPE_HIST_TipoAsistenciaTarea',N'Descripcion',1,1),
(1053,N'Numero',4,N'Número',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1),
(1053,N'Observaciones',5,N'Observaciones',1,NULL,0,0,N'nvarchar',8000,NULL,NULL,1,1);


-- Asistencias sociales - demandantes
INSERT INTO ColumnasTM (IdTablasMaestras,Columna,Orden,Etiqueta,Busqueda,Defecto,Nulo,Duplicado,Tipo,Longitud,TablaRelacionada,CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(1054,N'IdAsistenciaSocial',2,N'Asistencia social',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1),
(1054,N'Numero',3,N'Número',1,NULL,0,0,N'int',NULL,NULL,NULL,1,1),
(1054,N'Observaciones',4,N'Observaciones',1,NULL,0,0,N'nvarchar',8000,NULL,NULL,1,1),
(1054,N'IdTipoNacionalidadDemandante',5,N'Tipo nacionalidad',1,NULL,0,0,N'select',NULL,N'OPE_HIST_TipoNacionalidadDemandante',N'Descripcion',1,1),
(1054,N'IdTipoEdadDemandante',6,N'Tipo edad',1,NULL,0,0,N'select',NULL,N'OPE_HIST_TipoEdadDemandante',N'Descripcion',1,1),
(1054,N'IdTipoSexoDemandante',7,N'Tipo sexo',1,NULL,0,0,N'select',NULL,N'OPE_HIST_TipoSexoDemandante',N'Descripcion',1,1),
(1054,N'IdTipoPaisResidencia',8,N'Tipo país residencia',1,NULL,0,0,N'select',NULL,N'OPE_HIST_TipoPaisResidencia',N'Descripcion',1,1);



--------------------------------------
---  FIN  OPE - HISTÓRICO SIGE 2    --
--------------------------------------



-- -- SIGEMAD - CATÁLOGO
INSERT INTO ColumnasTM (IdTablasMaestras, Columna, Orden, Etiqueta, Busqueda, Defecto, Nulo, Duplicado, Tipo, Longitud, TablaRelacionada, CampoRelacionado, ApareceEnListado, ApareceEnEdicion) VALUES
(1, N'Descripcion',2,N'Descripcion',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(1, N'Prioridad',3,N'Prioridad',1,0,0,0,N'int',NULL,NULL,NULL,1,1),
(2, N'Descripcion',2,N'Descripcion',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(3, N'Descripcion',2,N'Descripcion',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(4, N'Nombre',2,N'Nombre',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(4, N'Descripcion',3,N'Descripción',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(4, N'Gestionado',4,N'Gestionado',1,0,0,1,N'bit',NULL,NULL,NULL,1,1),
(4, N'IdTipoCapacidad',5,N'Tipo de capacidad',0,NULL,0,1,N'select',NULL,N'TipoCapacidad',N'Nombre',1,1),
(4, N'IdEntidad',6,N'Entidad',0,NULL,0,1,N'select',NULL,N'Entidad',N'Codigo;Nombre',1,1),
(5, N'Descripcion',2,N'Descripción',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(6,N'Codigo',2,N'Código',1,NULL,0,0,N'nvarchar',20,NULL,NULL,1,1),
(6,N'Nombre',3,N'Nombre',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(6,N'IdOrganismo',5,N'Organismo',0,NULL,0,1,N'select',NULL,N'Organismo',N'Codigo;Nombre',1,1),
(33,N'Codigo',2,N'Código',1,NULL,0,0,N'nvarchar',20,NULL,NULL,1,1),
(33,N'Nombre',3,N'Nombre',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(33,N'IdAdministracion',5,N'Administración',0,NULL,0,1,N'select',NULL,N'Administracion',N'Codigo;Nombre',1,1),
(34,N'Codigo',2,N'Código',1,NULL,0,0,N'nvarchar',20,NULL,NULL,1,1),
(34,N'Nombre',3,N'Nombre',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(34,N'IdTipoAdministracion',5,N'Tipo de administración',0,NULL,0,1,N'select',NULL,N'TipoAdministracion',N'Codigo;Nombre', 1,1),
(35,N'Codigo',2,N'Código',1,NULL,0,0,N'nvarchar',10,NULL,NULL,1,1),
(35,N'Nombre',3,N'Nombre',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(36,N'Nombre',2,N'Nombre',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(37,N'Descripcion',2,N'Descripcion',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(38,N'Descripcion',2,N'Descripcion',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1),
(40,N'Descripcion',2,N'Descripcion',1,NULL,0,0,N'nvarchar',255,NULL,NULL,1,1);
