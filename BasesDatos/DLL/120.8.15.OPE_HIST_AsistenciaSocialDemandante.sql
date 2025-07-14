CREATE TABLE OPE_HIST_AsistenciaSocialDemandante (
	Id int NOT NULL IDENTITY(1,1),
	IdAsistenciaSocial int NOT NULL,
	Numero int NOT NULL,
	IdTipoNacionalidadDemandante int NULL,
	IdTipoEdadDemandante int NULL,
	IdTipoSexoDemandante int NULL,
	IdTipoPaisResidencia int NULL,
	Observaciones varchar(8000) NULL,
	--
	[Borrado] [bit] DEFAULT 0 NOT NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_AsistenciaSocialDemandante PRIMARY KEY (Id),
	CONSTRAINT FK_OPE_HIST_AsistenciaSocialOPE_HIST_AsistenciaSocialDemandante FOREIGN KEY (IdAsistenciaSocial) REFERENCES OPE_HIST_AsistenciaSocial(Id) ON DELETE NO ACTION 
		 ON UPDATE NO ACTION,
	CONSTRAINT FK_OPE_HIST_TipoEdadDemandanteOPE_HIST_AsistenciaSocialDemandante FOREIGN KEY (IdTipoEdadDemandante) REFERENCES OPE_HIST_TipoEdadDemandante(Id) 
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION,
	CONSTRAINT FK_OPE_HIST_TipoNacionalidadDemandanteOPE_HIST_AsistenciaSocialDemandante FOREIGN KEY (IdTipoNacionalidadDemandante) REFERENCES OPE_HIST_TipoNacionalidadDemandante(Id) 
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION,
	CONSTRAINT FK_OPE_HIST_TipoPaisResidenciaOPE_HIST_AsistenciaSocialDemandante FOREIGN KEY (IdTipoPaisResidencia) REFERENCES OPE_HIST_TipoPaisResidencia(Id) 
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION,
	CONSTRAINT FK_OPE_HIST_TipoSexoDemandanteOPE_HIST_AsistenciaSocialDemandante FOREIGN KEY (IdTipoSexoDemandante) REFERENCES OPE_HIST_TipoSexoDemandante(Id) 
		ON DELETE NO ACTION 
		ON UPDATE NO ACTION
);