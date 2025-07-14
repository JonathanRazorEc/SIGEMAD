CREATE TABLE OPE_HIST_OcupacionAreaEstacionamiento (
	Id int NOT NULL IDENTITY(1,1),
	IdAreaEstacionamiento int NOT NULL,
	IdOcupacion int NULL,
	Fecha datetime2 NULL,
	NumeroVehiculos int NULL,
	Observaciones varchar(250) NULL,
	UsrAltaAuditoria varchar(256) NOT NULL,
	FechaAltaAuditoria datetime2 NOT NULL,
	UsrAuditoria varchar(256) NULL,
	FechaAuditoria datetime2 NULL,
	--
	[Borrado] [bit] DEFAULT 0 NOT NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OcupacionAreaEstacionamiento PRIMARY KEY (Id),
	CONSTRAINT FK_OPE_HIST_AreaEstacionamientoOPE_HIST_OcupacionAreaEstacionamiento FOREIGN KEY (IdAreaEstacionamiento) REFERENCES OPE_HIST_AreaEstacionamiento(Id) 
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION,
	CONSTRAINT FK_OPE_HIST_TipoOcupacionOPE_HIST_OcupacionAreaEstacionamiento FOREIGN KEY (IdOcupacion) REFERENCES OPE_HIST_TipoOcupacion(Id)
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION
);