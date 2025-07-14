SET IDENTITY_INSERT dbo.ComparativaFecha ON;

INSERT INTO dbo.ComparativaFecha (Id, Descripcion) VALUES
	 (1, N'entre'),
	 (2, N'igual a'),
	 (3, N'mayor que'),
	 (4, N'menor que'),
	 (5, N'no entre');

SET IDENTITY_INSERT dbo.ComparativaFecha OFF;