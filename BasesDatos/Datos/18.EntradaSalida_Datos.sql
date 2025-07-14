SET IDENTITY_INSERT dbo.EntradaSalida ON;

INSERT INTO dbo.EntradaSalida (Id, Descripcion) VALUES
	 (1, N'Entrada'),
	 (2, N'Salida'),
	 (5, N'Interna'),
	 (6, N'Sin especificar');

SET IDENTITY_INSERT dbo.EntradaSalida OFF;