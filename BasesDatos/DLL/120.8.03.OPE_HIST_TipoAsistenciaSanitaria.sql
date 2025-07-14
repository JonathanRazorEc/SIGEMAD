CREATE TABLE OPE_HIST_TipoAsistenciaSanitaria (
	Id int NOT NULL IDENTITY(1,1),
	Descripcion varchar(250) NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_TipoAsistenciaSanitaria PRIMARY KEY (Id)
);