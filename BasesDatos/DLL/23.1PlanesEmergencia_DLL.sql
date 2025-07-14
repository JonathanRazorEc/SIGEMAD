
-- =============================================
-- DATOS MAESTROS
-- =============================================
CREATE TABLE TipoRiesgo (
    Id INT NOT NULL PRIMARY KEY,
    Descripcion NVARCHAR(255) NOT NULL,
    IdTipoSuceso int NULL FOREIGN KEY REFERENCES TipoSuceso(Id),
    Codigo VARCHAR(5) NOT NULL
);


CREATE TABLE TipoPlan (
	Id INT NOT NULL PRIMARY KEY,
	Descripcion NVARCHAR(255) NOT NULL,
    Codigo VARCHAR(5) NOT NULL
);

CREATE TABLE TipoPlanMapeo (
    IdAntiguo INT NOT NULL PRIMARY KEY,
    IdNuevo INT NOT NULL
);

CREATE TABLE AmbitoPlan (
	Id INT NOT NULL PRIMARY KEY,
	Descripcion NVARCHAR(255) NOT NULL,
);

CREATE TABLE TipoNotificacion (
	Id INT NOT NULL PRIMARY KEY,
	Descripcion NVARCHAR(255) NOT NULL,
);


CREATE TABLE PlanEmergencia (
  Id INT NOT NULL PRIMARY KEY,
  Codigo NVARCHAR(15) NOT NULL,
  Descripcion NVARCHAR(255) NOT NULL,
  IdCCAA INT NULL FOREIGN KEY REFERENCES CCAA(Id),
  IdProvincia INT NULL FOREIGN KEY REFERENCES Provincia(Id),
  IdMunicipio INT NULL FOREIGN KEY REFERENCES Municipio(Id),
  IdTipoPlan INT NOT NULL FOREIGN KEY REFERENCES TipoPlan(Id),
  IdTipoRiesgo INT NOT NULL FOREIGN KEY REFERENCES TipoRiesgo(Id),
  IdAmbitoPlan INT NOT NULL FOREIGN KEY REFERENCES AmbitoPlan(Id),
);

CREATE INDEX IX_Codigo ON PlanEmergencia (Codigo);

CREATE TABLE FaseEmergencia (
    Id INT NOT NULL PRIMARY KEY,
    IdPlanEmergencia INT NULL FOREIGN KEY REFERENCES PlanEmergencia(Id),
    Orden INT NOT NULL,
    Descripcion NVARCHAR(255) NULL
);

CREATE TABLE PlanSituacion (
    Id INT NOT NULL PRIMARY KEY,
    IdPlanEmergencia INT NULL FOREIGN KEY REFERENCES PlanEmergencia(Id),
    IdFaseEmergencia INT NULL FOREIGN KEY REFERENCES FaseEmergencia(Id),
    Orden INT NOT NULL,
    Descripcion NVARCHAR(MAX) NULL,
    Nivel NVARCHAR(150) NULL,
    Situacion NVARCHAR(150) NULL,
    SituacionEquivalente NVARCHAR(150) NULL
);

CREATE TABLE TipoSistemaEmergencia (
    Id INT NOT NULL PRIMARY KEY,
    Descripcion NVARCHAR(255) NULL
);

CREATE TABLE TipoSistemaEmergenciaTipoSuceso (
    Id INT NOT NULL PRIMARY KEY,
    IdTipoSistemaEmergencia INT NOT NULL FOREIGN KEY REFERENCES TipoSistemaEmergencia(Id),
    IdTipoSuceso INT NULL FOREIGN KEY REFERENCES TipoSuceso(Id),
);

CREATE TABLE ModoActivacion (
    Id INT NOT NULL PRIMARY KEY,
    Descripcion NVARCHAR(255) NULL
);

-- =============================================
-- DATOS PANTALLAS
-- =============================================


-- Tabla principal para almacenar la información general de Actuaciones Relevantes
CREATE TABLE ActuacionRelevanteDGPCE (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdSuceso INT NOT NULL FOREIGN KEY REFERENCES Suceso(Id),
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0,

	CONSTRAINT UQ_ActuacionRelevanteDGPCE_IdSuceso UNIQUE (IdSuceso)
);


