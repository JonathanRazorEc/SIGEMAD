CREATE TABLE OPE_HIST_DatoEmbarque (
	Id int NOT NULL IDENTITY(1,1),
	Fecha datetime2 NOT NULL,
	IdLinea int NOT NULL,
	Rotaciones int NOT NULL,
	Pasajeros int NOT NULL,
	Turismos int NOT NULL,
	Autocares int NOT NULL,
	Camiones int NOT NULL,
	UsrAltaAuditoria varchar(256) NOT NULL,
	FechaAltaAuditoria datetime2 NOT NULL,
	UsrAuditoria varchar(256) NULL,
	FechaAuditoria datetime2 NULL,
	--
	[Borrado] [bit] DEFAULT 0 NOT NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_DatoEmbarque PRIMARY KEY (Id),
	CONSTRAINT FK_OPE_HIST_LineaOPE_HIST_DatoEmbarque FOREIGN KEY (IdLinea) REFERENCES OPE_HIST_Linea(Id) 
	
	ON DELETE NO ACTION 
    ON UPDATE NO ACTION
);