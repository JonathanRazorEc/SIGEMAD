SET IDENTITY_INSERT OPE_HIST_AreaEstacionamiento ON;

INSERT INTO OPE_HIST_AreaEstacionamiento (Id,Nombre,IdPuerto,UtmX,UtmY,Capacidad) VALUES
	 (1,N'Puerto de Algeciras',1,280456,4001147,7790),
	 (3,N'Los Barrios',NULL,275910,4007741,2000),
	 (4,N'Puerto de Almería',3,546555,4077000,6000),
	 (5,N'Puerto de Málaga',2,373500,4064500,3000),
	 (6,N'Puerto de Alicante',8,720000,4246505,1500),
	 (7,N'Puerto de Ceuta',4,290357,3974710,1300),
	 (8,N'Puerto de Melilla',5,504472,3905830,600),
	 (13,N'Prueba',NULL,1,1,1000);

SET IDENTITY_INSERT OPE_HIST_AreaEstacionamiento OFF;