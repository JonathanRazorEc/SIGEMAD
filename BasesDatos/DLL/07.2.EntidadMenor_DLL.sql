CREATE TABLE dbo.EntidadMenor (
	Id int NOT NULL PRIMARY KEY,
	IdMunicipio int NOT NULL FOREIGN KEY REFERENCES Municipio(Id),
	Descripcion varchar(255) NOT NULL,
	UtmX int NULL,
	UtmY int NULL,
	Huso int NULL,
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

CREATE INDEX IX_EntidadMenor_IdMunicipio 
ON EntidadMenor(IdMunicipio);

CREATE INDEX IX_EntidadMenor_Descripcion
ON EntidadMenor(Descripcion);
