CREATE TABLE Auditoria_CancelacionMedio (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FechaRegistro DATETIME2 NOT NULL,
    TipoMovimiento CHAR(1) CHECK (TipoMovimiento IN ('I', 'U', 'D')) NOT NULL,
    IdCancelacionMedio INT NOT NULL,
    IdEjecucionPaso INT NOT NULL,
    Motivo NVARCHAR(MAX) NOT NULL,
    FechaHoraCancelacion DATETIME2 NOT NULL,
    FechaCreacion DATETIME2 NOT NULL,
    CreadoPor UNIQUEIDENTIFIER,
    FechaModificacion DATETIME2,
    ModificadoPor UNIQUEIDENTIFIER,
    FechaEliminacion DATETIME2,
    EliminadoPor UNIQUEIDENTIFIER,
    Borrado BIT NOT NULL,
	FOREIGN KEY (IdCancelacionMedio) REFERENCES dbo.CancelacionMedio(Id),
    FOREIGN KEY (IdEjecucionPaso) REFERENCES dbo.EjecucionPaso(Id)
);
GO

CREATE TRIGGER trg_Auditoria_CancelacionMedio
ON dbo.CancelacionMedio
FOR INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @FechaRegistro DATETIME2 = SYSDATETIME();
    DECLARE @TipoMovimiento CHAR(1);

    -- Para INSERT
    IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted) 
    BEGIN
        INSERT INTO Auditoria_CancelacionMedio (FechaRegistro, TipoMovimiento, IdCancelacionMedio, IdEjecucionPaso, Motivo, FechaHoraCancelacion, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado)
        SELECT @FechaRegistro, 'I', Id, IdEjecucionPaso, Motivo, FechaHoraCancelacion, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado
        FROM inserted;
    END

    -- Para UPDATE Y DELETE
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        INSERT INTO Auditoria_CancelacionMedio (FechaRegistro, TipoMovimiento, IdCancelacionMedio, IdEjecucionPaso, Motivo, FechaHoraCancelacion, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado)
        SELECT @FechaRegistro, CASE WHEN (i.Borrado = 1 AND d.Borrado = 0) THEN 'D' ELSE 'U' END, i.Id, i.IdEjecucionPaso, i.Motivo, i.FechaHoraCancelacion, i.FechaCreacion, i.CreadoPor, i.FechaModificacion, i.ModificadoPor, i.FechaEliminacion, i.EliminadoPor, i.Borrado
        FROM inserted i
        JOIN deleted d ON i.Id = d.Id;
    END
END;
GO