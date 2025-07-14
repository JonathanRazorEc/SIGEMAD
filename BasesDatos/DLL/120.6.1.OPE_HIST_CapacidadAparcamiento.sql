CREATE TABLE OPE_HIST_CapacidadAparcamiento (
	Id int NOT NULL IDENTITY(1,1),
	IdAreaEstacionamiento int NOT NULL,
	Capacidad int NOT NULL,
	IdPeriodoDetalle int NULL,
	--
	[Borrado] [bit] DEFAULT 0 NOT NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_CapacidadAparcamiento PRIMARY KEY (Id),
	CONSTRAINT FK_OPE_HIST_AreaEstacionamientoOPE_HIST_CapacidadAparcamiento FOREIGN KEY (IdAreaEstacionamiento) REFERENCES OPE_HIST_AreaEstacionamiento(Id) 
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION,
	CONSTRAINT FK_OPE_HIST_CapacidadAparcamiento_OPE_HIST_PeriodoDetalle FOREIGN KEY (IdPeriodoDetalle) REFERENCES OPE_HIST_PeriodoDetalle(Id) 
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION
);