CREATE TABLE OPE_HIST_PeriodoDetalle (
	Id int NOT NULL IDENTITY(1,1),
	IdPeriodo int NOT NULL,
	Nombre varchar(250) NOT NULL,
	FechaInicial datetime2 NOT NULL,
	FechaFinal datetime2 NOT NULL,
	UsrAltaAuditoria varchar(150) NOT NULL,
	FechaAltaAuditoria datetime2 NOT NULL,
	FechaFinSalida datetime2 NULL,
	FechaInicioRetorno datetime2 NULL,
	
	--
	[Borrado] [bit] DEFAULT 0 NOT NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_PeriodoDetalle PRIMARY KEY (Id),
	CONSTRAINT FK_OPE_HIST_PeriodoOPE_HIST_PeriodoDetalle FOREIGN KEY (IdPeriodo) REFERENCES OPE_HIST_Periodo(Id) 
	
	ON DELETE NO ACTION 
    ON UPDATE NO ACTION
);