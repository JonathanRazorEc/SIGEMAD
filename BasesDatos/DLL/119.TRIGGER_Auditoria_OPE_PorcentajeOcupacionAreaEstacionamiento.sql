CREATE TRIGGER trg_Auditoria_OPE_PorcentajeOcupacionAreaEstacionamiento

ON OPE_PorcentajeOcupacionAreaEstacionamiento

FOR INSERT, UPDATE, DELETE

AS

BEGIN

    DECLARE @FechaRegistro DATETIME2 = SYSDATETIME();

    DECLARE @TipoMovimiento CHAR(1);

 

    -- Para INSERT

    IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted)

    BEGIN

        INSERT INTO Auditoria_OPE_PorcentajeOcupacionAreaEstacionamiento (FechaRegistro, TipoMovimiento, IdOpePorcentajeOcupacionAreaEstacionamiento, IdOpeOcupacion, PorcentajeInferior, PorcentajeSuperior, FechaCreacion,CreadoPor,FechaModificacion,ModificadoPor,FechaEliminacion,EliminadoPor,Borrado)

        SELECT @FechaRegistro, 'I', Id, IdOpeOcupacion, PorcentajeInferior, PorcentajeSuperior, FechaCreacion,CreadoPor,FechaModificacion,ModificadoPor,FechaEliminacion,EliminadoPor,Borrado

        FROM inserted;

    END

 

    -- Para UPDATE Y DELETE

    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)

    BEGIN

        INSERT INTO Auditoria_OPE_PorcentajeOcupacionAreaEstacionamiento(FechaRegistro, TipoMovimiento, IdOpePorcentajeOcupacionAreaEstacionamiento, IdOpeOcupacion, PorcentajeInferior, PorcentajeSuperior,FechaCreacion,CreadoPor,FechaModificacion,ModificadoPor,FechaEliminacion,EliminadoPor,Borrado)

        SELECT @FechaRegistro, CASE WHEN (i.Borrado = 1 AND d.Borrado = 0) THEN 'D' ELSE 'U' END, i.Id, i.IdOpeOcupacion, i.PorcentajeInferior, i.PorcentajeSuperior, i.FechaCreacion, i.CreadoPor, i.FechaModificacion, i.ModificadoPor, i.FechaEliminacion, i.EliminadoPor, i.Borrado

        FROM inserted i

        JOIN deleted d ON i.Id = d.Id;

    END

END;