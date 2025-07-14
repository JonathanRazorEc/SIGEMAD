DROP TABLE IF EXISTS dbo.TipoRegistro_;
GO

CREATE TABLE dbo.TipoRegistro_ (
	Id int NOT NULL IDENTITY(1,1),
	Descripcion varchar (255) NOT NULL,	
    CONSTRAINT PK_TipoRegistro PRIMARY KEY (Id)	
);