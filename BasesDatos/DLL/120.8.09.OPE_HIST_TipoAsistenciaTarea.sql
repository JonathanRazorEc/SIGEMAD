CREATE TABLE OPE_HIST_TipoAsistenciaTarea (
	Id int NOT NULL IDENTITY(1,1),
	Descripcion varchar(250) NOT NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_TipoAsistenciaTarea PRIMARY KEY (Id)
);