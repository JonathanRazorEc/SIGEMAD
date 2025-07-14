CREATE TABLE MunicipioExtranjero (
	Id INT NOT NULL PRIMARY KEY,
	IdDistrito INT NOT NULL FOREIGN KEY REFERENCES Distrito(Id),
	CodigoOficial VARCHAR(10) NULL,
	Descripcion varchar(255) NOT NULL,
	UtmX int NULL,
	UtmY int NULL,
	Huso int NULL,
	EsFronterizo BIT NOT NULL DEFAULT 0,
	GeoPosicion GEOMETRY,
	---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);
GO