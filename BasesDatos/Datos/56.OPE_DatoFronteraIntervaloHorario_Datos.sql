SET IDENTITY_INSERT OPE_DatoFronteraIntervaloHorario ON;

INSERT INTO OPE_DatoFronteraIntervaloHorario (Id, Inicio, Fin, Borrado)
VALUES 
(1, '00:00', '07:59', 'False'),
(2, '08:00', '15:59', 'False'),
(3, '16:00', '23:59', 'False');

SET IDENTITY_INSERT OPE_DatoFronteraIntervaloHorario OFF;