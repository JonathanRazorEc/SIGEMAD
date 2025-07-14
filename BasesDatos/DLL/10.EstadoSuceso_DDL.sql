DROP TABLE IF EXISTS dbo.EstadoSuceso;
GO

CREATE TABLE dbo.EstadoSuceso (
	Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	Descripcion varchar (255) NOT NULL,
	Borrado bit DEFAULT 0 NOT NULL,
	Editable bit DEFAULT 1 NOT NULL
);