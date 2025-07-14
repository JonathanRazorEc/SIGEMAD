SET IDENTITY_INSERT OPE_EstadoOcupacion ON;

INSERT INTO OPE_EstadoOcupacion(Id, Nombre, PorcentajeInferior, PorcentajeSuperior, Borrado) VALUES 
(1, 'Baja', 0, 30, 'False'),
(2, 'Media', 30, 60, 'False'),
(3, 'Alta ', 60, 80, 'False'),
(4, 'Muy Alta', 80, 100, 'False');

SET IDENTITY_INSERT OPE_EstadoOcupacion OFF;
