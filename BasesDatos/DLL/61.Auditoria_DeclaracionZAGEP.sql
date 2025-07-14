CREATE TABLE Auditoria_DeclaracionZAGEP (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FechaRegistro DATETIME2 NOT NULL,
    TipoMovimiento CHAR(1) CHECK (TipoMovimiento IN ('I', 'U', 'D')) NOT NULL,
    IdDeclaracionZAGEP INT NOT NULL,
    IdActuacionRelevanteDGPCE INT NOT NULL,
    FechaSolicitud DATE NOT NULL,
    Denominacion NVARCHAR(510) NOT NULL,
    Observaciones NVARCHAR(MAX),
    FechaCreacion DATETIME2 NOT NULL,
    CreadoPor UNIQUEIDENTIFIER,
    FechaModificacion DATETIME2,
    ModificadoPor UNIQUEIDENTIFIER,
    FechaEliminacion DATETIME2,
    EliminadoPor UNIQUEIDENTIFIER,
    Borrado BIT NOT NULL,
	FOREIGN KEY (IdDeclaracionZAGEP) REFERENCES dbo.DeclaracionZAGEP(Id),
    FOREIGN KEY (IdActuacionRelevanteDGPCE) REFERENCES dbo.ActuacionRelevanteDGPCE(Id)
);
GO

CREATE TRIGGER trg_Auditoria_DeclaracionZAGEP
ON dbo.DeclaracionZAGEP
FOR INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @FechaRegistro DATETIME2 = SYSDATETIME();
    DECLARE @TipoMovimiento CHAR(1);

    -- Para INSERT
    IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted) 
    BEGIN
        INSERT INTO Auditoria_DeclaracionZAGEP (FechaRegistro, TipoMovimiento, IdDeclaracionZAGEP, IdActuacionRelevanteDGPCE, FechaSolicitud, Denominacion, Observaciones, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado)
        SELECT @FechaRegistro, 'I', Id, IdActuacionRelevanteDGPCE, FechaSolicitud, Denominacion, Observaciones, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado
        FROM inserted;
    END

    -- Para UPDATE Y DELETE
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        INSERT INTO Auditoria_DeclaracionZAGEP (FechaRegistro, TipoMovimiento, IdDeclaracionZAGEP, IdActuacionRelevanteDGPCE, FechaSolicitud, Denominacion, Observaciones, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado)
        SELECT @FechaRegistro, CASE WHEN (i.Borrado = 1 AND d.Borrado = 0) THEN 'D' ELSE 'U' END, i.Id, i.IdActuacionRelevanteDGPCE, i.FechaSolicitud, i.Denominacion, i.Observaciones, i.FechaCreacion, i.CreadoPor, i.FechaModificacion, i.ModificadoPor, i.FechaEliminacion, i.EliminadoPor, i.Borrado
        FROM inserted i
        JOIN deleted d ON i.Id = d.Id;
    END
END;
GO