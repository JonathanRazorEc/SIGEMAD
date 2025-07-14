SET IDENTITY_INSERT dbo.EstadoIncendio ON;

INSERT INTO dbo.EstadoIncendio (Id, Descripcion, Borrado, Editable) VALUES
	 (1,N'Activo',0,0),
	 (2,N'Extinguido',0,0),
	 (3,N'Controlado',0,1),
	 (4,N'Reavivado',1,1),
	 (5,N'Desconocido',1,1),
	 (6,N'Estabilizado',0,1),
	 (7,N'Sin seguimiento',1,1);

SET IDENTITY_INSERT dbo.EstadoIncendio OFF;