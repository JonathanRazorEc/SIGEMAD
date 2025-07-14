DROP TABLE IF EXISTS dbo.TitularidadMedio;
GO

CREATE TABLE dbo.TitularidadMedio (
	Id int NOT NULL PRIMARY KEY,
	Nombre varchar(50) NOT NULL,
	Descripcion varchar(255) NOT NULL
);
CREATE UNIQUE INDEX IX_TipoTitularidad ON dbo.TitularidadMedio (Nombre);