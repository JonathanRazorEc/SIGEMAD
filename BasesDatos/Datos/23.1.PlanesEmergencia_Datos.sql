-- =============================================
-- TIPO DE RIESGO
-- =============================================

INSERT INTO TipoRiesgo (Id,Descripcion, IdTipoSuceso, Codigo) VALUES
	 (1, N'General', NULL, '00'),
	 (2, N'Accidentes con sustancias biológicas', 8, '01'),
	 (3, N'Accidentes con sustancias químicas', 10, '02'),
	 (4, N'Accidentes con sustancias radiactivas', 16, '03'),
	 (5, N'Accidentes de aviación civil', 7, '04'),
	 (6, N'Accidentes en centrales nucleares', 15, '05'),
	 (7, N'Accidentes en el transporte de mercancías peligrosas', 12, '06'),
	 (8, N'Accidentes en el transporte de viajeros por carretera y ferrocarril', 19, '07'),
	 (9, N'Accidentes en túneles', 20, '08'),
	 (10, N'Aludes', 21, '09'),
	 (11, N'Bélico', 22, '10'),
	 (12, N'Contaminación marina', 23, '11'),
	 (13, N'Erupción volcánica', 18, '12'),
	 (14, N'Fenómenos Meteorológicos Adversos', 14, '13'),
	 (15, N'Incendios forestales', 6, '14'),
	 (16, N'Inundaciones', 9, '15'),
	 (17, N'Maremotos', 17, '16'),
	 (18, N'Nevadas', 24, '17'),
	 (19, N'Otros', 13, '18'),
	 (20, N'Terremotos', 11, '19'),
	 (21, N'Viento', 25, '20');

-- =============================================
-- TIPO DE PLAN
-- =============================================


 INSERT INTO TipoPlan (Id, Descripcion, Codigo) VALUES
(1, N'Estatal', N'00'),
(2, N'Territorial','01'),
(3, N'Especial', N'02'),
(4, N'Normativa Básica','03'),
(5, N'Autoprotección', N'04'),
(6, N'Otros', N'05');



INSERT INTO TipoPlanMapeo (IdAntiguo, IdNuevo)
VALUES 
	(2, 1), --Estatal
	(5, 2),  --Territorial
	(6, 3), --Especial de CA
	(7, 5), --Autoprotección
	(8, 6); --Otros


-- =============================================
-- AMBITO PLAN
-- =============================================

INSERT INTO AmbitoPlan (Id,Descripcion) VALUES
    (1, 'Estatal'),
    (2, 'Autonómico');

-- =============================================
-- TIPO NOTIFICACION
-- =============================================

INSERT INTO TipoNotificacion (Id,Descripcion) VALUES
    (1, 'Activación plan estatal'),
    (2, 'Activación PLEGEM'),
	(3, 'Declaración situación'),
	(4, 'Desactivación plan estatal'),
	(5, 'Otros')


-- =============================================
-- PLANES DE EMERGENCIA
-- =============================================

INSERT [PlanEmergencia] ([Id], [Codigo], [Descripcion], [IdCCAA], [IdProvincia], [IdMunicipio], [IdTipoPlan], [IdTipoRiesgo], [IdAmbitoPlan]) VALUES 
(1, N'00000000000', N'Plan Estatal General de Emergencias de Protección Civil', NULL, NULL, NULL, 1, 1, 1),
(2, N'00000000015', N'Plan Estatal de Protección Civil ante el Riesgo de Inundaciones', NULL, NULL, NULL, 1, 16, 1),
(3, N'00000000014', N'Plan Estatal de Protección Civil para Emergencias por Incendios Forestales', NULL, NULL, NULL, 1, 15, 1),
(4, N'00000000019', N'Plan Estatal de Protección Civil ante el Riesgo Sísmico', NULL, NULL, NULL, 1, 20, 1),
(5, N'00000000002', N'Plan Estatal de Protección Civil ante el Riesgo Químico', NULL, NULL, NULL, 1, 3, 1),
(6, N'00000000012', N'Plan Estatal de Protección Civil ante el Riesgo Volcánico', NULL, NULL, NULL, 1, 13, 1),
(7, N'00000000003', N'Plan Estatal de Protección Civil ante el Riesgo Radiológico', NULL, NULL, NULL, 1, 4, 1),
(8, N'00000000016', N'Plan Estatal de Protección Civil ante el Riesgo de Maremotos', NULL, NULL, NULL, 1, 17, 1),
(9, N'00000000005', N'Plan de Emergencia Nuclear del Nivel Central de Respuesta y Apoyo', NULL, NULL, NULL, 1, 6, 1),
(10, N'07090000005', N'Plan de Emergencia Nuclear Exterior a la Central Nuclear de Santa María de Garoña en Burgos', 8, NULL, NULL, 1, 6, 1),
(11, N'11100000005', N'Plan de Emergencia Nuclear Exterior a la Central Nuclear de Almaraz en Cáceres', 14, 10, NULL, 1, 6, 1),
(12, N'08190000005', N'Plan de Emergencia Nuclear Exterior a las Centrales Nucleares de José Cabrera y Trillo en Guadalajara', 11, 19, NULL, 1, 6, 1),
(13, N'09430000005', N'Plan de Emergencia Nuclear Exterior a las Centrales Nucleares de Ascó y Vandellós en Tarragona', 7, 43, NULL, 1, 6, 1),
(14, N'10460000005', N'Plan de Emergencia Nuclear Exterior a la Central Nuclear de Cofrentes en Valencia', 12, 46, NULL, 1, 6, 1),
(15, N'00000000518', N'Plan Especial Operación Paso del Estrecho 2024', NULL, NULL, NULL, 6, 19, 1),
(16, N'02220000508', N'Plan de Socorro Binacional del Túnel de Somport', 6, 22, NULL, 6, 9, 1),
(17, N'09170000508', N'Plan de Socorro Binacional del Túnel del Perthus', 7, 17, NULL, 6, 9, 1),
(18, N'00000000513', N'Protocolo de Coordinación de la AGE para la Red de Carreteras del Estado (2023-2024)', NULL, NULL, NULL, 6, 14, 1),
(19, N'01000000100', N'Plan Territorial de Emergencias de Protección Civil de Andalucía', 15, NULL, NULL, 2, 1, 2),
(20, N'01000000214', N'Plan de Emergencia por Riesgo de Incendios Forestales de Andalucía', 15, NULL, NULL, 3, 15, 2),
(21, N'01000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones en Andalucía', 15, NULL, NULL, 3, 16, 2),
(22, N'01000000206', N'Plan de Emergencia ante Accidentes en el Transporte de Mercancías Peligrosas por Carretera y Ferrocarril en Andalucía', 15, NULL, NULL, 3, 7, 2),
(23, N'01000000219', N'Plan de Emergencia ante el Riesgo Sísmico en Andalucía', 15, NULL, NULL, 3, 20, 2),
(24, N'01000000511', N'Plan de Emergencia ante el Riesgo de Contaminación del Litoral en Andalucía', 15, NULL, NULL, 6, 12, 2),
(25, N'01000000516', N'Plan de Emergencia ante el Riesgo de Maremotos en Andalucía', 15, NULL, NULL, 6, 17, 2),
(26, N'02000000100', N'Plan Territorial de Protección Civil de Aragón', 6, NULL, NULL, 2, 1, 2),
(27, N'02000000214', N'Plan de Emergencia por Incendios Forestales de Aragón', 6, NULL, NULL, 3, 15, 2),
(28, N'02000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones en Aragón', 6, NULL, NULL, 3, 16, 2),
(29, N'02000000206', N'Plan de Emergencia Producidas en el Transporte de Mercancías Peligrosas por Carretera y Ferrocarril en Andalucía', 6, NULL, NULL, 3, 7, 2),
(30, N'02000000203', N'Plan de Emergencia ante el Riesgo Radiológico en Aragón', 6, NULL, NULL, 3, 4, 2),
(31, N'02000000219', N'Plan de Emergencia ante el Riesgo Sísmico de Aragón', 6, NULL, NULL, 3, 20, 2),
(32, N'02000000202', N'Plan de Emergencia ante Accidentes en Gasoductos y Oleoductos de Aragón', 6, NULL, NULL, 3, 3, 2),
(33, N'03000000100', N'Plan Territorial de Protección Civil del Principado de Asturias', 2, NULL, NULL, 2, 1, 2),
(34, N'03000000214', N'Plan de Emergencia por Incendios Forestales del Principado de Asturias', 2, NULL, NULL, 3, 15, 2),
(35, N'03000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones del Principado de Asturias', 2, NULL, NULL, 3, 16, 2),
(36, N'03000000206', N'Plan de Emergencia ante el Transporte de Mercancías Peligrosas por Carretera y Ferrocarril del Principado de Asturias', 2, NULL, NULL, 3, 7, 2),
(37, N'03000000511', N'Plan de Emergencia frente a Contingencias por Contaminación Marina Accidental en Asturias', 2, NULL, NULL, 6, 12, 2),
(38, N'03000000517', N'Plan de Emergencia ante Nevadas en Asturias', 2, NULL, NULL, 6, 18, 2),
(39, N'04000000100', N'Plan Territorial de Protección Civil de Illes Balears', 13, NULL, NULL, 2, 1, 2);


INSERT [PlanEmergencia] ([Id], [Codigo], [Descripcion], [IdCCAA], [IdProvincia], [IdMunicipio], [IdTipoPlan], [IdTipoRiesgo], [IdAmbitoPlan]) VALUES 
(40, N'04000000214', N'Plan de Emergencia ante el Riesgo de Incendios Forestales en Illes Balears', 13, NULL, NULL, 3, 15, 2),
(41, N'04000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones en Illes Balears', 13, NULL, NULL, 3, 16, 2),
(42, N'04000000206', N'Plan de Emergencia ante el Riesgo de Transporte de Mercancías Peligrosas en Illes Balears', 13, NULL, NULL, 3, 7, 2),
(43, N'04000000203', N'Plan de Emergencia ante el Riesgo de Emergencias Radiológicas en Illes Balears', 13, NULL, NULL, 3, 4, 2),
(44, N'04000000219', N'Plan de Emergencia ante Riesgo Sísmico en Illes Balears', 13, NULL, NULL, 3, 20, 2),
(45, N'04000000213', N'Plan de Emergencia frente a Riesgo de Fenómenos Meteorológicos Adversos en Illes Balears', 13, NULL, NULL, 3, 14, 2),
(46, N'04000000511', N'Plan de Emergencia ante Contaminación de Aguas Marinas de Las Illes Balears', 13, NULL, NULL, 6, 12, 2),
(47, N'04000000218', N'Plan de Emergencia de Actuación en Situaciones de Alerta y Eventual Sequía de Illes Balears', 13, NULL, NULL, 3, 19, 2),
(48, N'05000000100', N'Plan Territorial de Protección Civil de Canarias', 17, NULL, NULL, 2, 1, 2),
(49, N'05000000214', N'Plan de Emergencia por Incendios Forestales en Canarias', 17, NULL, NULL, 3, 15, 2),
(50, N'05000000215', N'Plan de Emergencia por Riesgo de Inundaciones en Canarias', 17, NULL, NULL, 3, 16, 2),
(51, N'05000000206', N'Plan de Emergencia por Accidentes en el Transporte de Mercancías Peligrosas por Carretera de Canarias', 17, NULL, NULL, 3, 7, 2),
(52, N'05000000203', N'Plan de Emergencia por Riesgo Radiológico de Canarias', 17, NULL, NULL, 3, 4, 2),
(53, N'05000000219', N'Plan de Emergencia por Riesgo Sísmico en Canarias', 17, NULL, NULL, 3, 20, 2),
(54, N'05000000212', N'Plan de Emergencia por Riesgo Volcánico de Canarias', 17, NULL, NULL, 3, 13, 2),
(55, N'05000000202/1', N'Plan de Emergencia ante Riesgo Químico en Canarias', 17, NULL, NULL, 3, 3, 2),
(56, N'05000000513', N'Plan de Emergencia de Canarias ante Riesgos de Fenómenos Meteorológicos Adversos ', 17, NULL, NULL, 6, 14, 2),
(57, N'05000000202/2', N'Plan de emergencia por Accidentes de Sustancias Explosivas de Canarias', 17, NULL, NULL, 3, 3, 2),
(58, N'06000000100', N'Plan Territorial de Protección Civil de Cantabria', 3, NULL, NULL, 2, 1, 2),
(59, N'06000000214', N'Plan de Emergencia Sobre Riesgo de Incendios Forestales de Cantabria', 3, NULL, NULL, 3, 15, 2),
(60, N'06000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones de Cantabria', 3, NULL, NULL, 3, 16, 2),
(61, N'06000000206', N'Plan de Emergencia sobre Transporte de Mercancías Peligrosas por Carretera y Ferrocarril de Cantabria', 3, NULL, NULL, 3, 7, 2),
(62, N'07000000100', N'Plan Territorial de Protección Civil de Castilla y León', 8, NULL, NULL, 2, 1, 2),
(63, N'07000000214', N'Plan de Emergencia por Riesgo de Incendios Forestales en Castilla y León', 8, NULL, NULL, 3, 15, 2),
(64, N'07000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones en Castilla y León', 8, NULL, NULL, 3, 16, 2),
(65, N'07000000206', N'Plan de Emergencia ante el Riesgo de Transporte de Mercancías Peligrosas de Castilla y León', 8, NULL, NULL, 3, 7, 2),
(66, N'08000000100', N'Plan Territorial de Protección Civil de Castilla-La Mancha', 11, NULL, NULL, 2, 1, 2),
(67, N'08000000214', N'Plan de Emergencia por Incendios Forestales de Castilla-La Mancha', 11, NULL, NULL, 3, 15, 2),
(68, N'08000000215', N'Plan de Emergencia ante Inundaciones de Castilla-La Mancha', 11, NULL, NULL, 3, 16, 2),
(69, N'08000000206', N'Plan de Emergencia ante el Riesgo de Accidente en el Transporte de Mercancías Peligrosas por Carretera y Ferrocarril en Castilla-La Mancha', 11, NULL, NULL, 3, 7, 2),
(70, N'08000000203', N'Plan de Emergencia ante el Riesgo Radiológico en Castilla-la Mancha', 11, NULL, NULL, 3, 4, 2),
(71, N'08000000219', N'Plan de Emergencia por Riesgo Sísmico en Castilla-La Mancha', 11, NULL, NULL, 3, 20, 2),
(72, N'08000000513', N'Plan de Emergencia ante el Riesgo por Fenómenos Meteorológicos Adversos en Castilla La Mancha', 11, NULL, NULL, 6, 14, 2),
(73, N'08000000507', N'Plan de Respuesta ante Accidentes de Tráfico con Múltiples Víctimas en Castilla-La Mancha', 11, NULL, NULL, 6, 8, 2),
(74, N'09000000100', N'Plan Territorial de Protección Civil de Catalunya', 7, NULL, NULL, 2, 1, 2),
(75, N'09000000214', N'Plan de Emergencia ante Incendios Forestales de Catalunya', 7, NULL, NULL, 3, 15, 2),
(76, N'09000000215', N'Plan de Emergencia por Inundaciones en Catalunya', 7, NULL, NULL, 3, 16, 2),
(77, N'09000000206', N'Plan de emergencia por accidentes en el transporte de mercancías peligrosas por carretera y ferrocarril en Catalunya', 7, NULL, NULL, 3, 7, 2),
(78, N'09000000203', N'Plan de Emergencia ante Riesgos Radiológicos en Catalunya', 7, NULL, NULL, 3, 4, 2),
(79, N'09000000219', N'Plan de Emergencia por Riesgo Sísmico en Catalunya', 7, NULL, NULL, 3, 20, 2),
(80, N'09000000202', N'Plan de Emergencia Exterior del Sector Químico de Catalunya', 7, NULL, NULL, 3, 3, 2),
(81, N'09430000202', N'Plan de Emergencia Exterior del Sector Químico de Tarragona', 7, 43, NULL, 3, 3, 2),
(82, N'09000000511', N'Plan de Emergencia por Contaminación de las Aguas Marinas de Catalunya', 7, NULL, NULL, 6, 12, 2),
(83, N'09000000204', N'Plan de Emergencia Aeronáuticas de Catalunya', 7, NULL, NULL, 3, 5, 2),
(84, N'09000000509', N'Plan de Emergencia por Aludes en Catalunya', 7, NULL, NULL, 6, 10, 2),
(85, N'09000000217', N'Plan de Emergencia ante Nevadas en Catalunya', 7, NULL, NULL, 3, 18, 2),
(86, N'09000000220', N'Plan de Emergencia por Riesgo de Viento de Catalunya', 7, NULL, NULL, 3, 21, 2),
(87, N'09000000518', N'Plan de Emergencia por Pandemias en Catalunya', 7, NULL, NULL, 6, 19, 2),
(88, N'10000000100', N'Plan Territorial de Protección Civil de la Comunitat Valenciana', 12, NULL, NULL, 2, 1, 2),
(89, N'10000000214', N'Plan de Emergencia frente al riesgo de Incendios Forestales de la Comunitat Valenciana', 12, NULL, NULL, 3, 15, 2),
(90, N'10000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones en La Comunitat Valenciana', 12, NULL, NULL, 3, 16, 2),
(91, N'10000000206', N'Plan de Emergencia ante el Riesgo de Accidentes en el Transporte de Mercancías Peligrosas por Carretera y Ferrocarril en Comunitat Valenciana', 12, NULL, NULL, 3, 7, 2),
(92, N'10000000203', N'Plan de Emergencia ante el Riesgo Radiológico de la Comunitat Valenciana', 12, NULL, NULL, 3, 4, 2),
(93, N'10000000219', N'Plan de Emergencia Frente al Riesgo Sísmico en La Comunitat Valenciana', 12, NULL, NULL, 3, 20, 2),
(94, N'10000000511', N'Plan de Emergencia ante la Contaminación Marina Accidental en la Comunitat Valenciana', 12, NULL, NULL, 6, 12, 2),
(95, N'10000000517', N'Plan de Emergencia ante el Riesgo de Nevadas en la Comunitat Valenciana', 12, NULL, NULL, 6, 18, 2),
(96, N'11000000100', N'Plan Territorial de Protección Civil de Extremadura', 14, NULL, NULL, 2, 1, 2),
(97, N'11000000214', N'Plan de Emergencia ante Incendios Forestales en Extremadura', 14, NULL, NULL, 3, 15, 2),
(98, N'11000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones de Extremadura', 14, NULL, NULL, 3, 16, 2),
(99, N'11000000206', N'Plan de Emergencia sobre Transporte de Mercancías Peligrosas por Carretera y Ferrocarril de Extremadura', 14, NULL, NULL, 3, 7, 2),
(100, N'11000000203', N'Plan de emergencia ante Riesgos radiológicos de Extremadura', 14, NULL, NULL, 3, 4, 2),
(101, N'11000000219', N'Plan de Emergencia ante el Riesgo Sísmico de Extremadura', 14, NULL, NULL, 3, 20, 2),
(102, N'12000000100', N'Plan Territorial de Protección Civil de Galicia', 1, NULL, NULL, 2, 1, 2),
(103, N'12000000214', N'Plan de Emergencia de Galicia ante el Riesgo de Incendios Forestales', 1, NULL, NULL, 3, 15, 2),
(104, N'12000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones de Galicia', 1, NULL, NULL, 3, 16, 2),
(105, N'12000000206', N'Plan de Emergencia por Accidentes en el Transporte de Mercancías Peligrosas de Galicia', 1, NULL, NULL, 3, 7, 2),
(106, N'12000000219', N'Plan de Emergencia ante el Riesgo Sísmico de Galicia', 1, NULL, NULL, 3, 20, 2),
(107, N'12000000511', N'Plan de Emergencia ante Contingencias por Contaminación Marina Accidental de Galicia', 1, NULL, NULL, 6, 12, 2),
(108, N'13000000100', N'Plan Territorial de Protección Civil de la Comunidad de Madrid', 10, NULL, NULL, 2, 1, 2),
(109, N'13000000214', N'Plan de Emergencia por Incendios Forestales en la Comunidad de Madrid', 10, NULL, NULL, 3, 15, 2),
(110, N'13000000215', N'Plan de Emergencia ante Inundaciones en la Comunidad de Madrid', 10, NULL, NULL, 3, 16, 2),
(111, N'13000000206', N'Plan de Emergencia ante el riesgo de accidentes en el transporte de mercancías peligrosas por carretera y ferrocarril de la Comunidad de Madrid', 10, NULL, NULL, 3, 7, 2),
(112, N'13000000203', N'Plan de Emergencia ante Riesgos Radiológicos en la Comunidad de Madrid', 10, NULL, NULL, 3, 4, 2),
(113, N'13000000213', N'Plan de Emergencia ante Inclemencias Invernales en la Comunidad de Madrid', 10, NULL, NULL, 3, 14, 2),
(114, N'14000000100', N'Plan Territorial de Protección Civil de la Región de Murcia', 16, NULL, NULL, 2, 1, 2),
(115, N'14000000214', N'Plan de Emergencia por Riesgo de Incendios Forestales de Murcia', 16, NULL, NULL, 3, 15, 2),
(116, N'14000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones de Murcia', 16, NULL, NULL, 3, 16, 2),
(117, N'14000000206', N'Plan de Emergencia sobre Transporte de Mercancías Peligrosas por Carretera y Ferrocarril de Murcia', 16, NULL, NULL, 3, 7, 2),
(118, N'14000000219', N'Plan de Emergencia ante el Riesgo Sísmico de Murcia', 16, NULL, NULL, 3, 20, 2),
(119, N'14000000513', N'Plan de Emergencia ante Fenómenos Meteorológicos Adversos de Murcia', 16, NULL, NULL, 6, 14, 2),
(120, N'14000000511', N'Plan de Emergencia ante Contingencias por Contaminación Marina Accidental de Murcia', 16, NULL, NULL, 6, 12, 2),
(121, N'15000000100', N'Plan Territorial de Protección Civil de Navarra', 5, NULL, NULL, 2, 1, 2),
(122, N'15000000214', N'Plan de Emergencia por Riesgo de Incendios Forestales de Navarra.', 5, NULL, NULL, 3, 15, 2),
(123, N'15000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones de Navarra', 5, NULL, NULL, 3, 16, 2),
(124, N'15000000206', N'Plan de Emergencia por Accidentes en el Transporte de Mercancías Peligrosas por Carreteras y Ferrocarriles de Navarra', 5, NULL, NULL, 3, 7, 2),
(125, N'15000000203', N'Plan de Emergencia ante el Riesgo Radiológico de Navarra', 5, NULL, NULL, 3, 4, 2),
(126, N'15000000219', N'Plan de Emergencia ante el Riesgo Sísmico de Navarra', 5, NULL, NULL, 3, 20, 2),
(127, N'15000000213', N'Plan de Emergencia ante Fenómenos Meteorológicos Adversos en Navarra', 5, NULL, NULL, 3, 14, 2),
(128, N'15000000517', N'Plan de Emergencia por Nevadas en Navarra', 5, NULL, NULL, 6, 18, 2),
(129, N'15000000208', N'Plan de Emergencia para Túneles de La Red de Carreteras de Navarra', 5, NULL, NULL, 3, 9, 2),
(130, N'16000000100', N'Plan Territorial de Protección Civil de Euskadi', 4, NULL, NULL, 2, 1, 2),
(131, N'16000000214', N'Plan de Emergencia para Riesgo de Incendios Forestales de País Vasco', 4, NULL, NULL, 3, 15, 2),
(132, N'16000000215', N'Plan de Emergencia ante el Riesgo de Inundaciones del País Vasco', 4, NULL, NULL, 3, 16, 2),
(133, N'16000000206', N'Plan de Emergencia ante el Riesgo de Accidentes en el Transporte de Mercancías Peligrosas por Carretera y Ferrocarril de País Vasco', 4, NULL, NULL, 3, 7, 2),
(134, N'16000000203', N'Plan de Emergencia ante el Riesgo Radiológico de País Vasco', 4, NULL, NULL, 3, 4, 2),
(135, N'16000000219', N'Plan de Emergencia ante el Riesgo Sísmico de País Vasco', 4, NULL, NULL, 3, 20, 2),
(136, N'16000000511', N'Plan de Emergencia ante la contaminación de la Ribera del Mar de País Vasco', 4, NULL, NULL, 6, 12, 2),
(137, N'16000000204', N'Plan de Emergencia Aeronáuticas de País Vasco', 4, NULL, NULL, 3, 5, 2),
(138, N'17000000100', N'Plan Territorial de Protección Civil de La Rioja', 9, NULL, NULL, 2, 1, 2),
(139, N'17000000214', N'Plan de Emergencia por Incendios Forestales en La Rioja', 9, NULL, NULL, 3, 15, 2),
(140, N'17000000215', N'Plan de Emergencia ante Inundaciones de La Rioja', 9, NULL, NULL, 3, 16, 2),
(141, N'17000000206', N'Plan de Emergencia sobre Transporte de Mercancías Peligrosas por Carretera y Ferrocarril de La Rioja', 9, NULL, NULL, 3, 7, 2),
(142, N'17000000203', N'Plan de Emergencia ante Riesgos Radiológicos de La Rioja', 9, NULL, NULL, 3, 4, 2),
(143, N'18000000100', N'Plan Territorial de Protección Civil de Ceuta', 18, NULL, NULL, 2, 1, 2),
(144, N'18000000214', N'Plan de Emergencia de Incendios Forestales de Ceuta', 18, NULL, NULL, 3, 15, 2),
(145, N'18000000215', N'Plan de Emergencia ante Inundaciones de Ceuta', 18, NULL, NULL, 3, 16, 2),
(146, N'18000000219', N'Plan de Emergencia ante el Riesgo de Seísmos y Maremotos en Ceuta', 18, NULL, NULL, 3, 20, 2),
(147, N'19000000100', N'Plan Territorial de Protección Civil de la Ciudad Autónoma de Melilla', 19, NULL, NULL, 2, 1, 2);

-- =============================================
-- FASES DE EMERGENCIA
-- =============================================

INSERT INTO FaseEmergencia (Id,IdPlanEmergencia,Orden,Descripcion) VALUES
	 (1,1,1,N'Alerta y seguimiento'),
	 (2,1,2,N'Preemergencia'),
	 (3,1,3,N'Emergencia'),
	 (4,1,4,N'Recuperación'),
	 (5,2,1,N'Preemergencia'),
	 (6,2,2,N'Emergencia'),
	 (7,2,3,N'Normalización'),
	 (8,3,1,N'Alerta y seguimiento'),
	 (9,3,2,N'Emergencia'),
	 (10,4,1,N'Seguimiento'),
	 (11,4,2,N'Emergencia'),
	 (12,4,3,N'Normalización'),
	 (13,5,1,N'Seguimiento'),
	 (14,5,2,N'Emergencia'),
	 (15,5,3,N'Normalización'),
	 (16,6,1,N'Seguimiento'),
	 (17,6,2,N'Emergencia'),
	 (18,6,3,N'Normalización'),
	 (19,7,1,N'Preemergencia'),
	 (20,7,2,N'Emergencia'),
	 (21,7,3,N'Transición'),
	 (22,8,1,N'Alerta y seguimiento'),
	 (23,8,2,N'Preemergencia'),
	 (24,8,3,N'Emergencia'),
	 (25,8,4,N'Recuperación'),
	 (26,9,1,N'Emergencia'),
	 (27,10,1,N'Alerta'),
	 (28,10,2,N'Emergencia General'),
	 (29,10,3,N'Recuperación'),
	 (30,11,1,N'Alerta'),
	 (31,11,2,N'Emergencia'),
	 (32,11,3,N'Recuperación'),
	 (33,12,1,N'Alerta'),
	 (34,12,2,N'Emergencia'),
	 (35,12,3,N'Recuperación'),
	 (36,13,1,N'Alerta'),
	 (37,13,2,N'Emergencia'),
	 (38,13,3,N'Recuperación'),
	 (39,14,1,N'Alerta'),
	 (40,14,2,N'Emergencia'),
	 (41,14,3,N'Recuperación'),
	 (42,15,1,N'Normalidad'),
	 (43,15,2,N'Preemergencia'),
	 (44,15,3,N'Emergencia'),
	 (45,16,1,N'-'),
	 (46,17,1,N'Alerta'),
	 (47,17,2,N'Emergencia'),
	 (48,18,1,N'Alerta'),
	 (49,18,2,N'Preemergencia'),
	 (50,18,3,N'Emergencia'),
	 (51,19,1,N'Preemergencia'),
	 (52,19,2,N'Emergencia '),
	 (53,19,3,N'Recuperación '),
	 (54,20,1,N'Fase 0'),
	 (55,20,2,N'Fase 1 '),
	 (56,20,3,N'Fase 2'),
	 (57,20,4,N'Fase 3'),
	 (58,20,5,N'Fase de Normalización'),
	 (59,21,1,N'Preemergencia '),
	 (60,21,2,N'Emergencia            '),
	 (61,21,3,N'Emergencia            '),
	 (62,21,4,N'Normalización'),
	 (63,22,1,N'Emergencia            '),
	 (64,22,2,N'Normalización'),
	 (65,23,1,N'Preemergencia '),
	 (66,23,2,N'Emergencia '),
	 (67,23,3,N'Emergencia '),
	 (68,23,4,N'Fase de Normalización'),
	 (69,24,1,N'Preemergencia'),
	 (70,24,2,N'Emergencia            '),
	 (71,24,3,N'Emergencia            '),
	 (72,24,4,N'Normalización'),
	 (73,25,1,N'Preemergencia'),
	 (74,25,2,N'Emergencia '),
	 (75,25,3,N'Recuperación '),
	 (76,26,1,N'Alerta'),
	 (77,26,2,N'Emergencia'),
	 (78,27,1,N'Alerta'),
	 (79,27,2,N'Emergencia '),
	 (80,27,3,N'Fase de Normalización'),
	 (81,28,1,N'Alerta'),
	 (82,28,2,N'Emergencia '),
	 (83,28,3,N'Emergencia'),
	 (84,28,4,N'Normalización'),
	 (85,29,1,N'Alerta'),
	 (86,29,2,N'Emergencia '),
	 (87,29,3,N'Emergencia'),
	 (88,30,1,N'Alerta'),
	 (89,30,2,N'Emergencia '),
	 (90,31,1,N'Fase de Intesificación del seguimiento y la información'),
	 (91,31,2,N'Emergencia'),
	 (92,31,3,N'Emergencia '),
	 (93,32,1,N'Alerta'),
	 (94,32,2,N'Emergencia'),
	 (95,32,3,N'Emergencia '),
	 (96,32,4,N'Fin de la Emergencia'),
	 (97,33,1,N'Alerta '),
	 (98,33,2,N'Alerta'),
	 (99,33,3,N'Emergencia '),
	 (100,33,4,N'Emergencia'),
	 (101,33,5,N'Normalización'),
	 (102,34,1,N'Alerta'),
	 (103,34,2,N'Emergencia'),
	 (104,34,3,N'Recuperación '),
	 (105,35,1,N'Preemergencia'),
	 (106,35,2,N'Emergencia'),
	 (107,35,3,N'Normalización'),
	 (108,36,1,N'Alerta'),
	 (109,36,2,N'Emergencia'),
	 (110,36,3,N'Fin de la Emergencia'),
	 (111,37,1,N'Emergencia'),
	 (112,37,2,N'Fin de la Emergencia'),
	 (113,38,1,N'Preemergencia'),
	 (114,38,2,N'Emergencia'),
	 (115,39,1,N'Alerta'),
	 (116,39,2,N'Emergencia'),
	 (117,40,1,N'Prealerta'),
	 (118,40,2,N'Alerta'),
	 (119,40,3,N'Emergencia'),
	 (120,40,4,N'Normalización'),
	 (121,41,1,N'Preemergencia'),
	 (122,41,2,N'Emergencia'),
	 (123,41,3,N'Normalización'),
	 (124,42,1,N'Alerta'),
	 (125,42,2,N'Emergencia'),
	 (126,42,3,N'Fin de la Emergencia'),
	 (127,43,1,N'Prealerta'),
	 (128,43,2,N'Emergencia'),
	 (129,43,3,N'Fin de la Emergencia'),
	 (130,43,4,N'Recuperación '),
	 (131,44,1,N'Prealerta'),
	 (132,44,2,N'Emergencia'),
	 (133,44,3,N'Normalización'),
	 (134,45,1,N'Preemergencia'),
	 (135,45,2,N'Emergencia'),
	 (136,45,3,N'Normalización'),
	 (137,46,1,N'Prealerta'),
	 (138,46,2,N'Alerta'),
	 (139,46,3,N'Emergencia'),
	 (140,46,4,N'Normalización'),
	 (141,47,1,N'Normalidad'),
	 (142,47,2,N'Prealerta'),
	 (143,47,3,N'Alerta'),
	 (144,47,4,N'Emergencia'),
	 (145,48,1,N'Prealerta'),
	 (146,48,2,N'Alerta'),
	 (147,48,3,N'Alerta Máxima'),
	 (148,48,4,N'Emergencia'),
	 (149,48,5,N'Fin de la Emergencia'),
	 (150,49,1,N'Prealerta'),
	 (151,49,2,N'Alerta'),
	 (152,49,3,N'Emergencia'),
	 (153,49,4,N'Fin de la Emergencia'),
	 (154,50,1,N'Prealerta'),
	 (155,50,2,N'Alerta'),
	 (156,50,3,N'Alerta máxima'),
	 (157,50,4,N'Emergencia'),
	 (158,50,5,N'Fin de emergencia'),
	 (159,51,1,N'Prealerta'),
	 (160,51,2,N'Alerta'),
	 (161,51,3,N'Emergencia'),
	 (162,51,4,N'Fin de la Emergencia'),
	 (163,52,1,N'Alerta'),
	 (164,52,2,N'Alerta máxima'),
	 (165,52,3,N'Emergencia'),
	 (166,52,4,N'Recuperación'),
	 (167,53,1,N'Prealerta'),
	 (168,53,2,N'Alerta'),
	 (169,53,3,N'Alerta Máxima'),
	 (170,53,4,N'Emergencia '),
	 (171,53,5,N'Emergencia'),
	 (172,53,6,N'Fin de la Emergencia'),
	 (173,54,1,N'Prealerta'),
	 (174,54,2,N'Alerta '),
	 (175,54,3,N'Alerta Máxima'),
	 (176,NULL,1,NULL),
	 (177,54,4,N'Emergencia'),
	 (178,54,5,N'Fin de la Emergencia'),
	 (179,55,1,N'Prealerta/Alerta'),
	 (180,55,2,N'Alerta Máxima'),
	 (181,55,3,N'Emergencia'),
	 (182,55,4,N'Fin de la Emergencia'),
	 (183,56,1,N'Prealerta'),
	 (184,56,2,N'Alerta'),
	 (185,56,3,N'Alerta Máxima'),
	 (186,56,4,N'Emergencia'),
	 (187,56,5,N'Emergencia '),
	 (188,56,6,N'Fin de la Emergencia'),
	 (189,57,1,N'Prealerta'),
	 (190,57,2,N'Alerta/Alerta máxima'),
	 (191,57,3,N'Emergencia'),
	 (192,57,4,N'Emergencia '),
	 (193,58,1,N'Preemergencia'),
	 (194,58,2,N'Emergencia'),
	 (195,58,3,N'Recuperación'),
	 (196,59,1,N'Emergencia'),
	 (197,59,2,N'Fin de la Emergencia'),
	 (198,60,1,N'Alerta'),
	 (199,60,2,N'Alerta máxima'),
	 (200,60,3,N'Emergencia');
	 
