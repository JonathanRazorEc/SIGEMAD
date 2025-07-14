CREATE TABLE OPE_HIST_TipoOcupacion (
	Id int NOT NULL,
	Descripcion varchar(250) NOT NULL,
	Valor int NOT NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_TipoOcupacion PRIMARY KEY (Id)
);