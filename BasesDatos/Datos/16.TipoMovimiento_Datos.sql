SET IDENTITY_INSERT dbo.TipoMovimiento ON;

INSERT INTO dbo.TipoMovimiento (Id, Descripcion) VALUES
	 (1, N'Registro'),
	 (2, N'Inicio suceso'),
	 (3, N'Modificación'),
	 (4, N'Cualquiera');

SET IDENTITY_INSERT dbo.TipoMovimiento OFF;