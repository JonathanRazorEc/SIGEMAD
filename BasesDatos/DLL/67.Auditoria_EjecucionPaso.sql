CREATE TABLE Auditoria_EjecucionPaso (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FechaRegistro DATETIME2 NOT NULL,
    TipoMovimiento CHAR(1) CHECK (TipoMovimiento IN ('I', 'U', 'D')) NOT NULL,
    IdEjecucionPaso INT NOT NULL,
    IdMovilizacionMedio INT NOT NULL,
    IdPasoMovilizacion INT NOT NULL,
    FechaCreacion DATETIME2 NOT NULL,
    CreadoPor UNIQUEIDENTIFIER,
    FechaModificacion DATETIME2,
    ModificadoPor UNIQUEIDENTIFIER,
    FechaEliminacion DATETIME2,
    EliminadoPor UNIQUEIDENTIFIER,
    Borrado BIT NOT NULL,
	FOREIGN KEY (IdEjecucionPaso) REFERENCES dbo.EjecucionPaso(Id),
    FOREIGN KEY (IdMovilizacionMedio) REFERENCES dbo.MovilizacionMedio(Id),
    FOREIGN KEY (IdPasoMovilizacion) REFERENCES dbo.PasoMovilizacion(Id)
);
GO

CREATE TRIGGER trg_Auditoria_EjecucionPaso
ON dbo.EjecucionPaso
FOR INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @FechaRegistro DATETIME2 = SYSDATETIME();
    DECLARE @TipoMovimiento CHAR(1);

    -- Para INSERT
    IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted) 
    BEGIN
        INSERT INTO Auditoria_EjecucionPaso (FechaRegistro, TipoMovimiento, IdEjecucionPaso, IdMovilizacionMedio, IdPasoMovilizacion, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado)
        SELECT @FechaRegistro, 'I', Id, IdMovilizacionMedio, IdPasoMovilizacion, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado
        FROM inserted;
    END

    -- Para UPDATE Y DELETE
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        INSERT INTO Auditoria_EjecucionPaso (FechaRegistro, TipoMovimiento, IdEjecucionPaso, IdMovilizacionMedio, IdPasoMovilizacion, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado)
        SELECT @FechaRegistro, CASE WHEN (i.Borrado = 1 AND d.Borrado = 0) THEN 'D' ELSE 'U' END, i.Id, i.IdMovilizacionMedio, i.IdPasoMovilizacion, i.FechaCreacion, i.CreadoPor, i.FechaModificacion, i.ModificadoPor, i.FechaEliminacion, i.EliminadoPor, i.Borrado
        FROM inserted i
        JOIN deleted d ON i.Id = d.Id;
    END
END;
GO