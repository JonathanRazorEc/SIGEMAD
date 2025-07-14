DROP TABLE IF EXISTS dbo.Territorio;
GO

CREATE TABLE dbo.Territorio (
	Id int NOT NULL,
	Descripcion varchar(255) NOT NULL,
	Comun bit NOT NULL,
	CONSTRAINT Territorio_PK PRIMARY KEY (Id)
);