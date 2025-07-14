SET IDENTITY_INSERT dbo.CaracterMedio ON;

INSERT INTO dbo.CaracterMedio (Id, Descripcion, Obsoleto) VALUES
	 (1,N'Ordinario',0),
	 (2,N'Extraordinario',0),
	 (3,N'En Prácticas',1),
	 (4,N'Protocolo bilateral',0);

SET IDENTITY_INSERT dbo.CaracterMedio OFF;