SET IDENTITY_INSERT dbo.TipoSuceso ON;

INSERT INTO dbo.TipoSuceso (Id, Nombre, Descripcion, EsMigrado) VALUES
	 (1, N'Incendios', N'Incendio', 1),
	 (2, N'Fma', N'Suceso FMA', 1),
	 (3, N'IncendiosEnExtranjero', N'Incendio Extranjero', 1),
	 (4, N'Ope', N'Ope', 1),
	 (5, N'OtraInformacion', N'Otra Información', 1),
	 (6, N'Incendios forestales', N'Incendios forestales', 0),
	 (7, N'Accidentes de aviación civil', N'Accidentes de aviación civil', 0),
	 (8, N'Accidentes sustancias biológicas', N'Accidentes sustancias biológicas', 0),
	 (9, N'Inundaciones', N'Inundaciones', 0),
	 (10, N'Químico', N'Químico', 0),
	 (11, N'Terremotos', N'Terremotos', 0),
	 (12, N'TMP', N'Accidentes en el transporte de mercancías peligrosas', 0),
	 (13, N'Otros riesgos', N'Otros riesgos', 0),
	 (14, N'FMA', N'Fenómenos Meteorológicos Adversos', 0),
	 (15, N'Nuclear', N'Accidentes en centrales nucleares', 0),
	 (16, N'Radiológico', N'Accidentes con sustancias radiactivas', 0),
	 (17, N'Maremoto', N'Maremoto', 0),
	 (18, N'Volcanes', N'Erupción volcánica', 0),
	 (19, N'Accidentes en el transporte de viajeros por carretera y ferrocarril', N'Accidentes en el transporte de viajeros por carretera y ferrocarril', 0),
	 (20, N'Accidentes en túneles', N'Accidentes en túneles', 0),
	 (21, N'Aludes', N'Aludes', 0),
	 (22, N'Bélico', N'Bélico', 0),
	 (23, N'Contaminación marina', N'Contaminación marina', 0),
	 (24, N'Nevadas', N'Nevadas', 0),
	 (25, N'Viento', N'Viento', 0),
	 (26, N'OPE', N'Operación Paso del Estrecho', 0);

SET IDENTITY_INSERT dbo.TipoSuceso OFF;