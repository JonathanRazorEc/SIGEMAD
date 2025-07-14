SET IDENTITY_INSERT dbo.EstadoSuceso ON;

INSERT INTO dbo.EstadoSuceso (Id, Descripcion,Borrado,Editable) VALUES
	(1, N'En curso',0,0),
	(2, N'Sin seguimiento',0,1),
	(3, N'Cerrado',0,0);

SET IDENTITY_INSERT dbo.EstadoSuceso OFF;