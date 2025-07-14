CREATE TRIGGER trg_Auditoria_OPE_AreaEstacionamiento

ON OPE_AreaEstacionamiento

FOR INSERT, UPDATE, DELETE

AS

BEGIN

    DECLARE @FechaRegistro DATETIME2 = SYSDATETIME();

    DECLARE @TipoMovimiento CHAR(1);

 

    -- Para INSERT

    IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted)

    BEGIN

        INSERT INTO Auditoria_OPE_AreaEstacionamiento (FechaRegistro, TipoMovimiento, IdOpeAreaEstacionamiento, Nombre, IdCcaa,IdProvincia,IdMunicipio,Carretera, PK, CoordenadaUTM_X,CoordenadaUTM_Y, InstalacionPortuaria, IdOpePuerto, Capacidad, FechaCreacion,CreadoPor,FechaModificacion,ModificadoPor,FechaEliminacion,EliminadoPor,Borrado)

        SELECT @FechaRegistro, 'I', Id, Nombre, IdCcaa,IdProvincia,IdMunicipio,Carretera, PK, CoordenadaUTM_X,CoordenadaUTM_Y, InstalacionPortuaria, IdOpePuerto, Capacidad, FechaCreacion,CreadoPor,FechaModificacion,ModificadoPor,FechaEliminacion,EliminadoPor,Borrado

        FROM inserted;

    END

 

    -- Para UPDATE Y DELETE

    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)

    BEGIN

        INSERT INTO Auditoria_OPE_AreaEstacionamiento (FechaRegistro, TipoMovimiento, IdOpeAreaEstacionamiento, Nombre, IdCcaa,IdProvincia,IdMunicipio,Carretera, PK, CoordenadaUTM_X,CoordenadaUTM_Y, InstalacionPortuaria, IdOpePuerto, Capacidad,FechaCreacion,CreadoPor,FechaModificacion,ModificadoPor,FechaEliminacion,EliminadoPor,Borrado)

        SELECT @FechaRegistro, CASE WHEN (i.Borrado = 1 AND d.Borrado = 0) THEN 'D' ELSE 'U' END, i.Id, i.Nombre, i.IdCcaa,i.IdProvincia,i.IdMunicipio,i.Carretera, i.PK, i.CoordenadaUTM_X,i.CoordenadaUTM_Y, i.InstalacionPortuaria, i.IdOpePuerto, i.Capacidad, i.FechaCreacion, i.CreadoPor, i.FechaModificacion, i.ModificadoPor, i.FechaEliminacion, i.EliminadoPor, i.Borrado

        FROM inserted i

        JOIN deleted d ON i.Id = d.Id;

    END

END;