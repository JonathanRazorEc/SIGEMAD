CREATE TABLE TipoDireccionEmergencia (
	Id int NOT NULL Identity(1,1),
	Descripcion nvarchar(255) NOT NULL,
	IdTipoSuceso int NULL,
	Borrado bit DEFAULT 0 NOT NULL,
	Editable bit NOT NULL,
	CONSTRAINT PK_TipoDireccionEmergencia PRIMARY KEY (Id),
	CONSTRAINT FK_TipoDireccionEmergencia_TipoSuceso FOREIGN KEY (IdTipoSuceso) REFERENCES TipoSuceso(Id)
);

CREATE TABLE TipoGestionDireccion (
	Id int NOT NULL Identity(1,1),
	Descripcion nvarchar(100) NOT NULL,
	Formulario int NOT NULL,
	Borrado bit DEFAULT 0 NOT NULL,
	Editable bit NOT NULL,
	CONSTRAINT PK_TipoGestionDireccion PRIMARY KEY (Id)
);



CREATE TABLE Direccion (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	--IdTipoGestionDireccion INT NOT NULL FOREIGN KEY REFERENCES TipoGestionDireccion(Id),
    IdRegistro INT NOT NULL FOREIGN KEY REFERENCES Registro(Id),
    IdTipoDireccionEmergencia INT NOT NULL FOREIGN KEY REFERENCES TipoDireccionEmergencia(Id),  -- Tipo de dirección, ejemplo: "Autonómica"
    AutoridadQueDirige NVARCHAR(255) NOT NULL,
    FechaInicio DATETIME2(7) NOT NULL,
    FechaFin DATETIME2(7) NULL,
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


CREATE TABLE CoordinacionCecopi (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	--IdTipoGestionDireccion INT NOT NULL FOREIGN KEY REFERENCES TipoGestionDireccion(Id),
    IdRegistro INT NOT NULL FOREIGN KEY REFERENCES Registro(Id),
    FechaInicio DATETIME2(7) NOT NULL,
    FechaFin DATETIME2(7) NULL,
    IdProvincia INT NOT NULL FOREIGN KEY REFERENCES Provincia(Id),
    IdMunicipio INT NOT NULL FOREIGN KEY REFERENCES Municipio(Id),
    Lugar NVARCHAR(255) NOT NULL,
    GeoPosicion GEOMETRY NULL,
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


CREATE TABLE CoordinacionPMA (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	--IdTipoGestionDireccion INT NOT NULL FOREIGN KEY REFERENCES TipoGestionDireccion(Id),
    IdRegistro INT NOT NULL FOREIGN KEY REFERENCES Registro(Id),
    FechaInicio DATETIME2(7) NOT NULL,
    FechaFin DATETIME2(7) NULL,
    IdProvincia INT NOT NULL FOREIGN KEY REFERENCES Provincia(Id),
    IdMunicipio INT NOT NULL FOREIGN KEY REFERENCES Municipio(Id),
    Lugar NVARCHAR(255) NOT NULL,
    GeoPosicion GEOMETRY NULL,
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

