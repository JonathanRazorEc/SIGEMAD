-- SIGEMAD.dbo.TablasMaestras definition

-- Drop table

-- DROP TABLE SIGEMAD.dbo.TablasMaestras;

CREATE TABLE TablasMaestras (
	Id int NOT NULL IDENTITY(1,1),
	tabla varchar(255) NOT NULL,
	etiqueta varchar(255) NOT NULL,
	[IdTablaMaestraGrupo] int NOT NULL FOREIGN KEY REFERENCES TablasMaestrasGrupos(Id),
	Listable bit, -- indica si la tabla aparecerá en los listados
	Editable bit, -- indica si sus registros se podrán editar o no
	CONSTRAINT TablasMaestras_PK PRIMARY KEY (Id)
);
