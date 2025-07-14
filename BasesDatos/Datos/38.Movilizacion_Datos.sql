-- =============================================
-- PROCEDENCIA
-- =============================================

INSERT INTO ProcedenciaMedio (Id,Descripcion) VALUES
    (1, 'CCAA'),
	(2, 'Órgano SNPC'),
	(3, 'Iniciativa DGPCE');

-- =============================================
-- DESTINO
-- =============================================

INSERT INTO DestinoMedio (Id,Descripcion) VALUES
    (1, 'CCAA'),
	(2, 'UME'),
	(3, 'Otros SNPC'),
	(4, 'DGPCE'),
	(5, 'Países bilaterales'),
	(6, 'UCPM');


INSERT INTO EstadoMovilizacion (Id, Descripcion)
VALUES 
(1, 'Inicio'),
(2, 'En Proceso'),
(3, 'Fin');

INSERT INTO PasoMovilizacion (Id, Descripcion, IdEstadoMovilizacion)
VALUES 
(1, 'Solicitud', 1),
(2, 'Tramitación', 2),
(3, 'Ofrecimiento', 2),
(4, 'Cancelación', 3),
(5, 'Aportación', 2),
(6, 'Despliegue', 2),
(7, 'Fin de intervención', 2),
(8, 'Llegada a Base', 3);

INSERT INTO FlujoPasoMovilizacion (Id, IdPasoActual, IdPasoSiguiente, Orden)
VALUES 
(1, NULL, 1, 1), -- Inicio "Solicitud"
(2, 1, 2, 1), -- Desde "Solicitud" a "Tramitación"
(3, 1, 3, 2), -- Desde "Solicitud" a "Ofrecimiento"
(4, 1, 4, 3), -- Desde "Solicitud" a "Cancelación"
(5, 2, 2, 1), -- Desde "Tramitación" a "Tramitación"
(6, 2, 3, 2), -- Desde "Tramitación" a "Ofrecimiento"
(7, 2, 5, 3), -- Desde "Tramitación" a "Aprobación"
(8, 2, 4, 4), -- Desde "Tramitación" a "Cancelación"
(9, 3, 2, 1), -- Desde "Ofrecimiento" a "Tramitación"
(10, 3, 3, 2), -- Desde "Ofrecimiento" a "Ofrecimiento"
(11, 3, 5, 3), -- Desde "Ofrecimiento" a "Aprobación"
(12, 3, 4, 4), -- Desde "Ofrecimiento" a "Cancelación"
(13, 5, 6, 4), -- Desde "Aprobación" a "Despliegue"
(14, 5, 7, 4), -- Desde "Aprobación" a "Fin de intervención"
(15, 6, 7, 4), -- Desde "Despliegue" a "Fin de intervención"
(16, 7, 8, 4); -- Desde "Fin de intervención" a "Repliegue"

SET IDENTITY_INSERT TipoAdministracion ON;

INSERT INTO TipoAdministracion (Id, Nombre, Codigo,Borrado,Editable) VALUES
	 (1,N'Estatal',N'AE',0,1),
	 (2,N'Autonómica',N'CA',0,1);

SET IDENTITY_INSERT TipoAdministracion OFF;

SET IDENTITY_INSERT Administracion ON;

INSERT INTO Administracion (Id, Codigo, Nombre, IdTipoAdministracion,Borrado,Editable) VALUES
	 (1,N'E00003301',N'AGE',1,0,1),
	 (2,N'E05068001',N'AGE',1,0,1),
	 (3,N'A01002820',N'Junta de Andalucía',2,0,1),
	 (4,N'A13002908',N'Comunidad de Madrid',2,0,1),
	 (5,N'A10002983',N'Generalitat Valenciana',2,0,1),
	 (6,N'A04003003',N'Gobierno de las Illes Balears',2,0,1),
	 (7,N'A07002862',N'Junta de Castilla y León',2,0,1),
	 (8,N'A02002834',N'Gobierno de Aragón',2,0,1),
	 (9,N'A08002880',N'Comunidad Autónoma de Castilla-La Mancha',2,0,1),
	 (10,N'A11002926',N'Junta de Extremadura',2,0,1),
	 (11,N'A12002994',N'Comunidad Autónoma de Galicia',2,0,1),
	 (12,N'A14002961',N'Región de Murcia',2,0,1),
	 (13,N'A05003638',N'Comunidad Autónoma de Canarias',2,0,1);

SET IDENTITY_INSERT Administracion OFF;

SET IDENTITY_INSERT Organismo ON;

