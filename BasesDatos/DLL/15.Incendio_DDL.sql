
CREATE TABLE dbo.Incendio (
	Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdSuceso int NOT NULL FOREIGN KEY REFERENCES Suceso(Id),
	IdTerritorio int NOT NULL FOREIGN KEY REFERENCES Territorio(Id),
	IdClaseSuceso int NOT NULL FOREIGN KEY REFERENCES ClaseSuceso(Id),
	IdEstadoSuceso int NULL FOREIGN KEY REFERENCES EstadoSuceso(Id),
	IdPais INT NOT NULL FOREIGN KEY REFERENCES dbo.Pais(Id),
	EsLimitrofe BIT NOT NULL DEFAULT 0,
	IdDistrito INT FOREIGN KEY REFERENCES dbo.Distrito(Id),
	IdMunicipioExtranjero INT FOREIGN KEY REFERENCES dbo.MunicipioExtranjero(Id),
	IdProvincia INT FOREIGN KEY REFERENCES dbo.Provincia(Id),
    IdMunicipio INT FOREIGN KEY REFERENCES dbo.Municipio(Id),
	FechaInicio DATETIME2(7) NOT NULL,
	Ubicacion NVARCHAR(255) NULL,
	Denominacion NVARCHAR(255) NOT NULL,
	NotaGeneral NVARCHAR(MAX) NULL,
	RutaMapaRiesgo NVARCHAR(MAX),
	UTM_X decimal(18,9) NULL,
	UTM_Y decimal(18,9) NULL,
	Huso int NULL,
    GeoPosicion GEOMETRY NULL,
	---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor UNIQUEIDENTIFIER NULL,
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor UNIQUEIDENTIFIER NULL,
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor UNIQUEIDENTIFIER NULL,
	Borrado BIT NOT NULL DEFAULT 0
);

CREATE INDEX IX_Incendio ON dbo.Incendio (Denominacion);
CREATE INDEX IX_Incendio_1 ON dbo.Incendio (IdSuceso);

ALTER TABLE Incendio ALTER COLUMN FechaInicio datetimeoffset;
