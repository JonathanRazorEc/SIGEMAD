DROP TABLE IF EXISTS dbo.EntradaSalida;
GO

CREATE TABLE dbo.EntradaSalida (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Descripcion varchar(255) NULL,
);