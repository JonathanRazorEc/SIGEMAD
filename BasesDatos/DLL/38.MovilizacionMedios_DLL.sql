CREATE TABLE ProcedenciaMedio (
    Id INT NOT NULL PRIMARY KEY,
    Descripcion NVARCHAR(255) NULL
);

CREATE TABLE DestinoMedio (
    Id INT NOT NULL PRIMARY KEY,
    Descripcion NVARCHAR(255) NULL
);


CREATE TABLE EstadoMovilizacion (
    Id INT NOT NULL PRIMARY KEY,
    Descripcion NVARCHAR(50) NOT NULL,
);

CREATE TABLE PasoMovilizacion (
    Id INT NOT NULL PRIMARY KEY,
    Descripcion NVARCHAR(50) NOT NULL,
    IdEstadoMovilizacion INT NOT NULL FOREIGN KEY REFERENCES EstadoMovilizacion(Id)
);

CREATE TABLE FlujoPasoMovilizacion (
    Id INT NOT NULL PRIMARY KEY,
    IdPasoActual INT NULL FOREIGN KEY REFERENCES PasoMovilizacion(Id), -- ID del paso actual
    IdPasoSiguiente INT NOT NULL FOREIGN KEY REFERENCES PasoMovilizacion(Id), -- ID del paso siguiente permitido
    Orden INT NOT NULL, -- Orden de ejecución
);

ALTER TABLE FlujoPasoMovilizacion
ADD CONSTRAINT UQ_FlujoPasoUnique UNIQUE (IdPasoActual, IdPasoSiguiente, Orden);


CREATE TABLE TipoAdministracion (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(255) NOT NULL,
    Codigo NVARCHAR(10) NOT NULL,
    Borrado bit DEFAULT 0 NOT NULL,
    Editable bit DEFAULT 1 NOT NULL
);

CREATE TABLE Administracion (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Codigo NVARCHAR(20) NOT NULL,
    Nombre NVARCHAR(255) NOT NULL,
    IdTipoAdministracion INT NOT NULL FOREIGN KEY REFERENCES TipoAdministracion(Id),
    Borrado bit DEFAULT 0 NOT NULL,
    Editable bit DEFAULT 1 NOT NULL
);

CREATE TABLE Organismo (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Codigo NVARCHAR(20) NOT NULL,
    Nombre NVARCHAR(255) NOT NULL,
    IdAdministracion INT NOT NULL FOREIGN KEY REFERENCES Administracion(Id),
    Borrado bit DEFAULT 0 NOT NULL,
    Editable bit DEFAULT 1 NOT NULL
);

CREATE TABLE Entidad (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Codigo NVARCHAR(20) NOT NULL,
    Nombre NVARCHAR(255) NOT NULL,
    IdOrganismo INT NOT NULL FOREIGN KEY REFERENCES Organismo(Id),
    Borrado bit DEFAULT 0 NOT NULL,
    Editable bit DEFAULT 1 NOT NULL
);

CREATE TABLE TipoCapacidad (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(255) NOT NULL,
    Borrado bit DEFAULT 0 NOT NULL,
    Editable bit DEFAULT 1 NOT NULL
);


CREATE TABLE Capacidad (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(255) NOT NULL,
    Descripcion NVARCHAR(255) NOT NULL,
    Gestionado BIT NOT NULL,
    IdTipoCapacidad INT NULL FOREIGN KEY REFERENCES TipoCapacidad(Id),
    IdEntidad INT NULL FOREIGN KEY REFERENCES Entidad(Id),
    Borrado bit DEFAULT 0 NOT NULL,
    Editable bit DEFAULT 1 NOT NULL
);


-- ================================================
-- REGISTROS DE LA TABLA DE MOVILIZACIÓN DE MEDIOS
-- ================================================

CREATE TABLE MovilizacionMedio (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdActuacionRelevanteDGPCE INT NOT NULL FOREIGN KEY REFERENCES ActuacionRelevanteDGPCE(Id),
    Solicitante NVARCHAR(200) NOT NULL,
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);


-- ================================================
-- REGISTROS DE LA COLUMNA SITUACION DE LA
-- TABLA DE MOVILIZACIÓN DE MEDIOS
-- ================================================

CREATE TABLE EjecucionPaso (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdMovilizacionMedio INT NOT NULL FOREIGN KEY REFERENCES MovilizacionMedio(Id),
    IdPasoMovilizacion INT NOT NULL FOREIGN KEY REFERENCES PasoMovilizacion(Id), -- Referencia al paso maestro
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);

