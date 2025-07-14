-- Insertar valores iniciales
INSERT INTO EstadoRegistro (Id, Nombre) VALUES 
(1, 'Creado'),
(2, 'Creado en registro anterior'),
(3, 'Creado y Modificado'),
(4, 'Modificado'),
(5, 'Permanente'),
(6, 'Eliminado'),
(7, 'Creado y Eliminado');


-- Insertar Tipos de Registro
INSERT INTO TipoRegistroActualizacion (Id, Nombre) VALUES 
(1, 'Datos de evolución'),
(2, 'Dirección y Coordinación'),
(3, 'Actuaciones Relevantes'),
(4, 'Documentación'),
(5, 'Otra Información'),
(6, 'Sucesos Relacionados'),
(7, 'Registro');


-- Insertar Apartados de Evolución
INSERT INTO ApartadoRegistro (Id, IdTipoRegistroActualizacion, Nombre, Orden) VALUES 
(2, 1, 'Datos principales', 2),
(3, 1, 'Parámetros', 3),
(4, 1, 'Área Afectada', 4),
(5, 1, 'Consecuencias / Actuaciones', 5),
(6, 1, 'Intervención de Medios', 6);

-- Insertar Apartados de Direccion y Coordinación
INSERT INTO ApartadoRegistro (Id, IdTipoRegistroActualizacion, Nombre, Orden) VALUES 
(7, 2, 'Dirección', 1),
(8, 2, 'Coord. CECOPI', 2),
(9, 2, 'Coord. PMA', 3);

-- Insertar Apartados de Actuaciones Relevantes
INSERT INTO ApartadoRegistro (Id, IdTipoRegistroActualizacion, Nombre, Orden) VALUES 
(10, 3, 'Movilización', 1),
(11, 3, 'Conv. CECOD', 2),
(12, 3, 'Activación Planes', 3),
(13, 3, 'Notificaciones oficiales', 4),
(14, 3, 'Activación de Sistemas', 5),
(15, 3, 'Declaración ZAGEP', 6),
(16, 3, 'Emergencia Nacional', 7);

-- Insertar Apartados de Documentacion, Otra Información y Sucesos Relacionados
INSERT INTO ApartadoRegistro (Id, IdTipoRegistroActualizacion, Nombre, Orden) VALUES 
(17, 4, 'Documentación', 1),
(18, 5, 'Otra Información', 1),
(19, 6, 'Sucesos Relacionados', 1);