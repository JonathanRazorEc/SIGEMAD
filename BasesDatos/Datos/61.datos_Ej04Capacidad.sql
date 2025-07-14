/*EJECUTAR CAPACIDAD EJ04 DESPUES DE TODOS LOS DATOS*/

IF EXISTS (SELECT 1 FROM Capacidad)
BEGIN
    INSERT INTO dbo.Ej04Capacidad
    (Nombre, Descripcion, Gestionado, IdTipoCapacidad, IdEntidad)
    SELECT Nombre, Descripcion, Gestionado, IdTipoCapacidad, IdEntidad
    FROM Capacidad;
END

UPDATE TablasMaestras
SET etiqueta = N'Situación operativa equivalente'
WHERE tabla = 'EJ01SituacionEquivalente';