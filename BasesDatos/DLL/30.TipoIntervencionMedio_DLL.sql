DROP TABLE IF EXISTS dbo.TipoIntervencionMedio;
GO

CREATE TABLE dbo.TipoIntervencionMedio (
	Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	Descripcion varchar(255) NOT NULL,
	IdClasificacion int NULL,
	IdTitularidad int NULL,
	IdTitularidadEstatal int NULL,
	IdTitularidadAutonomica int NULL,
	IdTitularidadAutonomicaMunicipal int NULL,
	IdTitularidadProvinciaMunicipal int NULL,
	IdTitularidadMunicipal int NULL,
	IdTitularidadPais int NULL,
	TitularidadOtra varchar(255) NULL,
	CONSTRAINT FK_TipoMedioPresente_CCAA FOREIGN KEY (IdTitularidadAutonomica) REFERENCES CCAA(Id),
	CONSTRAINT FK_TipoMedioPresente_CCAA1 FOREIGN KEY (IdTitularidadAutonomicaMunicipal) REFERENCES CCAA(Id),
	CONSTRAINT FK_TipoMedioPresente_MediosClasificacion FOREIGN KEY (IdClasificacion) REFERENCES ClasificacionMedio(Id),
	CONSTRAINT FK_TipoMedioPresente_Municipio FOREIGN KEY (IdTitularidadMunicipal) REFERENCES Municipio(Id),
	CONSTRAINT FK_TipoMedioPresente_Pais FOREIGN KEY (IdTitularidadPais) REFERENCES Pais(Id),
	CONSTRAINT FK_TipoMedioPresente_Provincia FOREIGN KEY (IdTitularidadProvinciaMunicipal) REFERENCES Provincia(Id),
	CONSTRAINT FK_TipoMedioPresente_TipoEntidadTitularidadMedio FOREIGN KEY (IdTitularidadEstatal) REFERENCES TipoEntidadTitularidadMedio(Id),
	CONSTRAINT FK_TipoMedioPresente_TitularidadMedio FOREIGN KEY (IdTitularidad) REFERENCES TitularidadMedio(Id)
);
CREATE UNIQUE INDEX IX_TipoMedioPresente ON dbo.TipoIntervencionMedio (Descripcion);