INSERT INTO FaseEmergencia (Id,IdPlanEmergencia,Orden,Descripcion) VALUES
	 (201,60,4,N'Normalización'),
	 (202,61,1,N'Emergencia'),
	 (203,61,2,N'Fin de Emergencia'),
	 (204,62,1,N'Seguimiento'),
	 (205,62,2,N'Alerta'),
	 (206,62,3,N'Emergencia'),
	 (207,62,4,N'Recuperación'),
	 (208,63,1,N'Emergencia'),
	 (209,64,1,N'Alerta'),
	 (210,64,2,N'Emergencia'),
	 (211,64,3,N'Normalización'),
	 (212,65,1,N'Emergencia '),
	 (213,65,2,N'Emergencia'),
	 (214,65,3,N'Normalización'),
	 (215,65,4,N'Fin de Emergencia'),
	 (216,66,1,N'Alerta'),
	 (217,66,2,N'Emergencia'),
	 (218,66,3,N'Fin de Emergencia'),
	 (219,67,1,N'Alerta (Preemergencia)'),
	 (220,67,2,N'Emergencia'),
	 (221,67,3,N'Normalización'),
	 (222,68,1,N'Alerta'),
	 (223,68,2,N'Emergencia'),
	 (224,68,3,N'Normalización'),
	 (225,69,1,N'Alerta'),
	 (226,69,2,N'Emergencia'),
	 (227,69,3,N'Fin de Emergencia'),
	 (228,70,1,N'Alerta'),
	 (229,70,2,N'Emergencia'),
	 (230,70,3,N'Recuperación'),
	 (231,71,1,N'Alerta'),
	 (232,71,2,N'Emergencia'),
	 (233,71,3,N'Normalización'),
	 (234,72,1,N'Alerta'),
	 (235,72,2,N'Emergencia'),
	 (236,72,3,N'Normalización'),
	 (237,73,1,N'Alerta'),
	 (238,73,2,N'Emergencia'),
	 (239,73,3,N'Normalización'),
	 (240,74,1,N'Prealerta'),
	 (241,74,2,N'Alerta'),
	 (242,74,3,N'Emergencia'),
	 (243,74,4,N'Fin de Emergencia'),
	 (244,75,1,N'Prealerta'),
	 (245,75,2,N'Alerta'),
	 (246,75,3,N'Emergencia'),
	 (247,75,4,N'Fin de Emergencia'),
	 (248,76,1,N'Prealerta'),
	 (249,76,2,N'Alerta'),
	 (250,76,3,N'Emergencia'),
	 (251,76,4,N'Fin de Emergencia'),
	 (252,77,1,N'Categoría 0'),
	 (253,77,2,N'Prealerta'),
	 (254,77,3,N'Alerta'),
	 (255,77,4,N'Emergencia'),
	 (256,77,5,N'Fin de Emergencia'),
	 (257,78,1,N'Prealerta'),
	 (258,78,2,N'Alerta'),
	 (259,78,3,N'Emergencia'),
	 (260,79,1,N'Prealerta'),
	 (261,79,2,N'Alerta'),
	 (262,79,3,N'Emergencia'),
	 (263,79,4,N'Rehabilitación'),
	 (264,80,1,N'Prealerta'),
	 (265,80,2,N'Alerta'),
	 (266,80,3,N'Emergencia'),
	 (267,80,4,N'Fin de Emergencia'),
	 (268,81,1,N'Prealerta'),
	 (269,81,2,N'Alerta'),
	 (270,81,3,N'Emergencia'),
	 (271,82,1,N'Prealerta'),
	 (272,82,2,N'Alerta'),
	 (273,82,3,N'Emergencia'),
	 (274,82,4,N'Fin de la Emergencia'),
	 (275,83,1,N'Prealerta'),
	 (276,83,2,N'Alerta'),
	 (277,83,3,N'Emergencia'),
	 (278,83,4,N'Fin de la Emergencia'),
	 (279,84,1,N'Prealerta'),
	 (280,84,2,N'Alerta'),
	 (281,84,3,N'Emergencia'),
	 (282,84,4,N'Recuperación'),
	 (283,85,1,N'Prealerta'),
	 (284,85,2,N'Alerta'),
	 (285,85,3,N'Emergencia'),
	 (286,85,4,N'Fin de Emergencia'),
	 (287,85,5,N'Rehabilitación'),
	 (288,86,1,N'Prealerta'),
	 (289,86,2,N'Alerta'),
	 (290,86,3,N'Emergencia'),
	 (291,86,4,N'Fin de Emergencia'),
	 (292,87,1,N'Prealerta'),
	 (293,87,2,N'Alerta'),
	 (294,87,3,N'Emergencia'),
	 (295,88,1,N'Alerta'),
	 (296,88,2,N'Emergencia'),
	 (297,88,3,N'Fin de la Emergencia'),
	 (298,88,4,N'Vuelta a la normalidad'),
	 (299,89,1,N'Preemergencia'),
	 (300,89,2,N'Emergencia'),
	 (301,89,3,N'Fin de la Emergencia'),
	 (302,89,4,N'Vuelta a la normalidad'),
	 (303,90,1,N'Alerta'),
	 (304,90,2,N'Emergencia'),
	 (305,90,3,N'Fin de Emergencia'),
	 (306,91,1,N'Alerta'),
	 (307,91,2,N'Emergencia'),
	 (308,91,3,N'Fin de Emergencia'),
	 (309,92,1,N'Preemergencia'),
	 (310,92,2,N'Alerta'),
	 (311,92,3,N'Emergencia'),
	 (312,92,4,N'Fin de emergencia'),
	 (313,93,1,N'Alerta'),
	 (314,93,2,N'Emergencia'),
	 (315,93,3,N'Vuelta a la normalidad'),
	 (316,94,1,N'Preemergencia'),
	 (317,94,2,N'Emergencia'),
	 (318,95,1,N'Preemergencia'),
	 (319,95,2,N'Emergencia'),
	 (320,95,3,N'Fin de Emergencia'),
	 (321,96,1,N'Alerta'),
	 (322,96,2,N'Emergencia'),
	 (323,96,3,N'Recuperación'),
	 (324,97,1,N'Alerta'),
	 (325,97,2,N'Emergencia'),
	 (326,97,3,N'Recuperación'),
	 (327,98,1,N'Alerta'),
	 (328,98,2,N'Emergencia'),
	 (329,98,3,N'Normalización'),
	 (330,99,1,N'Alerta'),
	 (331,99,2,N'Emergencia'),
	 (332,100,1,N'Prealerta'),
	 (333,100,2,N'Alerta'),
	 (334,100,3,N'Emergencia'),
	 (335,100,4,N'Normalización'),
	 (336,101,1,N'Alerta'),
	 (337,101,2,N'Emergencia'),
	 (338,101,3,N'Normalización'),
	 (339,102,1,N'Alerta'),
	 (340,102,2,N'Emergencia'),
	 (341,103,1,N'Emergencia'),
	 (342,103,2,N'Fin de Emergencia'),
	 (343,104,1,N'Alerta'),
	 (344,104,2,N'Emergencia'),
	 (345,104,3,N'Normalización'),
	 (346,105,1,N'Alerta'),
	 (347,105,2,N'Emergencia'),
	 (348,105,3,N'Recuperación'),
	 (349,106,1,N'Alerta'),
	 (350,106,2,N'Emergencia'),
	 (351,106,3,N'Normalización'),
	 (352,107,1,N'Alerta'),
	 (353,107,2,N'Emergencia'),
	 (354,107,3,N'Fin de Emergencia'),
	 (355,108,1,N'Alerta'),
	 (356,108,2,N'Emergencia'),
	 (357,108,3,N'Rehabilitación'),
	 (358,109,1,N'Emergencia'),
	 (359,110,1,N'Preemergencia'),
	 (360,110,2,N'Emergencia'),
	 (361,110,3,N'Vuelta a la normalidad'),
	 (362,111,1,N'Alerta'),
	 (363,111,2,N'Emergencia'),
	 (364,111,3,N'Fin de Emergencia'),
	 (365,112,1,N'Alerta'),
	 (366,112,2,N'Emergencia'),
	 (367,112,3,N'Fin de Emergencia'),
	 (368,113,1,N'Alerta'),
	 (369,113,2,N'Emergencia'),
	 (370,113,3,N'Fin de Emergencia'),
	 (371,114,1,N'Preemergencia'),
	 (372,114,2,N'Emergencia'),
	 (373,114,3,N'Fin de Emergencia'),
	 (374,115,1,N'Preemergencia'),
	 (375,115,2,N'Emergencia'),
	 (376,116,1,N'Preemergencia'),
	 (377,116,2,N'Emergencia'),
	 (378,116,3,N'Normalización'),
	 (379,117,1,N'Alerta'),
	 (380,117,2,N'Emergencia'),
	 (381,117,3,N'Fin de la Emergencia'),
	 (382,118,1,N'Seguimiento'),
	 (383,118,2,N'Emergencia'),
	 (384,118,3,N'Normalización'),
	 (385,119,1,N'Seguimiento'),
	 (386,119,2,N'Emergencia'),
	 (387,120,1,N'Preemergencia'),
	 (388,120,2,N'Emergencia'),
	 (389,120,3,N'Fin de Emergencia'),
	 (390,121,1,N'Preemergencia'),
	 (391,121,2,N'Emergencia'),
	 (392,121,3,N'Vuelta a la normalidad'),
	 (393,122,1,N'Alerta'),
	 (394,122,2,N'Preemergencia'),
	 (395,122,3,N'Emergencia'),
	 (396,122,4,N'Recuperación'),
	 (397,123,1,N'Preemergencia'),
	 (398,123,2,N'Emergencia'),
	 (399,123,3,N'Normalización'),
	 (400,124,1,N'Alerta');

INSERT INTO FaseEmergencia (Id,IdPlanEmergencia,Orden,Descripcion) VALUES
	 (401,124,2,N'Emergencia'),
	 (402,124,3,N'Fin de la Emergencia'),
	 (403,125,1,N'Preemergencia'),
	 (404,125,2,N'Emergencia'),
	 (405,125,3,N'Seguimiento'),
	 (406,126,1,N'Seguimiento'),
	 (407,126,2,N'Emergencia'),
	 (408,126,3,N'Normalización'),
	 (409,127,1,N'Preemergencia'),
	 (410,127,2,N'Emergencia'),
	 (411,127,3,N'Normalización'),
	 (412,128,1,N'Preemergencia'),
	 (413,128,2,N'Emergencia'),
	 (414,129,1,N'Emergencia'),
	 (415,130,1,N'Alerta'),
	 (416,130,2,N'Emergencia'),
	 (417,130,3,N'Recuperación'),
	 (418,131,1,N'Alerta'),
	 (419,131,2,N'Emergencia'),
	 (420,131,3,N'Recuperación'),
	 (421,132,1,N'Alerta'),
	 (422,132,2,N'Emergencia'),
	 (423,132,3,N'Recuperación'),
	 (424,133,1,N'Alerta'),
	 (425,133,2,N'Emergencia'),
	 (426,133,3,N'Vuelta a la normalidad'),
	 (427,134,1,N'Seguimiento'),
	 (428,134,2,N'Emergencia'),
	 (429,134,3,N'Desactivación del Plan'),
	 (430,135,1,N'Seguimiento'),
	 (431,135,2,N'Emergencia'),
	 (432,135,3,N'Normalización'),
	 (433,136,1,N'Alerta'),
	 (434,136,2,N'Emergencia'),
	 (435,137,1,N'Alerta'),
	 (436,137,2,N'Emergencia'),
	 (437,137,3,N'Recuperación'),
	 (438,138,1,N'Alerta'),
	 (439,138,2,N'Emergencia'),
	 (440,138,3,N'Fin de la Emergencia'),
	 (441,139,1,N'Emergencia'),
	 (442,139,2,N'Fin de la Emergencia'),
	 (443,140,1,N'Preemergencia'),
	 (444,140,2,N'Emergencia'),
	 (445,140,3,N'Normalización'),
	 (446,141,1,N'Emergencia'),
	 (447,141,2,N'Fin de Emergencia'),
	 (448,142,1,N'Emergencia'),
	 (449,142,2,N'Fin de la Emergencia'),
	 (450,143,1,N'Alerta'),
	 (451,143,2,N'Emergencia'),
	 (452,143,3,N'Fin de la Emergencia'),
	 (453,144,1,N'Anticipación'),
	 (454,144,2,N'Prevención Operativa'),
	 (455,144,3,N'Emergencia'),
	 (456,144,4,N'Recuperación'),
	 (457,145,1,N'Anticipación'),
	 (458,145,2,N'Prevención Operativa'),
	 (459,145,3,N'Emergencia'),
	 (460,145,4,N'Recuperación'),
	 (461,146,1,N'Seguimiento'),
	 (462,146,2,N'Emergencia'),
	 (463,146,3,N'Recuperación'),
	 (464,147,1,NULL);

INSERT INTO TipoSistemaEmergencia (Id,Descripcion) VALUES
	 (1,N'Copernicus'),
	 (2,N'UCPM'),
	 (3,N'Cruz roja'),
	 (4,N'Convenios con otros países'),
	 (5,N'RANET'),
	 (6,N'CERET');

INSERT INTO TipoSistemaEmergenciaTipoSuceso (Id,IdTipoSistemaEmergencia,IdTipoSuceso) VALUES
	 (1,1,NULL),
	 (2,2,NULL),
	 (3,3,NULL),
	 (4,4,NULL),
	 (5,5,15),
	 (6,5,16),
	 (7,6,12);

INSERT INTO ModoActivacion (Id,Descripcion) VALUES
	 (1,N'Rapid Mapping'),
	 (2,N'Risk and Recovery');

