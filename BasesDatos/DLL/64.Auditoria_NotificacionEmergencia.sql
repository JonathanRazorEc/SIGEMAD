CREATE TABLE Auditoria_NotificacionEmergencia (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FechaRegistro DATETIME2 NOT NULL,
    TipoMovimiento CHAR(1) CHECK (TipoMovimiento IN ('I', 'U', 'D')) NOT NULL,
    IdNotificacionEmergencia INT NOT NULL,
    IdActuacionRelevanteDGPCE INT NOT NULL,
    IdTipoNotificacion INT NOT NULL,
    FechaHoraNotificacion DATETIME2 NOT NULL,
    OrganosNotificados NVARCHAR(510) NOT NULL,
    UCPM NVARCHAR(510),
    OrganismoInternacional NVARCHAR(510),
    OtrosPaises NVARCHAR(510),
    Observaciones NVARCHAR(MAX),
    FechaCreacion DATETIME2 NOT NULL,
    CreadoPor UNIQUEIDENTIFIER,
    FechaModificacion DATETIME2,
    ModificadoPor UNIQUEIDENTIFIER,
    FechaEliminacion DATETIME2,
    EliminadoPor UNIQUEIDENTIFIER,
    Borrado BIT NOT NULL DEFAULT 0,
	FOREIGN KEY (IdNotificacionEmergencia) REFERENCES dbo.NotificacionEmergencia(Id),
    FOREIGN KEY (IdActuacionRelevanteDGPCE) REFERENCES dbo.ActuacionRelevanteDGPCE(Id),
    FOREIGN KEY (IdTipoNotificacion) REFERENCES dbo.TipoNotificacion(Id)
);
GO


CREATE TRIGGER trg_Auditoria_NotificacionEmergencia
ON dbo.NotificacionEmergencia
FOR INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @FechaRegistro DATETIME2 = SYSDATETIME();
    DECLARE @TipoMovimiento CHAR(1);

    -- Para INSERT
    IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted) 
    BEGIN
        INSERT INTO Auditoria_NotificacionEmergencia (FechaRegistro, TipoMovimiento, IdNotificacionEmergencia, IdActuacionRelevanteDGPCE, IdTipoNotificacion, FechaHoraNotificacion, OrganosNotificados, UCPM, OrganismoInternacional, OtrosPaises, Observaciones, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado)
        SELECT @FechaRegistro, 'I', Id, IdActuacionRelevanteDGPCE, IdTipoNotificacion, FechaHoraNotificacion, OrganosNotificados, UCPM, OrganismoInternacional, OtrosPaises, Observaciones, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado
        FROM inserted;
    END

    -- Para UPDATE Y DELETE
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        INSERT INTO Auditoria_NotificacionEmergencia (FechaRegistro, TipoMovimiento, IdNotificacionEmergencia, IdActuacionRelevanteDGPCE, IdTipoNotificacion, FechaHoraNotificacion, OrganosNotificados, UCPM, OrganismoInternacional, OtrosPaises, Observaciones, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado)
        SELECT @FechaRegistro, CASE WHEN (i.Borrado = 1 AND d.Borrado = 0) THEN 'D' ELSE 'U' END, i.Id, i.IdActuacionRelevanteDGPCE, i.IdTipoNotificacion, i.FechaHoraNotificacion, i.OrganosNotificados, i.UCPM, i.OrganismoInternacional, i.OtrosPaises, i.Observaciones, i.FechaCreacion, i.CreadoPor, i.FechaModificacion, i.ModificadoPor, i.FechaEliminacion, i.EliminadoPor, i.Borrado
        FROM inserted i
        JOIN deleted d ON i.Id = d.Id;
    END
END;
GO