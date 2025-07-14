CREATE TABLE OPE_HIST_TipoPaisPuerto (
	Id int NOT NULL IDENTITY(1,1),
	Descripcion varchar(150) NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_TipoPaisPuerto PRIMARY KEY (Id)
);