CREATE TABLE OPE_HIST_PuntoControl (
	Id int NOT NULL IDENTITY(1,1),
	Nombre varchar(250) NOT NULL,
	IdPuerto int NULL,
	UtmX int NOT NULL,
	UtmY int NOT NULL,
	UmbralAlto int NOT NULL,
	UmbralMedio int NOT NULL,
	UmbralMuyAlto int NOT NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_PuntoControl PRIMARY KEY (Id),
	CONSTRAINT FK_OPE_HIST_PuertoOPE_HIST_PuntoControl FOREIGN KEY (IdPuerto) REFERENCES OPE_HIST_Puerto(Id) 
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION
);