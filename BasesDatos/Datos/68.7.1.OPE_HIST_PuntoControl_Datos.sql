SET IDENTITY_INSERT OPE_HIST_PuntoControl ON;

INSERT INTO OPE_HIST_PuntoControl (Id,Nombre,IdPuerto,UtmX,UtmY,UmbralAlto,UmbralMedio,UmbralMuyAlto) VALUES
	 (1,N'Manilva',NULL,296500,4023582,700,400,999),
	 (2,N'Entrada Puerto Algeciras',1,280454,4002804,600,200,700),
	 (3,N'Entrada Puerto Almería',3,546000,4078000,500,200,600),
	 (4,N'Puerto de Algeciras',1,280500,4002800,600,200,700),
	 (9,N'Entrada Puerto de Tarifa',14,1,2,1,0,2);

SET IDENTITY_INSERT OPE_HIST_PuntoControl OFF;