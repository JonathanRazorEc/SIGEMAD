CREATE TABLE TipoDocumento (
	Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	Descripcion NVARCHAR(255) NOT NULL,
	Borrado bit DEFAULT 0 NOT NULL,
	Editable bit DEFAULT 1 NOT NULL,
);

CREATE TABLE Documentacion (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdSuceso INT NOT NULL FOREIGN KEY REFERENCES Suceso(Id),
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
    CreadoPor UNIQUEIDENTIFIER NULL,
    FechaModificacion DATETIME2(7) NULL,
    ModificadoPor UNIQUEIDENTIFIER NULL,
    FechaEliminacion DATETIME2(7) NULL,
    EliminadoPor UNIQUEIDENTIFIER NULL,
    Borrado BIT NOT NULL DEFAULT 0

	CONSTRAINT UQ_Documentacion_IdSuceso UNIQUE (IdSuceso)
);
 
 
CREATE TABLE DetalleDocumentacion (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdDocumentacion INT NOT NULL FOREIGN KEY REFERENCES Documentacion(Id),
    FechaHora DATETIME2(7) NOT NULL,
    FechaHoraSolicitud DATETIME2(7) NOT NULL,
    IdTipoDocumento INT NOT NULL FOREIGN KEY REFERENCES TipoDocumento(Id),
    Descripcion NVARCHAR(255) NULL,
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
 
 
CREATE TABLE Documentacion_ProcedenciaDestino (
    IdDetalleDocumentacion int NOT NULL FOREIGN KEY REFERENCES DetalleDocumentacion(Id),
    IdProcedenciaDestino int NOT NULL FOREIGN KEY REFERENCES ProcedenciaDestino(Id),
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
    CreadoPor UNIQUEIDENTIFIER NULL,
    FechaModificacion DATETIME2(7) NULL,
    ModificadoPor UNIQUEIDENTIFIER NULL,
    FechaEliminacion DATETIME2(7) NULL,
    EliminadoPor UNIQUEIDENTIFIER NULL,
    Borrado BIT NOT NULL DEFAULT 0

		-- Composite Primary Key
    CONSTRAINT PK_Documentacion_ProcedenciaDestino PRIMARY KEY (IdDetalleDocumentacion, IdProcedenciaDestino)
);