CREATE TABLE OPE_HIST_TipoOperacion (
	Id int NOT NULL,
	Descripcion varchar(50) NOT NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_TipoOperacion PRIMARY KEY (Id)
);