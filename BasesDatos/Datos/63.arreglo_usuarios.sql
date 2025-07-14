
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO


UPDATE u
SET 
    u.Nombre       = a.Nombre,
    u.Apellidos    = a.Apellidos,
    u.PhoneNumber  = a.Telefono
FROM dbo.AspNetUsers AS u
JOIN dbo.ApplicationUsers AS a
    ON u.Id = a.IdentityId;

GO


UPDATE a
SET 
    a.Nombre    = u.Nombre,
    a.Apellidos = u.Apellidos,
    a.Telefono  = u.PhoneNumber
FROM dbo.ApplicationUsers AS a
JOIN dbo.AspNetUsers AS u
    ON a.Id = u.Id;
GO

	UPDATE ColumnasTM
SET Duplicado = 1
WHERE Columna = 'RutaImagen' AND IdTablasMaestras = 32;

UPDATE ColumnasTM
SET Duplicado = 1
WHERE Columna = 'Descripcion' AND IdTablasMaestras = 4;
