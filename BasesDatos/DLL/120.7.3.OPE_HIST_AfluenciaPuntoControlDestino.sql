CREATE TABLE OPE_HIST_AfluenciaPuntoControlDestino (
	Id int NOT NULL IDENTITY(1,1),
	IdAfluenciaPuntoControl int NOT NULL,
	IdPuertoDestino int NOT NULL,
	NumeroVehiculos int NOT NULL,
	--
	[Borrado] [bit] DEFAULT 0 NOT NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_AfluenciaPuntoControlDestino PRIMARY KEY (IdAfluenciaPuntoControl,IdPuertoDestino),
	CONSTRAINT FK_OPE_HIST_AfluenciaPuntoControlOPE_HIST_AfluenciaPuntoControlDestino FOREIGN KEY (IdAfluenciaPuntoControl) REFERENCES OPE_HIST_AfluenciaPuntoControl(Id) 
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION,
	CONSTRAINT FK_OpePuertoOpeAfluenciaPuntoControlDestino FOREIGN KEY (IdPuertoDestino) REFERENCES OPE_HIST_Puerto(Id)
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION
);