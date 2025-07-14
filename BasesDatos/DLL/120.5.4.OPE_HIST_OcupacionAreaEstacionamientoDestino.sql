CREATE TABLE OPE_HIST_OcupacionAreaEstacionamientoDestino (
	Id int NOT NULL IDENTITY(1,1),
	IdOcupacionAreaEstacionamiento int NOT NULL,
	IdPuertoDestino int NOT NULL,
	NumeroVehiculos int NULL,
	EsperaMaxima int NOT NULL,
	--
	[Borrado] [bit] DEFAULT 0 NOT NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_AreaEstacionamientoDestino PRIMARY KEY (IdOcupacionAreaEstacionamiento,IdPuertoDestino),
	CONSTRAINT FK_OPE_HIST_OcupacionAreaEstacionamientoOPE_HIST_OcupacionAreaEstacionamientoDestino FOREIGN KEY (IdOcupacionAreaEstacionamiento) REFERENCES OPE_HIST_OcupacionAreaEstacionamiento(Id) 
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION,
	CONSTRAINT FK_OpePuertoOpeOcupacionAreaEstacionamientoDestino FOREIGN KEY (IdPuertoDestino) REFERENCES OPE_HIST_Puerto(Id) 
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION
);