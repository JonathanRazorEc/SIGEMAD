use Sigemad

UPDATE u
SET 
    u.Nombre    = a.Nombre,
    u.Apellidos = a.Apellidos
FROM AspNetUsers AS u
JOIN ApplicationUsers AS a
    ON u.Id = a.IdentityId;
