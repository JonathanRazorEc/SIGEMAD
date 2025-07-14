CREATE TABLE Auditoria_OfrecimientoMedio (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FechaRegistro DATETIME2 NOT NULL,
    TipoMovimiento CHAR(1) CHECK (TipoMovimiento IN ('I', 'U', 'D')) NOT NULL,
    IdOfrecimientoMedio INT NOT NULL,
    IdEjecucionPaso INT NOT NULL,
    TitularMedio NVARCHAR(400) NOT NULL,
    GestionCECOD BIT,
    FechaHoraOfrecimiento DATETIME2 NOT NULL,
    Descripcion NVARCHAR(MAX),
    FechaHoraDisponibilidad DATETIME2,
    Observaciones NVARCHAR(MAX),
    FechaCreacion DATETIME2 NOT NULL,
    CreadoPor UNIQUEIDENTIFIER,
    FechaModificacion DATETIME2,
    ModificadoPor UNIQUEIDENTIFIER,
    FechaEliminacion DATETIME2,
    EliminadoPor UNIQUEIDENTIFIER,
    Borrado BIT NOT NULL,
    FOREIGN KEY (IdOfrecimientoMedio) REFERENCES dbo.OfrecimientoMedio(Id),
	FOREIGN KEY (IdEjecucionPaso) REFERENCES dbo.EjecucionPaso(Id)
);
GO

CREATE TRIGGER trg_Auditoria_OfrecimientoMedio
ON dbo.OfrecimientoMedio
FOR INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @FechaRegistro DATETIME2 = SYSDATETIME();
    DECLARE @TipoMovimiento CHAR(1);

    -- Para INSERT
    IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted) 
    BEGIN
        INSERT INTO Auditoria_OfrecimientoMedio (FechaRegistro, TipoMovimiento, IdOfrecimientoMedio, IdEjecucionPaso, TitularMedio, GestionCECOD, FechaHoraOfrecimiento, Descripcion, FechaHoraDisponibilidad, Observaciones, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado)
        SELECT @FechaRegistro, 'I', Id, IdEjecucionPaso, TitularMedio, GestionCECOD, FechaHoraOfrecimiento, Descripcion, FechaHoraDisponibilidad, Observaciones, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado
        FROM inserted;
    END

    -- Para UPDATE Y DELETE
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        INSERT INTO Auditoria_OfrecimientoMedio (FechaRegistro, TipoMovimiento, IdOfrecimientoMedio, IdEjecucionPaso, TitularMedio, GestionCECOD, FechaHoraOfrecimiento, Descripcion, FechaHoraDisponibilidad, Observaciones, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado)
        SELECT @FechaRegistro, CASE WHEN (i.Borrado = 1 AND d.Borrado = 0) THEN 'D' ELSE 'U' END, i.Id, i.IdEjecucionPaso, i.TitularMedio, i.GestionCECOD, i.FechaHoraOfrecimiento, i.Descripcion, i.FechaHoraDisponibilidad, i.Observaciones, i.FechaCreacion, i.CreadoPor, i.FechaModificacion, i.ModificadoPor, i.FechaEliminacion, i.EliminadoPor, i.Borrado
        FROM inserted i
        JOIN deleted d ON i.Id = d.Id;
    END
END;
GO