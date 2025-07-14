CREATE TABLE OPE_HIST_Puerto (
	Id int NOT NULL IDENTITY(1,1),
	Nombre varchar(150) NOT NULL,
	IdTipoOperacion int NOT NULL,
	IdPais int NOT NULL,
	Principal bit NOT NULL,
	UtmX int NOT NULL,
	UtmY int NOT NULL,
	FechaDesde datetime2 NOT NULL,
	FechaHasta datetime2 NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_Puerto PRIMARY KEY (Id),
	CONSTRAINT FK_OPE_HIST_TipoOperacionOPE_HIST_Puerto FOREIGN KEY (IdTipoOperacion) REFERENCES OPE_HIST_TipoOperacion(Id) 
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION,
	
	CONSTRAINT FK_OPE_HIST_TipoPaisPuertoOPE_HIST_Puerto FOREIGN KEY (IdPais) REFERENCES OPE_HIST_TipoPaisPuerto(Id)
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION
);