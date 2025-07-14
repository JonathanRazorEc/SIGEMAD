CREATE TABLE OPE_HIST_AsistenciaSocialOrganismo (
	Id int NOT NULL IDENTITY(1,1),
	IdAsistenciaSocial int NOT NULL,
	IdTipoOrganismo int NOT NULL,
	Numero int NOT NULL,
	Observaciones varchar(8000) NOT NULL,
	--
	[Borrado] [bit] DEFAULT 0 NOT NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_AsistenciaSocialOrganismo PRIMARY KEY (Id),
	CONSTRAINT FK_OPE_HIST_AsistenciaSocialOPE_HIST_AsistenciaSocialOrganismo FOREIGN KEY (IdAsistenciaSocial) REFERENCES OPE_HIST_AsistenciaSocial(Id) 
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION,
	CONSTRAINT FK_OPE_HIST_TipoOrganismoOPE_HIST_AsistenciaSocialOrganismo FOREIGN KEY (IdTipoOrganismo) REFERENCES OPE_HIST_TipoOrganismo(Id) 
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION
);