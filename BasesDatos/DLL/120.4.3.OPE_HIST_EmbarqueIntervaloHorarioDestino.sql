CREATE TABLE OPE_HIST_EmbarqueIntervaloHorarioDestino (
	Id int NOT NULL,
	IdEmbarqueIntervalo int NOT NULL,
	NumeroVehiculos int NOT NULL,
	IdLineaDestino int NOT NULL,
	--
	[Borrado] [bit] DEFAULT 0 NOT NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_EmbarqueIntervaloHorarioDestino PRIMARY KEY (IdEmbarqueIntervalo,IdLineaDestino),
	CONSTRAINT FK_OPE_HIST_EmbarqueIntervaloHorarioDestinoLinea FOREIGN KEY (IdLineaDestino) REFERENCES OPE_HIST_Linea(Id)
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION,
	CONSTRAINT FK_OpeEmbarqueIntervaloHorarioOpeEmbarqueIntervaloDestino FOREIGN KEY (IdEmbarqueIntervalo) REFERENCES OPE_HIST_EmbarqueIntervaloHorario(Id) 
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION
);