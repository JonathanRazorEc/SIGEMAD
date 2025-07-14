SET IDENTITY_INSERT OPE_Pais ON;

INSERT INTO OPE_Pais (Id, IdPais, Extranjero, OpePuertos, OpeDatosAsistencias, RutaImagen, Borrado)
VALUES 
(1, 60, 0, 1, 1, '/assets/assets/img/ope/administracion/bandera-espana.png', 'False'), -- España
(2, 19, 1, 0, 1, '/assets/assets/img/ope/administracion/bandera-espana.png', 'False'), -- Bélgica
(3, 65, 1, 0, 1, '/assets/assets/img/ope/administracion/bandera-espana.png', 'False'), -- Francia
(4, 93, 1, 0, 1, '/assets/assets/img/ope/administracion/bandera-espana.png', 'False'), -- Italia
(5, 131, 1, 0, 1, '/assets/assets/img/ope/administracion/bandera-espana.png', 'False'), -- Holanda
(6, 115, 1, 1, 0, '/assets/assets/img/ope/administracion/bandera-marruecos.png', 'False'), -- Marruecos
(7, 4, 1, 1, 0, '/assets/assets/img/ope/administracion/bandera-argelia.png', 'False'), -- Argelia
(8, 500, 1, 0, 1, '', 'False'); -- Otra

SET IDENTITY_INSERT OPE_Pais OFF;
--(8, 500, 1, 0, 1, '/assets/assets/img/ope/administracion/bandera-argelia.png', 'False'); -- Otra

UPDATE ColumnasTM
SET Duplicado = 1
WHERE Columna = 'RutaImagen' AND IdTablasMaestras = 32;
