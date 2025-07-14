DROP TABLE IF EXISTS dbo.Pais;
GO

CREATE TABLE dbo.Pais (
	Id int NOT NULL,
	Descripcion varchar(255) NOT NULL,
	Fronterizo BIT NOT NULL DEFAULT 0,
	X decimal(18,6) NULL,
	Y decimal(18,6) NULL,
	GeoPosicion GEOMETRY,
	CONSTRAINT Pais_PK PRIMARY KEY (Id)
);