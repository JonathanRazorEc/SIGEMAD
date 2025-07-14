CREATE TABLE OPE_HIST_TipoPaisResidencia (
	Id int NOT NULL IDENTITY(1,1),
	Descripcion varchar(150) NOT NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_TipoPaisResidencia PRIMARY KEY (Id)
);