SET IDENTITY_INSERT OPE_HIST_TipoAsistenciaSocial ON;

INSERT INTO OPE_HIST_TipoAsistenciaSocial (Id,Descripcion) VALUES
	 (1,N'Abandono de Personas'),
	 (2,N'Accidentes de Vehículos'),
	 (3,N'Averías de Vehículos'),
	 (4,N'Embarques Prioritarios'),
	 (5,N'Fraudes'),
	 (6,N'Extravío de Bienes'),
	 (7,N'Extravío de Personas'),
	 (8,N'Robos'),
	 (9,N'Solicitud de Información'),
	 (10,N'Problemas de Documentación');
INSERT INTO OPE_HIST_TipoAsistenciaSocial (Id,Descripcion) VALUES
	 (11,N'Otros');

SET IDENTITY_INSERT OPE_HIST_TipoAsistenciaSocial OFF;