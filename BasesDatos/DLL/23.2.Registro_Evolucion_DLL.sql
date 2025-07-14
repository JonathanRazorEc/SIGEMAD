CREATE TABLE SituacionOperativa (
	Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	Descripcion NVARCHAR(255) NOT NULL,
);

CREATE TABLE SituacionEquivalente (
	Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	Descripcion NVARCHAR(255) NOT NULL,
	Prioridad int DEFAULT 0 NOT NULL,
	Obsoleto bit DEFAULT 0 NOT NULL,
	Borrado bit DEFAULT 0 NOT NULL,
	Editable bit DEFAULT 1 NOT NULL
);


CREATE TABLE Evolucion (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdSuceso int NOT NULL FOREIGN KEY REFERENCES Suceso(Id),
    EsFoto BIT NOT NULL DEFAULT 0,
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);

SET QUOTED_IDENTIFIER ON;

CREATE UNIQUE INDEX UX_IdSuceso_EsFoto
ON Evolucion(IdSuceso)
WHERE EsFoto = 0;


-- Crear índice filtrado para permitir nuevas inserciones si el anterior está eliminado
-- SET QUOTED_IDENTIFIER ON;
-- CREATE UNIQUE INDEX UQ_Registro_Evolucion
-- ON Registro (IdEvolucion)
-- WHERE Borrado = 0;

CREATE TABLE Registro_ProcedenciaDestino (
	IdRegistro INT NOT NULL FOREIGN KEY REFERENCES Registro(Id),
	IdProcedenciaDestino INT NOT NULL FOREIGN KEY REFERENCES ProcedenciaDestino(Id),
    -- Audit Fields
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
    CreadoPor UNIQUEIDENTIFIER NULL,
    FechaModificacion DATETIME2(7) NULL,
    ModificadoPor UNIQUEIDENTIFIER NULL,
    FechaEliminacion DATETIME2(7) NULL,
    EliminadoPor UNIQUEIDENTIFIER NULL,
    Borrado BIT NOT NULL DEFAULT 0,
    -- Composite Primary Key
    CONSTRAINT PK_Registro_ProcedenciaDestino PRIMARY KEY (IdRegistro, IdProcedenciaDestino)
);

--CREATE TABLE dbo.DatoPrincipal (
--	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
--    IdEvolucion INT NOT NULL FOREIGN KEY REFERENCES Evolucion(Id),
--    FechaHora DATETIME2(7) NULL,
--    Observaciones NVARCHAR(MAX) NULL,
--    Prevision NVARCHAR(MAX) NULL,
--    ---
--    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
--	CreadoPor UNIQUEIDENTIFIER NULL,
--	FechaModificacion DATETIME2(7) NULL,
--	ModificadoPor UNIQUEIDENTIFIER NULL,
--	FechaEliminacion DATETIME2(7) NULL,
--	EliminadoPor UNIQUEIDENTIFIER NULL,
--	Borrado BIT NOT NULL DEFAULT 0
--);

-- Crear índice filtrado para permitir nuevas inserciones si el anterior está eliminado
-- SET QUOTED_IDENTIFIER ON;
-- CREATE UNIQUE INDEX UQ_DatoPrincipal_Evolucion
-- ON DatoPrincipal (IdEvolucion)
-- WHERE Borrado = 0;

CREATE TABLE Parametro (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdRegistro INT NOT NULL FOREIGN KEY REFERENCES Registro(Id),
    IdEstadoIncendio INT NULL FOREIGN KEY REFERENCES EstadoIncendio(Id),
    FechaFinal DATETIME2(7) NULL,
    IdPlanEmergencia INT NULL FOREIGN KEY REFERENCES PlanEmergencia(Id),
    IdFaseEmergencia INT NULL FOREIGN KEY REFERENCES FaseEmergencia(Id),
    IdPlanSituacion INT NULL FOREIGN KEY REFERENCES PlanSituacion(Id),
    IdSituacionEquivalente INT NULL FOREIGN KEY REFERENCES SituacionEquivalente(Id),
	FechaHoraActualizacion DATETIME2(7) NOT NULL,
	Observaciones NVARCHAR(MAX) NULL,
    Prevision NVARCHAR(MAX) NULL,
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);


CREATE TABLE AreaAfectada (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdRegistro INT NOT NULL FOREIGN KEY REFERENCES Registro(Id),
    FechaHora DATETIME2(7) NOT NULL,
    IdProvincia INT NOT NULL FOREIGN KEY REFERENCES Provincia(Id),
    IdMunicipio INT NOT NULL FOREIGN KEY REFERENCES Municipio(Id),
    IdEntidadMenor INT NULL FOREIGN KEY REFERENCES EntidadMenor(Id),
    GeoPosicion GEOMETRY,
    Observaciones NVARCHAR(MAX) NULL,
	SuperficieAfectadaHectarea DECIMAL(10, 2) NULL,
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);