INSERT INTO PlanSituacion (Id, IdPlanEmergencia, IdFaseEmergencia, Orden, Descripcion, Nivel , Situacion, SituacionEquivalente) VALUES
	('1', '1', '1', '1', 'Recibir y transmitir avisos, alertas y cualesquiera
otras informaciones relevantes para la detección de
posibles situaciones de riesgos de protección civil,
así como la valoración de las mismas, la difusión que
proceda y el apoyo a los órganos de dirección y gestión
de las mismas.', '', '', '0'),
	('2', '1', '2', '1', 'Cuando se prevea que la evolución de una emergencia declarada por una Comunidad o Ciudad
Autónoma pueda requerir la aportación de recursos de protección civil de otras Comunidades Autónomas o de las Ciudades de Ceuta y Melilla, o de la Administración General del Estado, o movilizables por esta.', '', '', '0'),
	('3', '1', '3', '1', 'Cuando la o las emergencias puedan controlarse mediante el empleo de los medios y recursos ordinarios disponibles en la Comunidad o Comunidades afectadas, o Ciudades de Ceuta y Melilla, o con apoyos puntuales de recursos de otros ámbitos territoriales cuya movilización no requiera de una coordinación específica por los órganos centrales del Sistema Nacional de Protección Civil.', '', '1', '1'),
	('4', '1', '3', '2', 'Cuando la o las emergencias no puedan controlarse, o haya un riesgo cierto de que no puedan controlarse, con los medios ordinarios propios de la o las Comunidades o Ciudades Autónomas afectadas, y sea o pueda ser necesaria la aportación de recursos y medios extraordinarios de
la Administración General del Estado, o movilizables
por esta, o de otras Comunidades Autónomas o de las
Ciudades de Ceuta y Melilla, así como cuando se prevea
que alguna de las emergencias declaradas puedan
derivar en una situación de interés nacional.', '', '2', '2'),
	('5', '1', '3', '3', 'Se activará con la declaración de interés nacional de una emergencia. La Dirección Operativa de la Emergencia se encomendará por la persona titular del
Ministerio del Interior a la persona titular de la jefatura
de la Unidad Militar de Emergencias, salvo que la misma
no fuera desplegada en atención a la naturaleza de la
emergencia.', '', '3', '3'),
	('6', '1', '3', '4', 'Se declarará por la persona titular del Ministerio del Interior en las activaciones del Plan en su fase de apoyo a otros
Sistemas Nacionales, de acuerdo con el Real Decreto de
declaración de la situación de que se trate.', '', 'E', 'E'),
	('7', '1', '4', '1', 'Fase de apoyo a otros Sistemas Nacionales, y se prolongará
hasta que se restablezcan las condiciones mínimas para
el retorno a la normalidad de las personas afectadas por la emergencia o catástrofe y para el restablecimiento de
los servicios esenciales en la zona o zonas afectadas.', '', '', '0'),
	('8', '2', '5', '1', 'Fase caracterizada por la existencia de información sobre la posibilidad de ocurrencia de sucesos capaces de dar lugar a inundaciones, tanto por desbordamiento como por
«precipitaciones in situ».
Esta fase se iniciará a partir de notificaciones sobre predicciones meteorológicas de precipitaciones fuertes o muy fuertes, u otras causas de las contempladas en el apartado 2.1 de la Directriz que puedan ocasionar riesgo de inundaciones y se prolongará, con el seguimiento hasta el análisis del mismo.', '', '', '0'),
	('9', '2', '6', '1', 'Las informaciones meteorológicas e hidrológicas permiten prever la inminencia de inundaciones en el ámbito del Plan, con peligro para personas y bienes.', '', '0', '0'),
	('10', '2', '6', '2', 'Se han producido inundaciones en zonas localizadas, cuya atención puede quedar asegurada mediante el empleo de los medios y recursos disponibles en las zonas
afectadas.', '', '1', '1'),
	('11', '2', '6', '3', 'Superan la capacidad de atención de los medios y recursos disponibles, además, los datos pluviométricos e hidrológicos y las predicciones meteorológicas permiten prever una extensión o agravación significativa de aquéllas.', '', '2', '2'),
	('12', '2', '6', '4', 'Emergencias que, habiéndose considerado que está en juego el interés nacional, así
sean declaradas por el/la Ministro/a de Interior.', '', '3', '3'),
	('13', '2', '7', '1', 'Fase consecutiva a la de emergencia, que se prolongará hasta el restablecimiento de las condiciones mínimas imprescindibles para el retorno a la normalidad en las zonas
afectadas por la inundación.
Durante esta fase se realizarán las primeras tareas de rehabilitación en dichas zonas', '', '', '0'),
	('14', '3', '8', '1', 'Seguimiento de incendios forestales en cuanto se tiene conocimiento de la existencia de un incendio comunicado por fuentes fidedignas (órganos competentes en materia de extinción o los servicios del 112 de las comunidades
autónomas en su caso, Fuerzas y Cuerpos de Seguridad del Estado u otras similares.', '', '', '0'),
	('15', '3', '9', '1', 'La notificación por parte de alguna comunidad autónoma de la puesta en marcha de medidas de protección y socorro de personas afectadas, independientemente de la magnitud del incendio o los medios de intervención en el mismo.', '1', '', '1'),
	('16', '3', '9', '2', 'El requerimiento de la movilización de medios extraordinarios de toda índole de
acuerdo a sus normas específicas de movilización, así como a su disponibilidad en función de los medios y recursos previamente utilizados o comprometidos.', '2', '', '2'),
	('17', '3', '9', '3', 'Declaración del interés nacional por parte del Ministro del
Interior según lo indicado en el punto 1.2 del Real Decreto 407/1992, de 24 de Abril por el que se aprueba la Norma Básica de Protección Civil.', '3', '', '3'),
	('18', '4', '10', '1', 'Ocurrencia de fenómenos sísmicos ampliamente
sentidos por la población que requerirá de las autoridades y órganos competentes una actuación coordinada, dirigida a intensificar la información a los ciudadanos sobre dichos
fenómenos.', '', '0', '0'),
	('19', '4', '11', '1', 'Se han producido fenómenos sísmicos, cuya atención, en lo relativo a la protección de personas y bienes, puede quedar asegurada mediante el empleo de los
medios y recursos disponibles en las zonas afectadas.', '', '1', '1'),
	('20', '4', '11', '2', 'Fenómenos sísmicos que por la gravedad de los daños
ocasionados, el número de víctimas o la extensión de las áreas afectadas, hacen necesario, para el socorro y protección de personas y bienes, el concurso de medios, recursos o servicios ubicados fuera de dichas áreas.', '', '2', '2'),
	('21', '4', '11', '3', 'Cuando la emergencia reúna las características de catástrofe o calamidad pública, por el número de víctimas y daños
ocasionados, el Ministro del Interior.', '', '3', '3'),
	('22', '4', '12', '1', 'Fase consecutiva a la de emergencia, que se prolongará
hasta el restablecimiento de las condiciones mínimas imprescindibles para el retorno a la
normalidad en las zonas afectadas por el terremoto.', '', '', '0'),
	('23', '5', '13', '1', 'Seguimiento de dichos fenómenos y por el consiguiente proceso de intercambio de información con los órganos y autoridades competentes en materia de protección civil, así
como por la información a la población en general.', '', '1', '1'),
	('24', '5', '14', '1', 'La gravedad de los daños ocasionados, el número de víctimas o la extensión de las áreas afectadas, hacen necesaria la intervención de medios, recursos o
servicios pertenecientes a otras Comunidades Autónomas, a los Órganos Estatales o a los mecanismos de ayuda internacional.', '', '2', '2'),
	('25', '5', '14', '2', 'Fenómenos cuya gravedad determina que se considere en
juego el interés nacional, habiéndose declarado así por el Ministro de Interior.', '', '3', '3'),
	('26', '5', '15', '1', 'Se prolongará hasta el restablecimiento de las condiciones mínimas imprescindibles para el retorno a la
normalidad en la población, medio ambiente y bienes de las áreas afectadas por el
accidente.', '', '', '0'),
	('27', '6', '16', '1', 'Fenómenos sísmicos ampliamente sentidos por la
población y, en su caso, otros fenómenos asociados a la actividad volcánica, que hacen necesaria una actuación coordinada de las autoridades y órganos competentes, dirigida a intensificar tanto la información como la formación a los ciudadanos sobre dichos fenómenos.', '', '0', '0'),
	('28', '6', '17', '1', 'Protección asegurada mediante el empleo de los medios y recursos adscritos al Plan Especial de Protección Civil
ante el Riesgo Volcánico de la Comunidad Autónoma de Canarias o al correspondiente Plan
Territorial, si fuera otra comunidad autónoma la afectada, y, en su caso, de los medios de la Administración General del Estado presentes en el ámbito territorial de la misma.', '', '1', '1'),
	('29', '6', '17', '2', 'Para la adecuada atención de personas y bienes, se requiere el concurso de medios, recursos o servicios ubicados fuera del ámbito territorial de la comunidad autónoma afectada.', '', '2', '2'),
	('30', '6', '17', '3', 'La gravedad de la situación hace que sea declarada de interés nacional por el Ministro del Interior.', '', '3', '3'),
	('31', '6', '18', '1', 'Se prolongará hasta el restablecimiento de
las condiciones mínimas imprescindibles para el retorno a la normalidad en las zonas
afectadas.', '', '', '0'),
	('32', '7', '19', '1', 'Situación en la que los riesgos se limitan a la propia instalación y pueden ser controlados por los medios disponibles en el correspondiente plan de emergencia interior o plan de autoprotección.', '', '0', '0'),
	('33', '7', '20', '1', 'Situación en la que el riesgo sobre la población, el medio ambiente o los bienes, aun siendo muy improbable, requiere la adopción de medidas de protección,
pudiendo ser controlada con los medios y recursos correspondientes a los planes de la
comunidad autónoma afectada.', '', '1', '1'),
	('34', '7', '20', '2', 'Se hacen necesaria la intervención de medios, recursos o servicios diferentes a los adscritos a los
planes de la comunidad autónoma afectada, por lo que es requerida la intervención de la Administración General del Estado en la aportación de tales medios, recursos o servicios.', '', '2', '2'),
	('35', '7', '20', '3', 'Fenómenos cuya naturaleza, gravedad o alcance de los
riesgos determinan que se considere en juego el interés nacional, habiéndose declarado así por el Ministro de Interior.', '', '3', '3'),
	('36', '7', '21', '1', 'Puede ser necesario aplicar medidas de
larga duración a las que hace referencia el anexo II del presente Plan Estatal que se
prolongará hasta el restablecimiento de las condiciones mínimas imprescindibles para el
inicio del retorno a la normalidad en la población, el medio ambiente y los bienes de las
áreas afectadas por el accidente.', '', '', '0'),
	('37', '8', '22', '1', 'Recibir y transmitir avisos, alertas y cualesquiera
otras informaciones relevantes para la detección de
posibles situaciones de riesgos de protección civil,
así como la valoración de las mismas, la difusión que
proceda y el apoyo a los órganos de dirección y gestión
de las mismas.', '', '', '0'),
	('38', '8', '23', '1', 'Cuando se prevea que la evolución de una emergencia declarada por una Comunidad o Ciudad
Autónoma pueda requerir la aportación de recursos de protección civil de otras Comunidades Autónomas o de las Ciudades de Ceuta y Melilla, o de la Administración General del Estado, o movilizables por esta.', '', '', '0'),
	('39', '8', '24', '1', 'Cuando la o las emergencias puedan controlarse mediante el empleo de los medios y recursos ordinarios disponibles en la Comunidad o Comunidades afectadas, o Ciudades de Ceuta y Melilla, o con apoyos puntuales de recursos de otros ámbitos territoriales cuya movilización no requiera de una coordinación específica por los órganos centrales del Sistema Nacional de Protección Civil.', '', '1', '1'),
	('40', '8', '24', '2', 'Cuando la o las emergencias no puedan controlarse, o haya un riesgo cierto de que no puedan controlarse, con los medios ordinarios propios de la o las Comunidades o Ciudades Autónomas afectadas, y sea o pueda ser necesaria la aportación de recursos y medios extraordinarios de
la Administración General del Estado, o movilizables
por esta, o de otras Comunidades Autónomas o de las
Ciudades de Ceuta y Melilla, así como cuando se prevea
que alguna de las emergencias declaradas puedan
derivar en una situación de interés nacional.', '', '2', '2'),
	('41', '8', '24', '3', 'Se activará con la declaración de interés nacional de una emergencia. La Dirección Operativa de la Emergencia se encomendará por la persona titular del
Ministerio del Interior a la persona titular de la jefatura
de la Unidad Militar de Emergencias, salvo que la misma
no fuera desplegada en atención a la naturaleza de la
emergencia.', '', '3', '3'),
	('42', '8', '24', '4', 'Se declarará por la persona titular del Ministerio del Interior en las activaciones del Plan en su fase de apoyo a otros
Sistemas Nacionales, de acuerdo con el Real Decreto de
declaración de la situación de que se trate.', '', 'E', 'E'),
	('43', '8', '25', '1', 'Fase de apoyo a otros Sistemas Nacionales, y se prolongará
hasta que se restablezcan las condiciones mínimas para
el retorno a la normalidad de las personas afectadas por la emergencia o catástrofe y para el restablecimiento de
los servicios esenciales en la zona o zonas afectadas.', '', '', '0'),
	('44', '9', '26', '1', 'Declaración del interés nacional por parte del Ministro del
Interior según lo indicado en el punto 1.2 del Real Decreto 407/1992, de 24 de Abril por el que se aprueba la Norma Básica de Protección Civil.', '3', '', '3'),
	('45', '10', '27', '1', 'No conlleva la aplicación de ninguna medida de seguridad. Sin embargo, sí conlleva la notificación y verificación del incidente. Además de, una primera evolución y un seguimiento de la misma.', '', '1', '1'),
	('46', '10', '28', '1', 'En la declaración del nivel 2 de emergencia, se pondrá en funcionamiento el CECOP, y conllevará una integración de medios y recursos extraordinarios, junto a una adopción de medidas urgentes para la población.', '', '2', '2'),
	('47', '10', '28', '2', 'Conllevará la clasificación y descontaminación, tanto de personas, como infraestructuras y animales. Además de, una integración de medios y recursos extraordinarios al plan.', '', '3', '3'),
	('48', '10', '29', '1', 'Se inicia cuando se ha declarado el final de la
fase de emergencia y comprende todas aquellas
medidas encaminadas a recuperar las condiciones
normales de vida en las zonas afectadas.', '', '', '0'),
	('49', '11', '30', '1', 'Es un período de consultas entre el Director del PENCA, el Director de Emergencias del CSN y el Director de Emergencia de la central nuclear de Almaraz, orientadas al análisis, estudio y seguimiento del suceso notificado. No se hace necesaria la adopción de medidas de protección a
la población.', '', '0', '0'),
	('50', '11', '31', '1', 'En esta situación, aunque no se prevee la aplicación de medidas de protección urgentes, es necesario ir desarrollando determinadas actuaciones al objeto de preparar la posible adopción de las mismas si la
situación empeora.', '', '1', '1'),
	('51', '11', '31', '2', 'Esta situación viene caracterizada por la adopción de medidas de protección urgentes a la población.', '', '2', '2'),
	('52', '11', '31', '3', 'Declaración del interés nacional por parte del Ministro del
Interior según lo indicado en el punto 1.2 del Real Decreto 407/1992, de 24 de Abril por el que se aprueba la Norma Básica de Protección Civil.', '', '3', '3'),
	('53', '11', '32', '1', 'Se realizan operaciones de recuperación, una vez se haya controlado plenamente la situación tras el accidente y se hayan restablecido los servicios esenciales en la zona afectada.', '', '', '0'),
	('54', '12', '33', '1', 'Es un período de consultas entre el Director del PENCA, el Director de Emergencias del CSN y el Director de Emergencia de la central nuclear de Almaraz, orientadas al análisis, estudio y seguimiento del suceso notificado. No se hace necesaria la adopción de medidas de protección a
la población.', '', '0', '0'),
	('55', '12', '34', '1', 'En esta situación, aunque no se prevee la aplicación de medidas de protección urgentes, es necesario ir desarrollando determinadas actuaciones al objeto de preparar la posible adopción de las mismas si la
situación empeora.', '', '1', '1'),
	('56', '12', '34', '2', 'Esta situación viene caracterizada por la adopción de medidas de protección urgentes a la población.', '', '2', '2'),
	('57', '12', '34', '3', 'Declaración del interés nacional por parte del Ministro del
Interior según lo indicado en el punto 1.2 del Real Decreto 407/1992, de 24 de Abril por el que se aprueba la Norma Básica de Protección Civil.', '', '3', '3'),
	('58', '12', '35', '1', 'Se realizan operaciones de recuperación, una vez se haya controlado plenamente la situación tras el accidente y se hayan restablecido los servicios esenciales en la zona afectada.', '', '', '0'),
	('59', '13', '36', '1', 'Es un período de consultas entre el Director del PENCA, el Director de Emergencias del CSN y el Director de Emergencia de la central nuclear de Almaraz, orientadas al análisis, estudio y seguimiento del suceso notificado. No se hace necesaria la adopción de medidas de protección a
la población.', '', '0', '0'),
	('60', '13', '37', '1', 'En esta situación, aunque no se prevee la aplicación de medidas de protección urgentes, es necesario ir desarrollando determinadas actuaciones al objeto de preparar la posible adopción de las mismas si la
situación empeora.', '', '1', '1'),
	('61', '13', '37', '2', 'Esta situación viene caracterizada por la adopción de medidas de protección urgentes a la población.', '', '2', '2'),
	('62', '13', '37', '3', 'Declaración del interés nacional por parte del Ministro del
Interior según lo indicado en el punto 1.2 del Real Decreto 407/1992, de 24 de Abril por el que se aprueba la Norma Básica de Protección Civil.', '', '3', '3'),
	('63', '13', '38', '1', 'Se realizan operaciones de recuperación, una vez se haya controlado plenamente la situación tras el accidente y se hayan restablecido los servicios esenciales en la zona afectada.', '', '', '0'),
	('64', '14', '39', '1', 'Es un período de consultas entre el Director del PENCA, el Director de Emergencias del CSN y el Director de Emergencia de la central nuclear de Almaraz, orientadas al análisis, estudio y seguimiento del suceso notificado. No se hace necesaria la adopción de medidas de protección a
la población.', '', '0', '0'),
	('65', '14', '40', '1', 'En esta situación, aunque no se prevee la aplicación de medidas de protección urgentes, es necesario ir desarrollando determinadas actuaciones al objeto de preparar la posible adopción de las mismas si la
situación empeora.', '', '1', '1'),
	('66', '14', '40', '2', 'Esta situación viene caracterizada por la adopción de medidas de protección urgentes a la población.', '', '2', '2'),
	('67', '14', '40', '3', 'Declaración del interés nacional por parte del Ministro del
Interior según lo indicado en el punto 1.2 del Real Decreto 407/1992, de 24 de Abril por el que se aprueba la Norma Básica de Protección Civil.', '', '3', '3'),
	('68', '14', '41', '1', 'Se realizan operaciones de recuperación, una vez se haya controlado plenamente la situación tras el accidente y se hayan restablecido los servicios esenciales en la zona afectada.', '', '', '0'),
	('69', '15', '42', '1', 'En esta fase se entiende que el tráfico marítimo es normal y fluido. Los vehículos que acuden a los puertos y embarcan según los horarios previstos, siendo, asimismo, su
entrada por frontera en flujos de escasa intensidad.', '-', '-', '-'),
	('70', '15', '43', '1', 'Viene marcada por un incremento notable en el grado de ocupación de las zonas de
aparcamiento previstas en cada puerto y es posible llegar a superar el 65% de su capacidad. Suele coincidir con un aumento en el número de vehículos atravesando las
fronteras o un aumento significativo del tráfico en las vías de acceso a los diferentes puertos, y es necesario establecer un CECOD.', '-', '-', '-'),
	('71', '15', '44', '1', 'Se declarará cuando se prevea el desbordamiento de las capacidades de aparcamiento previsto por las autoridades competentes de cada puerto, teniendo que
tomarse medidas de regulación de tráfico en áreas de estacionamiento alternativo previstas
en cada Plan. Cuando la ocupación de las zonas de aparcamiento portuario pueda superar el 80% de su capacidad y haya un flujo importante de entrada de vehículos por fronteras, así como al puerto.', '-', '-', '-'),
	('72', '16', '45', '1', '-', '-', '-', '-'),
	('73', '17', '46', '1', 'Tienen como misión tomar de forma inmediata, en caso de incidente, accidente o incendio, las medidas destinadas a garantizar la seguridad del personal, de los usuarios y de las instalaciones.', '-', '-', '0'),
	('74', '17', '47', '1', 'Evento de seguridad susceptible de ser gestionado por
el A.I. de la S.I. con sus medios propios y con los de las
empresas ferroviarias dentro del ámbito del PIS.', '-', '-', '1'),
	('75', '17', '47', '2', 'Evento de seguridad que requiere la intervención de los
servicios de emergencia públicos y la activación del PSB.', '-', '-', '2'),
	('76', '18', '48', '1', 'Se iniciará cuando se de un aviso por nevadas de nivel rojo o naranja por la AEMET, y se dará por finalizada cuando cesen las circunstancias que dieron lugar a la misma o cuando la 
situación haga necesario pasar a la fase de preemergencia.', '-', '-', '0'),
	('77', '18', '49', '1', 'Se producirá cuando la intensidad de la nevada haga prever dificultades para la circulación o la nieve caída en la calzada, el hielo o cualquier otra circunstancia, dificulte  efectivamente la circulación en algún tramo de la Red de Carreteras del Estado.', '-', '-', '0'),
	('78', '18', '50', '1', 'Se alcanza esta fase cuando resulte necesario prestar atención a personas que han quedado bloqueadas o retenidas y no pueden seguir el viaje por sus propios medios.', '-', '-', '-'),
	('79', '19', '51', '1', 'Seguimiento del fenómeno producido o de las previsiones y predicciones disponibles.', '0', '', '0'),
	('80', '19', '52', '1', 'Cuando la emergencia pueda controlarse mediante el empleo de los medios y recursos ordinarios disponibles de la junta.', '1', '', '1'),
	('81', '19', '52', '2', 'Cuando la emergencia no pueda controlarse, con los medios ordinarios propios de la Junta.', '2', '', '2'),
	('82', '19', '52', '3', 'Especial gravedad por sus dimensiones efectivas o previsibles.', '3', '', '3'),
	('83', '19', '53', '1', 'Dirigir y coordinar las labores y actuaciones necesarias para conseguir la recuperación de los servicios.', '', '', '0'),
	('84', '20', '54', '1', 'Se producirá la movilización del Grupo de Intervención con personal adscrito a la Consejería de competente en materia de medio ambiente.', '0', '', '0'),
	('85', '20', '55', '1', 'Se activará el personal técnico de gestión de emergencias según los protocolos establecidos.', '1', '', '1'),
	('86', '20', '56', '1', 'La solicitud de los medios extraordinarios estatales será realizada por la Dirección Operativa del Plan a través del COR en caso de tratarse de medios de extinción o a través del CECOP Regional en el resto de los medios.', '2', '', '2'),
	('87', '20', '57', '1', ' La activación del Plan INFOCA en nivel 3 se realizará según Normativa Estatal vigente.', '3', '', '3'),
	('88', '20', '58', '1', 'La Declaración del fin de la emergencia corresponde a la Dirección del Plan.', '', '', '0'),
	('89', '21', '59', '1', 'Durante la fase de Preemergencia se realizará el seguimiento de los sucesos que se van produciendo,
con objeto de realizar un análisis de las distintas evoluciones.', '', '', '0'),
	('90', '21', '60', '1', 'La información meteorológica e hidrológica permiten prever la inminencia de las inundaciones con peligro para personas y bienes.', '', '0', '0'),
	('91', '21', '60', '2', 'Se han producido inundaciones en zonas localizadas, cuya atención puede quedar
asegurada mediante el empleo de los medios y recursos locales adscritos a los planes de actuación
local.', '', '1', '1'),
	('92', '21', '61', '1', 'Se han producido inundaciones que superan la capacidad de atención de los medios y
recursos locales o aún sin producirse esta última circunstancia, los datos aportados por los sistemas
de predicción permiten prever una extensión o agravación significativa de las mismas', '', '2', '2'),
	('93', '21', '60', '3', 'Inundaciones en las que está en juego el interés nacional y son declaradas por el Ministro del Interior', '', '3', '3'),
	('94', '21', '62', '1', 'Fase consecutiva a la de emergencia y que se mantiene hasta el restablecimiento de las condiciones
mínimas imprescindibles para un retorno a la normalidad en las zonas afectadas por la inundación', '', '0', '0'),
	('95', '22', '63', '1', 'Referida a aquellos accidentes que puedan ser controlados por los medios locales disponibles y que no suponen peligro para las personas', '', '0', '0'),
	('96', '22', '63', '2', 'Referida a accidentes que pudiendo ser controlados con los medios de intervención disponibles, requieren de la puesta en práctica de medidas de protección', '', '1', '1'),
	('97', '22', '63', '3', ' Referida a accidentes que para su control  se prevé el concurso de medios de intervención del Plan de Comunidad Autónoma de nivel regional, e incluso, recursos proporcionados por el Plan Estatal.', '', '2', '2'),
	('98', '22', '63', '4', 'Referida a accidentes que declaran el interés nacional, declarados por el Ministerio del
Interior. ', '', '3', '3'),
	('99', '22', '64', '1', 'Fase consecutiva a la de emergencia que se mantiene hasta el restablecimiento de las condiciones mínimas imprescindibles para un retorno a la normalidad en las zonas afectadas por el accidente', '', '0', '0'),
	('100', '23', '65', '1', 'El Comité de Seguimiento Sísmico evaluará la situación y realizará los estudios oportunos con los datos obtenidos.', '', '0', '0'),
	('101', '23', '66', '1', 'Se han producido fenómenos sísmicos, cuya atención, en lo relativo a la protección de personas y
bienes, puede quedar asegurada mediante el empleo de los medios y recursos locales.', '', '1', '1'),
	('102', '23', '67', '1', 'Se han producido fenómenos sísmicos que superan la capacidad de atención de los medios y
recursos locales.Por lo que, pueden darse a Nivel Provincial o a Nivel Regional.', '', '2', '2'),
	('103', '23', '67', '2', 'Situación en la que se ven afectadas por un sismo de graves consecuencias varias Comunidades
Autónomas o varios Estados, o cuando afectando sólo a una Comunidad Autónoma, por sus graves
efectos, se declare como situación de interés nacional.', '', '3', '3'),
	('104', '23', '68', '1', 'Fase consecutiva a la de emergencia que se prolongará hasta el restablecimiento de las condiciones para el retorno a la normalidad en las zonas afectadas por el terremoto.', '', '', '0'),
	('105', '24', '69', '1', 'Situación en la que por las informaciones recibidas existe  la probabilidad de que el litoral se vea afectado por un vertido contaminante.', '', '', '0'),
	('106', '24', '70', '1', 'Situación en la que se ve afectada de forma 
leve el litoral, pudiendo ser controlado el accidente con los medios locales disponibles.', '', '0', '0'),
	('107', '24', '70', '2', 'Situación en la que se ve afectada de forma 
grave el litoral correspondiente a una delimitación provincial.', '', '1', '1'),
	('108', '24', '71', '1', 'Situación en la que se ve afectada de forma 
grave el litoral correspondiente a más de una provincia o que 
afectando sólo a una provincia sus efectos son de especial magnitud y gravedad.', '', '2', '2'),
	('109', '24', '70', '3', 'Situación en la que se ve afectado de forma 
grave el litoral correspondiente a la  Comunidades Autónomas o varios Estados, se declara 
situación de interés nacional.', '', '3', '3'),
	('110', '24', '72', '1', 'Situación en la que habiéndose concluido las tareas, es necesario  mantener una  coordinación dirigida al restablecimiento de unas condiciones   para el 
retorno a la normalidad en las zonas afectadas.', '', '', '0'),
	('111', '25', '73', '1', 'Incluye, además de las definidas en el PTEAnd, unas
primeras actuaciones de alerta, desde la notificación por parte del IGN de la detección de un fenómeno susceptible de generar un maremoto y del área de costa potencialmente afectada.', '0', '', '0'),
	('112', '25', '74', '1', 'Cuando la emergencia pueda controlarse mediante el empleo de los medios y recursos ordinarios disponibles de la junta.', '1', '', '1'),
	('113', '25', '74', '2', 'Cuando la emergencia no pueda controlarse, con los medios ordinarios propios de la Junta.', '2', '', '2'),
	('114', '25', '74', '3', 'Especial gravedad por sus dimensiones efectivas o previsibles.', '3', '', '3'),
	('115', '25', '75', '1', 'Dirigir y coordinar las labores y actuaciones necesarias para conseguir la recuperación de los servicios.', '', '', '0'),
	('116', '26', '76', '1', 'Informaciones procedentes de servicios de previsión y alerta o de los servicios ordinarios de
intervención, que por evolución desfavorable, pudiesen ser generadoras de una
emergencia en la que haya que aplicar medidas de protección civil.', '', '', '0'),
	('117', '26', '77', '1', 'Cuando la situación generada
o la evolución previsible de la misma pueda ser controlada con los medios y recursos asignados al PLATEAR. ', '1', '', '1'),
	('118', '26', '77', '2', 'Cuando la situación generada
o la evolución previsible de la misma requiera medios y recursos no asignados
al PLATEAR.', '2', '', '2'),
	('119', '26', '77', '3', 'Cuando la situación generada
o la evolución previsible de la misma afecte al interés nacional. ', '3', '', '3'),
	('120', '27', '78', '1', ' Se establece cuando el estado en que se encuentran los combustibles forestales origina una probabilidad de ignición 
media y unas condiciones favorables para la propagación de los incendios forestales.', '', '', '0'),
	('121', '27', '79', '1', 'Situación de emergencia ordinaria provocada por uno o varios incendios forestales que puedan ser controlados con los medios y recursos del Plan, y los medios del Estado que tengan la 
consideración de ordinarios.', '0', '', '0'),
	('122', '27', '79', '2', 'Situación de emergencia extraordinaria provocada por uno o varios incendios forestales que puedan ser controlados con los medios y recursos del Plan, 
pudiendo ser incorporados a solicitud de la Comunidad Autónoma medios extraordinarios.', '1', '', '1'),
	('123', '27', '79', '3', 'Situación de emergencia extraordinaria provocada por uno o varios incendios forestales que puedan afectar gravemente a la población , y pueda ser necesario que, a solicitud de la 
Comunidad Autónoma, sean incorporados medios extraordinarios, o puedan comportar situaciones que 
deriven hacia el interés nacional.', '2', '', '2'),
	('124', '27', '79', '4', 'Situación de emergencia extraordinaria correspondiente y consecutiva a la declaración de 
emergencia de interés nacional por el Ministro del Interior', '3', '', '3'),
	('125', '27', '80', '1', 'La Fase de Normalización es consecutiva a la fase emergencia, manteniéndose hasta el restablecimiento de 
las condiciones mínimas imprescindibles para un retorno a la normalidad en las zonas afectadas por el 
incendio forestal.', '', '', '0'),
	('126', '28', '81', '1', 'Durante esta fase se realizará un seguimiento de los escenarios meteorológicos e hidrológicos que
posteriormente se desarrollen, hasta que del análisis de su evolución se concluya la inminencia de una
situación de emergencia por inundaciones.', '', '', '0'),
	('127', '28', '82', '1', 'Cuando se han producido inundaciones en zonas localizadas, cuya atención puede quedar asegurada
mediante el empleo de los medios y recursos adscritos a los planes de actuación municipal y/o adscritos al
PROCINAR. ', '1', '', '1'),
	('128', '28', '83', '1', 'Cuando se han producido inundaciones que superan la capacidad de atención de los medios y
recursos locales y/o adscritos al PROCINAR, siendo necesaria la participación de medios y recursos ajenos al
PROCINAR. O cuando se prevenga una agravación de Inundaciones.', '2', '', '2'),
	('129', '28', '82', '2', 'Situaciones de emergencia en las que la naturaleza, gravedad o alcance de los riesgos pongan en juego el interés nacional,
declarándose por el Ministro del interior la emergencia de interés nacional.', '3', '', '3'),
	('130', '28', '84', '1', 'Es la fase consecutiva a la de emergencia y que se mantiene hasta el restablecimiento de las condiciones
mínimas imprescindibles para un retorno a la normalidad en las zonas afectadas por la inundación.', '', '', '0'),
	('131', '29', '85', '1', 'Permite establecer medidas de 
aviso para que, en caso de evolución desfavorable de la 
emergencia, se traducen en una respuesta más rápida y eficaz. Se caracteriza  por el seguimiento de los fenómenos y por el consiguiente proceso 
de intercambio de información.', '0', '', '0'),
	('132', '29', '86', '1', 'Cuando las consecuencias derivadas 
del accidente se puedan controlar con los medios y recursos asignados a dicho Plan. ', '1', '', '1'),
	('133', '29', '87', '1', 'Cuando las consecuencias derivadas 
del accidente precisen para su control de medios y recursos no asignados a dicho Plan.', '2', '', '2'),
	('134', '29', '86', '2', 'Cuando las consecuencias derivadas 
del accidente afecten al interés nacional. ', '3', '', '3'),
	('135', '30', '88', '1', 'Cuando el accidente radiológico no suponga 
riesgo inmediato para la población, bienes y el medio ambiente, o por activación de un Plan de ámbito municipal o comarcal en fase de 
emergencia.', '0', '', '0'),
	('136', '30', '89', '1', 'Se activará cuando las consecuencias derivadas del 
accidente no se puedan controlar únicamente con los recursos propios del Plan de 
Autoprotección o cuando se necesiten la puesta en práctica de medidas para la 
protección, siempre y cuando se puedan controlan con los medios y recursos asignados a dicho Plan.', '1', '', '1'),
	('137', '30', '89', '2', 'Se activará, por el director del plan, cuando se requieran medios y recursos no 
asignados al Plan.', '2', '', '2'),
	('138', '30', '89', '3', 'Se activará en aquellas situaciones de emergencia en las que se declare el interés nacional por concurrir alguna de las circunstancias contenidas 
en la Norma Básica de Protección Civil.', '3', '', '3'),
	('139', '31', '90', '1', 'Cuando se ha percibido por la población de 
una determinada zona de Aragón, sin producir víctimas ni daños materiales 
considerables. Se caracteriza por el aviso de la información a la población.', '0', '', '0'),
	('140', '31', '91', '1', 'Se han producido fenómenos sísmicos, cuya atención,
a la protección de personas y bienes, puede quedar asegurada mediante el 
empleo de los medios y recursos disponibles en las zonas afectadas.', '', '1', '1'),
	('141', '31', '92', '1', 'Por la gravedad de los 
daños ocasionados, el número de víctimas o la extensión de las áreas afectadas, 
hacen necesario el concurso 
de medios, recursos o servicios ubicados fuera de dichas áreas.', '', '2', '2'),
	('142', '31', '92', '2', 'Se ha producido una emergencia ocasionada por un fenómeno sísmico, que el 
Ministro del Interior considera afecta al interés nacional.', '', '3', '3'),
	('143', '32', '93', '1', 'Cuando el accidente no 
suponga riesgo inmediato para la población, bienes y sus consecuencias se puedan controlar 
con los medios propios de la empresa propietaria u operadora del oleoducto o gasoducto o por 
activación de un plan de ámbito municipal o comarcal en fase de emergencia.', '', '', '0'),
	('144', '32', '94', '1', 'Ante 
situaciones de emergencia que, para su control, solo es necesaria la intervención de medios y recursos asignados 
al Plan.', '1', '', '1'),
	('145', '32', '95', '1', 'Ante 
situaciones de emergencia que, para su control se precisa de medios de intervención no asignados al Plan.', '2', '', '2'),
	('146', '32', '95', '2', 'Ante 
situaciones de emergencia en las que la naturaleza, gravedad o alcance de los riesgos supongan la declaración de la emergencia de interés nacional.', '3', '', '3'),
	('147', '32', '96', '1', 'Se declarará el fin de la emergencia una vez comprobado e 
informado por la Dirección Técnica de la emergencia que han desaparecido o se han reducido 
 las causas que provocaron la activación del Plan y que se han restablecido los 
niveles normales de seguridad y los servicios mínimos a la población.', '', '', '0'),
	('148', '33', '97', '1', 'Se declara este Nivel cuando de la información procedente de los sistemas de previsión y alerta y/o de los sistemas ordinarios de intervención se realizan medidas de
seguimiento de los parámetros que provocan la alerta y/o previsión de las zonas,
bienes y/o de la población que se pudiera ver afectada en base a la evaluación de la acción generadora y de sus posibles consecuencias.', '1', '', '1'),
	('149', '33', '98', '1', 'Se declara este Nivel cuando de la información procedente de los sistemas de
previsión y alerta y/o de los sistemas ordinarios de intervención se decide aplicar
medidas de protección a la población y/o sus bienes porque se prevea que la
evolución de los parámetros pueda causar daños significativos para la misma.', '2', '', '2'),
	('150', '33', '99', '1', 'Emergencias localizadas, controladas mediante respuesta de los medios y recursos disponibles en la zona, y/o cuya gravedad potencial puede causar daños poco significativos.', '', '0', '0'),
	('151', '33', '99', '2', 'Emergencias localizadas que requieren la concurrencia de medios y recursos asignados
al PLATERPA ajenos al área afectada y/o cuya gravedad potencial puede causar daños
significativos. ', '', '1', '1'),
	('152', '33', '99', '3', 'Emergencias que requieren la concurrencia de medios y recursos no asignados al
PLATERPA y cuya gravedad potencial y/o extensión territorial puede causar daños
significativos.', '', '2', '2'),
	('153', '33', '100', '1', 'Emergencias en las que se presenten circunstancias en las que está presente el interés
nacional con arreglo a los supuestos previstos en la Norma Básica, capítulo I, apartado
1.2.', '', '3', '3'),
	('154', '33', '101', '1', 'Fase consecutiva a la de Alerta o Emergencia que se prolongará hasta que
desaparezcan las causas generadoras de la alerta y/o hasta el restablecimiento de los
servicios públicos esenciales en las zonas afectadas por la emergencia.', '', '0', '0'),
	('155', '34', '102', '1', 'La declaración de Activación del Plan corresponde a la Dirección del Plan, en cualquiera 
de sus Fases. Se hace efectiva automáticamente de forma preventiva cada temporada, 
en los periodos.', '', '', '0'),
	('156', '34', '103', '1', 'Pertenecen los incendios forestales que pueden ser  
combatidos y controlados con los medios de extinción asignados al Plan, y que no suponen peligro para personas no relacionadas 
con las labores de extinción.', '', '0', '0'),
	('157', '34', '103', '2', 'Aquella en la que los incendios forestales existentes, requieren para su extinción, 
el concurso de medios, procedimientos y protocolos de uso habitual con la aplicación 
de operativos especiales.', '', '1', '1'),
	('158', '34', '103', '3', 'Referido a aquellos incendios para cuya extinción se prevé la necesidad incorporar 
medios extraordinarios procedentes de otra Administración, Entidad u Organismo 
Público o Privado no asignados al Plan, o puedan comportar situaciones de 
emergencia que deriven hacia el interés nacional. ', '', '2', '2'),
	('159', '34', '103', '4', 'Referido a aquellos incendios o situaciones en que habiéndose considerado que está 
en juego el interés nacional así sean declarados por el Ministerio de Interior.', '', '3', '3'),
	('160', '34', '104', '1', 'Fase consecutiva a la de Alerta o Emergencia que se prolongará hasta que 
desaparezcan las causas generadoras de la alerta y/o hasta el restablecimiento de los 
servicios públicos esenciales en las zonas afectadas por la emergencia. ', '', '', '0'),
	('161', '35', '105', '1', 'Se contemplará la fase de Preemergencia ante la existencia de informaciones proporcionadas 
por los Sistemas de Previsión y Alerta que por evolución desfavorable pudiesen dar lugar a 
inundaciones.', '', '', '0'),
	('162', '35', '106', '1', 'No implica una activación de toda la estructura del Plan, ya que se trata de emergencias 
localizadas y ordinarias que se pueden resolver con los medios y recursos asignados a la zona 
afectada.', '', '0', '0'),
	('163', '35', '106', '2', 'Se producen inundaciones en zonas generalizadas, cuya atención puede 
quedar asegurada mediante el empleo de los medios y recursos disponibles en las zonas 
afectadas', '', '1', '1'),
	('164', '35', '106', '3', 'Se han producido inundaciones que superan la capacidad de atención de los 
medios y recursos disponibles en las zonas afectadas o aún sin producirse esta última 
circunstancia. Así mismo, serán declaradas como situación 
2 aquellas emergencias en presas definidas como Escenarios 2 y 3.', '', '2', '2'),
	('165', '35', '106', '4', 'Inundaciones en las que se considera que está en juego el interés nacional y así 
sean declaradas por el Ministro del Interior', '', '3', '3'),
	('166', '35', '107', '1', 'Fase consecutiva a la de emergencia que se prolongará hasta el restablecimiento de las 
condiciones mínimas imprescindibles para un retorno a la normalidad en las zonas afectadas por 
la inundación', '', '', '0'),
	('167', '36', '108', '1', 'Referida a aquellos accidentes que pueden ser 
controlados por los medios disponibles y que, aun en su evolución 
más desfavorable, no suponen peligro.', '', '0', '0'),
	('168', '36', '109', '1', 'Accidentes que pudiendo ser 
controlados con los medios de intervención disponibles, requieren de la 
puesta en práctica de medidas para la protección.', '', '1', '1'),
	('169', '36', '109', '2', 'Accidentes que para su control, se prevé el concurso de medios 
de intervención, no asignados al plan de la Comunidad Autónoma, a 
proporcionar por la organización del plan estatal. ', '', '2', '2'),
	('170', '36', '109', '3', 'Accidentes en el transporte de 
mercancías peligrosas considerándose el 
interés nacional por el Ministro de Justicia e Interior.', '', '3', '3'),
	('171', '36', '110', '1', 'Una vez comprobado e informado por los responsables de los Grupos de Acción 
que han desaparecido o se han reducido suficientemente las causas que provocaron la activación del Plan y que se han restablecido los niveles 
normales de seguridad y los servicios mínimos a la población. ', '', '', '0'),
	('172', '37', '111', '1', 'Cuando como consecuencia de cualquiera de las hipótesis accidentales previstas,  se haya iniciado un episodio de contaminación que requiera la activación de un dispositivo de vigilancia y análisis de los parámetros que determinen su evolución para evitar y/o prever su llegada a la costa. ', '', '0', '0'),
	('173', '37', '111', '2', 'Como consecuencia de un
accidente en un buque o en una instalación industrial costera o portuaria que almacene produzca o transporte mercancías contaminantes, y esté produciendo o haya producido una contaminación leve y localizada.', '', '1', '1'),
	('174', '37', '111', '3', 'Como consecuencia de un
accidente en un buque o en una instalación industrial costera o portuaria que almacene, produzca o transporte mercancías contaminantes, esté produciendo una contaminación grave y extensa.', '', '2', '2'),
	('175', '37', '111', '4', 'Cuando las circunstancias y la envergadura de la emergencia, requieran la declaración
de Situación 3, el conjunto del PLACAMPA se integrará en el Plan Nacional, cuya
dirección corresponde al Organismo Rector. ', '', '3', '3'),
	('176', '37', '112', '1', 'Cuando la situación esté definitivamente controlada y el grado de descontaminación así lo permita.', '', '', '0'),
	('177', '38', '113', '1', 'No hay predicción de fenómenos meteorológicos adversos por nieve o temperaturas mínimas pero el Plan está activado y los medios en situación de disponibilidad.', '0', '', '0'),
	('178', '38', '113', '2', 'Nivel amarillo de Meteoalerta: No existe riesgo meteorológico para la población en general aunque sí para alguna actividad concreta', '1', '', '1'),
	('179', '38', '113', '3', 'Nivel naranja de Meteoalerta: Existe un riesgo meteorológico importante.', '2', '', '2'),
	('180', '38', '113', '4', 'Nivel rojo de Meteoalerta: El riesgo meteorológico es extremo.', '3', '', '3'),
	('181', '38', '114', '1', 'Emergencia que se han producido en zonas localizadas, cuya atención puede quedar asegurada mediante el empleo de los medios y recursos disponibles en las zonas afectadas y/o cuya gravedad potencial puede causar daños poco significativos.', '', '0', '0'),
	('182', '38', '114', '2', 'Emergencia o emergencias localizadas en las que es necesaria la movilización de medios y recursos distintos de los de las zonas afectadas pero adscritos al Plan, y/o cuya gravedad potencial puede causar daños significativos.', '', '1', '1'),
	('183', '38', '114', '3', 'Emergencias que requieren la concurrencia de medios y recursos no asignados al PLAN y cuya gravedad potencial y/o extensión territorial puede causar daños significativos.', '', '2', '2'),
	('184', '38', '114', '4', 'Emergencias en las que se den circunstancias en las que esté presente el interés nacional y así sean declaradas por el Ministerio del Interior.', '', '3', '3'),
	('185', '39', '115', '1', 'Lleva a cabo tareas de seguimiento y de evaluación de la situación, proporciona
información a las autoridades de los municipios afectados, así como al director o directora del PLATERBAL.', '', '0', '0'),
	('186', '39', '116', '1', ' Corresponde a situaciones de emergencia declarada, que pueden ser controladas con los recursos de la Comunidad Autónoma de
las Illes Balears.', '', '1', '1'),
	('187', '39', '116', '2', 'Corresponde a situaciones en las que la magnitud de la emergencia puede hacer recomendable la incorporación de recursos extraordinarios bajo la dirección de la Comunidad Autónoma. En los casos en que se incorporen recursos extraordinarios de titularidad estatal, el CECOP actuará como CECOPI.', '', '2', '2'),
	('188', '39', '116', '3', 'Aquellas situaciones en que, habiéndose considerado que está en juego el interés nacional, así sean declaradas por la persona titular del Ministerio del Interior. ', '', '3', '3'),
	('189', '40', '117', '1', ' Riesgo Bajo de conformidad con la previsión diaria 
establecida en el Mapa de niveles de riesgo de incendio previstos. Se considerará 
en esta fase nivel Bajo de riesgo de incendio forestal.', '', '', '0'),
	('190', '40', '118', '1', 'Cuando el estado en que se 
encuentran los combustibles forestales origina una probabilidad de 
ignición y unas condiciones favorables para la propagación de los incendios 
forestales.', '', '', '0'),
	('191', '40', '119', '1', 'Situación de emergencia provocada por uno o varios incendios forestales que  puedan afectar sólo a bienes de naturaleza 
forestal.', '', '0', '0'),
	('192', '40', '119', '2', 'Situación de emergencia provocada por uno o varios incendios forestales que puedan afectar gravemente a bienes, y puedan ser controlados con los medios y recursos del INFOBAL, o para cuya 
extinción pueda ser necesario que sean incorporados medios extraordinarios.', '', '1', '1'),
	('193', '40', '119', '3', 'Situación de emergencia provocada por uno o varios incendios forestales que puedan afectar gravemente a la población y bienes, exigiendo la adopción inmediata de medidas de protección
y socorro.', '', '2', '2'),
	('194', '40', '119', '4', 'Situación de emergencia correspondiente y consecutiva a la declaración de 
emergencia de interés nacional por el Ministro de Interior', '', '3', '3'),
	('195', '40', '120', '1', 'Una vez 
extinguido el incendio forestal hasta el restablecimiento de las 
condiciones mínimas imprescindibles para un retorno a la normalidad en las 
zonas afectadas por cada incendio forestal. ', '', '', '0'),
	('196', '41', '121', '1', 'Se enfoca en el aviso a las autoridades competentes y servicios, así como informar y sensibilizar a la población ante un posible riesgo.', '', '', '0'),
	('197', '41', '122', '1', 'Se llevará a cabo por las autoridades municipalesmediante a Aplicación del Plan de Acción Municipal, siempre que el municipio disponga de éste. En caso de no tener, será la CCAA de Islas Baleares, a través de la Dirección General de Emergencias e Interior, quien gestione la emergencia.', '', '0', '0'),
	('198', '41', '122', '2', 'Se llevará a cabo por las autoridades municipales mediante a Aplicación del Plan de Acción Municipal, siempre que el municipio disponga de éste y tenga capacidad suficiente para gestionarla.', '', '1', '1'),
	('199', '41', '122', '3', 'Situación de emergencia provocada por inundaciones  que puedan afectar gravemente a la población y sus bienes, exigiendo la adopción inmediata de medidas de protección.', '', '2', '2'),
	('200', '41', '122', '4', 'Situación de emergencia, que habiendose considerado que está en juego el interés nacional, es declarada por el Ministro de Interior', '', '3', '3'),
	('201', '41', '123', '1', 'Fase consecutiva a la de emergencia y, se mantiene hasta el restablecimiento de las condiciones
mínimas imprescindibles para un retorno a la normalidad en las zonas afectadas por la inundación.', '', '', '0'),
	('202', '42', '124', '1', 'Aunque puedan producirse aspectos perceptibles para la población, no requieren medidas de intervención que la de información, salvo para ciertos grupos de personas cuyo estado pueda hacerlas especialmente vulnerables y puedan requerir medidas de protección específica', '', '', '0'),
	('203', '42', '125', '1', 'Pueden ser controlados con los medios disponibles y que no suponen peligro para personas
no relacionadas con las labores de intervención, ni para el medio ambiente, ni para bienes.', '', '0', '0'),
	('204', '42', '125', '2', 'Pudiendo ser controlados con los medios de intervención disponibles, requieren de la puesta en práctica de medidas para la protección.', '', '1', '1'),
	('205', '42', '125', '3', 'Accidentes que para su control o la puesta en práctica de las  medidas necesarias de protección de las personas, los bienes y/o el medio ambiente se prevé el concurso de medios de intervención no asignados al Plan de la Comunidad Autónoma de Las Illes Balears y se deban proporcionar por la organización del
Plan Estatal.', '', '2', '2'),
	('206', '42', '125', '4', 'Accidentes que, habiéndose considerado que está implicado el interés nacional, así sean declarados por el Ministro del Interior', '', '3', '3'),
	('207', '42', '126', '1', 'La emergencia no se dará por finalizada y el incidente como tal no será cerrado en los programas de gestión de emergencias hasta que no sean recogidos los
residuos contaminantes provocados por el accidente de la calzada o sus alrededores, no obstante se podrá decretar por parte del director del PMA la retirada de los
recursos no necesarios para la gestión y recogida de los citados residuos contaminantes.', '', '', '0'),
	('208', '43', '127', '1', ' Situación de emergencia en la que los riesgos se limitan a la propia instalación y pueden ser controlados por los medios disponibles en el 
correspondiente plan de emergencia interior o plan de autoprotección.', '', '0', '0'),
	('209', '43', '128', '1', 'Situación de emergencia en la que se prevé que los riesgos, no puedan ser 
controladas únicamente con los recursos propios del plan de emergencia interior 
o del plan de autoprotección, siendo necesaria la intervención de servicios del 
Plan Autonómico.', '', '1', '1'),
	('210', '43', '128', '2', 'Accidentes que para su control o la puesta en práctica de medidas de 
protección de las personas se prevé el concurso de medios de apoyo de 
titularidad estatal, no asignados al Plan Autonómico.', '', '2', '2'),
	('211', '43', '128', '3', ' Situación de emergencia en la que la naturaleza, gravedad o alcance 
de los riesgos requiere la declaración del interés nacional por el Ministro del 
Interior.', '', '3', '3'),
	('212', '43', '129', '1', 'Cuando la situación está controlada, bien porque ha desaparecido la causa que la 
originó, bien porque no se prevén más emisiones de sustancias radiactivas al 
exterior y se hayan aplicado todas las medidas de protección y actuaciones de 
emergencia necesarias se podrá declarar el fin de la emergencia.', '', '', '0'),
	('213', '43', '130', '1', 'Declarado el final de la emergencia, se inicia la fase de recuperación. Esta fase 
comprende todas aquellas actuaciones encaminadas a recuperar las condiciones 
normales de vida en las zonas afectadas.', '', '', '0'),
	('214', '44', '131', '1', 'Requiere una actuación coordinada de las 
autoridades y Órganos competentes.
Garantiza la información a los ciudadanos 
sobre dichos fenómenos.', '', '0', '0'),
	('215', '44', '132', '1', 'Se han producido fenómenos sísmicos.
La protección de personas y bienes, puede 
quedar asegurada mediante el empleo de los 
medios y recursos disponibles en las zonas 
afectadas.', '', '1', '1'),
	('216', '44', '132', '2', 'Se han producido fenómenos sísmicos graves.
Los daños ocasionados, el número de víctimas o la extensión de las áreas afectadas, hacen 
necesarios medios, recursos o servicios ubicados 
fuera de las zonas afectadas.', '', '2', '2'),
	('217', '44', '132', '3', 'Emergencias declaradas de interés nacional 
por el Ministro de Interior.', '', '3', '3'),
	('218', '44', '133', '1', 'Se realizan las primeras tareas de rehabilitación de las zonas afectadas.', '', '4', '0'),
	('219', '45', '134', '1', 'Fase en la cual se permanece en estado de alerta por si se produce una situación que pueda dar lugar a una situación de riesgo.El objeto de esta fase es alertar a las autoridades y servicios implicados,
así como informar a la población potencialmente afectada.', 'Verde', '', '0'),
	('220', '45', '135', '1', 'La emergencia se calificará así cuando la información meteorológica
permita prever la inminencia de un FMA habitual pero, potencialmente peligroso, para alguna actividad concreta y se corresponderá también
con fenómenos adversos observados que puedan considerarse como incidentes
locales menores y controlables mediante una respuesta local y rápida, sin producirse daños.', 'Amarillo', '0', '0'),
	('221', '45', '135', '2', 'Probabilidad elevada de riesgo, y se declarará este nivel en aquella
situación en la que se ha producido un FMA en zonas localizadas, cuya atención puede quedar asegurada mediante el empleo de los medios y recursos
disponibles en las zonas afectadas pero cuyo seguimiento debe ser supramunicipal.', 'Naranja', '1', '1'),
	('222', '45', '135', '3', 'Situación en la que se ha producido un FMA no habitual de intensidad
excepcional que supera la capacidad de atención de los medios y recursos
locales. Puede requerir entonces la constitución del CECOP/CECOPI y la
activación total del Plan', 'Rojo', '2', '2'),
	('223', '45', '135', '4', 'Emergencias en las que se declare el interés
nacional o se requiera la intervención de recursos extraordinarios de la
Administración del Estado o de otras Comunidades Autónomas.', 'Rojo', '3', '3'),
	('224', '45', '136', '1', 'Durante esta fase se realizarán las primeras tareas de rehabilitación en
las zonas afectadas.', '', '', '0'),
	('225', '46', '137', '1', ' Se ha producido a más de 12 millas de la costa un accidente en un buque
que transporta materias contaminantes y, como consecuencia de este accidente,
puede producirse contaminación costera o en aguas adyacentes. 
- Se ha producido un accidente en una instalación costera y este accidente
puede ser resuelto por la misma instalación sin afectar al mar. 
- Cuando a través del cauce de un torrente, alcantarillado u otro conducto
que desemboque en el mar, pueda llegar producto contaminante. ', '', '', '0'),
	('226', '46', '138', '1', 'Accidentes en instalaciones que almacenan, manipulan, producen o transportan estos materiales, especialmente si ocurre a menos de doce millas de la costa. También, accidentes en tierra que resultan en contaminación química leve en el mar, sin riesgo para la tierra.', '', '', '0'),
	('227', '46', '139', '1', 'Cuando se trata de una contaminación en el mar que puede afectar o afecta a tierra, en una zona localizada
o en una zona vulnerable o en ambas.', '1', '', '1'),
	('228', '46', '139', '2', 'Cuando la contaminación
puede afectar o afecta a una franja de tierra muy extensa o a una zona especialmente vulnerable o a ambas.', '2', '', '2'),
	('229', '46', '140', '1', 'El director o la directora del Plan tiene que decretar el fin de la emergencia sobre la base de las recomendaciones del Consejo Asesor, cuando esté controlado y eliminado el origen de la emergencia y minimizadas las consecuencias del accidente.', '', '', '0'),
	('230', '47', '141', '1', 'Se tienen que llevar a cabo tareas de
mantenimiento de las instalaciones desalinizadoras. Asimismo, se tiene que mantener un mínimo de agua desalada para satisfacer la demanda.', '', '', '0'),
	('231', '47', '142', '1', 'Trabajos necesarios para poner a punto todas las líneas de producción de las desalinizadoras que puedan aportar agua a las unidades de demanda
que estén en esta situación.', '', '', '0'),
	('232', '47', '143', '1', 'En unidades de demanda con
acceso a agua desalada, el Gobierno tiene que garantizar que las plantas desalinizadoras puedan funcionar a pleno rendimiento.', '', '', '0'),
	('233', '47', '144', '1', 'En unidades de demanda con
acceso a agua desalada, el Gobierno tiene que garantizar que las plantas desalinizadoras puedan funcionar a pleno rendimiento.', '', '', '1'),
	('234', '48', '145', '1', 'Se estima que no existe riesgo para la población en general aunque si para alguna
actividad concreta o localización de alta vulnerabilidad. Remitido a través de los medios que se estimen oportunos a los Organismos y Entidades del Plan.', '', '', '0'),
	('235', '48', '146', '1', 'Se estima que existe un riesgo importante (fenómenos no habituales y con cierto
grado de peligro para las actividades usuales). Remitido a través de los medios que se estimen oportunos a los Organismos y Entidades del Plan.', '', '', '0'),
	('236', '48', '147', '1', 'Se estima que el riesgo es extremo (fenómenos no habituales, de intensidad
excepcional y con un nivel de riesgo para la población muy alto). Remitido a través de los medios que se estimen oportunos a los Organismos y Entidades del Plan.', '', '', '0'),
	('237', '48', '148', '1', 'Se considera una emergencia de Nivel Municipal aquella que afecta exclusivamente a
un territorio municipal. Las emergencias a nivel municipal están controladas
mediante la movilización de medios y recursos locales, independiente de la
titularidad de los medios y recursos movilizados.', 'Municipal', '', '0'),
	('238', '48', '148', '2', 'Se considera una emergencia de Nivel Insular cuando se prevea que no pueda o no
puede ser controlada con los medios y recursos adscritos a los Planes Municipales.
En este caso, se activa el Plan de Emergencias Insular (PEIN) que materializará la
intervención de los medios y recursos propios o asignados.', 'Insular', '', '1'),
	('239', '48', '148', '3', 'Se consideran emergencias de Nivel Autonómico las que no pueden ser gestionadas
con los medios insulares y requieran la plena movilización de la estructura
organizativa y de los medios y recursos asignados y no asignados e incluso
particulares. ', 'Autonómico', '', '2'),
	('240', '48', '148', '4', 'Se consideran emergencias de este nivel aquéllas en las que esté presente el interés
nacional de acuerdo con el Capítulo IV de la Norma Básica de Protección Civil.', 'Estatal', '', '3'),
	('241', '48', '149', '1', 'Cuando la emergencia esté plenamente controlada y no exista condición de riesgo
para las personas, el Director/a del Plan declarará formalmente el fin de la
emergencia, sin perjuicio de lo establecido en los puntos anteriores respecto de la
desactivación de los diferentes niveles considerados.', '', '', '0'),
	('242', '49', '150', '1', 'No requerirá ninguna notificación especial por parte del CECOES 1-1-2. Los planes de actuación local 
(insulares, municipales), así como los planes de autoprotección estarán activados con el dispositivo 
establecido.', '', '', '0'),
	('243', '49', '151', '1', 'Deberán activarse los mecanismos para la actualización de la información e iniciarse 
las tareas de preparación que permitan disminuir los tiempos de respuesta ante una posible 
intervención establecidos por parte de los órganos correspondientes las medidas limitativas y 
prohibitivas para reducir el riesgo de incendio forestal.', '', '', '0'),
	('244', '49', '152', '1', 'Situación de emergencia provocada por uno o varios incendios forestales que, en su evolución más 
desfavorable, se prevé sólo la afectación a bienes de naturaleza forestal. Esta emergencia podrá ser 
controlada con los medios y recursos propios del Operativo Insular correspondiente, medios 
especializados en materia de protección contra incendios forestales del Gobierno de Canarias o medios 
de la Administración General del Estado.', '0', '', '0'),
	('245', '49', '152', '2', 'Situación de emergencia provocada por uno o varios incendios forestales que en su evolución más 
desfavorable, se prevé su afectación grave a bienes forestales y, en su caso, afectación leve a la 
población y bienes. Además de los medios y recursos incorporados en el Nivel 0, se podrán incorporar medios de otros operativos insulares o de 
otros recursos del Gobierno de Canarias en materia de PC, a solicitud del órgano competente de la 
Comunidad Autónoma. Así mismo se podrán incorporar medios extraordinarios del Estado.', '1', '', '1'),
	('246', '49', '152', '3', 'Situación de emergencia provocada por uno o varios incendios forestales que, en su evolución más 
desfavorable, se prevé su afectación grave a la población y bienes, exigiendo medidas de protección y socorro.Además de 
los medios y recursos incorporados en el Nivel 1, puede ser necesario que sean incorporados medios extraordinarios del Estado, o 
puedan comportar situaciones que deriven hacia el interés nacional. ', '2', '', '2'),
	('247', '49', '152', '4', 'Situación de emergencia en la que, habiéndose considerado que está en juego el interés nacional, así 
sea declarada por el Ministro del Interior.', '3', '', '3'),
	('248', '49', '153', '1', 'Una vez controlado el incendio, corresponde al Director/a de Extinción determinar el número y 
distribución de medios que han de hacer las labores de liquidación y vigilancia de la zona afectada 
para evitar que el incendio se reproduzca. ', '', '', '0'),
	('249', '50', '154', '1', 'Se estima que existe riesgo de inundación bajo o moderado causado por fenómenos
meteorológicos habituales potencialmente peligrosos sobre una actividad concreta o
localización de alta vulnerabilidad, pero no existe riesgo por rotura de presa o
embalse, ni existe riesgo para la población en general.', '', '', '0'),
	('250', '50', '155', '1', 'Se estima que existe riesgo de inundación alto causado por fenómenos meteorológicos
no habituales y/o riesgo grave por rotura de presa o embalse, con cierto grado de
peligro para las actividades usuales de la población.', '', '', '0'),
	('251', '50', '156', '1', 'Se estima que existe riesgo de inundación muy alto causado por fenómenos
meteorológicos no habituales de intensidad excepcional y/o existe un riesgo
inminente de rotura de presa o embalse con el consecuente peligro para la población.', '', '0', '0'),
	('252', '50', '157', '1', 'Se considera una emergencia de nivel municipal cuando afectando al municipio, pueda
ser controlada o se prevea que pueda ser controlada con medios y recursos locales.', '', '1', '1'),
	('253', '50', '157', '2', 'Se considera una emergencia de Nivel Insular cuando la inundación afecte a más de un
municipio de la isla, o cuando afectando a un sólo municipio de la isla, se prevea que
no pueda ser controlada con los medios y recursos adscritos al Plan Municipal.', '', '1', '1'),
	('254', '50', '157', '3', 'Se considera una emergencia de Nivel Autonómico cuando las inundaciones afecten a
más de una isla, o cuando afectando a una sola isla no pueda, o se prevea, que no se
gestiona la emergencia con los medios insulares.', '', '2', '2'),
	('255', '50', '157', '4', 'Cuando la emergencia no pueda ser atendida con los medios locales, insulares y
autonómicos, o por interés nacional, se podrá declarar el nivel nacional.', '', '3', '3'),
	('256', '50', '158', '1', 'Situación consecutiva a la de emergencia que se prolongará durante el
restablecimiento de las condiciones mínimas imprescindibles para el retorno a la normalidad en las zonas afectadas por la emergencia.', '', '', '0'),
	('257', '51', '159', '1', 'Situaciones que pueden ser controlados por los medios disponibles y que, en su evolución más desfavorable, no suponen peligro para población, ni para 
bienes.', '', '', '0'),
	('258', '51', '160', '1', 'Accidentes que pudiendo ser controlados con los medios de 
intervención disponibles, requieren de la puesta en práctica de medidas para la protección 
de las personas, bienes o el medio ambiente que estén o que puedan verse amenazados por 
los efectos derivados del accidente.', '', '', '0'),
	('259', '51', '161', '1', 'Corresponde a aquellos accidentes que para su control es necesaria la movilización de la estructura del Plan y de todos los medios y recursos asignados o no al Plan de la Comunidad 
Autónoma y la puesta en práctica de las necesarias medidas de protección de las personas, 
los bienes o el medio ambiente.', 'Municipal- Insular Autonómico', '', '1'),
	('260', '51', '161', '2', 'Corresponde a aquellos accidentes que para su control es necesaria la movilización de la estructura del Plan y de todos los medios y recursos asignados o no al Plan de la Comunidad 
Autónoma y la puesta en práctica de las necesarias medidas de protección de las personas, 
los bienes o el medio ambiente.', 'Municipal- Insular Autonómico', '', '2'),
	('261', '51', '161', '3', 'Accidentes, que habiéndose considerado que está implicado el interés nacional, así sean declarados por el Ministro 
de Interior. Se identifica con la Situación 3 a efectos de su interconexión con la normativa 
estatal.', '3', '3', '3'),
	('262', '51', '162', '1', 'Cuando la emergencia esté plenamente controlada y no exista condición de riesgo para 
las personas, el Director del Plan declarará formalmente el fin de la emergencia, sin perjuicio de lo establecido en los puntos anteriores respecto de la desactivación de los diferentes 
niveles considerados.', '', '', '0'),
	('263', '52', '163', '1', ' En instalaciones con Plan de Emergencia Interior o Plan de Autoprotección, los riesgos
limitados a la propia instalación y que pueden ser controlados por los medios disponibles en
el correspondiente Plan de Emergencia interior o Plan de Autoprotección.', '', '', '0'),
	('264', '52', '164', '1', 'Si la dirección lo considera necesario, se avisará a los elementos vulnerables que se puedan ver afectados por una evolución desfavorable de la situación y se tomarán las medidas necesarias en el caso de que hubiera personas especialmente vulnerables a las radiaciones ionizantes: niños, lactantes y mujeres embarazadas.', '', '', '0'),
	('265', '52', '165', '1', 'Nivel de emergencia en la que los riesgos se limitan a la propia instalación y pueden ser controlados por los medios disponibles en el correspondiente plan de emergencia interior o plan de
autoprotección.', '0', '', '0'),
	('266', '52', '165', '2', 'Nivel de emergencia en la que se prevé que los riesgos pueden afectar a las personas en el interior de la instalación, mientras que las repercusiones en el exterior, no pueden ser
controladas únicamente con los recursos propios del plan de emergencia interior o del plan de
autoprotección, siendo necesaria la intervención de servicios del Plan Autonómico.', '1', '', '1'),
	('267', '52', '165', '3', 'Nivel de emergencia en la que se prevea que los riesgos pueden afectar a las personas tanto en el
interior como en el exterior de la instalación y, en consecuencia, se prevé el concurso de medios de
apoyo de titularidad estatal no asignados al Plan Autonómico.', '2', '', '2'),
	('268', '52', '165', '4', 'Declarada de interés nacional, el Consejero/a competente en
materia de protección civil y atención de emergencias del Gobierno de Canarias (Director/a del Plan), designará la autoridad que junto a la correspondiente por parte de la administración estatal,
constituya el Comité de Dirección.', '3', '', '3'),
	('269', '52', '166', '1', 'Es el periodo que se inicia cuando se ha declarado el final de la fase de emergencia y comprende todas aquellas actuaciones encaminadas a recuperar las condiciones normales de vida en las zonas
afectadas.', '', '', '0'),
	('270', '53', '167', '1', 'Se estima que no existe riesgo para la población en general aunque si para
alguna actividad concreta o localización de alta vulnerabilidad.', '', '', '0'),
	('271', '53', '168', '1', 'Se estima que existe un riesgo importante (fenómenos no habituales y con
cierto grado de peligro para las actividades usuales). 
La declaración de esta situación se remitirá a través de los medios que se
estimen oportunos a los Organismos y Entidades del Plan.', '', '', '0'),
	('272', '53', '169', '1', 'Se realizará con una predicción a muy corto plazo y es una acción que tiene por objeto inducir de forma inmediata al que la recibe a tomar
medidas que le protejan de los riesgos o sucesos catastróficos que le amenacen. ', '', '', '0'),
	('273', '53', '170', '1', 'Emergencia que se identifica cuando, aún produciéndose un terremoto
ampliamente sentido por la población esta discurre sin existir importantes
riesgos para la población, las infraestructuras o el medio ambiente. Y puede
quedar asegurada mediante el empleo de los medios y recursos disponibles en
las zonas afectadas.', '0', '', '0'),
	('274', '53', '170', '2', 'Emergencia que se identifica cuando, aún produciéndose un terremoto
ampliamente sentido por la población esta discurre sin existir importantes
riesgos para la población, las infraestructuras o el medio ambiente. Y puede
quedar asegurada mediante el empleo de los medios y recursos disponibles en
las zonas afectadas.', '1', '', '1'),
	('275', '53', '171', '1', 'El nivel 2 implica que se ha producido un terremoto y que por la gravedad de lo
daños ocasionados, el número de víctimas o la extensión de las áreas
afectadas, hacen necesario, para el socorro y protección de personas y bienes,
el concurso de medios, recursos o servicios ubicado fuera de dichas áreas.', '2', '', '2'),
	('276', '53', '171', '2', 'Se han producido fenómenos sísmicos que por su gravedad se ha considerado
que está en juego el interés nacional, habiéndose declarado así por el Ministro
de Interior.', '3', '', '3'),
	('277', '53', '172', '1', 'Cuando la emergencia esté plenamente controlada y no exista condición de
riesgo para las personas, el Director/a del Plan declarará formalmente el fin de
la emergencia, sin perjuicio de lo establecido en los puntos anteriores respecto
de la desactivación de los diferentes niveles considerados.', '', '', '0'),
	('278', '54', '173', '1', 'Cuando ocurre la situación de prealerta, como predicción de procesos eruptivos a
medio plazo, deben dirigirse comunicaciones a la población y a los órganos del Plan capaz de inducir un estado de atención y
vigilancia sobre las circunstancias que la provocan. Deben llevar implícitas las tareas de preparación con el objeto de disminuir los tiempos de respuesta para una rápida
intervención y mantenerse atentos a la recepción de nuevas informaciones.', '', '', '0'),
	('279', '54', '174', '1', 'En esta situación deberán activarse los mecanismos para la actualización de la
información e iniciarse las tareas de preparación que permitan disminuir los tiempos
de respuesta ante una posible intervención. Se establecerán por parte de los órganos
correspondientes las medidas limitativas y prohibitivas para reducir el riesgo volcánico.', '', '', '0'),
	('280', '54', '175', '1', 'Traerá consigo el cambio al semáforo naranja,
estando todos los servicios operativos listos, emitiéndose avisos y comunicados a las
instituciones que corresponda y orientaciones de autoprotección a través de los
medios establecidos por el Gabinete de Información. Se hace llegar a las poblaciones
de riesgo directo las órdenes de inicio de la evacuación preventiva si pueden verse
afectadas por la erupción', '', '', '0'),
	('282', '54', '177', '1', 'Emergencia que se identifica cuando, aún existiendo erupción volcánica ésta discurre
sin existir importantes riesgos para la población, las infraestructuras o el medio
ambiente. ', '0', '', '0'),
	('283', '54', '177', '2', 'Emergencia que se identifica cuando, aún existiendo erupción volcánica ésta discurre
sin existir importantes riesgos para la población, las infraestructuras o el medio
ambiente. ', '1', '', '0'),
	('284', '54', '177', '3', 'El nivel 2 refleja un aumento en la gravedad de la situación, con graves afectaciones o
incremento del riesgo para la población. En esta situación, se pueden producir efectos
derivados sobre sectores de población relativamente alejados, ya sea por los efectos
de incendios forestales donde se ha perdido el control o por la acción de las cenizas.', '2', '', '2'),
	('285', '54', '177', '4', 'El establecimiento de la situación de emergencia a Nivel 3, implica que existen unas
condiciones de elevada peligrosidad que pueden cubrir extensas zonas en las que no
es posible asegurar la vida de las personas con los recursos previstos en el Plan.', '3', '', '3'),
	('286', '54', '178', '1', 'Cuando la emergencia esté plenamente controlada y no exista condición de riesgo
para las personas, el Director/a del Plan declarará formalmente el fin de la emergencia,
sin perjuicio de lo establecido en los puntos anteriores respecto de la desactivación de
los diferentes niveles considerados.', '', '', '0'),
	('287', '55', '179', '1', 'En aquellos casos en que los efectos del accidente sean perceptibles por la población, la actuación del 
RISQCAN se limitará a una labor de información, seguimiento y apoyo del PEMU o Plan de Actuación frente al Riesgo Químico del Municipio. Se corresponde a accidentes de categoría 1.', '', '1', '1'),
	('288', '55', '180', '1', 'Aquellos para los que se prevea, como consecuencias, posibles víctimas y daños 
materiales en el establecimiento; mientras que las repercusiones exteriores se limitan a daños 
leves o efectos adversos sobre el medio ambiente en zonas limitadas. Se corresponden a accidentes de categoría 2.', '', '2', '2'),
	('289', '55', '181', '1', 'Aquellos para los que se prevea, como consecuencias, posibles víctimas, daños 
materiales graves o alteraciones graves del medio ambiente en zonas extensas y en el exterior 
del establecimiento. Se corresponde a accidentes de categoría 3.', '', '3', '3'),
	('290', '55', '182', '1', 'Situación operativa que se prolongará durante el
restablecimiento de las condiciones mínimas imprescindibles para el retorno a la normalidad en las zonas afectadas por la emergencia.', '', '', '0'),
	('291', '56', '183', '1', 'Se estima que no existe riesgo meteorológico para la población en general, aunque si 
para alguna actividad concreta o localización de alta vulnerabilidad (fenómenos 
meteorológicos habituales pero potencialmente peligrosos). A través de los medios que se estimen 
oportunos a los Organismos y Entidades del Plan.', '', '', '0'),
	('292', '56', '184', '1', 'Se estima que existe un riesgo meteorológico importante (fenómenos meteorológicos 
no habituales y con cierto grado de peligro para las actividades usuales).A través de los medios que se estimen 
oportunos a los Organismos y Entidades del Plan.', '', '', '0'),
	('293', '56', '185', '1', 'Se estima que el riesgo meteorológico es extremo (fenómenos meteorológicos no 
habituales, de intensidad excepcional y con un nivel de riesgo para la población muy 
alto).A través de los medios que se estimen 
oportunos a los Organismos y Entidades del Plan.', '', '', '0'),
	('294', '56', '186', '1', 'Se considera una emergencia de nivel municipal cuando afecta a un solo municipio y 
pueda ser controlada o se prevea que pueda ser controlada con medios y recursos 
locales. Los Municipios afectados por la declaración de la Nivel Municipal activarán su 
PEMU en dicho Nivel constituyendo el CECOPAL.', 'Municipal', '', '0'),
	('295', '56', '187', '1', 'Se considera una emergencia de Nivel Insular cuando el FMA afecte a más de un 
municipio de la isla, o cuando afectando a un sólo municipio de la isla, se prevea que 
no pueda o no puede ser controlada con los medios y recursos adscritos al Plan 
Municipal. ', 'Insular', '', '1'),
	('296', '56', '186', '2', 'Se considera una emergencia de Nivel Autonómico cuando el FMA afecte a más de una 
isla, o cuando afectando a una sola isla no pueda, o se prevea, que no se gestiona la 
emergencia con los medios insulares.', 'Autonómico', '', '2'),
	('297', '56', '186', '3', 'Cuando la emergencia no pueda ser atendida con los medios locales, insulares y 
autonómicos, o por interés nacional, se podrá declarar el nivel nacional.', 'Estatal', '', '3'),
	('298', '56', '188', '1', 'Situación consecutiva a la de emergencia que se prolongará durante el  restablecimiento de las condiciones mínimas imprescindibles para el retorno a la normalidad en las zonas afectadas por la emergencia. 
Durante esta situación se realizarán las primeras tareas de rehabilitación en dichas 
zonas.', '', '', '0'),
	('299', '57', '189', '1', 'Estas situaciones se definen como el incidente cuyas consecuencias se limitan al interior de la empresa 
siniestrada y que puede ser controlado con los recursos previstos en el Plan de Emergencia Interior/Plan 
de Autoprotección sin necesidad de movilizar recursos externos de emergencia. ', '', '', '0'),
	('300', '57', '190', '1', 'Estas situaciones se definen como el accidente cuyas consecuencias se limitan al interior de la empresa 
siniestrada y que puede ser controlado con los recursos previstos en el Plan de Emergencia Interior/Plan  de Autoprotección, más la movilización de un número reducido de vehículos de intervención y/o de  dotaciones sanitarias y no suponen peligro para población, ni para bienes distintos a la propia instalación 
en la que se ha producido el accidente.', '', '', '0'),
	('301', '57', '191', '1', 'Accidentes para los que se prevea, como consecuencias, posibles víctimas y daños materiales en el establecimiento, mientras que las repercusiones exteriores se limitan a daños leves o efectos adversos sobre el medio ambiente en zonas limitadas. Aplicables una serie de medidas de protección para los bienes y personas. ( Nivel Municipal).', '0', '', '0'),
	('302', '57', '192', '1', 'Accidentes para los que se prevea, como consecuencias, posibles 
víctimas y daños materiales en el establecimiento, mientras que las repercusiones exteriores se limitan a 
daños leves o efectos adversos sobre el medio ambiente en zonas limitadas. (Nivel Insular)', '1', '', '1'),
	('303', '57', '191', '2', 'Accidentes clasificados como Categoría 3 para los que se prevea, como consecuencias, posibles víctimas,  daños materiales graves o alteraciones graves del medio ambiente en zonas extensas y en el exterior del  establecimiento. Estarán activados los correspondientes PEMU’s para el ejercicio de las acciones más cercanas a la población y el PEIN afectado. (Nivel Autonómico).', '2', '', '2'),
	('304', '57', '191', '3', 'Corresponde a aquellos accidentes de Categoría 3 habiéndose considerado que está implicado el interés  nacional, sean declarados por el Ministro de Interior. (Nivel Nacional)', '3', '', '3'),
	('305', '58', '193', '1', 'Responde a previsiones de posibles emergencias no manifestadas, pero que en caso de una desvolución desfavorable, es posible su desencadenamiento.', '', '', '0'),
	('306', '58', '194', '1', 'Emergencia que tiene una afectación del territorio limitada con población, bienes o medio ambiente en la que se requeriere una respuesta coordinada, y una activación de medios y recursos propios o asignados al plan. ', '', '1', '1'),
	('307', '58', '194', '2', 'Emergencia que tiene una afectación del territorio intensa a la población, bienes o medio ambiente en la que puede requerirse una respuesta coordinada, y un requieran una plena organización de la estructura organizativa y de los medios y recursos asignados ao no asiganados, e incluso particulares.', '', '2', '2'),
	('308', '58', '194', '3', 'Situaciones en las que esté presente el interés nacional conforme a lo previsto en la norma básica de protección civil.', '', '3', '3'),
	('309', '58', '195', '1', 'La emergencia ha sido acabada sin que existan significativas posibilidades de su reactivación. Sien embargo, se seguirá con la vigilancia preventiva, y las labores de rehabilitación.', '', '', '0'),
	('310', '59', '196', '1', 'Pertenecen a este nivel los incendios forestales que
pueden ser eficazmente combatidos y controlados con los medios de extinción ordinarios previstos y que, aún en su evolución mas desfavorable, no suponen ningún peligro para las personas ajenas a las labores de extinción, ni para los bienes distintos a los de naturaleza forestal.', '0', '', '0'),
	('311', '59', '196', '2', 'Incendios que pudiendo ser controlados con los
medios de extinción ordinarios previstos en el Plan, se
prevé, por su posible evolución, la necesidad de la puesta
en práctica de medidas de protección de las personas y
de los bienes que puedan verse amenazados por el fuego.', '1', '', '1'),
	('312', '59', '196', '3', 'Incendio para cuya extinción se prevé la necesidad de que, a solicitud del Director del Plan, sean incorporados medios estatales no asignados al Plan o pueda comportar situaciones
de emergencia que deriven hacia el interés nacional.', '2', '', '2'),
	('313', '59', '196', '4', 'Serán de Nivel 3 aquellos incendios que, por considerarse que está en juego el interés nacional, así sean
declarados por el Ministro del Interior.', '3', '', '3'),
	('314', '59', '197', '1', 'Cuando la emergencia esté plenamente controlada el Director del Plan en cada supuesto podrá declarar
el fin de la emergencia.', '', '', '0'),
	('315', '60', '198', '1', 'El objetivo de esta fase es la alerta de las autoridades y servicios implicados en el plan
correspondiente, así como la Información a la población potencialmente afectada.
Se caracteriza por la existencia de informaciones hidrológicas y meteorológicas que, por evolución
desfavorable, pudiesen dar lugar a inundaciones a medio y corto plazo.', '', '', '0'),
	('316', '60', '199', '1', 'El objetivo de esta fase es la alerta de las autoridades y servicios implicados en el plan
correspondiente, así como la Información a la población potencialmente afectada.
Se caracteriza por la existencia de informaciones hidrológicas y meteorológicas que, por evolución
desfavorable, pudiesen dar lugar a inundaciones a muy corto plazo.', '', '', '0'),
	('317', '60', '200', '1', 'Pertenecen a este nivel los incidentes por inundaciones de ámbito municipal, muy localizados y de
escaso desarrollo que pueden ser controlados con los medios y recursos de carácter local, y que,aún en su evolución mas desfavorable, no suponen ningún peligro para las personas y las
viviendas afectadas. ', '0', '', '0'),
	('318', '60', '200', '2', 'Cuando existan inundaciones en las que la respuesta local sea insuficiente
para controlar la situación y aquellas otras que afecten a más de un municipio y sea necesaria una
coordinación superior de los servicios actuantes. Se pondrán en prácticas, medidas para la protección.', '1', '', '1'),
	('319', '60', '200', '3', 'Este nivel será declarado en aquellas emergencias que por la gravedad y/o extensión del riesgo y sus efectos se requiere la plena movilización de la estructura organizativa y de los medios y recursos asignados y no asignados e incluso particulares. Asimismo, cuando exista una inundación en la que para su mitigación se prevea la necesidad de que, a solicitud del Director del Plan, sean incorporados medios estatales no asignados al Plan o pueda comportar situaciones de
emergencia que deriven hacia el interés nacional.', '2', '', '2'),
	('320', '60', '200', '4', 'Inundaciones que, por considerarse que esta en juego el interés
nacional, así sean declaradas por el Ministerio del Interior.', '3', '', '3'),
	('321', '60', '201', '1', 'Fase subsiguiente a la de emergencia que se prolongará hasta que se produzca el
restablecimiento de las condiciones mínimas imprescindibles para el retorno a la normalidad en
las zonas afectadas por la inundación. ', '', '', '0'),
	('322', '61', '202', '1', 'Accidentes que pueden ser controlados por los medios disponibles y que, aun en su evolución más desfavorable, no suponen peligro
para las personas no relacionadas con las labores de intervención, ni para el medio ambiente, ni para bienes distintos a la propia red viaria en la que se ha producido el accidente.', '0', '', '0'),
	('323', '61', '202', '2', 'Se activa el Nivel 1 cuando existan accidentes que pudiendo ser controlados con los medios de intervención
disponibles, requieren de la puesta en práctica de medidas para la protección de las personas, los bienes o el
 medio ambiente que estén o puedan verse amenazadas
por los efectos derivados del accidente y requieren la activación del TRANSCANT.', '1', '', '1'),
	('324', '61', '202', '3', 'El Nivel 2 corresponde a aquellos accidentes que por su gravedad y/o extensión del riesgo y sus efectos requieren
la plena movilización de la estructura organizativa del Plan
y de otros medios y recursos no asignados, a proporcionar por la Administración del Estado.', '2', '', '2'),
	('325', '61', '202', '4', 'Este Nivel será declarado cuando se notifique un accidente en el que esté presente el interés nacional, con arreglo a los supuestos previstos en la Norma Básica de
Protección Civil.', '3', '', '3'),
	('326', '61', '203', '1', 'Sin perjuicio de lo establecido en los puntos anteriores
respecto de la desactivación de los diferentes Niveles considerados, cuando la emergencia esté plenamente controlada el Director del Plan en cada supuesto podrá declarar
el fin de la emergencia.', '', '', '0'),
	('327', '62', '204', '1', 'La Fase de Seguimiento corresponde a los escenarios previos a una posible Fase de Alerta en que no se prevé emergencia. Se estima que no existe riesgo para la población en general,
aunque sí para alguna actividad concreta o localización de alta vulnerabilidad. No existen
previsiones de que puedan materializarse daños o se prevé que estos serán muy limitados.', '', '', '0'),
	('328', '62', '205', '1', 'Esta Fase podrá iniciarse, bien directamente por la ocurrencia de un fenómeno o suceso
en un escenario de normalidad, bien por el progreso desfavorable del fenómeno.', '', '', '0'),
	('329', '62', '206', '1', 'Corresponde a emergencias en las que se han producido fenómenos o
sucesos muy localizados cuya atención, queda asegurada con la capacidad de respuesta local, o queda incluida dentro
del ámbito de aplicación de los planes de autoprotección o de planes territoriales de protección
civil de ámbito local.', '', '0', '0'),
	('330', '62', '206', '2', 'Emergencias provocadas por fenómenos de gravedad,superando así la capacidad de respuesta de los planes territoriales de protección civil de
ámbito local y, en las que se requiere una respuesta coordinada por parte de la Administración
autonómica de Castilla y León, bien por afectar a más de un municipio, bien por ser precisa la
activación de medios y recursos propios o asignados al PLANCAL.', '', '1', '1'),
	('331', '62', '206', '3', 'Emergencias provocadas por fenómenos de tal
gravedad que superan la capacidad de respuesta ordinaria que se puede afrontar con la estructura y medios
del PLANCAL para la Situación 1.', '', '2', '2'),
	('332', '62', '206', '4', 'Corresponde a emergencias en las que por sus graves efectos son declaradas
como de interés nacional conforme a lo previsto en la Norma Básica de Protección Civil.', '', '3', '3'),
	('333', '62', '207', '1', 'La Fase de Recuperación comienza cuando
se considere que no existen significativas posibilidades de reactivación. Y, se establecerán una serie de medidas para la rehabilitación de las zonas afectadas.', '', '', '0'),
	('334', '63', '208', '1', 'Accidentes que pueden ser controlados por los medios disponibles y que, aun en su evolución más desfavorable, no suponen peligro.', '0', '', '0'),
	('335', '63', '208', '2', 'Se incorpora a un representante de cada uno de los distintos Grupos de Acción
que a propuesta del Director del Plan sean necesarios y delimita la Zona de Operaciones.
El Alcalde atiende a la movilización de los medios
locales a propuesta del Jefe de Extinción y da la información a la población afectada de acuerdo con lo previsto en el Plan Municipal y, si es necesario, con la
ayuda del Grupo de Seguridad.', '1', '', '1'),
	('336', '63', '208', '3', 'Este nivel será declarado en aquellas emergencias que por la gravedad y/o extensión del riesgo y sus efectos se requiere la plena movilización de la estructura organizativa y de los medios y recursos asignados y no asignados.Y,  puede ser necesario la evacuación de la población de la
Zona.', '2', '', '2'),
	('337', '63', '208', '4', 'La declaración de interés nacional será solicitada del Ministro de Interior por el Comité de Dirección del Plan.', '3', '', '3'),
	('338', '64', '209', '1', 'Fase caracterizada, por la existencia de información sobre la posibilidad de ocurrencia
de sucesos capaces de dar lugar a inundaciones.
El objetivo general de esta fase es la alerta de las autoridades y servicios implicados en
el presente Plan, así como la información a la población potencialmente afectada.', '', '', '0'),
	('339', '64', '210', '1', 'La información meteorológica e hidrológica permite
prever la inminencia de inundaciones con peligro para personas y bienes.', '', '0', '0'),
	('340', '64', '210', '2', 'Se han producido inundaciones en zonas localizadas,
cuya atención puede quedar asegurada mediante el empleo de los medios y recursos locales y/o asignados disponibles en las zonas afectadas.', '', '1', '1'),
	('341', '64', '210', '3', 'Se han producido inundaciones que superan la
capacidad de atención de los medios y recursos locales en todas o alguna de las zonas afectadas o aún sin producirse esta última circunstancia, los datos pluviométricos e hidrológicos y las predicciones
meteorológicas, permiten prever una extensión o agravación significativa de aquellas. Así mismo, serán declaradas como situación 2
de este Plan, las emergencias que en sus planes de presas están definidas como escenarios 2 y 3.', '', '2', '2'),
	('342', '64', '210', '4', 'Emergencias que, habiéndose considerado que está en
juego el interés nacional, así sean declaradas por el Ministro del Interior.', '', '3', '3'),
	('343', '64', '211', '1', 'Fase consecutiva a la de emergencia, que se prolongará hasta el restablecimiento de
las condiciones mínimas imprescindibles para un retorno a la normalidad en las zonas
afectadas por la inundación. Durante esta fase se realizarán las primeras tareas de rehabilitación', '', '', '0'),
	('344', '65', '212', '1', 'Aquellos accidentes que puedan ser controlados por los medios locales disponibles.', '', '0', '0'),
	('345', '65', '213', '1', 'Aquellos accidentes que pudiendo ser controlados por los medios locales disponibles, requiere la aplicación de medidas adicionales para la protección.', '', '1', '1'),
	('346', '65', '213', '2', 'Aquellos accidentes que necesitan la intervención de los medios Y  recursos autonómicos, e incluso recursos y medios no asigandos al Plan de la CCAA.', '', '2', '2'),
	('347', '65', '213', '3', 'Accidentes en los cuales el Minsterio del Interior declara el interés nacional.', '', '3', '3'),
	('348', '65', '214', '1', 'Aquellos accidentes que pudiendo ser controlados por los medios locales disponibles, requiere la aplicación de medidas adicionales para la protección.', '', '', '0'),
	('349', '65', '215', '1', 'El director del plan declarará el final de la emergencia, cuando haya sido informado a través de los responsables de los grupos de acción, de que ya se han finalizado o reducido las causas que provocaron la activación del plan, y que se han alcanzado unos niveles aceptables de seguridad.', '', '', '0'),
	('350', '66', '216', '1', 'Seguimiento de la situación  y en la gestión de la información, y su difusión  hacia los diferentes responsables, los recursos y hacia la
población. Su activación es compatible con la actuación de los diferentes servicios de urgencia bajo
sus propios procedimientos de actuación y coordinación.', '', '', '0'),
	('351', '66', '217', '1', 'Genera la posibilidad de integrar bajo una acción coordinada y bajo una única dirección (la
Dirección del PLATECAM), la intervención de todos los medios y recursos adscritos al Plan.', '1', '', '1'),
	('352', '66', '217', '2', 'La activación del nivel 2 de la fase de Emergencia genera la posibilidad de integrar en la estructura
de respuesta del PLATECAM  a medios y recursos extraordinarios (no adscritos al Plan).', '2', '', '2'),
	('353', '66', '217', '3', 'La activación de nivel 3 conlleva la declaración del interés nacional. Declarará el interés nacional
el Ministro del Interior conforme al artículo 29 de la Ley 17/2015, de 9 de julio, del Sistema
Nacional de Protección Civil, por propia iniciativa o a instancia de la persona titular de la
Presidencia de la Comunidad Autónoma) o del Delegado/a del Gobierno.', '3', '', '3'),
	('354', '66', '218', '1', 'En el seno del CECOP o CECOPI, llegado el caso, se determinará
la finalización de la emergencia y desactivación del Plan. La decisión es exclusivamente potestad de la Dirección del PLATECAM.', '', '', '0'),
	('355', '67', '219', '1', 'No hay que tomar medidas especiales de vigilancia o pronto ataque.', '0', '', '0'),
	('356', '67', '219', '2', 'Trabajos de prevención de incendios incluidos en el Plan', '1', '', '1'),
	('357', '67', '219', '3', 'Además de los recursos asignados a la fase de preemergencia 1, en esta fase, se activa al menos la red primaria de vigilancia del SEIF.', '2', '', '2'),
	('358', '67', '219', '4', 'Los medios de extinción pertenecientes al Servicio Operativo de Extinción de Incendios Forestales 
permanecerán en situación de disponibilidad absoluta.', '3', '', '3'),
	('359', '67', '219', '5', 'Además de los recursos movilizados en las fases anteriores, las labores de detección se complementaran con la vigilancia efectuada por los medios aéreos asignados al Plan cuando se considere necesario. ', '4', '', '-'),
	('360', '67', '220', '1', 'Situación de emergencia provocada por uno o varios incendios forestales que, en su evolución previsible, puedan afectar a bienes de naturaleza forestal. Esta emergencia podrá ser controlada con los 
medios y recursos del SEIF, medios locales de extinción o medios de la Administración General del Estado.', '0', '', '0'),
	('361', '67', '220', '2', ' Situación de emergencia provocada por uno o varios incendios forestales, que en su evolución previsible, puedan afectar gravemente a bienes forestales o afectación leve a la población y bienes de naturaleza no forestal.Además de los medios y recursos incorporados en el nivel 0, se podrán incorporar medios de otras Comunidades Autónomas, a solicitud del órgano competente de la Comunidad Autónoma. ', '1', '', '1'),
	('362', '67', '220', '3', ' Situación de emergencia provocada por uno o varios incendios forestales que, en su evolución previsible, puedan afectar gravemente a la población y bienes de naturaleza no forestal, exigiendo la adopción inmediata de medidas de protección y socorro. Además de los medios y recursos incorporados en el nivel 1, puede ser necesario que sean incorporados medios extraordinarios del Estado.', '2', '', '2'),
	('363', '67', '220', '4', 'Situación de emergencia en la que, habiéndose considerado que está en juego el interés nacional, 
así fuera declarada por el Ministro del Interior.', '3', '', '3'),
	('364', '67', '221', '1', 'Esta fase dura hasta el restablecimiento de las condiciones mínimas imprescindibles para un retorno 
a la normalidad. ', '', '', '0'),
	('365', '68', '222', '1', 'Permite establecer medidas de aviso o de preparación de recursos que en caso de evolución de la emergencia se 
traducen en una respuesta más rápida y eficaz. ', '', '', '0'),
	('366', '68', '223', '1', 'Se declara cuando se han producido inundaciones en zonas localizadas, cuya atención puede quedar asegurada 
mediante el empleo de medios y recursos disponibles en las zonas afectadas, o cuando los datos 
meteorológicos o hidrológicos permiten prever la inminencia de inundaciones en el ámbito del Plan, con peligro 
para personas y bienes. ', '1', '', '1'),
	('367', '68', '223', '2', 'Cuando se han producido inundaciones que superan la capacidad de atención de los medios y recursos locales o aún sin producirse esta última circunstancia, los datos pluviométricos e hidrológicos y las predicciones meteorológicas permiten prever una extensión o agravación significativa.', '2', '', '2'),
	('368', '68', '223', '3', 'Se activará en las emergencias en las que esté presente el interés nacional que, según lo dispuesto en el 
artículo 28 de la Ley 17/2015, de 9 de julio, del Sistema Nacional de Protección Civil.', '3', '', '3'),
	('369', '68', '224', '1', 'Se prolongará hasta el restablecimiento de las condiciones mínimas 
imprescindibles para un retorno a la normalidad en las zonas afectadas por la inundación.', '', '', '0'),
	('370', '69', '225', '1', 'Es el nivel básico de operatividad del PETCAM y consiste en el seguimiento 
de la emergencia y la información a la población. Pueden ser controlados por los medios disponibles y, 
aún en su evolución más desfavorable, no suponen peligro para las personas no relacionadas con las labores de 
intervención, ni para el medio ambiente, ni para los bienes.', '', '', '0'),
	('371', '69', '226', '1', 'Se corresponde con la SITUACIÓN 1,al tratarse de accidentes que 
pueden ser controlados por los medios disponibles y que, requieren la puesta en práctica de medidas de protección 
para las personas, los bienes o el medio ambiente. ', '1', '', '1'),
	('372', '69', '226', '2', 'Se corresponde con la SITUACIÓN 2, al tratarse de accidentes que 
requieren medidas de protección para las personas, los bienes o el medio ambiente, siendo 
necesaria la participación de medios de intervención extraordinarios, no asignados al plan.', '2', '', '2'),
	('373', '69', '226', '3', 'Se corresponde con la SITUACIÓN 3, al considerarse que está implicado 
en dicho accidente el interés nacional. Es declarado por el Ministro del Interior.', '3', '', '3'),
	('374', '69', '227', '1', 'El Director del Plan declarará el fin de la emergencia una vez comprobado e informado por el PMA que han
desaparecido, o se han reducido suficientemente las causas, y que se
han restablecido los niveles normales de seguridad y los servicios mínimos a la población. ', '', '', '0'),
	('375', '70', '228', '1', 'Seguimiento de la situación y el intercambio de información con los órganos y autoridades 
competentes, así como la información a la población en general. El objetivo de esta fase es confirmar o no la situación de riesgo y el análisis de su evolución.', '', '', '0'),
	('376', '70', '229', '1', 'Situaciones en las que el riesgo sobre la población, el medio ambiente o los bienes, aun siendo muy improbable, requieren la adopción de medidas 
de protección, pudiendo ser controlada con los medios y recursos correspondientes al 
RADIOCAM.', '1', '', '1'),
	('377', '70', '229', '2', 'Situaciones en las que la gravedad de las posibles afecciones para la salud y seguridad de la población, el número de personas amenazadas o la 
extensión de las áreas afectadas, hacen necesaria la intervención de medios, recursos o servicios diferentes a los adscritos al RADIOCAM.', '2', '', '2'),
	('378', '70', '229', '3', 'Situaciones en las que se han producido fenómenos cuya 
naturaleza, gravedad o alcance de los riesgos determinan que se considere en juego por el Ministerio del Interior el interés nacional.', '3', '', '3'),
	('379', '70', '230', '1', 'Es el período en el que es necesario aplicar medidas de larga duración que se 
prolongarán hasta el restablecimiento de las condiciones mínimas imprescindibles para el inicio 
del retorno a la normalidad en la población, el medio ambiente y los bienes de las áreas afectadas por el accidente.', '', '', '0'),
	('380', '71', '231', '1', 'No producen víctimas ni daños 
relevantes, pero sí alarma social, por lo que no se llevará a cabo el despliegue operativo 
de toda la estructura del SISMICAM, permaneciendo el CECOP en situación de Alerta por la posible aparición de nuevos movimientos sísmico. Esta fase  está dirigida a la información y seguimiento.', '', '', '0'),
	('381', '71', '232', '1', 'Se han producido fenómenos sísmicos, cuya atención, en lo 
relativo a la protección de personas y bienes, puede quedar asegurada mediante el 
empleo de los medios y recursos disponibles en las zonas afectadas.', '1', '', '1'),
	('382', '71', '232', '2', 'Fenómenos sísmicos de gravedad tal que los daños ocasionados, el número de víctimas o la extensión de las áreas afectadas, superan 
la capacidad de atención de los medios y recursos disponibles en las zonas afectadas, requiere la incorporación e integración de medios y recursos extraordinarios.', '2', '', '2'),
	('383', '71', '232', '3', ' Emergencias que, habiéndose considerado que está en juego el interés nacional, así sean declaradas por el Ministro del Interior conforme al punto 29 de la Ley 17/2015, de 9 de julio, del Sistema Nacional de Protección Civil', '3', '', '3'),
	('384', '71', '233', '1', 'Fase Consecutiva a la de emergencia que se prolongará hasta el restablecimiento de las 
condiciones mínimas imprescindibles para el retorno a la normalidad en las zonas afectadas por el terremoto.', '', '', '0'),
	('385', '72', '234', '1', 'La activación en Alerta es el nivel básico de operatividad del METEOCAM y 
consiste principalmente en el seguimiento de la emergencia y la información a la 
población.', '', '', '0'),
	('386', '72', '235', '1', ' FEMA cuyas consecuencias superen la Fase de Alerta, y en las que sea necesario 
establecer una actuación coordinada de los recursos movilizados por las 
administraciones competentes, y de este modo fijar prioridades y los ámbitos de 
actuación de los citados recursos.', '1', '', '1'),
	('387', '72', '235', '2', 'Cuando se produzca un agravamiento de las consecuencias del Nivel 1 o cuando sea necesario movilizar recursos no adscritos al 
Plan. Lo declara, y asume la dirección del METEOCAM .', '2', '', '2'),
	('388', '72', '235', '3', 'Se podrá solicitar la 
declaración de Emergencia de Interés Nacional o la activación de un Plan Estatal, 
en cuyo caso, la dirección de la emergencia será transferida a la Administración General del Estado.', '3', '', '3'),
	('389', '72', '236', '1', 'Esta fase dura hasta el restablecimiento de las condiciones mínimas imprescindibles para el retorno a la normalidad, es decir, el establecimiento de las condiciones mínimas para restituir el tráfico en
condiciones de seguridad.', '', '', '0'),
	('390', '73', '237', '1', 'Esta zona quedará comprendida entre el límite de la zona de intervención, hasta los puntos de control
de accesos establecidos por la Guardia Civil.', '', '', '0'),
	('391', '73', '238', '1', 'Se inicia en el momento en que se activa el Plan de Respuesta, al producirse un accidente, que por el
número de vehículos y personas afectadas y por tanto de recursos intervinientes, requiere una
coordinación jerárquicamente superior a las establecidas por los diferentes grupos actuantes.', '', '', '1'),
	('392', '73', '239', '1', 'Esta fase dura hasta el restablecimiento de las condiciones mínimas imprescindibles para el retorno a la normalidad, es decir, el establecimiento de las condiciones mínimas para restituir el tráfico en
condiciones de seguridad', '', '', '0'),
	('393', '74', '240', '1', 'Indicios o a la previsión de fenómenos que podrían desencadenar más adelante una activación del plan pero
con respecto a los cuales hay una cierta incertidumbre que hace que la materialización de la emergencia no sea clara. ', '', '', '0'),
	('394', '74', '241', '1', 'Comporta el despliegue parcial de la estructura que prevé el Plan, es decir,la activación de determinados grupos en función de las características de la situación, e implica la
información en la población, a través de los medios incluidos en el Plan.', '', '', '0'),
	('395', '74', '242', '1', 'Cuando hay una afectación en un territorio extenso o a una zona especialmente
vulnerable, ya sea para la población que lo ocupa o por el medio natural potencialmente afectado.', '', '', '1'),
	('396', '74', '243', '1', 'El/la director/a del PROCICAT decidirá las medidas a tomar, así como el fin de la emergencia
basándose en las recomendaciones del Consejo Asesor, una vez controlado y eliminado el origen
de la emergencia y minimizadas las consecuencias del accidente.', '', '', '0'),
	('397', '75', '244', '1', 'Incendios forestales que no requieren la intervención de la 
totalidad de los grupos de actuación pero que, por sus características, pueden derivar 
en situaciones que comporten la activación del Plan y que, por tanto, requieren de un 
seguimiento cercano y puntual de la actuación.', '', '', '0'),
	('398', '75', '245', '1', 'Aquellas situaciones que hagan necesario establecer
medidas preventivas y de control complementarias a las habituales o también medidas 
emergencia de cumplimiento obligatorio en un ámbito limitado.', '', '', '0'),
	('399', '75', '246', '1', 'En esta fase se despliega toda la estructura del Plan y todos los recursos disponibles 
adscritos. ', '', '', ''),
	('400', '75', '247', '1', 'El CECAT debe hacer el seguimiento puntual de la emergencia. También, las medidas a tomar o cambios de estrategia y debe decretar el fin de la emergencia basándose en la información disponible una vez se haya comprobado que no hay afectaciones graves a la población y, de manera general, los servicios básicos hayan sido restablecidos, aunque queden algunas afectaciones 
puntuales.', '', '', '0'),
	('401', '76', '248', '1', 'Se empezará a informar a la población que siga las previsiones meteorológicas ante la del riesgo de lluvias fuertes. Cada organismo continuará dando su información habitual e iniciará las tareas previstas.', '', '', '0'),
	('402', '76', '249', '1', 'Aquellas situaciones que hagan 
necesario el establecimiento de amplias medidas preventivas y de control. Implicará actuaciones preventivas y sistematizadas en los diferentes cuerpos que los integran.', '', '', '0'),
	('403', '76', '250', '1', 'Conlleva la puesta en funcionamiento de la estructura organizativa de gestión de la emergencia 
con la movilización total o parcial de las herramientas y medios adscritos al plan.', '', '', ''),
	('404', '76', '251', '1', 'El seguimiento del suceso será hecho desde el CECAT, a través de las informaciones que lleguen del CCA, de los diferentes centros de coordinación y a través de los datos que lleguen del grupo 
de evaluación hidrometeorológica.', '', '', '0'),
	('405', '77', '252', '1', 'Aquellas situaciones en que un
transporte de MP ve parada la marcha por diferentes motivos que no ponen en riesgo el contenido de la mercancía.', '', '', '0'),
	('406', '77', '253', '1', 'La fase de prealerta corresponde a situaciones en que haya tenido lugar un 
accidente en el transporte de MP cuyas consecuencias se prevén leves y controlables. No hay afectación en la población, ni itinerante ni fija. ', '', '', '0'),
	('407', '77', '254', '1', 'La activación en alerta es pertinente en aquellas situaciones en las que se ha 
producido un accidente con consecuencias que afectan o pueden afectar a la 
población, los bienes y el medio ambiente del entorno inmediato, no a la 
población fija de cascos urbanos. ', '', '', '0'),
	('408', '77', '255', '1', 'Incidentes en que el transporte no puede continuar la marcha pero no se han producido daños ni reales ni potenciales, las actuaciones irán encaminadas a avisar a los grupos actuantes a título informativo. ', '0', '', '0'),
	('409', '77', '255', '2', 'Accidentes en los que el contenido ha quedado afectado o puede quedar afectado, pero no hay afectación grave para la población ni itinerante 
ni fija, ni al medio ambiente.', '1', '', '1'),
	('410', '77', '255', '3', 'Accidentes que, a pesar de que son o pueden ser importantes, 
solo pueden afectar a las personas, los bienes y el medio ambiente del entorno 
inmediato.', '2', '', '2'),
	('411', '77', '255', '4', 'Accidentes que, además del entorno inmediato, pueden afectar a otras zonas más allá del entorno inmediato, incluyendo cascos urbanos o 
zonas de especial interés medioambiental.', '3', '', '3'),
	('412', '77', '256', '1', 'Una vez desactivado el TRANSCAT, los poderes públicos podrán establecer, si 
conviene, un plan de recuperación y rehabilitación de los servicios básicos y del 
entorno.', '', '', '0'),
	('413', '78', '257', '1', 'El grupo radiológico valorará las implicaciones radiológicas del incidente o el accidente, si 
en su caso neutralizará la fuente de riesgo radiológico y aconsejará la dirección del plan sobre si será necesario activar el plan y en qué fase.', '', '', '0'),
	('414', '78', '258', '1', 'Se avisará a los elementos vulnerables que se puedan 
ver afectados por una evolución desfavorable de la situación y se tomarán las medidas 
necesarias para proteger a las personas especialmente vulnerables a las radiaciones 
ionizantes.', '', '', '0'),
	('415', '78', '259', '1', 'Los grupos actuantes, de manera coordinada, llevarán a cabo todas aquellas acciones 
necesarias para combatir el accidente y controlar la emergencia. El grupo radiológico analizará y valorará el riesgo radiológico que conlleva el accidente y colaborará en las actuaciones necesarias con el fin de neutralizar el foco de peligro radiológico. El resto de grupos realizarán las actuaciones necesarias que correspondan a sus funciones.', '1', '', '1'),
	('416', '78', '259', '2', 'Aquellas acciones 
necesarias para combatir el accidente y controlar la emergencia. El grupo radiológico analizará y valorará el riesgo radiológico que conlleva el accidente y colaborará en las actuaciones necesarias con el fin de neutralizar el foco de peligro radiológico. El resto de 
grupos realizarán las actuaciones necesarias que correspondan a sus funciones', '2', '', '2'),
	('417', '79', '260', '1', 'Aquellos sismos que, a pesar de no ser causa de daño, sea 
relevante, son percibidos ampliamente por la población y pueden provocar cierta alarma 
social por su naturaleza extraordinaria.
En esta situación, el plan no se considera activado y la operativa estará dirigida a la 
intensificación del seguimiento y difusión de toda la información posible sobre 
el evento, teniendo en cuenta que, en este caso, se debe gestionar una posible alarma social.', '', '', '0'),
	('418', '79', '261', '1', 'Implica menor gravedad y una afectación de menor personas con respecto a la fase de emergencia.', '', '', '0'),
	('419', '79', '262', '1', 'En cada caso,una cantidad de recursos diferente para atender a las incidencias. La activación comporta el despliegue de toda la estructura y organización del Plan SISMICAT, y los distintos grupos operativos ejecutarán las actuaciones necesarias
para resolver las incidencias que se presenten y  específicamente las relacionadas.', '', '', ''),
	('420', '79', '263', '1', 'Fase Consecutiva a la de emergencia que se prolongará hasta el restablecimiento de las 
condiciones mínimas imprescindibles para el retorno a la normalidad en las zonas afectadas.', '', '', '0'),
	('421', '80', '264', '1', 'Para aquellos seísmos que, a pesar de no ser
causa de ningún daño personal o material, o, en todo caso, de un daño material que sea
relevante, son percibidos ampliamente por la población y pueden provocar cierta alarma', '', '', '0'),
	('422', '80', '265', '1', 'En accidentes de categoría 1 importantes se prevé que haya como única consecuencia
daños materiales a la instalación, sin ningún tipo de daño fuera de la industria, aunque impliquen ayuda exterior. Conllevan la activación en ALERTA del PLASEQCAT.', '', '', '0'),
	('423', '80', '266', '1', 'Los accidentes graves pueden implicar daños y víctimas en el interior (categoría 2) y/o daños
importantes o incluso víctimas en el exterior (categoría 3). El PLASEQCAT se activará en
emergencia siempre ante un accidente de categoría 2 o 3. El nivel de respuesta el
determinará el Director del Plan según el accidente y su posible evolución.', '', '', '2'),
	('424', '80', '267', '1', 'El director del PLASEQCAT será quien decrete el fin de la emergencia. Ésta será
inmediatamente comunicada por el CECAT a todos los grupos de actuación ya todas las instituciones y personas que hayan intervenido o estén interviniendo en la emergencia.', '', '', '0'),
	('425', '81', '268', '1', 'Accidentes de categoría 1, en los que no es necesaria la intervención de los grupos operativos externos pero sí un seguimiento de la posible evolución (Informando así a los ciudadanos del mismo) desde el CECAT, e incidentes con posibilidad de alarma social.', '', '', '0'),
	('426', '81', '269', '1', ' Conlleva el despliegue de toda la respuesta de nivel operativo y de las funciones asignadas a los grupos. Y, se corresponde a los accidentes de categoría 2 en el que las consecuencias están limitadas a el interior del establecimiento.
', '', '', '0'),
	('427', '81', '270', '1', 'Se corresponde a accidentes de categoría 3 donde se esperan daños graves en zonas extensas y en el exterior del establecimiento. Además, se ordenará el confinamiento general de la población y el control de accesos, con la activación de las sirenas de riesgo químico que cubran territorio incluido en la zona, y suspensión de eventos.', '', '', '-'),
	('428', '82', '271', '1', ' Realizar inspección visual de la zona
Pedir información de detección de manchas. Informar a los organismos del Plan implicados en la pre – alerta.
Hacer el seguimiento de la evolución de la situación.
En caso de haberse producido el derrame, hacer su seguimiento.
En caso de derrame el grupo de intervención mar deberá actuar.', '', '', '0'),
	('429', '82', '272', '1', 'Lleva a cabo tareas de seguimiento y de evaluación de la situación, proporciona
información a las autoridades y a los ciudadanos de los lugares afectados.', '', '', '0'),
	('430', '82', '273', '1', 'Conlleva la puesta en funcionamiento de la estructura organizativa de gestión de la emergencia, tanto en mar como en tierra, con la movilización parcial de las herramientas y medios adscritos al plan.', '1', '', '1'),
	('431', '82', '273', '2', 'Conlleva la puesta en funcionamiento de toda en la estructura organizativa de la gestión de la emergencia con la movilización total de las herramientas y medios adscritos al plan.', '2', '', '2'),
	('432', '82', '274', '1', 'Se puede considerar finalizada cuando las zonas prioritarias estén descontaminadas.
Los grupos actuantes realizarán sus tareas, siguiendo el Plan de recuperación y
rehabilitación.', '', '', '0'),
	('433', '83', '275', '1', 'Aquellos en los que la situación puede ser controlada por los operativos propios del aeropuerto o aeródromo, sin víctimas o como máximo algún herido leve que podrá ser atendido por los servicios sanitarios de la propia instalación.
', '', '', '0'),
	('434', '83', '276', '1', 'Son aquellos que, a pesar de comportar un riesgo potencial importante, pueden ser controlados por los medios de la instalación aeronáutica.', '', '', '0'),
	('435', '83', '277', '1', 'Accidente fuera de un aeropuerto/aeródromo con afectación a zona de pública 
concurrencia o con más de cinco víctimas, ya sea víctimas mortales o heridos', '1', '', '1'),
	('436', '83', '277', '2', 'Cuando se dé un escenario correspondiente a 
Emergencia 1 donde esté implicado más de un avión comercial. También, se activará en caso de accidente de avión comercial en zona de pública 
concurrencia, en caso de accidente que involucre dos aeronaves o en cualquier otra 
situación a criterio de la dirección del plan.', '2', '', '2'),
	('437', '83', '278', '1', 'Se tomaran una serie de medidas a tomar o cambios de estrategia, así como el fin de la emergencia basándose en las recomendaciones del consejo
asesor, una vez restablecida la normalidad y minimizadas las consecuencias de
el accidente.', '', '', '0'),
	('438', '84', '279', '1', 'Durante la fase de prealerta, todos los organismos involucrados deberán hacer el seguimiento de las 
informaciones enviadas desde CECAT,referido a la previsión de evolución y desarrollo del fenómeno. 
Además de, la emisión de comunicados de prealerta del VENTCAT,', '', '', '0'),
	('439', '84', '280', '1', 'Se mantendrá en alerta mientras la situación se pueda solucionar con los medios habituales 
de gestión de emergencias y la afectación a la población sea reducida.', '', '', '0'),
	('440', '84', '281', '1', 'Con daños importantes, por volumen o intensidad. Se producirá la movilización humana y material inmediata de los grupos de actuación y de los organismos implicados 
siguiendo lo estipulado en sus planes de actuación de grupo.', '', '', ''),
	('441', '84', '282', '1', 'Son medidas para el restablecimiento inmediato a la población de los servicios 
esenciales afectados por la catástrofe o calamidad producidas.', '', '', '0'),
	('442', '85', '283', '1', 'La finalidad de estos avisos es que, dada una predicción de nevadas, todos los organismos (grupos operativos, municipios...) involucrados en 
el Plan valoren la situación y tomen las medidas preventivas adecuadas con el fin de minimizar el riesgo.', '', '', '0'),
	('443', '85', '284', '1', 'Situaciones en las que, en caso de cumplirse las predicciones, es probable que se produzcan afectaciones importantes en servicios básicos, movilidad, 
infraestructuras o población', '', '', '0'),
	('444', '85', '285', '1', 'La activación del Plan NEUCAT en fase de emergencia implicará la puesta inmediata en funcionamiento 
de la estructura organizativa de gestión de la emergencia, con la movilización total o parcial de los medios y recursos adscritos al Plan.', '', '', ''),
	('445', '85', '286', '1', 'Una vez se haya comprobado que no hay afectaciones graves en la población y, de manera general, los servicios básicos hayan sido restablecidos, aunque queden 
algunas afectaciones puntuales. ', '', '', '0'),
	('446', '85', '287', '1', 'Fase posterior a la emergencia y que se prolonga hasta que se han restablecido los servicios. 
mínimos a la población de las zonas afectadas.', '', '', '0'),
	('447', '86', '288', '1', 'Durante la fase de prealerta, todos los organismos involucrados deberán hacer el seguimiento de las 
informaciones enviadas desde CECAT, tanto en lo que se refiere a la previsión de evolución del fenómeno como por lo que se refiere a la misma. 
al desarrollo del episodio de vientos una vez haya comenzado. ', '', '', '0'),
	('448', '86', '289', '1', 'Se mantendrá en alerta mientras la situación se pueda solucionar con los medios habituales de gestión de emergencias y la afectación a la población sea reducida. ', '', '', '0'),
	('449', '86', '290', '1', 'Se producirá la movilización humana y material inmediata de los grupos de actuación y de los organismos implicados 
siguiendo lo estipulado en sus planes de actuación de grupo. Se valorará la modificación de las actividades de la población.', '', '', ''),
	('450', '86', '291', '1', 'Medidas de rehabilitación urgente de los servicios 
esenciales afectados por una emergencia y entre las medidas de recuperación y restauración de las 
infraestructuras, servicios y suministros afectados.', '', '', '0'),
	('451', '87', '292', '1', 'Engloba aquellos supuestos en qué hay que prever posibles escenarios de impacto para los cuales es necesario preparar medidas de carácter preventivo para evitar y hacer frente a situaciones de emergencia que puedan ser significativas a medio o corto plazo.', '', '', '0'),
	('452', '87', '293', '1', 'Las principales actuaciones en alerta están destinadas a contener la expansión de la 
pandemia, así como a reducir el impacto psicosocial y sobre los servicios básicos.', '', '', '0'),
	('453', '87', '294', '1', 'Viene definida por la constatación de una afectación importante que supone un 
riesgo elevado de colapso del sistema sanitario o de los servicios esenciales y que puede hacer necesaria la restricción de derechos fundamentales de la ciudadanía. ', '', '', ''),
	('454', '88', '295', '1', 'La fase de preemergencia se inicia para todos los involucrados a partir de la recepción de la declaración debiendo activar, cada uno de ellos, sus protocolos internos de actuación tras la recepción de la citada preemergencia.', '', '', '0'),
	('455', '88', '296', '1', 'Situación en la que se han producido daños muy localizados y en la que para su
control es suficiente la activación y aplicación de un plan de ámbito local.', '0', '', '0'),
	('456', '88', '296', '2', 'Situación en la que se han producido daños moderados y en la que para su control es
necesario la activación del presente plan.', '1', '', '1'),
	('457', '88', '296', '3', 'Situación en la que se han producido daños extensos y en la que para su control es
necesario la activación del presente plan,  movilizando así a los recursos extraordinarios no adscritos al PTECV.', '2', '', '2'),
	('458', '88', '296', '4', 'Emergencias que, habiéndose considerado que está en juego el interés nacional, así
sean declaradas por el Ministro de Interior.', '3', '', '3'),
	('459', '88', '297', '1', 'Una vez finalizada la situación de peligro para las personas y los bienes, el Director del PTECV declarará el final de la situación de emergencia. Esta declaración será transmitida a todos los organismos, servicios y ayuntamientos previamente alertados por parte del
CCE Generalitat.', '', '', '0'),
	('460', '88', '298', '1', 'Finalizada la situación de riesgo para las personas y los bienes, si en el transcurso de la emergencia se han producido daños materiales,
como son la afectación a edificaciones e infraestructuras, cuyas consecuencias no permiten el normal funcionamiento de la sociedad,
el Director del PTECV podrá declarar la fase de vuelta a la normalidad que se prolongará hasta el restablecimiento de las condiciones
mínimas imprescindibles para el retorno a la normalidad de la zona afectada. ', '', '', '0'),
	('461', '89', '299', '1', 'Es una red de vigilancia fija la que de forma específica realizará labores de
detección. Estos puestos fijos estarán complementados con la vigilancia móvil que realizan las Unidades de Prevención, el voluntariado
forestal / medioambiental y los agentes medioambientales. ', '1', '', '1'),
	('462', '89', '299', '2', 'Además de los recursos asignados anteriormente, las Centrales de Coordinación
de los Consorcios Provinciales de Bomberos movilizarán a los Bomberos forestales de la Agencia Valenciana de Seguridad y Respuesta
a las Emergencias. Y, se establecerán rutas interprovinciales de vigilancia con los medios aéreos.', '2', '', '2'),
	('463', '89', '299', '3', 'Las brigadas realizarán tareas de vigilancia preventiva durante toda la jornada, en lugar de sus labores habituales. Por lo que, se llevará a cabo, a través de todos los medios, una tarea preventiva.', '3', '', '3'),
	('464', '89', '300', '1', 'Situación de emergencia provocada por uno o varios incendios forestales que, en su evolución previsible, puedan afectar sólo a bienes de naturaleza forestal; y puedan ser controlados con los medios y recursos del PEIF, e incluyendo medios del Estado, siempre y cuando éstos últimos actúen dentro de su zona de actuación preferente.', '0', '', '0'),
	('465', '89', '300', '2', 'Situación de emergencia provocada por uno o varios incendios forestales
que, en su evolución previsible, puedan afectar gravemente a bienes forestales y, levemente a la población y bienes de
naturaleza no forestal y puedan ser controlados con los medios y recursos del PEIF, o para cuya extinción pueda ser necesario que, a solicitud del Director del PEIF y previa valoración por parte de la administración estatal correspondiente, sean incorporados medios
extraordinarios.', '1', '', '1'),
	('466', '89', '300', '3', 'Situación de emergencia provocada por uno o varios incendios forestales
que, en su evolución previsible, puedan afectar gravemente a la
población y bienes de naturaleza no forestal, exigiendo la adopción
inmediata de medidas de protección y socorro; y pueda ser necesario
que, a solicitud del Director del PEIF, sean incorporados medios
extraordinarios.', '2', '', '2'),
	('467', '89', '300', '4', 'Situación de emergencia correspondiente y consecutiva a la declaración
de emergencia de interés nacional por parte de la persona titular del
Ministerio del Interior.', '3', '', '3'),
	('468', '89', '301', '1', 'La declaración de incendio extinguido supondrá implícitamente el final de la situación de emergencia. En todos los casos el final de la emergencia será trasmitida a los mismos organismos y servicios que se alertaron en su declaración.', '', '', '0'),
	('469', '89', '302', '1', 'Finalizada la situación de emergencia, si en el transcurso de la misma se han producido daños materiales, como son la afectación a
edificaciones e infraestructuras, cuyas consecuencias no permiten el normal funcionamiento de la sociedad, el Director del PEIF podrá
declarar la fase de vuelta a la normalidad que se prolongará hasta el restablecimiento de las condiciones mínimas imprescindibles para
el retorno a la normalidad de la zona afectada. ', '', '', '0'),
	('470', '90', '303', '1', 'Se inicia para todos los involucrados a partir de la recepción de la declaración debiendo activar, cada uno de 
ellos, sus protocolos internos de actuación tras la recepción de la citada preemergencia. ', '', '', '0'),
	('471', '90', '304', '1', 'Cuando los datos pluviométricos y hidrológicos permitan prever la inminencia de inundaciones, con peligro para personas y bienes y será declarada a propuesta de la Confederación Hidrográfica o por decisión del 
Director del Plan.', '0', '', '0'),
	('472', '90', '304', '2', 'Es una situación en la que se han producido inundaciones en zonas localizadas, cuya atención puede quedar asegurada con los recursos locales o ciertos recursos de ámbito superior, en primera intervención. O, también, se declararán 
emergencia de situación 1 cuando se declare el escenario 2 ó 3 de emergencia por rotura de presa.', '1', '', '1'),
	('473', '90', '304', '3', 'Inundaciones que superan la capacidad de atención de los medios y recursos locales o, aún sin producirse esta última circunstancia, los datos pluviométricos e hidrológicos y las predicciones meteorológicas, permiten prever una extensión o agravamiento significativo de aquéllas. También se declararán emergencia de situación 2 cuando se declare el escenario 2 ó 3 de emergencia por rotura de presa.', '2', '', '2'),
	('474', '90', '305', '1', 'Una vez finalizada la situación de peligro para las personas y los bienes y recuperada la normalidad en los servicios básicos, el Director 
del Plan declarará el final de la situación de emergencia. Esta declaración será transmitida a todos los organismos, servicios y ayuntamientos previamente alertados por parte del CCE Generalitat.', '', '', '0'),
	('475', '91', '306', '1', 'La movilización de los organismos implicados en la operatividad, dependerá de la situación de emergencia que se haya declarado y se hará por parte del CCE generalitat.', '', '', '0'),
	('476', '91', '307', '1', 'En el caso de accidentes de MMPP, los tipos 1 y 2 llevará asociado, en un primer momento, la situación 1. ', '', '0', '0'),
	('477', '91', '307', '2', 'En el caso de accidentes de MMPP, los tipos 3,4 y 5 llevará asociado, en un primer momento, la situación 1.', '', '1', '1'),
	('478', '91', '307', '3', 'Cuando se prevea la movilización de recursos no asignados al plan, o requiera la Constitución de CECOPI.', '', '2', '2'),
	('479', '91', '308', '1', 'Una vez fnalizada la situación de peligro y coordinadas las actuaciones, el Director/a del Plan declarará como finalizada la situación de emergencia.', '', '', '0'),
	('480', '92', '309', '1', 'La preemergencia es la fase que, por evolución desfavorable, puede dar lugar a una situación de emergencia. El objeto es alertar a las autoridades y servicios implicados, así como informar a la población potencialmente afectada. ', '', '', '0'),
	('481', '92', '310', '1', 'Es la acción de transmitir mensajes de aviso, prevención y protección a la población potencialmente afectada, e instrucciones a 
aquellos destinatarios que tengan algún tipo de responsabilidad preventiva u operativa asignada por el Plan. ', '', '', ''),
	('482', '92', '311', '1', 'Situación de emergencia en la que los riesgos se limitan a la propia instalación y pueden ser controlados por los 
medios disponibles en el correspondiente plan de emergencia interior o plan de autoprotección.', '', '0', '0'),
	('483', '92', '311', '2', ' Situación de emergencia en la que se prevé que los riesgos pueden afectar a las personas en el interior de la 
instalación, mientras que las repercusiones en el exterior, aunque muy improbables, no pueden ser controladas únicamente 
con los recursos propios del plan de emergencia interior o del plan de autoprotección, siendo necesaria la intervención de 
servicios del Plan Especial. ', '', '1', '1'),
	('484', '92', '311', '3', ' Situación de emergencia en la que se prevea que los riesgos pueden afectar a las personas tanto en el interior 
como en el exterior de la instalación y, en consecuencia, se prevé el concurso de medios de apoyo de titularidad estatal no 
asignados al Plan Especial. ', '', '2', '2'),
	('485', '92', '311', '4', ' Situación de emergencia en la que la naturaleza, gravedad o alcance de los riesgos requiere la declaración del 
interés nacional por el Ministro del Interior.', '', '3', '3'),
	('486', '92', '312', '1', ' Una vez que el material radiactivo esté bajo control y se hayan adoptado todas las medidas para 
responder a la emergencia radiológica, por parte de la dirección del Plan se declarará el fin de la emergencia, notificándose a 
los mismos organismos, servicios operativos y empresas alertados con la activación del presente plan.', '', '', '0'),
	('487', '93', '313', '1', 'Estará motivada por la ocurrencia de fenómenos sísmicos ampliamente sentidos por la población, y requerirá 
de las autoridades y órganos competentes una actuación coordinada, dirigida a intensificar la información a los ciudadanos 
sobre dichos fenómenos.', '', '0', '0'),
	('488', '93', '314', '1', ' Se han producido fenómenos sísmicos, cuya atención, en lo relativo a la protección de personas y bienes, 
puede quedar asegurada mediante el empleo de los medios y recursos disponibles en las zonas afectadas. ', '', '1', '1'),
	('489', '93', '314', '2', ' Se han producido fenómenos sísmicos que por la gravedad de los daños ocasionados, el número de víctimas o 
la extensión de las áreas afectadas, hacen necesario, para el socorro y protección de personas y bienes, el concurso de medios, recursos o servicios ubicados fuera de dichas áreas. ', '', '2', '2'),
	('490', '93', '314', '3', ' Emergencias que, habiéndose considerado que está en juego el interés nacional, así sean declaradas por el 
Ministro del Interior.', '', '3', '3'),
	('491', '93', '315', '1', 'Fase consecutiva a la de emergencia que se prolongará hasta el establecimiento de las condiciones imprescindibles para el retorno a la normalidad en las zonas afectadas por el terremoto.', '', '', '0'),
	('492', '94', '316', '1', 'Fase que, por evolución desfavorable, puede produc ir un vertido al mar de productos contaminantes, efectuando así un seguimiento. Puede ser controlado por los recursos del Plan de Emergencia del interior del Puerto o los adscritos al plan. ', '0', '', '0'),
	('493', '94', '317', '1', 'Situación que por sus consecuencias afecta a una zona aislada de la costa. Es efectuado con los recursos adscritos al plan, y es necesario un PMA.', '1', '', '1'),
	('494', '94', '317', '2', 'Situación que por sus consecuencias afecta a una zona extensa de la costa, y requiere la puesta en práctica de medidas de protección. Es necesario un PMA.', '2', '', '2'),
	('495', '95', '318', '1', 'No existe ningún riesgo meteorológico.', 'Verde', '', '0'),
	('496', '95', '318', '2', 'No existe riesgo meteorológico para la población en general 
aunque sí para alguna actividad concreta.', 'Amarillo', '', '0'),
	('497', '95', '318', '3', 'Existe un riesgo meteorológico importante (fenómenos 
meteorológicos no habituales).', 'Naranja', '', ''),
	('498', '95', '318', '4', 'El riesgo meteorológico es extremo.', 'Rojo', '', ''),
	('499', '95', '319', '1', ' Los efectos de la nevada hacen necesaria la movilización 
de recursos para efectuar la limpieza de los viales afectados por la misma. ', '0', '', '0'),
	('500', '95', '319', '2', 'El nivel de daños que ha ocasionado la nevada, hacen necesario constituir el Centro de Coordinación de Carreteras con objeto de establecer una actuación coordinada de los recursos movilizados por las administraciones 
competentes en el área de carreteras, y de este modo fijar prioridades, itinerarios de limpieza y los ámbitos de actuación de los citados 
recursos. ', '1', '', '1'),
	('501', '95', '319', '3', 'a permanencia en el tiempo de los efectos de la nevada, 
ha provocado una situación de aislamiento que puede prolongarse durante algunos días. También, cuando sea necesario constituir el 
CECOPI.', '2', '', '2'),
	('502', '95', '320', '1', 'Se entiende por recuperar la normalidad, el restablecimiento de los servicios básicos o esenciales,así como el restablecimiento de la circulación de las principales vías de comunicación.', '', '', '0'),
	('503', '96', '321', '1', 'Responde a previsiones de potenciales
emergencias no manifestadas, pero que dadas las circunstancias y en caso de una evolución desfavorable, es posible que se desencadenen.', '', '', '0'),
	('504', '96', '322', '1', 'Emergencias de ámbito municipal
controladas mediante respuesta local.', '', '0', '0'),
	('505', '96', '322', '2', 'Se trata de emergencias que
hayan producido daños considerables o que pudieran producirlas, y las puedan
requerir una respuesta coordinada por parte de las
instituciones comunes de la
Comunidad Autónoma, ya por
afectar a más de un municipio, ya por ser precisa la activación
de medios y recursos
propios.', '', '1', '1'),
	('506', '96', '322', '3', 'Emergencias que por
su naturaleza o gravedad y/o
extensión sobrepasen las
posibilidades de
respuesta de la
Administración Local
y Provincial siendo
posible que surja la
necesidad de solicitar
el concurso de los
medios y recursos de
las Fuerzas Armadas.', '', '2', '2'),
	('507', '96', '322', '4', 'Emergencias en las
que esté presente el
interés nacional conforme a lo
previsto en la Norma
Básica de Protección
Civil.', '', '3', '3'),
	('508', '96', '323', '1', 'La emergencia ha sido dada por
finalizada sin que existan significativas
posibilidades de su reactivación.', '', '', '0'),
	('509', '97', '324', '1', 'En esta fase todavía no se ha declarado ningún incendio, pero existe la previsión de que dada las circustancias de las condiciones meteorológicas desfavorables y épocas de peligro establecidos, se pueda llegar a producir.', '', '', '0'),
	('510', '97', '325', '1', 'Pertenece a esta situación, la emergencia provocada por uno o varios incendios forestales que, en su evolució, puedan afectar solo a bienes de naturaleza forestal; y puedan ser controlados con los medios y recursos del propio plan e incluyendo, en caso necesario, medios del Estado.', '', '0', '0'),
	('511', '97', '325', '2', 'Incendios forestales que, en su evolución, puedan afectar gravemente a bienes y, en su caso, afectar levemente a la población y bienes no forestales. Pudiendo ser controlados con los medios y recursos del Plan o , en caso necesario, los medios extraordinarios.', '', '1', '1'),
	('512', '97', '325', '3', 'Incendios que pueden afectar gravemente a la población y bienes de naturaleza o no forestal, exigiendo la adopción inmediata de medidas y pueda ser necesario que, solicitud del órgano competente, sean incorporados medios extraordinarios o puedan comportar situaciones que deriven hacia el interés nacional.', '', '2', '2'),
	('513', '97', '325', '4', 'Se establecerá esta situación, en aquellos incendios que por considerarse que están en juego el interés nacional, así se han declarado por el ministerio del interior.', '', '3', '3'),
	('514', '97', '326', '1', 'Es el periodo necesario hasta el restablecimiento de las condiciones mínimas imprescindible para retorno a la normalidad en las zonas afectadas. Durante esta fase se realizan las primeras tareas de rehabilitación en dichas zonas.', '', '', '0'),
	('515', '98', '327', '1', 'Responde a previsiones  que dadas las circunstancias y, en caso de una evolución desfavorable, es posible que se desencadenen y den lugar a inundaciones. El objetivo general de esta fase es la alerta de las autoridades y servicios implicados en el plan
correspondiente, así como la información a la población potencialmente afectada.', '', '', '0'),
	('516', '98', '328', '1', 'Los datos meteorológicos e hidrológicos, los informes del estado de las presas, 
así como las lecturas de los sensores de la red SPIDA, permiten prever la inminencia de inundaciones en el ámbito del Plan, con peligro para personas y bienes.', '', '0', '0'),
	('517', '98', '328', '2', ' Se han producido inundaciones en zonas localizadas, cuya atención puede quedar asegurada mediante el empleo de los medios y recursos disponibles en las zonas 
afectadas.', '', '1', '1'),
	('518', '98', '328', '3', ' Emergencias en la que se ha producido una inundación, que por su naturaleza o 
gravedad y/o extensión territorial del riesgo, sobrepasen las posibilidades de respuesta de la 
Administración Local y Provincial o aún sin producirse  que las predicciones obstenten una seria gravedad, siendo posible que 
surja la necesidad de solicitar el concurso de los medios y recursos del Estado.', '', '2', '2'),
	('519', '98', '328', '4', 'Emergencias en las que esté presente el interés nacional conforme a lo previsto 
en la Norma Básica de Protección Civil.', '', '3', '3'),
	('520', '98', '329', '1', 'Esta fase queda implícitamente activada en el momento en el que se desactiva la fase de 
emergencia en cualquiera de sus situaciones y se prolongará hasta el establecimiento de las 
condiciones previas a la que dio lugar a la emergencia y vuelta a la normalidad en la zona 
afectada por la misma.', '', '', '0'),
	('521', '99', '330', '1', 'Posible accidente con materias peligrosas transportadas por carretera o ferrocarril, y éste no 
supone peligro alguno. Sólo se realizarán funciones de seguimiento.', '', '', '0'),
	('522', '99', '331', '1', 'Accidentes que pudiendo 
ser controlados con los medios de intervención disponibles por la Comunidad Autónoma, 
requiere la aplicación de medidas.', '', '1', '1'),
	('523', '99', '331', '2', 'Aquellos accidentes que para su 
control se prevé el concurso de medios de intervención, no asignados al Plan Especial de 
Transporte de MM.PP en Extremadura, a proporcionar por la organización del Plan Estatal.', '', '2', '2'),
	('524', '99', '331', '3', 'Se activará en las emergencias en las que esté presente el interés nacional que, según el 
párrafo 1.2 del Capítulo I de la Norma Básica de Protección Civil.', '', '3', '3'),
	('525', '100', '332', '1', 'Conocimiento de que pueda estar produciéndose un suceso radiológico, se iniciará la movilización de recursos necesarios para una eficaz verificación de la ocurrencia o no del suceso.', '', '', '0'),
	('526', '100', '333', '1', 'Situación de emergencia en la que los riesgos se limitan a la propia instalación y pueden ser controlados por los medios disponibles en el PEI o Plan de Autoprotección de la instalación.', '', '0', '0'),
	('527', '100', '334', '1', 'Situación de emergencia en la que se prevé que los riesgos pueden afectar a las personas en el interior de la
instalación, mientras que las repercusiones en el exterior, aunque muy improbables, no pueden ser controladas únicamente con los recursos propios del PEI o del plan de Autoprotección, siendo necesaria la intervención de servicios del RADIOCAEX.', '', '1', '1'),
	('528', '100', '334', '2', 'Situación de emergencia en la que se prevea que los riesgos pueden afectar a las personas tanto en el interior como en el exterior de la instalación y, en consecuencia, se prevé el concurso de medios de apoyo
de titularidad estatal no asignados al RADIOCAEX.', '', '2', '2'),
	('529', '100', '334', '3', 'Situación de emergencia en la que la naturaleza, gravedad o alcance de los riesgos requiere la declaración
del interés nacional por la persona titular del Ministerio del Interior.', '', '3', '3'),
	('530', '100', '335', '1', 'La fase de normalización, o de recuperación es la fase consecutiva a la de emergencia, que se prolongará
hasta el establecimiento de las condiciones previas al suceso y el retorno a la normalidad en la zona afectada por el mismo.', '', '', '0'),
	('531', '101', '336', '1', 'Una vez producido el movimiento sísmico, las actuaciones van encaminadas a la información de la población y el seguimiento de la situación hasta la confirmación de que no hay efecto dominó ni otras complicaciones, es decir, hasta poder confirmar que la afectación de la población ha sido nula o irrelevante.', '', '', '0'),
	('532', '101', '337', '1', 'Ocurrencia de fenómenos sísmicos ampliamente sentidos por la población, sin ocasionar víctimas ni daños materiales relevantes, pero que requerirá de las autoridades y órganos competentes una actuación coordinada, dirigida a intensificar la información a los
ciudadanos sobre dichos fenómenos. ', '', '0', '0'),
	('533', '101', '337', '2', 'Ocurrencia de fenómenos sísmicos, cuya atención, en lo relativo a la protección de personas y bienes, puede quedar asegurada mediante el empleo de los medios y recursos disponibles en las zonas afectadas. ', '', '1', '1'),
	('534', '101', '337', '3', ' Ocurrencia de fenómenos sísmicos que por la gravedad de los daños ocasionados, el número de víctimas o la extensión de las áreas afectadas, hacen necesario, para el socorro y protección de personas y bienes, el concurso de medios, recursos o servicios ubicados fuera de dichas áreas. ', '', '2', '2'),
	('535', '101', '337', '4', ' Emergencias sísmicas en las que, habiéndose considerado que está en juego el interés nacional, así sean declaradas por el Ministro del Interior.', '', '3', '3'),
	('536', '101', '337', '5', 'Se iniciarán las primeras tareas de rehabilitación en las zonas afectadas, así como el realojo provisional de las personas afectadas y se adoptarán todas las medidas necesarias para el retorno a la normalidad.', '', '4', '-'),
	('537', '101', '338', '1', 'Fase consecutiva a la de emergencia que se prolongará hasta el restablecimiento de las condiciones mínimas imprescindibles para el retorno a la normalidad en las zonas afectadas por el terremoto.', '', '', '0'),
	('538', '102', '339', '1', 'Tiene como objetivo inducir a un estado de mayor atención y
vigilancia sobre los hechos y circunstancias que la provocan, y también la disminución de los tiempos de respuesta para que la actuación de los medios sea más rápida y permita mantener la atención para recibir
nuevas informaciones.', '', '', '0'),
	('539', '102', '340', '1', 'Cuando se produzca una emergencia cuyo ámbito de actuación sea local, siendo suficientes para contenerla los
medios adscritos al PEMU o PAM o a la competencia de la autoridad local, sin que se considere necesario activar los medios del PLATERGA a otros
niveles.', '0', '', '0'),
	('540', '102', '340', '2', 'Cuando se produzca una
emergencia cuyo ámbito de actuación sea local en aquellos casos en los que no exista PEMU homologado y la capacidad de respuesta por parte del ayuntamiento afectado no esté planificada.', 'OE', '', ''),
	('541', '102', '340', '3', 'Cuando se produzca una
emergencia cuyo ámbito de actuación sea supralocal o siendo local cuando los medios locales para contenerla sean claramente insuficientes desplazándose medios ajenos de forma generalizada, sin que se considere necesario activar el PLATERGA a otros niveles.', '1', '', '1'),
	('542', '102', '340', '4', 'Emergencia cuyo ámbito de actuación sea local, supralocal, provincial o
autonómico. Se llega a este nivel de emergencia por inicio súbito de una
emergencia con un nivel de gravedad elevado, por evolución de otros
niveles o porque los medios adscritos al Plan en niveles inferiores sean
insuficientes para contener la emergencia, una vez evaluado el riesgo y efectuada la propuesta por la AXEGA o por propia iniciativa de la Dirección
en su caso.', '2', '', '2'),
	('543', '102', '340', '5', 'Cuando se produzca una emergencia declarada como
de interés gallego por el Consello de la Xunta. ', 'IG', '', '2'),
	('544', '102', '340', '6', 'Cuando se produzca una emergencia en la que exista Interés Nacional, según se dispone en la Norma Básica de Protección Civil. ', '3', '', '3'),
	('545', '103', '341', '1', 'Provocada por uno o varios incendios forestales que, en  su evolución previsible, puedan afectar sólo a bienes; y puedan ser controlados con los medios y recursos del propio plan local o de Comunidad Autónoma.', '', '0', '0'),
	('546', '103', '341', '2', 'Provocada por uno o varios incendios forestales que en 
su evolución previsible, puedan afectar gravemente a bienes y, afectar levemente a la población y bienes de naturaleza no forestal y puedan ser controlados con los 
medios y recursos del plan de Comunidad Autónoma.', '', '1', '1'),
	('547', '103', '341', '3', 'Provocada por uno o varios incendios forestales que, en 
su evolución previsible, puedan afectar gravemente a la población y bienes de naturaleza no forestal, exigiendo la adopción de medidas; y pueda ser 
necesario que, sean incorporados medios extraordinarios.', '', '2', '2'),
	('548', '103', '341', '4', 'Correspondiente y consecutiva a la declaración de emergencia de interés nacional por el Ministro del Interior.', '', '3', '3'),
	('549', '103', '342', '1', 'Una vez controlado el incendio, se realizan las labores de liquidación y vigilancia de la zona afectada para evitar que el incendio se reproduzca.', '', '', '0'),
	('550', '104', '343', '1', 'Fase identificada con una situación que, por evolución desfavorable, puede dar lugar a una situación de emergencia. El objeto de esta fase es alertar a las autoridades y servicios implicados, así como informar a la población.', '', '', '0'),
	('551', '104', '344', '1', 'Cuando los datos meteorológicos e hidrológicos
permitan prever la inminencia de inundaciones, con peligro para personas y/o bienes.', '', '0', '0'),
	('552', '104', '344', '2', 'Situación en la que se han producido inundaciones en zonas localizadas, cuya atención puede quedar asegurada mediante el empleo de los medios y recursos disponibles en las zonas afectadas.', '', '1', '1'),
	('553', '104', '344', '3', 'Superan la capacidad de atención de los
medios y recursos locales o, aún sin producirse esta última circunstancia, los datos pluviométricos e
hidrológicos y las predicciones meteorológicas permiten prever una extensión o agravamiento
significativo.', '', '2', '2'),
	('554', '104', '344', '4', 'Emergencias que, habiéndose considerado que está en juego el interés nacional, así sean declaradas por el ministro de Interior.', '', '3', '3'),
	('555', '104', '345', '1', 'Se prolonga hasta el restablecimiento de las
condiciones mínimas imprescindibles para un retorno a la normalidad en las zonas afectadas por la
inundación', '', '', '0'),
	('556', '105', '346', '1', 'No se han producido daños, o estos son muy localizados, pudiendo bastar con un seguimiento y la movilización de algunos medios y recursos adscritos al plan.', '', '0', '0'),
	('557', '105', '347', '1', 'Pueden ser controlados con los medios adscritos al plan o medios propios de la Administración Pública responsable de la dirección de la Emergencia.', '', '1', '1'),
	('558', '105', '347', '2', 'Respectivo a la CCAA, y puede requerir la asistencia de medios de otras Adminsitraciones públicas no asigandos al plan, o movilizables por otras administraciones.', '', '2', '2'),
	('559', '105', '347', '3', 'Emergencias de Interés Nacional declaradas por la persona titular del Mnisterio del Interior.', '', '3', '3'),
	('560', '105', '348', '1', 'Consecutiva a la emegrencia, aunque pueden coincidir cuando las actuaciones sean compatibles con la intervención, y se prolonga hasta el restablecimiento de los servicios básicos.', '', '', '0'),
	('561', '106', '349', '1', 'Se produce sin ocasionar víctimas ni daños, por lo que,  está caracterizada por el seguimiento instrumental y el estudio de dichos fenómenos, y el proceso de información.', '', '', '0'),
	('562', '106', '349', '2', 'Ocurrencia de fenómenos sísmicos sentidos por la población y requerirá de las autoridades y órganos
competentes una actuación coordinada, dirigida a intensificar la información a
los ciudadanos sobre dichos fenómenos.', '', '0', '0'),
	('563', '106', '350', '1', 'Fenómenos sísmicos,
en la que la protección, puede
quedar asegurada mediante el empleo de los medios y recursos disponibles en
las zonas afectadas.', '', '1', '1'),
	('564', '106', '350', '2', 'Fenómenos sísmicos que, por la gravedad, hace necesario,
el concurso de medios, recursos o servicios ubicados fuera de dichas áreas, siendo suficientes los existentes en la comunidad autónoma. ', '', '2', '2'),
	('565', '106', '350', '3', ' En caso de que el Consejo de la Xunta así lo estime, se declarará
emergencia de interés gallego, tal y como se dispone en la ley 5/2007, de emergencias de Galicia, siempre que no aparezcan circunstancias que le
otorguen carácter de interés nacional (situación 3).', '', 'IG', '2'),
	('566', '106', '350', '4', 'Emergencias que habiéndose considerado que está en juego el interés nacional, así sean declaradas por el Ministro de Interior. ', '', '3', '3'),
	('567', '106', '351', '1', 'Fase consecutiva a la de emergencia que se prolongará hasta el restablecimiento de las condiciones mínimas imprescindibles para el retorno a la normalidad en las zonas afectadas por el terremoto. ', '', '', '0'),
	('568', '107', '352', '1', 'Riesgo de producirse algún tipo de contaminación con posibilidad de extenderse en una amplia zona de la costa o afectar a recursos
sensibles o zonas declaradas a proteger.', '', '', '0'),
	('569', '107', '353', '1', 'Contaminación marina de magnitud y peligro mínimo, y esté fuera del ámbito de aplicación de los planes interiores marítimos o de los locales.', 'Mínimo', '1', '1'),
	('570', '107', '353', '2', 'Cuando sea  considerado oportuno por parte de la 
dirección del Plan Camgal, por circunstancias de
vulnerabilidad de la zona afectada. Serán requeridos medios adscritos a plan ámbito estatal sin estar éstos activados.', 'Medio', '2', '2'),
	('571', '107', '353', '3', 'Capacidad de respuesta del Plan Camgal insuficiente para combatir la contaminación.
La contaminación supera los límites de aplicación del Plan Camgal.', 'Máximo', '3', '3'),
	('572', '107', '354', '1', 'Impulsar las acciones necesarias para rehabilitación y seguimiento de las zonas
dañadas, limpieza y retirada de los residuos, descontaminación de los equipos.', '', '', '0'),
	('573', '108', '355', '1', 'Referida a aquellas emergencias en las que se han producido daños muy localizados y de ámbito municipal controladas mediante respuesta local.', '', '0', '0'),
	('574', '108', '356', '1', 'Situación en la que se han producido daños moderados y, en la que para su control, pueda ser necesaria la activación del Plan, con la intervención de medios y recursos propios o asignados del PLATERCAM. ', '', '1', '1'),
	('575', '108', '356', '2', ' Situación en la que se han producido daños extensos y se necesita unas medidas
para la protección. Es necesaria la participación de medios de intervención no asignados al
PLATERCAM y pueden ser proporcionados por la Administración del Estado o Administraciones Locales.', '', '2', '2'),
	('576', '108', '356', '3', 'Cuando se declare el interés nacional, y así sean declarados por el Ministro del Interior o a instancia de la Comunidad de Madrid. ', '', '3', '3'),
	('577', '108', '357', '1', 'Cuando los factores desencadenantes de la
situación desaparecen, y sus consecuencias dejan de ser un peligro y se prolongará hasta el restablecimiento de las condiciones mínimas
imprescindibles para el retorno a la normalidad de la zona afectada.', '', '', '0'),
	('578', '109', '358', '1', 'Situación de emergencia provocada por incendios forestales que puedan afectar sólo a bienes de naturaleza forestal y puedan ser
controlados con los medios y recursos del propio plan local o de Comunidad Autónoma.', '', '0', '0'),
	('579', '109', '358', '2', 'Situación de emergencia provocada por incendios forestales que puedan afectar gravemente a bienes forestales y, en su caso,
afectar levemente a la población y bienes de naturaleza no forestal y puedan ser controlados con los medios y recursos del plan especial de Comunidad Autónoma, o para cuya extinción pueda ser necesario que sean incorporados medios
extraordinarios. ', '', '1', '1'),
	('580', '109', '358', '3', 'Situación de emergencia provocada por incendios forestales que puedan afectar gravemente a la población y bienes, exigiendo la adopción de medidas; y pueda ser necesario que, a solicitud del órgano competente de la Comunidad Autónoma, sean
incorporados medios extraordinarios, o puedan comportar situaciones que deriven hacia el
interés nacional.', '', '2', '2'),
	('581', '109', '358', '4', 'Situación de Emergencia correspondiente y consecutiva a la declaración de Emergenca de Interés Nacional por el Ministro del Interior.', '', '3', '3'),
	('582', '110', '359', '1', 'Caracterizada por la existencia de información hidrológica y meteorológica que indica la posibilidad de ocurrencia de sucesos y/o situaciones, que por evolución desfavorable, pueden dar lugar a inundaciones. Su objetivo es alertar.', '', '', '0'),
	('583', '110', '360', '1', 'Cuando los datos meteorológicos e
hidrológicos permitan prever la inminencia de inundaciones en el ámbito del Plan, con peligro
para personas o bienes, pudiendo ser resuelta por los
medios municipales y/o por los medios de la Comunidad Autónoma adscritos al Plan Municipal.', '', '0', '0'),
	('584', '110', '360', '2', 'Cuando se han producido inundaciones que afectan a más de un término municipal, o en zonas localizadas, pudiendo ser resuelta con los medios y recursos locales y de la Comunidad
Autónoma no adscritos al Plan Municipal.', '', '1', '1'),
	('585', '110', '360', '3', 'Situación en la que se han producido inundaciones que superan la capacidad de
atención de los medios y recursos locales, y se incorporan medios estatales no asignados al plan.', '', '2', '2'),
	('586', '110', '360', '4', 'Emergencias que, habiéndose considerado que está en juego el interés nacional,
así sean declaradas por el Ministro de Interior.', '', '3', '3'),
	('587', '110', '361', '1', 'Fase consecutiva a la de emergencia que se prolongará hasta el restablecimiento de las condiciones mínimas
imprescindibles para un retorno a la normalidad en las zonas afectadas por la inundación. A esta fase le siguen
las tareas de rehabilitación de mayor importancia.', '', '', '0'),
	('588', '111', '362', '1', 'se identifica con la existencia de informaciones procedentes de servicios de
prevención y alerta o de los servicios ordinarios de intervención que, por evolución desfavorable de un determinado accidente en el que estén involucradas mercancías peligrosas, pudiesen ser generadoras de una emergencia.', '', '', '0'),
	('589', '111', '363', '1', 'Referida a aquellos accidentes que pueden ser controlados con los medios disponibles y que, aún en su evolución más desfavorable, no suponen peligro.', '', '0', '0'),
	('590', '111', '363', '2', 'Referida a aquellos accidentes que pudiendo ser controlados con los medios de
intervención disponibles, requieren de la puesta en práctica de medidas para la
protección.', '', '1', '1'),
	('591', '111', '363', '3', 'Accidentes que para su control o la puesta en práctica de las
necesarias medidas, se prevé el concurso de medios de intervención no asignados al Plan de la Comunidad de Madrid , a proporcionar por la organización del plan estatal. ', '', '2', '2'),
	('592', '111', '363', '4', 'Referida a aquellos accidentes en el transporte de mercancías peligrosas que
habiéndose considerado que está implicado el interés nacional así sean
declarados por el Ministro de Interior.', '', '3', '3'),
	('593', '111', '364', '1', 'El Director del Plan declarará el fin de la emergencia una vez comprobado e informado por el Jefe del PMA que han desaparecido o se han reducido suficientemente las causas que provocaron la activación
del Plan y que se han restablecido los niveles normales de seguridad y los servicios mínimos a la
población.', '', '', '0'),
	('594', '112', '365', '1', 'Se caracteriza por el seguimiento de los
fenómenos y, por el consiguiente proceso de intercambio de información con los órganos y autoridades competentes en materia de protección civil, así como por la información a la población, en general, en caso de ser necesaria. Y, destacar que, no existe riesgo para la población.', '', '0', '0'),
	('595', '112', '366', '1', 'Cuando las consecuencias derivadas de la emergencia se puedan controlar con los medios y recursos asignados a dicho plan.', '', '1', '1'),
	('596', '112', '366', '2', 'Cuando se requieran medios y recursos no asignados a dicho plan. ', '', '2', '2'),
	('597', '112', '366', '3', 'Cuando la emergencia sea declarada de interés nacional por concurrir alguna de las circunstancias contenidas en el capítulo I (apartado 1.2.) de la Norma Básica de protección civil, o cuando lo solicite la Comunidad Autónoma. ', '', '3', '3'),
	('598', '112', '367', '1', 'Una vez se verifique que han desaparecido o
reducido suficientemente las causas que provocaron la activación del plan y que se han restablecido los niveles normales de seguridad y los servicios mínimos a la población.', '', '', '0'),
	('599', '113', '368', '1', 'Existencia de información meteorológica que indica la posibilidad de ocurrencia de sucesos y/o situaciones que, por evolución desfavorable, pueden dar lugar a emergencias por inclemencias invernales. Su objetivo es alertar.', '', '0', '0'),
	('600', '113', '369', '1', 'Con afección a los servicios básicos, o bien en zonas localizadas cuya respuesta y atención pueda quedar asegurada mediante el
empleo de los medios y recursos locales disponibles.', '', '1', '1'),
	('601', '113', '369', '2', 'Quedan cortadas las carreteras y aisladas las poblaciones y/o
pueden producirse daños que pueden afectar a personas y bienes. Además, se preve una extensión de la situación, pudiéndose ocasionar daños
significativos, que requieran la aplicación integral del Plan bajo la dirección de la Comunidad de Madrid, pudiendo resultar necesario que, a solicitud de la Dirección del Plan, sean incorporados medios estatales no asignados al
mismo. ', '', '2', '2'),
	('602', '113', '369', '3', 'El Plan se activará en Situación 3 en aquellas situaciones de emergencia en las que se declare el interés
nacional por concurrir alguna de las circunstancias contenidas en el capítulo VII, del título II (Artículo 28) de Ley 17/2015, de 9 de julio, del Sistema Nacional de Protección Civil. ', '', '3', '3'),
	('603', '113', '370', '1', 'La vuelta a la normalidad implica que, aunque subsistan una serie de secuelas, no es necesaria la participación de los Grupos de Acción, procediéndose al repliegue escalonado de las unidades intervinientes. Y, se ponen en práctica, las tareas de rehabilitación.', '', '', '0'),
	('604', '114', '371', '1', 'Podrá estar determinada por la previsión de la evolución desfavorable de una situación concreta o bien por la activación de los mecanismos establecidos para declarar una alerta precoz.', '', '', '0'),
	('605', '114', '372', '1', 'Aquellas emergencias que, o bien afectan a más de un término municipal, o
por su gravedad o alcance precisan de la intervención de los recursos de la Comunidad Autónoma no adscritos previamente al Plan Municipal al verse totalmente superados los recursos municipales.', '', '1', '1'),
	('606', '114', '372', '2', 'Son aquellas situaciones en las que se prevé que , sean incorporados medios estatales no asignados
previamente al Plan, o bien que la gravedad de la situación pueda derivar hacia el
interés nacional.', '', '2', '2'),
	('607', '114', '372', '3', 'Son aquellas emergencias en las que la situación es declarada de interés nacional por el Ministro del Interior, de acuerdo con los supuestos establecidos en la Norma Básica. ', '', '3', '3'),
	('608', '114', '373', '1', 'Aquellos servicios esenciales para la población, que se hubiesen visto afectados por la emergencia, se encuentren de nuevo operativos, aun cuando algunos équipos deban seguir
trabajando dentro de su cometido habitual para restablecer la normalidad.', '', '', '0'),
	('609', '115', '374', '1', 'Aquellas circunstancias en las que se prevea el desencadenamiento de episodios extraordinarios, que
pueden derivar hacia una situación de emergencia por incendios forestales.', '', '', '0'),
	('610', '115', '375', '1', 'Situación de emergencia provocada por incendios forestales que, en su evolución previsible, puedan afectar sólo a bienes de naturaleza forestal; y puedan ser controlados con los medios y recursos del propio plan local o de la Comunidad Autónoma.', '', '0', '0'),
	('611', '115', '375', '2', 'Situación de emergencia provocada por incendios forestales que en su evolución previsible, puedan afectar gravemente a bienes forestales y, en su caso, afectar levemente a la población y bienes de naturaleza no forestal y puedan ser controlados con los medios y recursos del plan INFOMUR.', '', '1', '1'),
	('612', '115', '375', '3', 'Situación de emergencia provocada por incendios forestales que, en su evolución previsible, puedan afectar gravemente a la población y bienes de naturaleza no forestal, exigiendo medidas, y a solicitud del Director del Plan INFOMUR, sean incorporados medios extraordinarios, o puedan comportar situaciones que deriven hacia el interés nacional.', '', '2', '2'),
	('613', '115', '375', '4', 'Situación de emergencia correspondiente y consecutiva a la declaración de emergencia de interés nacional por el
Ministro del Interior.', '', '3', '3'),
	('614', '116', '376', '1', 'Es la fase caracterizada por la existencia de información sobre la posibilidad de ocurrencia de sucesos capaces de dar lugar a inundaciones. El objeto de esta fase es alertar e informar.', '', '', '0'),
	('615', '116', '377', '1', 'Aquellas situaciones en las que se permitan prever inundaciones, con peligro para personas y bienes o bien aquellas en las que se podría haber producido la inundación, aunque muy localizada, afectando a un único término municipal y pudiendo ser resuelta por los medios municipales y/o por los medios de la Comunidad Autónoma adscritos al Plan Municipal.', '0', '', '0'),
	('616', '116', '377', '2', 'Situación en la que se han producido inundaciones que afectan a más de un término municipal y por su nivel de gravedad precisan de la intervención de los recursos de la Comunidad Autónoma no
adscritos al Plan Municipal.', '1', '', '1'),
	('617', '116', '377', '3', 'Situación en la que se han producido inundaciones en las que, se prevé que a solicitud de la Dirección del Plan, sean
incorporados medios estatales no asignados al Plan o bien los datos pluviométricos e hidrológicos y las predicciones meteorológicas permiten una
extensión o agravamiento de la situación que pudiera derivar hacia el interés nacional.', '2', '', '2'),
	('618', '116', '377', '4', ' Emergencias que, habiéndose considerado que está en juego el interés nacional, así sean declaradas por el Ministro de Interior, de acuerdo
con los supuestos establecidos en la Norma Básica.', '3', '', '3'),
	('619', '116', '378', '1', 'Se prolonga hasta el restablecimiento de las condiciones mínimas imprescindibles para el retorno a la normalidad
en las zonas afectadas por la inundación.', '', '', '0'),
	('620', '117', '379', '1', 'Es la zona en la que las consecuencias del accidente, aunque puedan
producirse aspectos perceptibles para la población, no requieren más medidas de
intervención.', '', '', '0'),
	('621', '117', '380', '1', 'Aquellos accidentes que pueden ser controlados por los medios disponibles y que, aun en su evolución más
desfavorable, no suponen peligro ', '', '0', '0'),
	('622', '117', '380', '2', 'Accidentes que pudiendo ser
controlados con los medios de intervención disponibles, requieren de la puesta en
práctica de medidas para la protección.', '', '1', '1'),
	('623', '117', '380', '3', 'Accidentes que para su
control o la puesta en práctica de las necesarias medidas de protección de las
personas, los bienes o el medio ambiente se prevé el concurso de medios de
intervención no asignados a este Plan, a proporcionar por la organización del Plan
Estatal. ', '', '2', '2'),
	('624', '117', '380', '4', 'Accidentes en el transporte de mercancías peligrosas
que habiéndose considerado que está implicado el interés nacional así sean
declarados por el Ministro de Interior. ', '', '3', '3'),
	('625', '117', '381', '1', 'La declaración de fin de la emergencia conlleva  la vigilancia preventiva en el lugar o zona afectada por el
accidente y se sigan realizando las tareas reparadoras y de rehabilitación. ', '', '', '0'),
	('626', '118', '382', '1', ' Los fenómenos sísmicos se producen sin ocasionar víctimas ni daños materiales relevantes, por lo que, está caracterizada
fundamentalmente por el seguimiento y el estudio.', '', '', '0'),
	('627', '118', '382', '2', 'Ocurrencia de fenómenos sísmicos sentidos por la población y requerirá de las autoridades y órganos
competentes una actuación coordinada.', '', '0', '0'),
	('628', '118', '383', '1', 'Fenómenos sísmicos, en la que la protección de personas y bienes puede quedar asegurada mediante el empleo de los medios y recursos de los municipios afectados y los de la Comunidad Autónoma.', '', '1', '1'),
	('629', '118', '383', '2', 'Fenómenos sísmicos que hacen necesario, para el socorro y protección de personas y bienes, el concurso de medios, recursos o servicios no asignados a este Plan, a proporcionar por la
organización del Plan Estatal.', '', '2', '2'),
	('630', '118', '383', '3', 'Emergencias que habiéndose considerado que está en juego el interés nacional, así sean declaradas por el Ministro de Interior.', '', '3', '3'),
	('631', '118', '384', '1', 'Fase que se prolongará hasta el restablecimiento
de las condiciones mínimas imprescindibles para el retorno a la normalidad en las zonas
afectadas por el terremoto. Y, se realizarán las primeras tareas de rehabilitación.', '', '', '0'),
	('632', '119', '385', '1', 'Se podrá mantener una comunicación continua con los organismos afectados con el fin de comprobar la materialización o no del fenómeno meteorológico y en su caso, determinar la magnitud y alcance del mismo', '', '', '0'),
	('633', '119', '386', '1', 'No existe ningún riesgo meteorológico. ', 'Verde', '', '0'),
	('634', '119', '386', '2', 'No existe riego meteorológico para la población en general aunque si para
alguna actividad concreta (fenómenos meteorológicos habituales pero potencialmente
peligrosos) o localización de alta vulnerabilidad, como una gran conurbación. ', 'Amarillo', '', '0'),
	('635', '119', '386', '3', 'Existe un riesgo meteorológico importante (fenómenos meteorológicos no
habituales y con cierto grado de peligro para las actividades usuales). ', 'Naranja', '', '1'),
	('636', '119', '386', '4', 'El riesgo meteorológico es extremo (fenómenos meteorológicos no habituales de intensidad excepcional y con un nivel de riesgo para la población muy alto). ', 'Rojo', '', '2'),
	('637', '120', '387', '1', 'Aquellas circunstancias en las que se prevea el
desencadenamiento de sucesos que pueden derivar hacia una situación de emergencia de contaminación marina accidental. ', '', '', '0'),
	('638', '120', '388', '1', 'Cuando estando activado el Plan, la situación puede ser controlada con los medios disponibles, precisando la actuación del Plan Territorial de Contingencias sólo en funciones de
seguimiento y apoyo.', '0', '', '0'),
	('639', '120', '388', '2', 'Situaciones que precisan la
intervención de los recursos de la Comunidad Autónoma no adscritos previamente al
Plan Interior de Contingencias o Plan de Actuación Municipal.', '1', '', '1'),
	('640', '120', '388', '3', 'Son aquellas situaciones en las que se prevé que, a solicitud de la Dirección del Plan Territorial, sean incorporados medios estatales no asignados previamente al Plan, o por su gravedad, se pueda activar el Plan Nacional de Contingencias por Contaminación Marina
Accidental en labores de seguimiento y apoyo.', '2', '', '2'),
	('641', '120', '388', '4', 'Situación en la que por su gravedad la emergencia requiera la dirección de la
Administración General del Estado y se active siempre el Plan Nacional de
Contingencias por Contaminación Marina Accidental. ', '3', '', '3'),
	('642', '120', '389', '1', 'Cuando el origen del accidente esté controlado, y se hayan minimizado las consecuencias del mismo, se declarará esta fase. Y, se continuarán con las labores de restauración y rehabilitación del entonro.', '', '', '0'),
	('643', '121', '390', '1', 'Se corresponde con la presencia de una serie de factores o parámetros que, en función de su evolución desfavorable, podrían dar lugar a una situación de emergencia.', '0', '', '0'),
	('644', '121', '391', '1', 'a se corresponde con la evolución desfavorable de aquellos riesgos que
hubieran motivado la fase de Preemergencia o con la confirmación de una emergencia de la que
había indicios y, en general, siempre que se haya producido o se estén produciendo daños de consideración a un determinado número de personas o bienes como consecuencia de cualquier tipo de riesgo.', '1', '', '1'),
	('645', '121', '391', '2', 'Cuando la evolución de la emergencia o el tipo de situación que la ha originado puede requerir la actuación de medios no previstos en el Plan, de otras Comunidades
Autónomas, del Estado o incluso medios de otros países. ', '2', '', '2'),
	('646', '121', '391', '3', 'Cuando la emergencia se declara que afecta al interés nacional, de acuerdo a lo previsto en el capítulo I, apartado 1.2, de la Norma Básica de Protección Civil.', '3', '', '3'),
	('647', '121', '392', '1', 'Es la fase que sigue a la de emergencia, y que se prolongará hasta el restablecimiento de las
condiciones mínimas imprescindibles para el retorno a la normalidad en las zonas afectadas por la emergencia. Se realizarán las primeras medidas de rehabilitación.', '', '', '0'),
	('648', '122', '393', '1', 'Se declara cuando se producen situaciones que requieren un seguimiento intensivo.', '', '', '0'),
	('649', '122', '394', '1', 'Situación en la que no se han producido daños graves, pero ya hay un incendio forestal muy localizado, que pueden afectar a bienes de naturaleza forestal, y puede ser controlado por los medios y recursos del plan.', '', '0', '0'),
	('650', '122', '395', '1', 'Situación de emergencia provocada por incendios forestales que en su evolución previsible, puedan afectar gravemente a bienes forestales y, en su caso, afectar levemente a la población y bienes de naturaleza no forestal y puedan ser controlados con los medios y recursos del plan.', '', '1', '1'),
	('651', '122', '395', '2', 'Situación de emergencia provocada por incendios forestales que puedan afectar gravemente a la población y bienes, exigiendo la adopción de medidas; y pueda ser necesario que, a solicitud del órgano competente de la Comunidad Autónoma, sean incorporados medios extraordinarios, o puedan comportar situaciones que deriven hacia el
interés nacional.', '', '2', '2'),
	('652', '122', '395', '3', 'Cuando se declare el interés nacional, y así sean declarados por el Ministro del Interior o a instancia de la Comunidad de Madrid. ', '', '3', '3'),
	('653', '122', '396', '1', 'Se dará por finalizada cuando haya desaparecido el origen del mismo, y se prolongará hasta que se recuperen las condiciones mínimas.', '', '', '0'),
	('654', '123', '397', '1', 'Fase identificada con una situación que, por evolución desfavorable, puede dar lugar a
una situación de emergencia. El objeto de esta fase es alertar a las autoridades y servicios
implicados, así como informar.', '', '', '0'),
	('655', '123', '398', '1', 'Cuando los datos Meteorológicos e
Hidrológicos permitan prever la inminencia de inundaciones, con peligro para personas y
bienes. ', '', '0', '0'),
	('656', '123', '398', '2', 'Situación en la que se han producido inundaciones en zonas localizadas, cuya atención
puede quedar asegurada mediante el empleo de los medios y recursos disponibles en las zonas afectadas, si bien es necesaria el seguimiento supramunicipal.', '', '1', '1'),
	('657', '123', '398', '3', 'Situación en la que se han producido inundaciones que superan la capacidad de atención de los medios y recursos ubicados en las zonas afectadas o, se prevé una extensión o agravamiento.', '', '2', '2'),
	('658', '123', '398', '4', 'Emergencias en las que ha sido declarado el interés nacional.', '', '3', '3'),
	('659', '123', '399', '1', 'Es una fase consecutiva a la de emergencia, que se prolonga hasta el restablecimiento de
las condiciones mínimas imprescindibles para un retorno a la normalidad en las zonas
afectadas por la inundación. ', '', '', '0'),
	('660', '124', '400', '1', 'Es aquella en la que las consecuencias de los accidentes provocan efectos
que, aunque perceptibles por la población, no justifican la actuación, excepto para los
grupos críticos de población.', '', '', '0'),
	('661', '124', '401', '1', 'Accidentes que pueden ser controlados por los medios disponibles y que, no suponen peligro.', '', '0', '0'),
	('662', '124', '401', '2', 'Accidentes que, pudiendo ser controlados con los medios de intervención disponibles en Navarra, requieren de la puesta en práctica de medidas para la protección. ', '', '', '1'),
	('663', '124', '401', '3', 'Accidentes que para su control o la puesta en práctica de las medidas necesarias  se prevé el concurso de medios de intervención no asignados al Plan de la Comunidad Foral, a proporcionar por la
organización del Plan Estatal. ', '', '', '2'),
	('664', '124', '401', '4', 'Referida a aquellos accidentes en el transporte de mercancías peligrosas, que habiéndose
considerado que está implicado el interés nacional, así sean declarados por el Ministro de Interior.', '', '', '3'),
	('665', '124', '402', '1', 'La emergencia finalizará una vez que sean recogidos los residuos contaminantes provocados por el accidente de la calzada/vía férrea o sus alrededores. Y, conllevará medidas de rehabilitación.', '', '', '0'),
	('666', '125', '403', '1', 'Situación de emergencia en la que los riesgos se limitan a la propia instalación y
pueden ser controlados por los medios disponibles en el correspondiente plan de emergencia interior o plan de autoprotección.', '', '0', '0'),
	('667', '125', '404', '1', 'Situación de emergencia en la que se prevé
que los riesgos pueden afectar a las personas
en el interior de la instalación, mientras que
las repercusiones en el exterior, aunque muy
improbables, no pueden ser controladas únicamente con los recursos propios del plan de
emergencia interior o del plan de autoprotección, siendo necesaria la intervención de servicios del presente Plan.', '', '1', '1'),
	('668', '125', '404', '2', 'Situación de emergencia en la que se prevea
que los riesgos pueden afectar a las personas tanto en el interior como en el exterior de la instalación y, en consecuencia, se prevé el concurso de medios de apoyo de titularidad
estatal no asignados al presente Plan.', '', '2', '2'),
	('669', '125', '404', '3', 'Situación de emergencia en la que la naturaleza, gravedad o alcance de los riesgos requiere
la declaración del interés nacional por el Ministro del Interior.', '', '3', '3'),
	('670', '125', '405', '1', 'Fase que se corresponde con la desactivación de las situaciones de preemergencia o emergencia del Plan, pero que la situación aconseja continuar monitorizando la zona afectada, o situación de pérdida o robo de una fuente radiactiva.', '', '', '0'),
	('671', '126', '406', '1', 'Ocurrencia de
fenómenos sísmicos ampliamente sentidos por la población y requerirá una actuación coordinada, dirigida a
intensificar la información a los ciudadanos sobre dichos fenómenos. ', '', '0', '0'),
	('672', '126', '407', '1', 'Los fenómenos sísmicos, y la protección de personas y bienes puede quedar asegurada
mediante el empleo de los medios y recursos de los municipios afectados y los de
la Comunidad Foral.', '', '1', '1'),
	('673', '126', '407', '2', 'Cuando se han producido
fenómenos sísmicos que hacen necesario el concurso de medios, recursos o servicios no
asignados a este Plan, a proporcionar por la organización del Plan Estatal.', '', '2', '2'),
	('674', '126', '407', '3', 'Emergencias que,
habiéndose considerado que está en juego el interés nacional, así sean declaradas
por el Ministro de Interior.', '', '3', '3'),
	('675', '126', '408', '1', 'Es la fase que sigue a la de emergencia, y que se prolongará hasta el restablecimiento de las
condiciones mínimas imprescindibles para el retorno a la normalidad en las zonas afectadas por la emergencia. Se realizarán las primeras medidas de rehabilitación.', '', '', '0'),
	('676', '127', '409', '1', 'Con la superación de los umbrales establecidos para cada zona o municipio; así mismo, como consecuencia de las llamadas de diversos alertantes sobre inclemencias meteorológicas
que se estén produciendo, que no hayan sido advertidas por la Agencia Estatal de Meteorología.', '', '', '0'),
	('677', '127', '410', '1', 'Fase en la que sea
necesario establecer una actuación coordinada de los recursos movilizados por las
administraciones competentes, y cuya posible evolución haga previsible la necesidad de poner en práctica medidas
extraordinarias de protección.', '', '1', '1'),
	('678', '127', '410', '2', 'Requiere una activación completa del Plan ante Fenómenos Meteorológicos Adversos utilizando
medios propios, o asignados por otras administraciones. Así mismo, quedarían integrados en el Plan Especial los planes inferiores; y si la situación se agrava, la constitución del CECOPI.', '', '2', '2'),
	('679', '127', '410', '3', 'Conlleva la declaración de interés nacional. Declarará
el interés nacional el Ministro del Interior, por propia iniciativa o a instancia de la persona titular
de la Presidencia de la Comunidad Foral de Navarra o del Delegado del Gobierno.', '', '3', '3'),
	('680', '127', '411', '1', 'Se mantendrá activado mientras que en las zonas
afectadas en las que existan carencias importantes en sus servicios esenciales que impidan un retorno a la vida normal. Y, conllevará la aplicación de una serie de medidas.', '', '', '0'),
	('681', '128', '412', '1', 'Es la fase en la que la previsión meteorológica origina una alerta roja o aquella en
la que aun siendo las alertas naranjas o amarillas, la evolución desfavorable de la
situación aconseja declarar esta Fase. El objetivo se basa en alertar, para el establecimiento de unas medidas.', '', '', '0'),
	('682', '128', '413', '1', 'La nevada es generalizada en una amplia zona, hay afección a los servicios
básicos, con los medios municipales no se puede hacer frente a la situación y es
necesario el concurso de medios autonómicos.', '', '1', '1'),
	('683', '128', '413', '2', 'Se producen alteraciones graves en la prestación de los servicios esenciales. Es necesario la utilización de recursos no
ordinarios y la adopción de medidas extraordinarias.', '', '2', '2'),
	('684', '128', '413', '3', 'La nevada es de tal magnitud que exige la declaración de interés nacional.', '', '3', '3'),
	('685', '129', '414', '1', 'Aquellos incidentes que se produzcan en los
túneles o en las proximidades de sus bocas de acceso que puedan ser eficazmente
controlados con los medios de seguridad y de logística previstos y que, aun en su
evolución más desfavorable, no supongan ningún peligro.', '1', '', '1'),
	('686', '129', '414', '2', 'Aquellos incidentes que se produzcan en el
interior de los túneles o en las proximidades de sus bocas de acceso, que, aun pudiendo
ser eficazmente controlados con los medios de seguridad logística previstos en el P.E.T.,
implican la necesidad de cerrar el túnel al tráfico, sin que se haya producido un incendio
en su interior. ', '2', '', '2'),
	('687', '129', '414', '3', 'Aquellos incidentes que impliquen un incendio
en el interior de los túneles o aquellos que no produzcan incendios pero que, por su
gravedad, exijan el concurso de medios excepcionales de intervención. ', '3', '', '3'),
	('688', '130', '415', '1', 'Responde a escenarios en los que existan previsiones de posibles emergencias no manifestadas, pero que dadas las circunstancias y en
caso de una evolución desfavorable, es posible su desencadenamiento.', '', '', '0'),
	('689', '130', '416', '1', 'Se trata de emergencias de ámbito municipal controladas mediante respuesta local o foral. Se realizan funciones de seguimiento y evaluación y se garantiza la prestación de los apoyos correspondientes.', '', '0', '0'),
	('690', '130', '416', '2', 'Se trata de emergencias que tienen una afectación 
limitada, en las que puede requerirse la activación de medios y recursos propios o asignados al Plan Territorial de Protección Civil de Euskadi.', '', '1', '1'),
	('691', '130', '416', '3', 'Se trata de emergencias que sobrepasan las posibilidades de respuesta de las administraciones municipales y forales. Afectan de forma intensa a la población, sus bienes o el medio ambiente, o pudieran afectarlos de existir incertidumbre sobre su evolución.', '', '2', '2'),
	('692', '130', '416', '4', 'Situaciones en las que esté presente el interés supraautonómico
conforme a lo previsto en la Norma Básica de Protección Civil.', '', '3', '3'),
	('693', '130', '417', '1', 'La emergencia ha sido dada por finalizada sin que existan significativas posibilidades de su reactivación, y , esta fase conlleva, la puesta en práctica de medidas.', '', '', '0'),
	('694', '131', '418', '1', 'Esta fase se basa en  el seguimiento de las condiciones que inducen a prever la eventualidad de que se manifieste una situación incidental grave. Puede contemplar ocasionalmente la movilización de algunos medios y recursos operativos en función de las características de la situación.', '', '', '0'),
	('695', '131', '419', '1', 'Pueden movilizarse parte de los medios del Plan Especial de Emergencia por Riesgo de Incendios Forestales para realizar funciones de
apoyo, aviso a los grupos de acción o preparar medidas de protección.', '', '0', '0'),
	('696', '131', '419', '2', 'Aquellos escenarios que, aun
pudiendo ser controlados con los medios de extinción ordinarios, por su posible evolución, puedan
verse desbordados y sea necesario la movilización de más recursos o
se prevea la necesidad de medidas de protección como evacuar a la
población.', '', '1', '1'),
	('697', '131', '419', '3', 'Cuando las circunstancias, el desarrollo y posible evolución del incendio forestal hagan insuficientes los medios previstos en el Plan, el
daño forestal esperable sea muy importante o se prevé que amenace
seriamente a las personas y bienes no forestal.', '', '2', '2'),
	('698', '131', '419', '4', 'Se determinará para los incendios en los que se considere que está en juego el interés estatal, y así sea declarada por el
Ministro/a competente en materia de Protección Civil y Emergencias.', '', '3', '3'),
	('699', '131', '420', '1', 'Se basa en labores de recuperación necesarias en función de los daños producidos, prestando particular atención a las víctimas y afectados por la emergencia con el concurso de los servicios integrados en el Sistema Vasco de
Atención de Emergencias.', '', '', '0'),
	('700', '132', '421', '1', 'Se declara desde el momento en que se recibe cualquiera de los avisos del Sistema de Previsión del peligro por Inundaciones y ante la previsión de que por evolución desfavorable puedan
dar lugar a una situación de emergencia.', '', '', '0'),
	('701', '132', '422', '1', 'Se corresponderá con incidentes, muy localizados de carácter súbito y corta evolución sin daños a personas
o con daños materiales leves, y que son controlables en todo caso mediante respuesta local.', '', '0', '0'),
	('702', '132', '422', '2', 'Emergencias que tienen una afectación del territorio limitada con población, bienes o medio ambiente en situación moderadamente vulnerable. Son situaciones en las que se
requiere una respuesta coordinada por parte de la CAV.', '', '1', '1'),
	('703', '132', '422', '3', 'Emergencia que sobrepasa las posibilidades de respuesta de las administraciones municipales y forales.
Son situaciones muy graves que afectan de forma intensa. ', '', '2', '2'),
	('704', '132', '422', '4', 'Situaciones en las que esté presente el interés supraautonómico conforme a lo previsto en
la Norma Básica de Protección Civil.', '', '3', '3'),
	('705', '132', '423', '1', 'Restablecimiento de las condiciones mínimas imprescindibles
que permitan satisfacer las necesidades básicas de subsistencia en las zonas afectadas.', '', '', '0'),
	('706', '133', '424', '1', 'Aquella emergencia
en la que las consecuencias del accidente, aunque puedan producirse aspectos perceptibles para la
población, no requieren más medidas de intervención que la de información a aquélla, salvo para ciertos grupos de personas.', '', '', '0'),
	('707', '133', '425', '1', 'Accidentes que pueden ser controlados por los medios disponibles y que, aún en su evolución más desfavorable, no suponen peligro.', '', '0', '0'),
	('708', '133', '425', '2', 'Accidentes que pudiendo ser controlados con los medios de
intervención disponibles, requieren de la puesta en práctica de medidas para la protección de la población que esté o que puedan verse amenazados por los efectos
derivados del accidente.', '', '1', '1'),
	('709', '133', '425', '3', 'Accidentes que para su control o la puesta en práctica de las necesarias medidas de protección de las personas, los bienes o el medio ambiente se prevé el concurso de medios
de intervención no asignados a este Plan, a proporcionar por la organización del Plan estatal.', '', '2', '2'),
	('710', '133', '425', '4', 'Emergencia que habiéndose
considerado que está implicado el interés nacional así sean declarados por el Ministro de Interior.', '', '3', '3'),
	('711', '133', '426', '1', ' Cuando los factores desencadenantes
desaparezcan,y se realicen medidas.', '', '', '0'),
	('712', '134', '427', '1', 'Riesgos limitados a la propia instalación y que pueden ser controlados por los medios disponibles en el correspondiente Plan de Emergencia interior o Plan de Autoprotección.', '', '0', '0'),
	('713', '134', '428', '1', ' Situación de emergencia en la que se prevé que los riesgos pueden afectar a las personas en el interior de la instalación, mientras que las repercusiones en el exterior, aunque muy improbables, no pueden ser controladas únicamente con los recursos propios del Plan de Emergencia Interior o del Plan de Autoprotección, siendo necesaria la intervención de servicios del Plan Autonómico.', '', '1', '1'),
	('714', '134', '428', '2', 'Situación de emergencia en la que se prevea que los riesgos pueden afectar a las personas tanto en el interior como en el exterior de la instalación y, en consecuencia, se prevé el concurso de medios de apoyo de titularidad estatal no asignados al Plan Autonómico.', '', '2', '2'),
	('715', '134', '428', '3', 'Situación de emergencia en la que la naturaleza, gravedad o alcance de los riesgos requiere la declaración del interés nacional por el Ministro del Interior. ', '', '3', '3'),
	('716', '134', '429', '1', 'El Plan se desactivará cuando el foco de peligro se haya eliminado y la población afectada haya sido
atendida.Si el Plan ha sido activado en Situación 1 o Situación 2, se puede pasar, si la Dirección del Plan lo cree
necesario como medida precautoria, a la Situación de seguimiento y control - 0 ó 1, o bien desactivar
directamente el Plan.', '', '', '0'),
	('717', '135', '430', '1', ' Se dá cuando ocurren fenómenos sísmicos percibidos por la población. Y, conlleva el Seguimiento de la evolución del suceso.', '', '0', '0'),
	('718', '135', '431', '1', '', '', '', ''),
	('719', '135', '431', '2', 'Cuando se han producidos fenómenos sísmicos graves.
Y, hacen necesaria la utilización de recursos, medios y servicios ubicados fuera de las zonas afectadas.', '', '2', '2'),
	('720', '135', '431', '3', 'Emergencias declaradas de interés nacional por el Ministro de Interior.', '', '3', '3'),
	('721', '135', '432', '1', 'Conllevará la aplicación de una serie de medidas para la rehabilitación de las zonas afectadas por el fenómeno sísmico.', '', '', '0'),
	('722', '136', '433', '1', 'La Fase de Alerta implica, ante un posible suceso de contaminación marina, la puesta en disposición de actuar de los medios y recursos
movilizables, de acuerdo con el grado de respuesta necesario.', '', '', '0'),
	('723', '136', '434', '1', 'La contaminación afecte, o pueda afectar, exclusivamente y de forma limitada al frente costero de una entidad local de
la CAPV, o esté dentro del ámbito de aplicación de un plan interior marítimo.', '', '0', '0'),
	('724', '136', '434', '2', 'La contaminación afecte o pueda afectar al tramo de costa
correspondiente a varios municipios, o se hubera producido fuera del ámbito de los P.I.M.. Reqeuerirá los medios disponibles de los planes.', '', '1', '1'),
	('725', '136', '434', '3', 'Cuando las circustancias del incidente, indiquen la necesidad de adoptar esta situación de emerencia. Se activará cuando los medios y recursos del plan del nivel 1, sean insuficientes, y se necesite medios extraordinarios.', '', '2', '2'),
	('726', '136', '434', '4', 'Cuando la
emergencia sea declarada de interés nacional por el ministro del Interior, según lo establecido en la Ley 17/2015 del Sistema Nacional de Protección Civil. ', '', '3', '3'),
	('727', '137', '435', '1', 'Responde a escenarios en los que existe previsión de posible
emergencia, no manifestada todavía, pero que dadas las circunstancias
y en caso de una evolución desfavorable, es posible su desencadenamiento.', '', '', '0'),
	('728', '137', '436', '1', 'Situaciones, controlables
por los operativos de una instalación aeronáutica o por el SVAE ordinario. En accidentes fuera de instalaciones aeroportuarias, la
Situación 0 se corresponderá a escenarios sin gran afectación.', '', '0', '0'),
	('729', '137', '436', '2', 'Situación en la que la emergencia no ordinaria, debido a las dimensiones del impacto producido que, si bien se puede hacer
frente con los medios y recursos ordinarios del SVAE, precisa de una Dirección y coordinación unificada, correspondiente a un Plan
Especial.', '', '1', '1'),
	('730', '137', '436', '3', 'Son situaciones muy graves que afectan de forma intensa generalmente, pudiendo existir incertidumbre sobre la evolución del episodio. Se hace precisa la intervención de gran parte de los recursos del Sistema Vasco de
Atención de Emergencias y pudiera ser necesaria la colaboración de recursos externos.', '', '2', '2'),
	('731', '137', '436', '4', 'Situaciones en las que esté presente el interés supraautonómico,
conforme a lo previsto en la Norma Básica de Protección Civil.', '', '3', '3'),
	('732', '137', '437', '1', 'En esta fase la emergencia ha sido dada por finalizada sin que
existan significativas posibilidades de su reactivación.
Corresponde a esta fase, los trabajos de rehabilitación para una vuelta a la normalidad.', '', '', '0'),
	('733', '138', '438', '1', 'Es una acción que tiene por objeto inducir al que la recibe a un estado de mayor
atención y vigilancia sobre los hechos y circunstancias que la provocan. Y, lleva implícitas las tareas de preparación que tienen por objeto disminuir los
tiempos de respuesta para una rápida intervención.', '', '', '0'),
	('734', '138', '439', '1', 'Riesgos o emergencias de ámbito municipal controladas mediante respuesta
local.En esta fase, se realiza
funciones de seguimiento y evaluación.', '0', '', '0'),
	('735', '138', '439', '2', 'Se trata de emergencias que de acuerdo con su magnitud y/o riesgo  requieren una respuesta eficaz del escalón de Comunidad Autónoma en coordinación y/u operatividad.
Es coordinado, por parte de los medios y recursos adscritos al plan.', '1', '', '1'),
	('736', '138', '439', '3', 'Emergencias que por su naturaleza o gravedad necesitan una respuesta operativa compleja y/o de gran
magnitud de la administración autonómica y sea necesario movilizar gran parte de los
medios y/o recursos considerados en el PLATERCAR.', '2', '', '2'),
	('737', '138', '439', '4', 'Cuando se presentan las circunstancias en las que está presente el Interés Nacional con arreglo a los supuestos previstos en la Norma Básica.', '3', '', '3'),
	('738', '138', '440', '1', 'Se mantendrá activado mientras que en las zonas afectadas en las que existan carencias importantes en sus servicios esenciales que impidan un retorno a la vida normal. Y, conllevará la aplicación de una serie de medidas.', '', '', '0'),
	('739', '139', '441', '1', 'Situación de emergencia provocada por incendios forestales que, en su evolución
previsible pueden afectar a bienes de naturaleza forestal y pueden ser controlados con los medios y recursos previstos en el propio INFOCAR.', '0', '', '0'),
	('740', '139', '441', '2', 'Situación de emergencia provocada por incendios forestales que en su evolución
previsible puedan afectar gravemente a bienes forestales o, en su caso, afectar levemente a la población y bienes de naturaleza no forestal y puedan ser controlados con los medios y recursos del INFOCAR.', '1', '', '1'),
	('741', '139', '441', '3', 'Situación de emergencia provocada por incendios forestales que, en su evolución
previsible requieren la movilización de toda o la mayor parte de los medios y recursos previstos en el INFOCAR, puedan afectar gravemente a la población y bienes de naturaleza no forestal,
exigiendo la adopción inmediata de medidas de protección y socorro, y pueda ser necesario que, a
solicitud del Director del Plan, sean incorporados medios extraordinarios, o puedan comportar situaciones que deriven hacia el interés nacional.', '2', '', '2'),
	('742', '139', '441', '4', 'Situación de emergencia correspondiente y consecutiva a la declaración de emergencia de interés nacional por el Ministro del Interior.', '3', '', '3'),
	('743', '139', '442', '1', 'La declaración de fin de la emergencia consta en el control y la vigilancia preventiva en el lugar o zona afectada por el incendio y se sigan realizando tareas reparadoras y de rehabilitación.', '', '', '0'),
	('744', '140', '443', '1', 'Fase caracterizada por la existencia de información sobre la posibilidad de ocurrencia de sucesos capaces de dar lugar
a inundaciones. El objetivo general de esta fase es la alerta , así como dar información.', '', '', '0'),
	('745', '140', '444', '1', 'Los datos meteorológicos e hidrológicos permiten prever la inminencia de crecidas o inundaciones en el
ámbito del Plan, con peligro para personas y bienes, se constatan daños en infraestructuras u ocurren inundaciones con consecuencias asumibles por el nivel local. Se atienden con medios locales.', '', '0', '0'),
	('746', '140', '444', '2', 'Inundaciones en zonas localizadas, superándose la capacidad de respuesta municipal, quedando la atención asegurada mediante el empleo de los medios y recursos propios o gestionados por la C.A.R', '', '1', '1'),
	('747', '140', '444', '3', 'Inundaciones en gran extensión o con grandes daños que movilizan los medios y recursos de la C.A.R. pudiendo resultar necesario apoyo externo a la C.A.R. O, cuando permiten prever una extensión o agravación significativa de las inundaciones.', '', '2', '2'),
	('748', '140', '444', '4', 'Emergencias que, habiéndose considerado que está en juego el interés nacional, así sean declaradas por el
Ministro del Interior.', '', '3', '3'),
	('749', '140', '445', '1', 'Se prolongará hasta el restablecimiento de las condiciones mínimas
imprescindibles para un retorno a la normalidad en las zonas afectadas por la inundación.
Durante esta fase se realizarán las primeras tareas de rehabilitación.', '', '', '0'),
	('750', '141', '446', '1', 'Accidentes que pueden ser controlados por
los medios disponibles y que, aun en su evolución más desfavorable, no suponen peligro.', '0', '', '0'),
	('751', '141', '446', '2', 'Accidentes que pudiendo ser controlados con los medios de intervención disponibles, requieren de la puesta en práctica de medidas para la protección de las personas, bienes o el medio ambiente que estén o puedan verse amenazadas por los efectos derivados del accidente.', '1', '', '1'),
	('752', '141', '446', '3', 'Accidentes que por su gravedad, y sus efectos requieren la plena movilización de la estructura organizativa del Plan y de todos los medios y recursos no asignados, a proporcionar por la Administración del Estado.', '2', '', '2'),
	('753', '141', '446', '4', 'Este Nivel será declarado cuando se notifique un accidente en el que esté presente el interés
nacional, con arreglo a los supuestos previstos en la Norma Básica.', '3', '', '3'),
	('754', '141', '447', '1', 'Se prolongará hasta el restablecimiento de las condiciones mínimas
imprescindibles para un retorno a la normalidad en las zonas afectadas por la inundación.
Durante esta fase se realizarán las primeras tareas de rehabilitación.', '', '', '0'),
	('755', '142', '448', '1', 'Situación de emergencia en la que los riesgos se limitan a la propia instalación y pueden ser controlados por los medios
disponibles en el correspondiente plan de emergencia interior o plan de autoprotección.
En el caso de que la emergencia radiológica no esté asociada a una instalación, será la referida a aquellos accidentes que puedan ser controlados por los medios disponibles en el Plan Autonómico y que, aún en su evolución más desfavorable, no suponen riesgo para la
población.', '0', '', '0'),
	('756', '142', '448', '2', 'Situación de emergencia en la que se prevé que los riesgos pueden afectar a las personas en el interior de la instalación, mientras que las repercusiones en el exterior, aunque muy improbables, no pueden ser controladas con los recursos propios del plan de emergencia interior o del plan de autoprotección, siendo necesaria la intervención de servicios del Plan.', '1', '', '1'),
	('757', '142', '448', '3', 'Situación de emergencia en la que se prevea que los riesgos pueden afectar a las personas tanto en el interior como en
el exterior de la instalación y, en consecuencia, se prevé el concurso de medios de apoyo de titularidad estatal no asignados al Plan.', '2', '', '2'),
	('758', '142', '448', '4', 'Esta declaración la efectúa el Ministro de Interior como consecuencia de haberse considerado que está en juego el Interés Nacional.', '3', '', '3'),
	('759', '142', '449', '1', 'Se prolongará hasta el restablecimiento de las condiciones mínimas
imprescindibles para un retorno a la normalidad en las zonas afectadas por la inundación.
Durante esta fase se realizarán las primeras tareas de rehabilitación.', '', '', '0'),
	('760', '143', '450', '1', 'Situación en la que se prevean acontecimientos
que puedan derivar hacia una situación de emergencia. La fase de alerta se basa en el seguimiento.', '', '', '0'),
	('761', '143', '451', '1', 'Emergencias que son resueltas por los distintos
servicios ordinarios acorde a protocolos propios de actuación, sin necesidad de activar el PLATERCE.', '0', '', '0'),
	('762', '143', '451', '2', 'Situación de emergencia, donde lo dispuesto en el plan puede ofrecer una respuesta eficaz a través de sus recursos públicos y privados. Requiriéndose para ello una actuación coordinada. ', '1', '', '1'),
	('763', '143', '451', '3', 'Cuando la situación de emergencia evoluciona desfavorablemente,
teniéndose que recurrir a la solicitud de medios estatales por parte del
Director del Plan.', '2', '', '2'),
	('764', '143', '451', '4', 'Emergencias en las que la situación provocada es declarada de interés nacional por el Ministro del Interior, de acuerdo con los supuestos establecidos en la Norma Básica. ', '3', '', '3'),
	('765', '143', '452', '1', 'La evolución favorable de los acontecimientos, al objeto de permitir que los
medios incorporados al Plan puedan ir retirándose de forma paulatina y coordinada. ', '', '', '0'),
	('766', '144', '453', '1', 'Se caracteriza por ser la situación en la cual, por evolución desfavorable de un proceso de riesgo o por existir una situación potencialmente dañina por su riesgo previsible entre la población, podría ser desencadenante de una emergencia.', 'Alerta', '', '0'),
	('767', '144', '454', '1', 'Es necesaria la movilización de recursos del INFOCE y ejecución, en su caso, de
tareas de prevención operativa a fin de garantizar una debida anticipación.', 'Alarma', '', '0'),
	('768', '144', '455', '1', 'Emergencia con daños puntuales en espacios concretos. Es ejecutado a través de los medios y recursos ordinarios del INFOCE. ', '', '0', '0'),
	('769', '144', '455', '2', 'Suceso con daños moderados en espacios puntualees o con daños marginales de carácter generalizado. Ejecutado por medios y recursos del INFOCE, y apoyos puntuales.', '', '1', '1'),
	('770', '144', '455', '3', 'Suceso con daños graves o moderados de carácter generalizado. Ejecutado por medios y recursos del INFOCE, y medios de otras administraciones para la generación de una respuesta.', '', '2', '2'),
	('771', '144', '455', '4', 'Emergencias en las que la situación provocada es
declarada de interés nacional por el Ministro del Interior, de acuerdo con los supuestos establecidos en la Norma Básica. ', '', '3', '3'),
	('772', '144', '456', '1', 'Una vez controlados los factores de peligrosidad emergentes sobre el territorio, es del
todo necesario reponer los servicios básicos dañados prestando la necesaria atención a las personas afectadas por incendios forestales desencadenados.', '', '', '0'),
	('773', '145', '457', '1', 'Situación en la cual, por
evolución desfavorable de un proceso de riesgo o por existir una situación
potencialmente dañina por su riesgo previsible entre la población, podría ser
desencadenante de una emergencia.', '', '', '0'),
	('774', '145', '458', '1', 'Es necesaria la movilización de recursos del INUNCE y ejecución, en su
caso, de tareas de prevención operativa a fin de garantizar una debida anticipación a
las posibles consecuencias.', '', '', '0'),
	('775', '145', '459', '1', 'Emergencia con daños puntuales en espacios concretos. Es ejecutado a través de los medios y recursos ordinarios del INUNCE. ', '', '0', '0'),
	('776', '145', '459', '2', 'Suceso con daños graves o moderados de carácter generalizado. Ejecutado por medios y recursos del INUNCE, y medios de otras administraciones para la generación de una respuesta.', '', '1', '1'),
	('777', '145', '459', '3', 'Suceso presentado
con daños graves en
espacios puntuales o
con daños moderados
o graves de carácter
generalizado en todo
el territorio. Movilizando los
medios y capacidades del
INUNCE y, en su
caso, con necesidad
de medios de otras
administraciones.', '', '2', '2'),
	('778', '145', '459', '4', 'Suceso presentado
con daños muy graves
en que requieren para
su control capacidades superiores a las de INUNCE, o
suceso cuyo ámbito
de afección territorial
supera al territorio de
Ceuta. Es declarado como interés nacional.', '', '3', '3'),
	('779', '145', '460', '1', 'Una vez controlados los factores de peligrosidad emergentes sobre el territorio,
es del todo punto necesario reponer los servicios básicos dañados prestando la
necesaria atención a las personas afectadas por inundaciones desencadenadas.', '', '', '0'),
	('780', '146', '461', '1', 'Ocurrencia de fenómenos
sísmicos ampliamente sentidos por la población y que requieren de una actuación coordinada dirigida a intensificar la información.', '0', '', '0'),
	('781', '146', '462', '1', 'Fenómenos sísmicos cuya atención, en lo relativo a la protección de personas y bienes, puede quedar asegurada mediante el empleo de medios y recursos ordinarios de la Ciudad Autónoma de Ceuta.', '1', '', '1'),
	('782', '146', '462', '2', 'Fenómenos sísmicos cuya gravedad, hace necesario el concurso de medios, recursos o servicios extraordinarios
pudiendo ser necesario contar con el apoyo de las capacidades de respuesta del plan estatal.', '2', '', '2'),
	('783', '146', '462', '3', 'Emergencias que, habiéndose considerado que está
en juego el interés nacional, así sean declaradas por el Ministerio del Interior.', '3', '', '3'),
	('784', '146', '463', '1', 'Acciones de apoyo a las personas afectadas por la emergencia una vez controlados los factores de peligrosidad ocasionados por los fenómenos sísmicos.', '', '', '0'),
	('785', '147', '464', '1', 'Contempla el empleo de medios propios de la Ciudad Autónoma y de la
Administración General del Estado, e incluso la solicitud de otros medios no
previstos.', '1', '', '1'),
	('786', '147', '464', '2', 'Cuando se den circunstancias en la que esté presente el
interés nacional, con arreglo a los supuestos previstos en la Norma Básica de
Protección Civil.', '2', '', '2');