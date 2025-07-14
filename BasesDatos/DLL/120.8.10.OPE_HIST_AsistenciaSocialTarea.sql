CREATE TABLE OPE_HIST_AsistenciaSocialTarea (
	Id int NOT NULL IDENTITY(1,1),
	IdAsistenciaSocial int NOT NULL,
	IdTipoAsistenciaTarea int NOT NULL,
	Numero int NOT NULL,
	Observaciones varchar(8000) NULL,
	--
	[Borrado] [bit] DEFAULT 0 NOT NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_AsistenciaSocialTarea PRIMARY KEY (Id),
	CONSTRAINT FK_OPE_HIST_AsistenciaSocialOPE_HIST_AsistenciaSocialTarea FOREIGN KEY (IdAsistenciaSocial) REFERENCES OPE_HIST_AsistenciaSocial(Id) 
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION,
	CONSTRAINT FK_OPE_HIST_TipoAsistenciaTareaOPE_HIST_AsistenciaSocialTarea FOREIGN KEY (IdTipoAsistenciaTarea) REFERENCES OPE_HIST_TipoAsistenciaTarea(Id) 
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION
);