SET IDENTITY_INSERT dbo.Medio ON;

INSERT INTO dbo.Medio (Id,Descripcion) VALUES
	 (1,N'E-Mail'),
	 (2,N'Agencia de Noticias'),
	 (3,N'CECIS'),
	 (5,N'Agencia EFE'),
	 (6,N'Otra Fuente'),
	 (7,N'El País'),
	 (10,N'Fax'),
	 (11,N'Teléfono'),
	 (12,N'Sin especificar'),
	 (18,N'Walkie-Talkie');
INSERT INTO dbo.Medio (Id,Descripcion) VALUES
	 (21,N'Radiodifusión'),
	 (22,N'Correo'),
	 (23,N'Radio'),
	 (24,N'Televisión'),
	 (25,N'Telex'),
	 (26,N'Internet'),
	 (27,N'Fax y Fax Red Cibeles'),
	 (28,N'CIRCA'),
	 (29,N'Fax Red Cibeles'),
	 (30,N'SMS');
INSERT INTO dbo.Medio (Id,Descripcion) VALUES
	 (31,N'Mensaje (SMS)'),
	 (32,N'DSS REMFIRESAT'),
	 (33,N'Hotspot (Seviri)'),
	 (34,N'Fichero FTP'),
	 (41,N'Fax RECOSAT'),
	 (42,N'SIGE'),
	 (43,N'Teléfono y SMS'),
	 (44,N'Hot Spot'),
	 (45,N'Satelite'),
	 (46,N'seviri');
INSERT INTO dbo.Medio (Id,Descripcion) VALUES
	 (47,N'hotspot'),
	 (48,N'CECAT'),
	 (49,N'Modem'),
	 (50,N'Aplicación Puntos Calientes'),
	 (51,N'Red Hermes'),
	 (52,N'Recosat');

SET IDENTITY_INSERT dbo.Medio OFF;