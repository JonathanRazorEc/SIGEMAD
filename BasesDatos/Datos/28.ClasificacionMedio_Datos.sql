SET IDENTITY_INSERT dbo.ClasificacionMedio ON;

INSERT INTO dbo.ClasificacionMedio (Id,Descripcion) VALUES
	 (2,N'Vehículos'),
	 (3,N'Medios humanos'),
	 (4,N'Maquinaria pesada'),
	 (6,N'Brigada helitransportada'),
	 (7,N'Otros medios materiales'),
	 (8,N'Avión'),
	 (9,N'Helicóptero'),
	 (10,N'Módulo intervención'),
	 (11,N'Material de albergue'),
	 (12,N'Embarcaciones');

INSERT INTO dbo.ClasificacionMedio (Id,Descripcion) VALUES
	 (13,N'Aeronave');

SET IDENTITY_INSERT dbo.ClasificacionMedio OFF;