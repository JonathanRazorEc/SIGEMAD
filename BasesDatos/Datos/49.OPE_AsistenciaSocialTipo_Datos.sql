SET IDENTITY_INSERT OPE_AsistenciaSocialTipo ON;

INSERT INTO OPE_AsistenciaSocialTipo (Id,Nombre, Borrado) VALUES
	 (1,N'Abandono de Personas', 'False'),
	 (2,N'Accidentes de Vehículos', 'False'),
	 (3,N'Averías de Vehículos', 'False'),
	 (4,N'Embarques Prioritarios', 'False'),
	 (5,N'Fraudes', 'False'),
	 (6,N'Extravío de Bienes', 'False'),
	 (7,N'Extravío de Personas', 'False'),
	 (8,N'Robos', 'False'),
	 (9,N'Solicitud de Información', 'False'),
	 (10,N'Problemas de Documentación', 'False');
INSERT INTO OPE_AsistenciaSocialTipo (Id,Nombre, Borrado) VALUES
	 (11,N'Otros', 'False');
	 
	 
SET IDENTITY_INSERT OPE_AsistenciaSocialTipo OFF;