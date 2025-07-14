CREATE TABLE OPE_HIST_DatoFrontera (
	Id int NOT NULL IDENTITY(1,1),
	IdFrontera int NOT NULL,
	Turismos int NOT NULL,
	Autocares int NOT NULL,
	FechaInicio datetime2 NOT NULL,
	FechaFin datetime2 NOT NULL,
	IdIntervalo int NOT NULL,
	UsrAltaAuditoria varchar(256) NOT NULL,
	FechaAltaAuditoria datetime2 NOT NULL,
	UsrAuditoria varchar(256) NULL,
	FechaAuditoria datetime2 NULL,
	--
	[Borrado] [bit] DEFAULT 0 NOT NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_DatoFrontera PRIMARY KEY (Id),
	CONSTRAINT FK_OPE_HIST_Frontera_OPE_HIST_DatoFrontera FOREIGN KEY (IdFrontera) REFERENCES OPE_HIST_Frontera(Id)
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION,
	CONSTRAINT FK_OPE_HIST_TipoHoraFronteraOPE_HIST_DatoFrontera FOREIGN KEY (IdIntervalo) REFERENCES OPE_HIST_TipoHoraFrontera(Id) 
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION
);