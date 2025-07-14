SET IDENTITY_INSERT dbo.ClaseSuceso ON;

INSERT INTO dbo.ClaseSuceso (Id, Descripcion,Borrado,Editable) VALUES
	(1, N'Suceso Real',0,1),
	(2, N'Ejercicio / Simulacro',0,1);

SET IDENTITY_INSERT dbo.ClaseSuceso OFF;
