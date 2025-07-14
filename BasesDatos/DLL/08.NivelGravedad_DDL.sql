-- dbo.NivelGravedad definition

-- Drop table

-- DROP TABLE dbo.NivelGravedad;

CREATE TABLE dbo.NivelGravedad (
	Id int NOT NULL IDENTITY(1,1),
	Descripcion varchar(255) NOT NULL,
	Gravedad int NOT NULL,
	RiesgoAemet int NOT NULL,
	EsNivelDeParte bit NULL,
	OrdenAemet int NULL,
	OrdenParte int NULL,
	CONSTRAINT NivelGravedad_PK PRIMARY KEY (Id)
);