CREATE TABLE OPE_HIST_AsistenciaSanitaria (
	Id int NOT NULL IDENTITY(1,1),
	IdAsistencia int NOT NULL,
	IdTipoAsistenciaSanitaria int NOT NULL,
	Numero int NOT NULL,
	Observaciones varchar(8000) NULL,
	--
	[Borrado] [bit] DEFAULT 0 NOT NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_AsistenciaSanitaria PRIMARY KEY (Id),
	CONSTRAINT FK_OPE_HIST_AsistenciaOPE_HIST_AsistenciaSanitaria FOREIGN KEY (IdAsistencia) REFERENCES OPE_HIST_Asistencia(Id) 
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION,
	CONSTRAINT FK_OPE_HIST_TipoAsistenciaSanitariaOPE_HIST_AsistenciaSanitaria FOREIGN KEY (IdTipoAsistenciaSanitaria) REFERENCES OPE_HIST_TipoAsistenciaSanitaria(Id) 
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION
);