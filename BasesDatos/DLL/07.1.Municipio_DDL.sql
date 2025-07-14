CREATE TABLE dbo.Municipio (
	Id int NOT NULL PRIMARY KEY,
	IdProvincia int NOT NULL FOREIGN KEY REFERENCES Provincia(Id),
	Descripcion varchar(255) NOT NULL,
	UtmX int NULL,
	UtmY int NULL,
	Huso varchar(3) NULL,
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

CREATE INDEX IX_Municipio ON dbo.Municipio (IdProvincia);


CREATE INDEX IX_Municipio_Descripcion
ON dbo.Municipio(Descripcion);
