CREATE TABLE OPE_HIST_TipoHoraPuntoControl (
	Id int NOT NULL,
	Descripcion varchar(150) NOT NULL,
	HoraInicio varchar(50) NOT NULL,
	HoraFin varchar(50) NOT NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_TipoHoraPuntoControl PRIMARY KEY (Id)
);