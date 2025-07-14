CREATE TABLE OPE_Menu (
	Id int NOT NULL,
	Nombre varchar(100) NOT NULL,
	Tipo varchar(5) NOT NULL,
	IdGrupo int NOT NULL,
	NumOrden int NOT NULL,
	Ruta varchar(100) NULL,
	Icono varchar(100) NULL,
	ColorRgb varchar(20) NULL,
	CONSTRAINT PK_opeMenu PRIMARY KEY (Id)
);