CREATE TABLE OPE_HIST_AreaEstacionamiento (
	Id int NOT NULL IDENTITY(1,1),
	Nombre varchar(200) NOT NULL,
	IdPuerto int NULL,
	UtmX int NULL,
	UtmY int NULL,
	Capacidad int NOT NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_AreaEstacionamiento PRIMARY KEY (Id),
	CONSTRAINT FK_OPE_HIST_PuertoOpeAreaEstacionamiento FOREIGN KEY (IdPuerto) REFERENCES OPE_HIST_Puerto(Id) 
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION
);