CREATE TABLE ConvocatoriaCECOD (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdActuacionRelevanteDGPCE INT NOT NULL FOREIGN KEY REFERENCES ActuacionRelevanteDGPCE(Id),
    FechaInicio DATE NOT NULL,
    FechaFin DATE NULL,
    Lugar NVARCHAR(255) NOT NULL,
    Convocados NVARCHAR(255) NOT NULL,
    Participantes NVARCHAR(255) NULL,
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


CREATE TABLE ActivacionPlanEmergencia (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdRegistro INT NOT NULL FOREIGN KEY REFERENCES Registro(Id),
    IdTipoPlan INT NULL FOREIGN KEY REFERENCES TipoPlan(Id),
    IdPlanEmergencia INT NULL FOREIGN KEY REFERENCES PlanEmergencia(Id),
    -- Opciones personalizadas
    TipoPlanPersonalizado NVARCHAR(255) NULL, -- Para guardar el texto personalizado
    PlanEmergenciaPersonalizado NVARCHAR(255) NULL, -- Para guardar el texto personalizado
    FechaHoraInicio datetimeoffset NOT NULL,
    FechaHoraFin datetimeoffset NULL,
    Autoridad NVARCHAR(255) NOT NULL,
    Observaciones NVARCHAR(MAX) NULL,
    IdArchivo UNIQUEIDENTIFIER NULL FOREIGN KEY REFERENCES Archivo(Id),
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);

CREATE TABLE NotificacionEmergencia (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdActuacionRelevanteDGPCE INT NOT NULL FOREIGN KEY REFERENCES ActuacionRelevanteDGPCE(Id),
    IdTipoNotificacion INT NOT NULL FOREIGN KEY REFERENCES TipoNotificacion(Id),
    FechaHoraNotificacion DATETIME2(7) NOT NULL,
    OrganosNotificados NVARCHAR(255) NOT NULL,
    -- Organos Extranjeros Notificados
    UCPM NVARCHAR(255) NULL,
    OrganismoInternacional NVARCHAR(255) NULL,
    OtrosPaises NVARCHAR(255) NULL,
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

CREATE TABLE ActivacionSistema (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdRegistro INT NOT NULL FOREIGN KEY REFERENCES Registro(Id),
    IdTipoSistemaEmergencia INT NOT NULL FOREIGN KEY REFERENCES TipoSistemaEmergencia(Id),
    FechaHoraActivacion DATETIME2(7) NULL,
    FechaHoraActualizacion DATETIME2(7) NULL,
    Autoridad NVARCHAR(255) NULL,
    DescripcionSolicitud NVARCHAR(MAX) NULL,
    Observaciones NVARCHAR(MAX) NULL,
    -- Copernicus
    IdModoActivacion INT NULL FOREIGN KEY REFERENCES ModoActivacion(Id),
    FechaActivacion DATE NULL,
    Codigo NVARCHAR(15) NULL,
    Nombre NVARCHAR(150) NULL,
    UrlAcceso NVARCHAR(MAX) NULL,
    -- UCPM
    FechaHoraPeticion DATETIME2(7) NULL,
    FechaAceptacion DATE NULL,
    Peticiones NVARCHAR(MAX) NULL,
    MediosCapacidades NVARCHAR(MAX) NULL,
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);


CREATE TABLE DeclaracionZAGEP (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdActuacionRelevanteDGPCE INT NOT NULL FOREIGN KEY REFERENCES ActuacionRelevanteDGPCE(Id),
    FechaSolicitud DATE NOT NULL,
    Denominacion NVARCHAR(255) NOT NULL,
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


CREATE TABLE EmergenciaNacional (
    Id INT PRIMARY KEY FOREIGN KEY REFERENCES ActuacionRelevanteDGPCE(Id),
    Autoridad NVARCHAR(255) NULL,
    DescripcionSolicitud NVARCHAR(255) NULL,
    FechaHoraSolicitud DATETIME2(7) NULL,
    FechaHoraDeclaracion DATETIME2(7) NULL,
    DescripcionDeclaracion NVARCHAR(255) NULL,
    FechaHoraDireccion DATETIME2(7) NULL,
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