-- ================================================
-- DATOS ESPECÍFICOS POR CADA PASO
-- ================================================


CREATE TABLE SolicitudMedio (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdEjecucionPaso INT NOT NULL FOREIGN KEY REFERENCES EjecucionPaso(Id),
    IdProcedenciaMedio INT NOT NULL FOREIGN KEY REFERENCES ProcedenciaMedio(Id),
    AutoridadSolicitante NVARCHAR(200) NOT NULL,
    FechaHoraSolicitud DATETIME2(7) NOT NULL,
    Descripcion NVARCHAR(MAX) NULL,
    IdArchivo UNIQUEIDENTIFIER NULL FOREIGN KEY REFERENCES Archivo(Id),
    Observaciones NVARCHAR(MAX) NULL,
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);


CREATE TABLE TramitacionMedio (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdEjecucionPaso INT NOT NULL FOREIGN KEY REFERENCES EjecucionPaso(Id),
    IdDestinoMedio INT NOT NULL FOREIGN KEY REFERENCES DestinoMedio(Id),
    TitularMedio NVARCHAR(200) NOT NULL,
    FechaHoraTramitacion DATETIME2(7) NOT NULL,
    Descripcion NVARCHAR(MAX) NULL,
    PublicadoCECIS BIT NULL,
    Observaciones NVARCHAR(MAX) NULL,
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);


CREATE TABLE CancelacionMedio (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdEjecucionPaso INT NOT NULL FOREIGN KEY REFERENCES EjecucionPaso(Id),
    Motivo NVARCHAR(MAX) NOT NULL,
    FechaHoraCancelacion DATETIME2(7) NOT NULL,
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);


CREATE TABLE OfrecimientoMedio (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdEjecucionPaso INT NOT NULL FOREIGN KEY REFERENCES EjecucionPaso(Id),
    TitularMedio NVARCHAR(200) NOT NULL,
    GestionCECOD BIT NULL,
    FechaHoraOfrecimiento DATETIME2(7) NOT NULL,
    Descripcion NVARCHAR(MAX) NULL,
    FechaHoraDisponibilidad DATETIME2(7) NULL,
    Observaciones NVARCHAR(MAX) NULL,
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);

CREATE TABLE AportacionMedio (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdEjecucionPaso INT NOT NULL FOREIGN KEY REFERENCES EjecucionPaso(Id),
    IdCapacidad INT NOT NULL FOREIGN KEY REFERENCES Capacidad(Id),
    MedioNoCatalogado NVARCHAR(200) NULL,
    IdTipoAdministracion INT NOT NULL FOREIGN KEY REFERENCES TipoAdministracion(Id),
    TitularMedio NVARCHAR(200) NULL,
    FechaHoraAportacion DATETIME2(7) NOT NULL,
    Descripcion NVARCHAR(MAX) NULL,
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);


CREATE TABLE DespliegueMedio (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdEjecucionPaso INT NOT NULL FOREIGN KEY REFERENCES EjecucionPaso(Id),
    IdCapacidad INT NOT NULL FOREIGN KEY REFERENCES Capacidad(Id),
    MedioNoCatalogado NVARCHAR(200) NULL,
    FechaHoraDespliegue DATETIME2(7) NOT NULL,
    FechaHoraInicioIntervencion DATETIME2(7) NULL,
    Observaciones NVARCHAR(MAX) NULL,
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);

CREATE TABLE FinIntervencionMedio (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdEjecucionPaso INT NOT NULL FOREIGN KEY REFERENCES EjecucionPaso(Id),
    IdCapacidad INT NOT NULL FOREIGN KEY REFERENCES Capacidad(Id),
    MedioNoCatalogado NVARCHAR(200) NULL,
    FechaHoraInicioIntervencion DATETIME2(7) NULL,
    Observaciones NVARCHAR(MAX) NULL,
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);

CREATE TABLE LlegadaBaseMedio (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdEjecucionPaso INT NOT NULL FOREIGN KEY REFERENCES EjecucionPaso(Id),
    IdCapacidad INT NOT NULL FOREIGN KEY REFERENCES Capacidad(Id),
    MedioNoCatalogado NVARCHAR(200) NULL,
    FechaHoraLlegada DATETIME2(7) NULL,
    Observaciones NVARCHAR(MAX) NULL,
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);

