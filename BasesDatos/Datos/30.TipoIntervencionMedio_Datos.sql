SET IDENTITY_INSERT dbo.TipoIntervencionMedio ON;


INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (407,N'Otros',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1282,N'*Agente Forestal/Medioambiental',3,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1283,N'**Aljibes',7,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1284,N'Ambulancia',2,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1285,N'Autobomba Autonomica',2,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1286,N'**Autocar',2,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1287,N'**Bombas',7,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1288,N'Bomberos y Brigadistas',3,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1289,N'Bulldozer',4,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1290,N'**CamiÃ³n',2,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);

INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1291,N'*CamiÃ³n autobomba Â ',2,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1292,N'Cisterna',4,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1293,N'Tecnicos y Coordinadores',3,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1294,N'Cuadrilla de extinción',3,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1295,N'**Equipo apoyo logÃ­stico',3,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1296,N'Equipo de atención psicosocial',3,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1297,N'Equipo médico',3,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1298,N'*Equipos de comunicaciones',7,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1299,N'*Equipos de iluminaciÃ³n',7,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1300,N'*Equipos geofÃ³nicos',7,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);

INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1301,N'Excavadora',4,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1302,N'**FurgÃ³n',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1303,N'*CamiÃ³n GÃ³ndola',4,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1304,N'**GrÃºa',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1305,N'**Grupo SAR',10,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1306,N'**Grupos electrÃ³genos Â ',7,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1307,N'Guardia Civil- Efectivos',3,1,79,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1308,N'Helicóptero',9,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1309,N'**Hospital de campaÃ±a',7,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1310,N'**JIC',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);

INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1311,N'**Motobombas',2,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1312,N'*Motoniveladora',4,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1313,N'Nodriza',2,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1314,N'Pala cargadora',4,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1315,N'Personal de rescate',3,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1316,N'Personal sanitario',3,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1317,N'Policía Autonómica',3,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1318,N'Polici­a Nacional Efectivos',3,1,79,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1319,N'Potabilizadoras de agua',7,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1320,N'Puesto de Mando Avanzado',7,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1321,N'Puesto Médico Avanzado',7,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1322,N'*Remolques Â ',7,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1323,N'Retenes',3,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1324,N'*TÃ©cnicos',3,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1325,N'**Todoterreno adaptado para extinciÃ³n',2,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1326,N'**Tractor adaptado',2,6,NULL,NULL,NULL,NULL,NULL,NULL,N'Particular sin especificar'),
	 (1327,N'**Tuneladora',4,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1328,N'**Unidad de MeteorologÃ­a y Trasmisiones',2,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1329,N'*Unidad tÃ©cnica del GRAF',3,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1330,N'*VehÃ­culo AnÃ­bal',2,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1331,N'*VehÃ­culo autobomba',2,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1332,N'*VehÃ­culo autotanque',2,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1333,N'*VehÃ­culo con depÃ³sito Â ',2,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1334,N'*VehÃ­culo de apoyo',2,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1335,N'**VehÃ­culo de coordinaciÃ³n',2,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1336,N'**VehÃ­culo de logÃ­stica/sanitario',2,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1337,N'*VehÃ­culo de Mando y CoordinaciÃ³n UME',2,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1338,N'**VehÃ­culo de comunicaciones',2,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1339,N'*VehÃ­culo de transporte',2,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1340,N'*VehÃ­culo Nodriza',2,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1341,N'Vehículo sanitario',2,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1342,N'Voluntarios',3,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1343,N'FOCA AA - Torrejon',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1344,N'FOCA - AA Matacan',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1345,N'FOCA-AA Talavera la Real',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1346,N'FOCA- AA Labacolla',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1347,N'FOCA-AA Zaragoza',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1348,N'FOCA-AA Pollensa',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1349,N'FOCA -AA Los Llanos',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1350,N'FOCA-AA Malaga',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1351,N'ALFA-Aa Reus',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1352,N'KILO-HK Huelma',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1353,N'*KILO-HK Villares de Jadraque',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1354,N'KILO-HK La Almoraima',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1355,N'KILO-HK Ibias',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1356,N'KILO-HK Tenerife sur',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1357,N'*KILO-HK Cabeza de Buey',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1358,N'KILO-HK Plasencia del Monte',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1359,N'KILO-HK Caravaca',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1360,N'*KILO-HK Mutxamiel',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1361,N'*KILO-HK Tabuyo',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1362,N'*KILO-HK Tineo',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1363,N'*KILO-HK Laza',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1364,N'*KILO-HK Cordoba',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1365,N'TANGO-ACT Ampuriabrava',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1366,N'TANGO -ACT Agoncillo',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1367,N'TANGO-ACT Niebla',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1368,N'TANGO-ACT Xinzo',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1369,N'TANGO-ACT Noain',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1370,N'TANGO-ACT Son Bonet',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1371,N'*ALFA-Aa Manises',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1372,N'ALFA-Aa Rosinos',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1373,N'BRIF/A - Tabuyo',6,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1374,N'BRIF/A - Pinofranqueado',6,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1375,N'BRIF/A - Daroca',6,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1376,N'BRIF/A - Cuenca/Prado de los Esquiladores',6,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1377,N'BRIF/A - Tineo',6,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1378,N'BRIF/A - Laza',6,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1379,N'BRIF/A - Lubia',6,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1380,N'BRIF/A - La Palma/Puntagorda',6,1,71,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1381,N'BRIF/A - La Iglesuela',6,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1383,N'BRIF/B - Puerto El Pico',6,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1386,N'*ACO - Matacan',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1388,N'ACO - Mutxamiel',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1389,N'**BK - Torrejon',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1390,N'*BLP-Brigada Prevencion',3,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1391,N'*EPRIF',3,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1392,N'**UME - BIEM I',10,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1393,N'**UME - BIEM II',10,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1394,N'**UME - BIEM III',10,1,2,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1395,N'**UME - BIEM IV',10,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1396,N'**UME - BIEM V',10,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1397,N'SAMUR - Madrid',3,3,NULL,NULL,10,28,28079,NULL,NULL),
	 (1398,N'Bomberos - Portugal',3,4,NULL,NULL,NULL,NULL,NULL,1,NULL),
	 (1399,N'Bomberos - Cataluña',3,2,NULL,7,NULL,NULL,NULL,NULL,NULL),
	 (1400,N'Bomberos - CAM',3,2,NULL,10,NULL,NULL,NULL,NULL,NULL),
	 (1401,N'Bomberos - Ayto Madrid',3,3,NULL,NULL,10,28,28079,NULL,NULL),
	 (1402,N'**Equipo de evaluaciÃ³n y anÃ¡lisis',3,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1403,N'**Kit sanitario para emergencias - IEHK ',7,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1404,N'**Tiendas',11,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1405,N'*Mantas',11,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1406,N'**Sacos de dormir',11,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1407,N'**Ropa',11,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1408,N'**Lancha',12,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1409,N'Maquinas quitanieves',2,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1410,N'*VehÃ­culo todo terreno',2,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1411,N'*VEMPAR UME',2,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1412,N'**Regimiento UME de Apoyo a Emergencias',10,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1413,N'**Equipo de Bombeo UME',7,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1414,N'**Buceadores',3,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1415,N'Puentes',7,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1478,N'*Director COP (Centro Operativo provincial) - AndalucÃ­a',3,2,NULL,15,NULL,NULL,NULL,NULL,NULL),
	 (1479,N'*TÃ©cnico de extinciÃ³n - AndalucÃ­a',3,2,NULL,15,NULL,NULL,NULL,NULL,NULL),
	 (1480,N'*Coordinador Regional y Provincial - AndalucÃ­a',3,2,NULL,15,NULL,NULL,NULL,NULL,NULL),
	 (1481,N'**Agentes BIIF (Brigada de InvestigaciÃ³n de Incendios Forestales) - AndalucÃ­a',3,2,NULL,15,NULL,NULL,NULL,NULL,NULL),
	 (1482,N'Agentes de Medio Ambiente/Forestales',3,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1483,N'*Especialistas de extinciÃ³n - AndalucÃ­a',3,2,NULL,15,NULL,NULL,NULL,NULL,NULL),
	 (1484,N'*Componentes de Grupo de Apoyo - Andaluci­a',3,2,NULL,15,NULL,NULL,NULL,NULL,NULL),
	 (1485,N'*Vigilantes fijos - Andaluci­a',3,2,NULL,15,NULL,NULL,NULL,NULL,NULL),
	 (1486,N'*Personal de apoyo logi­stico - Andaluci­a',3,2,NULL,15,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1487,N'*Tecnicos- Andaluci­a',3,2,NULL,15,NULL,NULL,NULL,NULL,NULL),
	 (1488,N'*Autobombas - Andalucia',2,2,NULL,15,NULL,NULL,NULL,NULL,NULL),
	 (1489,N'*Nodrizas - AndalucÃ­a',2,2,NULL,15,NULL,NULL,NULL,NULL,NULL),
	 (1490,N'*VehÃ­culo UMMT - AndalucÃ­a',2,2,NULL,15,NULL,NULL,NULL,NULL,NULL),
	 (1491,N'*Vehiculos de transporte de personal - Andaluci­a',2,2,NULL,15,NULL,NULL,NULL,NULL,NULL),
	 (1492,N'HT (Helicoptero de transporte y extincion) - Andalucia',9,2,NULL,15,NULL,NULL,NULL,NULL,NULL),
	 (1493,N'ACT - Andalucía',8,2,NULL,15,NULL,NULL,NULL,NULL,NULL),
	 (1494,N'ACO - Andaluci­a',8,2,NULL,15,NULL,NULL,NULL,NULL,NULL),
	 (1495,N'*TÃ©cnicos - Castilla y LeÃ³n',3,2,NULL,8,NULL,NULL,NULL,NULL,NULL),
	 (1496,N'*Agentes Medioambientales y Forestales - Castilla y LeÃ³n',3,2,NULL,8,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1497,N'CAR (Cuadrilla de Actuación Rápida) - Castilla y León',3,2,NULL,8,NULL,NULL,NULL,NULL,NULL),
	 (1498,N'CUPA (Cuadrilla de Pronto Acceso) - Castilla y León',3,2,NULL,8,NULL,NULL,NULL,NULL,NULL),
	 (1499,N'Cuadrillas Terrestres - Castilla y León',3,2,NULL,8,NULL,NULL,NULL,NULL,NULL),
	 (1500,N'*VehÃ­culos autobomba - Castilla y LeÃ³n',2,2,NULL,8,NULL,NULL,NULL,NULL,NULL),
	 (1501,N'Maquinaria pesada - Castilla y León',2,2,NULL,8,NULL,NULL,NULL,NULL,NULL),
	 (1502,N'Helicóptero de transporte y extinción - Castilla y León',9,2,NULL,8,NULL,NULL,NULL,NULL,NULL),
	 (1503,N'ACT - Castilla y Leon',8,2,NULL,8,NULL,NULL,NULL,NULL,NULL),
	 (1504,N'ACT- Castilla y Leon',8,2,NULL,8,NULL,NULL,NULL,NULL,NULL),
	 (1505,N'Retenes - La Rioja',3,2,NULL,9,NULL,NULL,NULL,NULL,NULL),
	 (1506,N'*Agentes forestales - La Rioja',3,2,NULL,9,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1507,N'Cuadrilla de Accion Rapida CAR- La Rioja',6,2,NULL,9,NULL,NULL,NULL,NULL,NULL),
	 (1508,N'*TÃ©cnicos - La Rioja',3,2,NULL,9,NULL,NULL,NULL,NULL,NULL),
	 (1509,N'**Personal de apoyo logÃ­stico - La Rioja',3,2,NULL,9,NULL,NULL,NULL,NULL,NULL),
	 (1510,N'*Autobomba - La Rioja',2,2,NULL,9,NULL,NULL,NULL,NULL,NULL),
	 (1511,N'*Cisternas - La Rioja',2,2,NULL,9,NULL,NULL,NULL,NULL,NULL),
	 (1512,N'*VehÃ­culos de patrullaje y primer ataque - La Rioja',2,2,NULL,9,NULL,NULL,NULL,NULL,NULL),
	 (1513,N'Maquinaria pesada Autonomica',4,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1514,N'Helicóptero de transporte y extinción - La Rioja',9,2,NULL,9,NULL,NULL,NULL,NULL,NULL),
	 (1515,N'Cuadrillas terrestres - Aragón',3,2,NULL,6,NULL,NULL,NULL,NULL,NULL),
	 (1516,N'Cuadrillas helitransportadas - Aragón',3,2,NULL,6,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1517,N'**BRAF (Brigada de uso de Retardantes con AplicaciÃ³n Forestal) - AragÃ³n',3,2,NULL,6,NULL,NULL,NULL,NULL,NULL),
	 (1518,N'*TÃ©cnicos - AragÃ³n',3,2,NULL,6,NULL,NULL,NULL,NULL,NULL),
	 (1519,N'*Agentes de ProtecciÃ³n de la Naturaleza - AragÃ³n',3,2,NULL,6,NULL,NULL,NULL,NULL,NULL),
	 (1520,N'Maquinaria pesada - Aragón',2,2,NULL,6,NULL,NULL,NULL,NULL,NULL),
	 (1521,N'*VehÃ­culo de puesto de mando avanzado - AragÃ³n',2,2,NULL,6,NULL,NULL,NULL,NULL,NULL),
	 (1522,N'*VehÃ­culos Autobomba - AragÃ³n',2,2,NULL,6,NULL,NULL,NULL,NULL,NULL),
	 (1523,N'Helicóptero de transporte y extinción - Aragón',9,2,NULL,6,NULL,NULL,NULL,NULL,NULL),
	 (1524,N'Helicóptero de coordinación - Aragón',9,2,NULL,6,NULL,NULL,NULL,NULL,NULL),
	 (1525,N'Retenes terrestres - Castilla-La Mancha',3,2,NULL,11,NULL,NULL,NULL,NULL,NULL),
	 (1526,N'Retenes helitransportados - Castilla-La Mancha',6,2,NULL,11,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1527,N'*Autobombas - Castilla-La Mancha',2,2,NULL,11,NULL,NULL,NULL,NULL,NULL),
	 (1528,N'**Nodrizas - Castilla-La Mancha',2,2,NULL,11,NULL,NULL,NULL,NULL,NULL),
	 (1529,N'Maquinaria pesada - Castilla-La Mancha',2,2,NULL,11,NULL,NULL,NULL,NULL,NULL),
	 (1530,N'HT (Helicoptero de transporte y extincion) - Castilla-La Mancha',9,2,NULL,11,NULL,NULL,NULL,NULL,NULL),
	 (1531,N'HT (Helicoptero de transporte y extincion) - Extremadura',9,2,NULL,14,NULL,NULL,NULL,NULL,NULL),
	 (1532,N'Retenes terrestres - Extremadura',3,2,NULL,14,NULL,NULL,NULL,NULL,NULL),
	 (1533,N'*Camiones - Extremadura',3,2,NULL,14,NULL,NULL,NULL,NULL,NULL),
	 (1534,N'*Agentes forestales - Extremadura',3,2,NULL,14,NULL,NULL,NULL,NULL,NULL),
	 (1535,N'*Coordinadores regionales - Extremadura',3,2,NULL,14,NULL,NULL,NULL,NULL,NULL),
	 (1536,N'*Agentes Medioambientales - Extremadura',3,2,NULL,14,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1537,N'*Maquinaria pesada - Extremadura',4,2,NULL,14,NULL,NULL,NULL,NULL,NULL),
	 (1538,N'*Vehi­culos - Extremadura',2,2,NULL,14,NULL,NULL,NULL,NULL,NULL),
	 (1539,N'UME Efectivos',3,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1540,N'*Ambulancia UME',2,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1541,N'**Autobomba UME',2,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1542,N'**Nodriza UME',2,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1543,N'*VehÃ­culo ligero UME',2,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1544,N'*VehÃ­culo pesado UME',2,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1545,N'*CamiÃ³n UME',2,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1546,N'*VehÃ­culo VAMTAC UME',2,1,2,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1547,N'*VehÃ­culo de comunicaciones UME',2,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1548,N'**Quitanieves UME',2,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1549,N'*VehÃ­culo todo terreno UME',2,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1550,N'**Maquinaria UME',4,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1551,N'*VehÃ­culo de transporte UME',2,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1552,N'*EmbarcaciÃ³n UME',12,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1553,N'*Equipo de bombeo de agua y lodos UME',2,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1554,N'**AviÃ³n anfibio UME',8,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1555,N'**Helicoptero extincion UME',9,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1556,N'UME Helicoptero',9,1,2,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1557,N'**Helicoptero de control y coordinacion UME',9,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1558,N'ACO - Asturias',8,2,NULL,2,NULL,NULL,NULL,NULL,NULL),
	 (1559,N'Helicóptero de coordinación - Asturias',9,2,NULL,2,NULL,NULL,NULL,NULL,NULL),
	 (1560,N'Helicóptero de transporte y extinción - Asturias',9,2,NULL,2,NULL,NULL,NULL,NULL,NULL),
	 (1561,N'ACT - I.Baleares',8,2,NULL,13,NULL,NULL,NULL,NULL,NULL),
	 (1562,N'ACO- I.Baleares',8,2,NULL,13,NULL,NULL,NULL,NULL,NULL),
	 (1563,N'Helicoptero de transporte y extincion - I.Baleares',9,2,NULL,13,NULL,NULL,NULL,NULL,NULL),
	 (1564,N'Helicóptero de transporte y extinción - Canarias',9,2,NULL,17,NULL,NULL,NULL,NULL,NULL),
	 (1565,N'Helicóptero de extinción - Cantabria',9,2,NULL,3,NULL,NULL,NULL,NULL,NULL),
	 (1566,N'Avión anfibio - Cataluña',8,2,NULL,7,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1567,N'ACT - Cataluña',8,2,NULL,7,NULL,NULL,NULL,NULL,NULL),
	 (1568,N'Helicóptero de mando y coordinación - Cataluña',9,2,NULL,7,NULL,NULL,NULL,NULL,NULL),
	 (1569,N'Helicóptero de transporte y extinción - Cataluña',9,2,NULL,7,NULL,NULL,NULL,NULL,NULL),
	 (1570,N'Avion anfibio Aa - Comunidad Valenciana',8,2,NULL,12,NULL,NULL,NULL,NULL,NULL),
	 (1571,N'ACT - Comunidad Valenciana',8,2,NULL,12,NULL,NULL,NULL,NULL,NULL),
	 (1572,N'Helicóptero bombardero - Comunidad Valenciana',9,2,NULL,12,NULL,NULL,NULL,NULL,NULL),
	 (1573,N'Helicóptero de coordinación y emergencias - Comunidad Valenciana',9,2,NULL,12,NULL,NULL,NULL,NULL,NULL),
	 (1574,N'Helicóptero de transporte y extinción - Comunidad Valenciana',9,2,NULL,12,NULL,NULL,NULL,NULL,NULL),
	 (1575,N'ACT - Galicia',8,2,NULL,1,NULL,NULL,NULL,NULL,NULL),
	 (1576,N'Helicóptero de transporte y extinción - Galicia',9,2,NULL,1,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1577,N'Helicóptero de transporte - Galicia',9,2,NULL,1,NULL,NULL,NULL,NULL,NULL),
	 (1578,N'Helicóptero bombardero - Madrid',9,2,NULL,10,NULL,NULL,NULL,NULL,NULL),
	 (1579,N'Helicóptero de coordinación - Madrid',9,2,NULL,10,NULL,NULL,NULL,NULL,NULL),
	 (1580,N'Helicóptero de transporte y extinción - Madrid',9,2,NULL,10,NULL,NULL,NULL,NULL,NULL),
	 (1581,N'HT (Helicoptero de transporte y extincion) - Murcia',9,2,NULL,16,NULL,NULL,NULL,NULL,NULL),
	 (1582,N'Helicóptero bombardero - Navarra',9,2,NULL,5,NULL,NULL,NULL,NULL,NULL),
	 (1583,N'Helicóptero de coordinación - Navarra',9,2,NULL,5,NULL,NULL,NULL,NULL,NULL),
	 (1584,N'Helicóptero de transporte - Navarra',9,2,NULL,5,NULL,NULL,NULL,NULL,NULL),
	 (1585,N'Helicóptero para uso del Director de Extinción - País Vasco',9,2,NULL,4,NULL,NULL,NULL,NULL,NULL),
	 (1586,N'**Equipo Sanitario - AndalucÃ­a',3,2,NULL,15,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1587,N'Cuadrillas Selvícolas - Andalucía',3,2,NULL,15,NULL,NULL,NULL,NULL,NULL),
	 (1588,N'Retenes especialistas - Andalucía',3,2,NULL,15,NULL,NULL,NULL,NULL,NULL),
	 (1589,N'Retenes móviles - Andalucía',3,2,NULL,15,NULL,NULL,NULL,NULL,NULL),
	 (1590,N'Bomberos - Andalucía',3,2,NULL,15,NULL,NULL,NULL,NULL,NULL),
	 (1591,N'CAR (Cuadrillas de Accion Rapida) - Andaluci­a',6,2,NULL,15,NULL,NULL,NULL,NULL,NULL),
	 (1592,N'Maquinaria Pesada - Andalucía',4,2,NULL,15,NULL,NULL,NULL,NULL,NULL),
	 (1593,N'BRICA - Andaluci­a',6,2,NULL,15,NULL,NULL,NULL,NULL,NULL),
	 (1594,N'*VehÃ­culo UNASIF - AndalucÃ­a',2,2,NULL,15,NULL,NULL,NULL,NULL,NULL),
	 (1595,N'HK(Helicoptero bombardero) - Andalucia',9,2,NULL,15,NULL,NULL,NULL,NULL,NULL),
	 (1596,N'HK (Helicoptero bombardero) - Castilla-La Mancha',9,2,NULL,11,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1597,N'ACT - Castilla-La Mancha',8,2,NULL,11,NULL,NULL,NULL,NULL,NULL),
	 (1598,N'ACO (Avion de coordinacion) - Autonomico',8,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1599,N'HK (Helicoptero bombardero) - Extremadura',9,2,NULL,14,NULL,NULL,NULL,NULL,NULL),
	 (1600,N'Helicóptero de coordinación - Extremadura',9,2,NULL,14,NULL,NULL,NULL,NULL,NULL),
	 (1601,N'KILO-HK Plasencia de Caceres',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1602,N'*ALFA -Aa La Gomera',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1603,N'*UMMT MAGRAMA',2,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1604,N'Avion anfibio  Aa- C Madrid',8,2,NULL,10,NULL,NULL,NULL,NULL,NULL),
	 (1605,N'*ALFA- Aa.',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1608,N'*ACO',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1610,N'*FOCA AA',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1611,N'*TANGO-ACT',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1612,N'Guardia Civil- Helicoptero',9,1,79,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1613,N'*BRIF/A',6,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1614,N'*BRIF/B',6,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1615,N'*KILO-HK',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1616,N'*BRIF-i',6,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1617,N'*BRIF-i - Tabuyo',6,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1618,N'*BRIF-i Pinofranqueado',6,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1619,N'*BRIF-i Tineo',6,1,71,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1620,N'*BRIF-i - Laza',6,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1621,N'*BRIF-i - Ruente',6,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1622,N'ELIF - Castilla y Leon',6,2,NULL,8,NULL,NULL,NULL,NULL,NULL),
	 (1623,N'*KILO-HK Los Rodeos',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1624,N'*UMMT',2,2,NULL,8,NULL,NULL,NULL,NULL,NULL),
	 (1626,N'*BLP-Brigada Prevencion Tineo',3,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1627,N'UMAP (Unidad Movil de Analisis y Planificacion)',2,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1628,N'*RPAS',7,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1629,N'TANGO-ACT La Gomera',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1630,N'ACO - Leon',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1631,N'ACO - Talavera la Real',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1632,N'ACO - Zaragoza',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1633,N'KILO-HK Villares/Las Minas',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1634,N'UMAP - Laza',2,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1635,N'UMAP - Rabanal del Camino',2,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1636,N'UMAP - Zaragoza',2,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1637,N'UMAP - Caceres',2,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1640,N'RPAS - Rabanal del Camino',7,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1641,N'RPAS - Zaragoza',7,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1642,N'RPAS - Caceres',7,1,71,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1643,N'RPAS - Albacete',7,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1644,N'UMAP - Albacete',2,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1645,N'UMAP - Valencia',2,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1646,N'UMAP - Sevilla',2,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1647,N'**Personal UME',3,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1648,N'*Agentes Forestales/Medioambientales  -Cataluna',3,2,NULL,7,NULL,NULL,NULL,NULL,NULL),
	 (1649,N'*Unidad de Vigilancia Movil- I. Baleares',2,2,NULL,13,NULL,NULL,NULL,NULL,NULL),
	 (1650,N'Brigadas Terrestres Autonomicas',3,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1651,N'Brigadas Helitransportadas Autonomicas',6,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1652,N'*Autobombas - I.Baleares',2,2,NULL,13,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1653,N'ACT - La Rioja',8,2,NULL,9,NULL,NULL,NULL,NULL,NULL),
	 (1654,N'Autobombas - Portugal',2,4,NULL,NULL,NULL,NULL,NULL,1,NULL),
	 (1655,N'FOCA AA - Francia',8,4,NULL,NULL,NULL,NULL,NULL,65,NULL),
	 (1656,N'*Autobombas - Extremadura',2,2,NULL,14,NULL,NULL,NULL,NULL,NULL),
	 (1657,N'Retenes helitransportados',6,2,NULL,14,NULL,NULL,NULL,NULL,NULL),
	 (1658,N'*Agentes medioambientales/forestales-  Murcia',3,2,NULL,16,NULL,NULL,NULL,NULL,NULL),
	 (1659,N'*Autobombas - Murcia',2,2,NULL,16,NULL,NULL,NULL,NULL,NULL),
	 (1660,N'**Nodrizas - Murcia',2,2,NULL,16,NULL,NULL,NULL,NULL,NULL),
	 (1661,N'Personal extincion - Castilla-La Mancha',3,2,NULL,11,NULL,NULL,NULL,NULL,NULL),
	 (1663,N'Medios aereos - Castilla-La Mancha',13,2,NULL,11,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1664,N'*Vehiculos terrestres - Castilla-La Mancha',2,2,NULL,11,NULL,NULL,NULL,NULL,NULL),
	 (1665,N'**Cruz Roja ',11,1,49,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1666,N'*KILO-HK El Serranillo',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1667,N'ALFA-Aa Requena',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1668,N'UME Vehi­culos',2,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1669,N'MIKE-HT Cuenca/Prado de los Esquiladores',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1670,N'MIKE-HT Daroca',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1671,N'MIKE-HT La Iglesuela',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1672,N'MIKE-HT Puerto el Pico',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1673,N'MIKE-HT La Palma-Puntagorda',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1674,N'MIKE-HT Laza',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1675,N'MIKE-HT Lubia',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1676,N'MIKE-HT Pinofranqueado',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1677,N'MIKE-HT Tabuyo',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1678,N'MIKE-HT Tineo',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1679,N'FOCA- AA Italia',8,4,NULL,NULL,NULL,NULL,NULL,93,NULL),
	 (1680,N'Guardia Civil- Vehiculos',2,1,79,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1681,N'UME Drones',7,1,2,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1682,N'MIKE-Tenerife Sur',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1683,N'EPAIF',3,1,71,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1684,N'EPAIF SANABRIA',3,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1685,N'MIKE- HT Ibias',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1686,N'MIKE-HT La Almoraima',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1688,N'MIKE-HT Caravaca',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1689,N'MIKE-HT Plasencia del Monte',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1690,N'MIKE-HT Plasencia de CÃ¡ceres',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1691,N'MIKE-HT Huelma',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1692,N'LIMA- HT Bellvei',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1693,N'LIMA-HT de Las Minas',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1694,N'LIMA-HT Carcabuey',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO dbo.TipoIntervencionMedio (Id,Descripcion,IdClasificacion,IdTitularidad,IdTitularidadEstatal,IdTitularidadAutonomica,IdTitularidadAutonomicaMunicipal,IdTitularidadProvinciaMunicipal,IdTitularidadMunicipal,IdTitularidadPais,TitularidadOtra) VALUES
	 (1695,N'LIMA-HT Rosinos',9,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1696,N'TANGO-ACT La Centenera',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1697,N'ALFA-Aa Viver',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1698,N'ALFA-Aa Mirabel',8,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1699,N' UMAP Noia',2,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1700,N'UMAP Sanabria',2,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1701,N'UMAP La Rioja',2,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1702,N'UMAP Madrid',2,1,71,NULL,NULL,NULL,NULL,NULL,NULL),
	 (1703,N'UMAP MÃ¡laga',2,1,71,NULL,NULL,NULL,NULL,NULL,NULL);



SET IDENTITY_INSERT dbo.TipoIntervencionMedio OFF;