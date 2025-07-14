-- SIGEMAD.dbo.ColumnasTM definition

-- Drop table

-- DROP TABLE dbo.ColumnasTM;

CREATE TABLE ColumnasTM (
	Id int NOT NULL IDENTITY(1,1),
	IdTablasMaestras int NOT NULL ,
	Columna varchar(50) NOT NULL,
	Orden int NOT NULL,
	Etiqueta varchar(255),
	Busqueda bit,
	Defecto varchar(50),
	Nulo bit,
	Duplicado bit,
	Tipo varchar(50),
	Longitud int,
	TablaRelacionada varchar(50),
	CampoRelacionado varchar(50),
	ApareceEnListado bit, -- si debe aparecer en el listado (sirve para los logs, simplificar los listados)
	ApareceEnEdicion bit -- -- si debe aparecer en la edición
	CONSTRAINT ColumnasTM_PK PRIMARY KEY (Id),
	CONSTRAINT FK_TablasMaestras FOREIGN KEY (IdTablasMaestras) REFERENCES TablasMaestras(Id)
);

