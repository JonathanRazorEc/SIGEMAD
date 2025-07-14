
CREATE TABLE dbo.OtraInformacion (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdSuceso INT NULL FOREIGN KEY REFERENCES Suceso(Id), 
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0

	CONSTRAINT UQ_OtraInformacion_IdSuceso UNIQUE (IdSuceso)
);

CREATE TABLE dbo.DetalleOtraInformacion (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdOtraInformacion int NOT NULL FOREIGN KEY REFERENCES OtraInformacion(Id),
    FechaHora DATETIME2(7) NOT NULL,
    IdMedio INT NOT NULL FOREIGN KEY REFERENCES Medio(Id),
    Asunto NVARCHAR(500) NULL,
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

CREATE TABLE dbo.DetalleOtraInformacion_ProcedenciaDestino (
	IdDetalleOtraInformacion int NOT NULL FOREIGN KEY REFERENCES DetalleOtraInformacion(Id),
	IdProcedenciaDestino int NOT NULL FOREIGN KEY REFERENCES ProcedenciaDestino(Id),
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0

	CONSTRAINT PK_DetalleOtraInformacion_ProcedenciaDestino PRIMARY KEY (IdDetalleOtraInformacion, IdProcedenciaDestino)
);
