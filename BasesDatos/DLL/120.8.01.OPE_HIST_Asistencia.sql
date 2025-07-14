CREATE TABLE OPE_HIST_Asistencia (
	Id int NOT NULL IDENTITY(1,1),
	IdPuerto int NOT NULL,
	Fecha datetime2 NOT NULL,
	UsrAltaAuditoria varchar(256) NOT NULL,
	FechaAltaAuditoria datetime2 NOT NULL,
	UsrAuditoria varchar(256) NULL,
	FechaAuditoria datetime2 NULL,
	--
	[Borrado] [bit] DEFAULT 0 NOT NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_Asistencia PRIMARY KEY (Id),
	CONSTRAINT FK_OPE_HIST_PuertoOPE_HIST_Asistencia FOREIGN KEY (IdPuerto) REFERENCES OPE_HIST_Puerto(Id) 
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION
);