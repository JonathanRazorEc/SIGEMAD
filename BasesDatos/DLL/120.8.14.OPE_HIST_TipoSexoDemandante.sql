CREATE TABLE OPE_HIST_TipoSexoDemandante (
	Id int NOT NULL,
	Descripcion varchar(50) NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_TipoSexoDemandante PRIMARY KEY (Id)
);