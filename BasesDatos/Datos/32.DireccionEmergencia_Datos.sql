SET IDENTITY_INSERT dbo.TipoDireccionEmergencia ON;

INSERT INTO dbo.TipoDireccionEmergencia (Id, Descripcion,IdTipoSuceso,Borrado,Editable) VALUES
(1, N'Estatal',NULL,0,1),
(2, N'Autonómica',NULL,0,1),
(3, N'Municipal',NULL,0,1),
(4, N'Provincial',NULL,0,1),
(5, N'Sin especificar',NULL,0,1),
(6, N'MUE',6,0,1);

SET IDENTITY_INSERT dbo.TipoDireccionEmergencia OFF;



SET IDENTITY_INSERT dbo.TipoGestionDireccion ON;

INSERT INTO dbo.TipoGestionDireccion (id, Descripcion, Formulario, Borrado, Editable) VALUES 
(1, N'Dirección', 1, 0, 1), 
(2, N'Coordinación CECOPI', 2, N'0', 1), 
(3, N'Coordinación PMA', 2, N'0', 1); 

SET IDENTITY_INSERT dbo.TipoGestionDireccion OFF;