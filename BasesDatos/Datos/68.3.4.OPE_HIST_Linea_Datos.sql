SET IDENTITY_INSERT OPE_HIST_Linea ON;

INSERT INTO OPE_HIST_Linea (Id,Nombre,IdPuertoOrigen,IdPuertoDestino,FechaDesde,FechaHasta) VALUES
	 (1,N'Algeciras/Ceuta',1,4,'1986-06-15 00:00:00.0',NULL),
	 (2,N'Algeciras/Tánger-Med',1,6,'1986-06-15 00:00:00.0',NULL),
	 (3,N'Málaga/Melilla',2,5,'1986-06-15 00:00:00.0',NULL),
	 (4,N'Almería/Melilla',3,5,'1986-06-15 00:00:00.0',NULL),
	 (5,N'Almería/Nador',3,7,'1995-06-15 00:00:00.0',NULL),
	 (6,N'Alicante/Orán',8,9,'1997-06-15 00:00:00.0',NULL),
	 (7,N'Ceuta/Algeciras',4,1,'1986-07-15 00:00:00.0',NULL),
	 (8,N'Tánger-Med/Algeciras',6,1,'1986-07-15 00:00:00.0',NULL),
	 (9,N'Melilla/Málaga',5,2,'1986-07-15 00:00:00.0',NULL),
	 (10,N'Melilla/Almería',5,3,'1986-07-15 00:00:00.0',NULL);
INSERT INTO OPE_HIST_Linea (Id,Nombre,IdPuertoOrigen,IdPuertoDestino,FechaDesde,FechaHasta) VALUES
	 (11,N'Nador/Almería',7,3,'1995-07-15 00:00:00.0',NULL),
	 (12,N'Orán/Alicante',9,8,'1997-07-15 00:00:00.0',NULL),
	 (13,N'Málaga/Ceuta',2,4,'2000-06-15 00:00:00.0','2000-09-16 00:00:00.0'),
	 (14,N'Ceuta/Málaga',4,2,'2000-07-15 00:00:00.0','2000-09-16 00:00:00.0'),
	 (15,N'Almería/Ghazaouet',3,10,'2002-06-15 00:00:00.0',NULL),
	 (16,N'Ghazaouet/Almería',10,3,'2002-06-15 00:00:00.0',NULL),
	 (17,N'Almería/Orán',3,9,'2003-06-15 00:00:00.0',NULL),
	 (18,N'Orán/Almería',9,3,'2003-07-15 00:00:00.0','2003-09-15 00:00:00.0'),
	 (19,N'Almería/Alhucemas',3,11,'2003-07-19 00:00:00.0','2013-01-01 00:00:00.0'),
	 (20,N'Alhucemas/Almería',11,3,'2003-07-19 00:00:00.0','2010-09-15 00:00:00.0');
INSERT INTO OPE_HIST_Linea (Id,Nombre,IdPuertoOrigen,IdPuertoDestino,FechaDesde,FechaHasta) VALUES
	 (21,N'Alicante/Argel',8,12,'2004-06-15 00:00:00.0','2019-08-15 00:00:00.0'),
	 (22,N'Argel/Alicante',12,8,'2004-06-15 00:00:00.0',NULL),
	 (23,N'Almería/Ghazaouet-Orán',3,15,'2009-06-15 00:00:00.0','2011-09-15 00:00:00.0'),
	 (24,N'Málaga/Alhucemas',2,11,'2007-07-14 00:00:00.0',NULL),
	 (25,N'Alhucemas/Málaga',11,2,'2007-07-25 00:00:00.0','2007-09-15 00:00:00.0'),
	 (27,N'Ghazaouet-Orán/Almería',15,3,'2008-07-15 00:00:00.0','2008-09-15 00:00:00.0'),
	 (28,N'Tánger-Med/Motril',6,18,'2016-07-15 00:00:00.0',NULL),
	 (30,N'Melilla/Motril',5,18,'2011-07-15 00:00:00.0',NULL),
	 (31,N'Motril/Melilla',18,5,'2011-06-15 00:00:00.0',NULL),
	 (32,N'Motril/Alhucemas',18,11,'2012-07-11 00:00:00.0',NULL);
INSERT INTO OPE_HIST_Linea (Id,Nombre,IdPuertoOrigen,IdPuertoDestino,FechaDesde,FechaHasta) VALUES
	 (33,N'Alhucemas/Motril',11,18,'2012-07-11 00:00:00.0',NULL),
	 (34,N'Nador/Motril',7,18,'2013-06-15 00:00:00.0',NULL),
	 (35,N'Motril/Nador',18,7,'2013-06-15 00:00:00.0',NULL),
	 (36,N'Motril/Tánger-Med',18,6,'2016-06-15 00:00:00.0',NULL),
	 (37,N'Tarifa/Tánger-Ville',14,19,'2008-06-15 00:00:00.0',NULL),
	 (38,N'Tánger-Ville/Tarifa',19,14,'2008-06-15 00:00:00.0',NULL),
	 (39,N'Alicante/Mostaganem',8,20,'2016-06-15 00:00:00.0','2017-08-15 00:00:00.0'),
	 (40,N'Mostaganem/Alicante',20,8,'2016-06-15 00:00:00.0',NULL),
	 (41,N'Valencia/Mostaganem',21,20,'2018-06-15 00:00:00.0',NULL),
	 (42,N'Mostaganem/Valencia',20,21,'2018-06-15 00:00:00.0',NULL);

SET IDENTITY_INSERT OPE_HIST_Linea OFF;