CREATE TABLE OPE_HIST_TipoEdadDemandante (
	Id int NOT NULL,
	Descripcion varchar(250) NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_TipoEdadDemandante PRIMARY KEY (Id)
);