INSERT INTO Organismo (Id, Codigo, Nombre, IdAdministracion,Borrado,Editable) VALUES
	 (1,N'E00003301',N'Ministerio de Defensa',1,0,1),
	 (2,N'E05068001',N'Ministerio para la Transición Ecológica y el Reto Demográfico',2,0,1),
	 (3,N'A01002820',N'Junta de Andalucía',3,0,1),
	 (4,N'A13002908',N'Comunidad de Madrid',4,0,1),
	 (5,N'A10002983',N'Generalitat Valenciana',5,0,1),
	 (6,N'A04003003',N'Gobierno de las Illes Balears',6,0,1),
	 (7,N'A07002862',N'Junta de Castilla y León',7,0,1),
	 (8,N'A02002834',N'Gobierno de Aragón',8,0,1),
	 (9,N'A08002880',N'Comunidad Autónoma de Castilla-La Mancha',9,0,1),
	 (10,N'A11002926',N'Junta de Extremadura',10,0,1),
	 (11,N'A12002994',N'Comunidad Autónoma de Galicia',11,0,1),
	 (12,N'A14002961',N'Región de Murcia',12,0,1),
	 (13,N'A05003638',N'Comunidad Autónoma de Canarias',13,0,1);

SET IDENTITY_INSERT Organismo OFF;


SET IDENTITY_INSERT Entidad ON;

INSERT INTO Entidad (Id, Codigo, Nombre, IdOrganismo,Borrado,Editable) VALUES
	 (1,N'E05077401',N'Dirección General de Biodiversidad, Bosques y Desertificación',2,0,1),
	 (2,N'A01025641',N'Consejería de Agricultura, Pesca, Agua y Desarrollo Rural',3,0,1),
	 (3,N'TEST0001',N'ADCIF',2,0,1),
	 (4,N'TEST0002',N'Bomberos Madrid',4,0,1),
	 (5,N'A10023472',N'Servicio de Extinción de Incendios Forestales (AVSRE)',5,0,1),
	 (6,N'A04013548',N'Instituto Balear de la Naturaleza (IBANAT)',6,0,1),
	 (7,N'A07023555',N'Servicio de Incendios Forestales',7,0,1),
	 (8,N'TEST0003',N'INFOAR',8,0,1),
	 (9,N'TEST0004',N'INFOCA',3,0,1),
	 (10,N'A08047750',N'Centro Operativo Regional de Lucha contra Incendios Forestales',9,0,1),
	 (11,N'A11030455',N'Servicio de Prevención y Extinción de Incendios Forestales',10,0,1),
	 (12,N'TEST0005',N'INFOGA',11,0,1),
	 (13,N'A14029799',N'Consorcio de Extinción de Incendios y Salvamento de la Región de Murcia',12,0,1),
	 (14,N'TEST0006',N'Servico Andaluz de Salud',3,0,1),
	 (15,N'TEST0007',N'Test - Canarias',13,0,1),
	 (16,N'E04720503',N'Unidad militar de emergencias',1,0,1);

SET IDENTITY_INSERT Entidad OFF;

SET IDENTITY_INSERT TipoCapacidad ON;

INSERT INTO TipoCapacidad (Id, Nombre,Borrado,Editable) VALUES
	 (1,N'BRIF/A',0,1),
	 (2,N'BRIF/i  BRIF/B',0,1),
	 (3,N'AA (FOCA)',0,1),
	 (4,N'HK',0,1),
	 (5,N'ACO',0,1),
	 (6,N'Aa (ALFA)',0,1),
	 (7,N'ACT (TANGO)',0,1),
	 (8,N'UMAP',0,1),
	 (9,N'BLP',0,1),
	 (10,N'MIKE',0,1),
	 (11,N'HOTEL',0,1),
	 (12,N'LIMA',0,1),
	 (13,N'Técnicos',0,1),
	 (14,N'Cuadrillas helitransportadas',0,1),
	 (15,N'Cuadrillas terrestres',0,1);

SET IDENTITY_INSERT TipoCapacidad OFF;

