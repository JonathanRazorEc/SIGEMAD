CREATE TABLE OPE_HIST_AfluenciaPuntoControl (
	Id int NOT NULL IDENTITY(1,1),
	IdPuntoControl int NOT NULL,
	VehiculosSinBillete int NULL,
	TotalVehiculos int NULL,
	FechaHoraInicio datetime2 NOT NULL,
	FechaHoraFin datetime2 NULL,
	Observaciones varchar(8000) NULL,
	UsrAltaAuditoria varchar(256) NOT NULL,
	FechaAltaAuditoria datetime2 NOT NULL,
	UsrAuditoria varchar(256) NULL,
	FechaAuditoria datetime2 NULL,
	--
	[Borrado] [bit] DEFAULT 0 NOT NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_AfluenciaPuntoControl PRIMARY KEY (Id),
	CONSTRAINT FK_OPE_HIST_PuntoControlOPE_HIST_AfluenciaPuntoControl FOREIGN KEY (IdPuntoControl) REFERENCES OPE_HIST_PuntoControl(Id) 
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION
);