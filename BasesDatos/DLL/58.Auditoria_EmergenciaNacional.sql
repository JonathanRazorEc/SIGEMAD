CREATE TABLE Auditoria_EmergenciaNacional (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FechaRegistro DATETIME2 NOT NULL,
    TipoMovimiento CHAR(1) CHECK (TipoMovimiento IN ('I', 'U', 'D')) NOT NULL,
    IdEmergencia INT NOT NULL,
    Autoridad NVARCHAR(510),
    DescripcionSolicitud NVARCHAR(510),
    FechaHoraSolicitud DATETIME2,
    FechaHoraDeclaracion DATETIME2,
    DescripcionDeclaracion NVARCHAR(510),
    FechaHoraDireccion DATETIME2,
    Observaciones NVARCHAR(MAX),
    FechaCreacion DATETIME2 NOT NULL,
    CreadoPor UNIQUEIDENTIFIER,
    FechaModificacion DATETIME2,
    ModificadoPor UNIQUEIDENTIFIER,
    FechaEliminacion DATETIME2,
    EliminadoPor UNIQUEIDENTIFIER,
    Borrado BIT NOT NULL,
    FOREIGN KEY (IdEmergencia) REFERENCES dbo.ActuacionRelevanteDGPCE(Id)
);
GO

CREATE TRIGGER trg_Auditoria_EmergenciaNacional
ON dbo.EmergenciaNacional
FOR INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @FechaRegistro DATETIME2 = SYSDATETIME();
    DECLARE @TipoMovimiento CHAR(1);

    -- Para INSERT
    IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted) 
    BEGIN
        INSERT INTO Auditoria_EmergenciaNacional (FechaRegistro, TipoMovimiento, IdEmergencia, Autoridad, DescripcionSolicitud, FechaHoraSolicitud, FechaHoraDeclaracion, DescripcionDeclaracion, FechaHoraDireccion, Observaciones, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado)
        SELECT @FechaRegistro, 'I', Id, Autoridad, DescripcionSolicitud, FechaHoraSolicitud, FechaHoraDeclaracion, DescripcionDeclaracion, FechaHoraDireccion, Observaciones, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado
        FROM inserted;
    END

    -- Para UPDATE Y DELETE
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        INSERT INTO Auditoria_EmergenciaNacional (FechaRegistro, TipoMovimiento, IdEmergencia, Autoridad, DescripcionSolicitud, FechaHoraSolicitud, FechaHoraDeclaracion, DescripcionDeclaracion, FechaHoraDireccion, Observaciones, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado)
        SELECT @FechaRegistro, CASE WHEN (i.Borrado = 1 AND d.Borrado = 0) THEN 'D' ELSE 'U' END, i.Id, i.Autoridad, i.DescripcionSolicitud, i.FechaHoraSolicitud, i.FechaHoraDeclaracion, i.DescripcionDeclaracion, i.FechaHoraDireccion, i.Observaciones, i.FechaCreacion, i.CreadoPor, i.FechaModificacion, i.ModificadoPor, i.FechaEliminacion, i.EliminadoPor, i.Borrado
        FROM inserted i
        JOIN deleted d ON i.Id = d.Id;
    END
END;
GO