SET IDENTITY_INSERT Capacidad ON;
INSERT INTO Capacidad (Id,Nombre,Descripcion,Gestionado,IdTipoCapacidad,IdEntidad,Borrado,Editable)
VALUES
	 (1,N'ACO  (Matacan)',N'Aeronave de Comunicaciones y Observación',0,5,1,0,1),
	 (2,N'ACO Leon',N'Aeronave de Comunicaciones y Observación',0,5,1,0,1),
	 (3,N'ACO Mutxamiel',N'Aeronave de Comunicaciones y Observación',0,5,1,0,1),
	 (4,N'ACO Talavera la Real',N'Aeronave de Comunicaciones y Observación',0,5,1,0,1),
	 (5,N'ACO Zaragoza',N'Aeronave de Comunicaciones y Observación',0,5,1,0,1),
	 (6,N'Aeronave de Coordinación (Andalucía)',N'Aeronave de Coordinación',0,NULL,2,0,1),
	 (7,N'Aeronave de Coordinación ACO (I.Baleares)',N'Aeronave de Coordinación',0,NULL,6,0,1),
	 (8,N'Agente Forestal/Medioambiental (Aragón)',N'Agente forestal/medioambiental',0,NULL,8,0,1),
	 (9,N'Agente Forestal/Medioambiental (Castilla y León)',N'Agente forestal/medioambiental',0,NULL,7,0,1),
	 (10,N'Agente Forestal/Medioambiental (Extremadura)',N'Agente forestal/medioambiental',0,NULL,11,0,1),
	 (11,N'Agente Forestal/Medioambiental (Extremadura)',N'Agente forestal/medioambiental',0,NULL,11,0,1),
	 (12,N'Autobombas  (Extremadura)',N'Camión autobomba para extinción de incendios',0,NULL,11,0,1),
	 (13,N'Autobombas  (I.Baleares)',N'Camión autobomba para extinción de incendios',0,NULL,6,0,1),
	 (14,N'Autobombas (Andalucia)',N'Camión autobomba para extinción de incendios',0,NULL,9,0,1),
	 (15,N'Autobombas (Castilla-La Mancha)',N'Camión autobomba para extinción de incendios',0,NULL,10,0,1),
	 (16,N'Avion anfibio Comunidad Valenciana (ALFA/Aa) ',N'Avión anfibio tipo 2 (ALFA)',0,NULL,5,0,1),
	 (17,N'Avión anfibio La Gomera (ALFA/Aa)',N'Avión anfibio tipo 2 (ALFA)',0,NULL,3,0,1),
	 (18,N'Avión anfibio Labacolla (FOCA/AA)',N'Avión anfibio tipo 1 (FOCA) Canadair',0,3,1,0,1),
	 (19,N'Avión anfibio Los Llanos-Albacete (FOCA/AA)',N'Avión anfibio tipo 1 (FOCA) Canadair',0,3,1,0,1),
	 (20,N'Avión anfibio Málaga (FOCA/AA)',N'Avión anfibio tipo 1 (FOCA) Canadair',0,3,1,0,1),
	 (21,N'Avión anfibio Manises (ALFA/Aa)',N'Avión anfibio tipo 2 (ALFA)',0,6,1,0,1),
	 (22,N'Avión anfibio Matacan (FOCA/AA)',N'Avión anfibio tipo 1 (FOCA) Canadair',0,3,1,0,1),
	 (23,N'Avión anfibio Mirabel (ALFA/Aa)',N'Avión anfibio tipo 2 (ALFA)',0,6,1,0,1),
	 (24,N'Avión anfibio Pollensa (FOCA/AA)',N'Avión anfibio tipo 1 (FOCA) Canadair',0,3,1,0,1),
	 (25,N'Avión anfibio Requena (ALFA/Aa)',N'Avión anfibio tipo 2 (ALFA)',0,6,1,0,1),
	 (26,N'Avión anfibio Reus (ALFA/Aa)',N'Avión anfibio tipo 2 (ALFA)',0,6,1,0,1),
	 (27,N'Avión anfibio Rosinos (ALFA/Aa)',N'Avión anfibio tipo 2 (ALFA)',0,6,1,0,1),
	 (28,N'Avión anfibio Talavera la Real (FOCA/AA)',N'Avión anfibio tipo 1 (FOCA) Canadair',0,3,1,0,1),
	 (29,N'Avión anfibio Torrejón (FOCA/AA)',N'Avión anfibio tipo 1 (FOCA) Canadair',0,3,1,0,1),
	 (30,N'Avión anfibio Viver (ALFA/Aa)',N'Avión anfibio tipo 2 (ALFA)',0,6,1,0,1),
	 (31,N'Avión anfibio Zaragoza (FOCA/AA)',N'Avión anfibio tipo 1 (FOCA) Canadair',0,3,1,0,1),
	 (32,N'Avión de extinción Agoncillo (TANGO/ACT)',N'Avión de extinción de carga en tierra MITECO',0,7,1,0,1),
	 (33,N'Avión de extinción Ampuriabrava (TANGO/ACT)',N'Avión de extinción de carga en tierra MITECO',0,7,1,0,1),
	 (34,N'Avión de extinción La Centenera (TANGO/ACT)',N'Avión de extinción de carga en tierra MITECO',0,7,1,0,1),
	 (35,N'Avión extinción La Gomera (TANGO/ACT)',N'Avión de extinción de carga en tierra MITECO',0,7,1,0,1),
	 (36,N'Avión extinción Niebla (TANGO/ACT)',N'Avión de extinción de carga en tierra MITECO',0,7,1,0,1),
	 (37,N'Avión extinción Noain (TANGO/ACT)',N'Avión de extinción de carga en tierra MITECO',0,7,1,0,1),
	 (38,N'Avión extinción Son Bonet (TANGO/ACT)',N'Avión de extinción de carga en tierra MITECO',0,7,1,0,1),
	 (39,N'Avión extinción Xinzo (TANGO/ACT)',N'Avión de extinción de carga en tierra MITECO',0,7,1,0,1),
	 (40,N'BLP Tineo',N'Brigada de Labores Preventivas',0,9,1,0,1),
	 (41,N'BRIF/A  Cuenca/Prado de los Esquiladores',N'Brigada de Refuerzo Contra Incendios Forestales',0,1,1,0,1),
	 (42,N'BRIF/A  La Palma/Puntagorda',N'Brigada de Refuerzo Contra Incendios Forestales',0,1,1,0,1),
	 (43,N'BRIF/A  Laza',N'Brigada de Refuerzo Contra Incendios Forestales',0,1,1,0,1),
	 (44,N'BRIF/A  Lubia',N'Brigada de Refuerzo Contra Incendios Forestales',0,1,1,0,1),
	 (45,N'BRIF/A  Pinofranqueado',N'Brigada de Refuerzo Contra Incendios Forestales',0,1,1,0,1),
	 (46,N'BRIF/A  Tabuyo',N'Brigada de Refuerzo Contra Incendios Forestales',0,1,1,0,1),
	 (47,N'BRIF/A  Tineo',N'Brigada de Refuerzo Contra Incendios Forestales',0,1,1,0,1),
	 (48,N'BRIF/A Daroca',N'Brigada de Refuerzo Contra Incendios Forestales',0,1,1,0,1),
	 (49,N'BRIF/A La Iglesuela',N'Brigada de Refuerzo Contra Incendios Forestales',0,1,1,0,1),
	 (50,N'BRIF/B  Puerto El Pico',N'Brigada de Refuerzo Contra Incendios Forestales',0,2,1,0,1),
	 (51,N'BRIF-i  (Laza)',N'Brigada de Refuerzo Contra Incendios Forestales',0,2,1,0,1),
	 (52,N'BRIF-i  (Pinofranqueado)',N'Brigada de Refuerzo Contra Incendios Forestales',0,2,1,0,1),
	 (53,N'BRIF-i  (Ruente)',N'Brigada de Refuerzo Contra Incendios Forestales',0,2,1,0,1),
	 (54,N'BRIF-i  (Tabuyo)',N'Brigada de Refuerzo Contra Incendios Forestales',0,2,1,0,1),
	 (55,N'BRIF-i (Tineo)',N'Brigada de Refuerzo Contra Incendios Forestales',0,2,1,0,1),
	 (56,N'Camión Nodriza (Castilla-La Mancha)',N'Camión nodriza para suministro de agua',0,NULL,10,0,1),
	 (57,N'Camiones de transporte Extremadura',N'Camiones TT para transporte Extremadura',0,NULL,11,0,1),
	 (58,N'Helicóptero de extinción Almoraima (KILO/HK)',N'Helicóptero de transporte y extinción pesado',0,4,1,0,1),
	 (59,N'Helicóptero de extinción Andalucía (KILO/HK)',N'Helicóptero de extinción de gran capacidad',0,4,2,0,1),
	 (60,N'Helicóptero de extinción Caravaca (KILO/HK)',N'Helicóptero de transporte y extinción pesado',0,4,1,0,1),
	 (61,N'Helicóptero de extinción Castilla-La Mancha (KILO/HK)',N'Helicóptero de extinción de gran capacidad',0,4,10,0,1),
	 (62,N'Helicóptero de extinción Huelma (KILO/HK)',N'Helicóptero de transporte y extinción pesado',0,4,1,0,1),
	 (63,N'Helicóptero de extinción Ibias (KILO/HK)',N'Helicóptero de transporte y extinción pesado',0,4,1,0,1),
	 (64,N'Helicóptero de extinción Plasencia de Cáceres (KILO/HK)',N'Helicóptero de transporte y extinción pesado',0,4,1,0,1),
	 (65,N'Helicóptero de extinción Plasencia del Monte (KILO/HK)',N'Helicóptero de transporte y extinción pesado',0,4,1,0,1),
	 (66,N'Helicóptero de extinción Tenerife Sur (KILO/HK)',N'Helicóptero de transporte y extinción pesado',0,4,1,0,1),
	 (67,N'Helicóptero de extinción Villlares/Las Minas (KILO/HK)',N'Helicóptero de transporte y extinción pesado',0,4,1,0,1),
	 (68,N'Helicóptero de transporte y extinción MIKE- Andalucía',N'	Helicóptero de transporte y extinción medio',0,10,2,0,1),
	 (69,N'Helicóptero de transporte y extinción MIKE- Aragón',N'	Helicóptero de transporte y extinción medio',0,10,8,0,1),
	 (70,N'Helicóptero de transporte y extinción MIKE- Canarias',N'	Helicóptero de transporte y extinción medio',0,10,15,0,1),
	 (71,N'Helicóptero de transporte y extinción MIKE- Castilla y León',N'Helicóptero de transporte y extinción medio',0,10,7,0,1),
	 (72,N'Helicóptero de transporte y extinción MIKE- Castilla-La Mancha',N'	Helicóptero de transporte y extinción medio',0,10,10,0,1),
	 (73,N'Helicóptero de transporte y extinción MIKE- Comunidad Valenciana',N'Helicóptero de transporte y extinción medio',0,10,5,0,1),
	 (74,N'Helicóptero de transporte y extinción MIKE- Extremadura',N'Helicóptero de transporte y extinción medio',0,10,11,0,1),
	 (75,N'Helicóptero de transporte y extinción MIKE- Galicia',N'Helicóptero de transporte y extinción medio',0,10,12,0,1),
	 (76,N'Helicóptero de transporte y extinción MIKE- Galicia',N'Helicóptero de transporte y extinción medio',0,10,12,0,1),
	 (77,N'Helicoptero de transporte y extincion MIKE- I.Baleares',N'Helicóptero de transporte y extinción medio',0,10,6,0,1),
	 (78,N'Helicóptero de transporte y extinción MIKE- Madrid',N'Helicóptero de transporte y extinción medio',0,10,4,0,1),
	 (79,N'Helicóptero de transporte y extinción MIKE- Murcia',N'Helicóptero de transporte y extinción medio',0,10,13,0,1),
	 (80,N'Helicóptero medio Caravaca (MIKE/HT)',N'Helicóptero de transporte y extinción medio',0,10,1,0,1),
	 (81,N'Helicóptero medio Huelma (MIKE/HT)',N'Helicóptero de transporte y extinción medio',0,10,1,0,1),
	 (82,N'Helicóptero medio Ibias (MIKE/HT)',N'Helicóptero de transporte y extinción medio',0,10,1,0,1),
	 (83,N'Helicóptero medio La Almoraima (MIKE/HT)',N'Helicóptero de transporte y extinción medio',0,10,1,0,1),
	 (84,N'Helicóptero medio Plasencia de Cáceres (MIKE/HT)',N'Helicóptero de transporte y extinción medio',0,10,1,0,1),
	 (85,N'Helicóptero medio Plasencia del Monte (MIKE/HT)',N'Helicóptero de transporte y extinción medio',0,10,1,0,1),
	 (86,N'Helicóptero medio Tenerife Sur (MIKE/HT)',N'Helicóptero de transporte y extinción medio',0,10,1,0,1),
	 (87,N'Personal sanitario Andalucía',N'Personal sanitario Andalucía',0,NULL,14,0,1),
	 (88,N'Técnicos de incendios - Andalucía',N'Personal Técnico',0,13,2,0,1),
	 (89,N'Técnicos de incendios - Aragón',N'Personal Técnico',0,13,8,0,1),
	 (90,N'Técnicos de incendios - Castilla y León',N'Personal Técnico',0,13,7,0,1),
	 (91,N'UME',N'Unidad Militar de Emergencias',1,NULL,16,0,1),
	 (92,N'Otros',N'Medios sin catalogar',0,NULL,NULL,0,0);

SET IDENTITY_INSERT Capacidad OFF;