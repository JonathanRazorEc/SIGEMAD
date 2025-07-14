CREATE TABLE GrupoMedio (
    Id INT NOT NULL PRIMARY KEY,
    Nombre NVARCHAR(255) NULL
);

CREATE TABLE SubgrupoMedio (
    Id INT NOT NULL PRIMARY KEY,
    Nombre NVARCHAR(255) NULL,
	IdGrupoMedio INT NOT NULL FOREIGN KEY REFERENCES GrupoMedio(Id),
);

CREATE TABLE TipoMedio (
    Id INT NOT NULL PRIMARY KEY,
    Nombre NVARCHAR(255) NULL,
	IdSubgrupoMedio INT NOT NULL FOREIGN KEY REFERENCES SubgrupoMedio(Id),
);

CREATE TABLE MediosCapacidad (
    Id INT NOT NULL PRIMARY KEY,
	IdTipoCapacidad INT NOT NULL FOREIGN KEY REFERENCES TipoCapacidad(Id),
	IdTipoMedio INT NOT NULL FOREIGN KEY REFERENCES TipoMedio(Id),
    Descripcion NVARCHAR(255) NULL,
	NumeroMedio INT NOT NULL
);



CREATE TABLE IntervencionMedio (
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdRegistro INT NOT NULL FOREIGN KEY REFERENCES dbo.Registro(Id),
	IdCaracterMedio INT NOT NULL FOREIGN KEY REFERENCES dbo.CaracterMedio(Id),
	IdCapacidad INT NOT NULL FOREIGN KEY REFERENCES dbo.Capacidad(Id),
	Descripcion NVARCHAR(255) NULL,
	MedioNoCatalogado NVARCHAR(255) NULL,
	NumeroCapacidades INT NOT NULL DEFAULT 1,
	IdTitularidadMedio int NOT NULL FOREIGN KEY REFERENCES dbo.TitularidadMedio(Id),
	Titular NVARCHAR(255) NULL,
	FechaHoraInicio DATETIME2(7) NOT NULL,
	FechaHoraFin DATETIME2(7) NULL,
	IdProvincia INT NULL FOREIGN KEY REFERENCES Provincia(Id),
    IdMunicipio INT NULL FOREIGN KEY REFERENCES Municipio(Id),
    GeoPosicion GEOMETRY,
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

CREATE TABLE DetalleIntervencionMedio (
	IdIntervencionMedio INT NOT NULL FOREIGN KEY REFERENCES IntervencionMedio(Id),
	IdMediosCapacidad INT NOT NULL FOREIGN KEY REFERENCES MediosCapacidad(Id),
	NumeroIntervinientes INT NOT NULL,
	-- Audit Fields
	FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
	-- Composite Primary Key
    CONSTRAINT PK_DetalleIntervencionMedio PRIMARY KEY (IdIntervencionMedio, IdMediosCapacidad)
);
