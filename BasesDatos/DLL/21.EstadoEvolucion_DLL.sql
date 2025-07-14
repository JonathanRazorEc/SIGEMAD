DROP TABLE IF EXISTS dbo.EstadoEvolucion;
GO

CREATE TABLE dbo.EstadoEvolucion (
	Id int NOT NULL IDENTITY(1,1),
	Descripcion varchar (255) NOT NULL,	
    CONSTRAINT PK_EstadoEvolucion PRIMARY KEY (Id)	
);