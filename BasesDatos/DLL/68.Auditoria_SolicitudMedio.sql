CREATE TABLE Auditoria_SolicitudMedio (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FechaRegistro DATETIME2 NOT NULL,
    TipoMovimiento CHAR(1) CHECK (TipoMovimiento IN ('I', 'U', 'D')) NOT NULL,
    IdSolicitudMedio INT NOT NULL,
    IdEjecucionPaso INT NOT NULL,
    IdProcedenciaMedio INT NOT NULL,
    AutoridadSolicitante NVARCHAR(400) NOT NULL,
    FechaHoraSolicitud DATETIME2 NOT NULL,
    Descripcion NVARCHAR(MAX),
    IdArchivo UNIQUEIDENTIFIER,
    Observaciones NVARCHAR(MAX),
    FechaCreacion DATETIME2 NOT NULL,
    CreadoPor UNIQUEIDENTIFIER,
    FechaModificacion DATETIME2,
    ModificadoPor UNIQUEIDENTIFIER,
    FechaEliminacion DATETIME2,
    EliminadoPor UNIQUEIDENTIFIER,
    Borrado BIT NOT NULL,
	FOREIGN KEY (IdSolicitudMedio) REFERENCES dbo.SolicitudMedio(Id),
    FOREIGN KEY (IdEjecucionPaso) REFERENCES dbo.EjecucionPaso(Id),
    FOREIGN KEY (IdProcedenciaMedio) REFERENCES dbo.ProcedenciaMedio(Id),
    FOREIGN KEY (IdArchivo) REFERENCES dbo.Archivo(Id)
);
GO

CREATE TRIGGER trg_Auditoria_SolicitudMedio
ON dbo.SolicitudMedio
FOR INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @FechaRegistro DATETIME2 = SYSDATETIME();
    DECLARE @TipoMovimiento CHAR(1);

    -- Para INSERT
    IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted) 
    BEGIN
        INSERT INTO Auditoria_SolicitudMedio (FechaRegistro, TipoMovimiento, IdSolicitudMedio, IdEjecucionPaso, IdProcedenciaMedio, AutoridadSolicitante, FechaHoraSolicitud, Descripcion, IdArchivo, Observaciones, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado)
        SELECT @FechaRegistro, 'I', Id, IdEjecucionPaso, IdProcedenciaMedio, AutoridadSolicitante, FechaHoraSolicitud, Descripcion, IdArchivo, Observaciones, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado
        FROM inserted;
    END

    -- Para UPDATE Y DELETE
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        INSERT INTO Auditoria_SolicitudMedio (FechaRegistro, TipoMovimiento, IdSolicitudMedio, IdEjecucionPaso, IdProcedenciaMedio, AutoridadSolicitante, FechaHoraSolicitud, Descripcion, IdArchivo, Observaciones, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado)
        SELECT @FechaRegistro, CASE WHEN (i.Borrado = 1 AND d.Borrado = 0) THEN 'D' ELSE 'U' END, i.Id, i.IdEjecucionPaso, i.IdProcedenciaMedio, i.AutoridadSolicitante, i.FechaHoraSolicitud, i.Descripcion, i.IdArchivo, i.Observaciones, i.FechaCreacion, i.CreadoPor, i.FechaModificacion, i.ModificadoPor, i.FechaEliminacion, i.EliminadoPor, i.Borrado
        FROM inserted i
        JOIN deleted d ON i.Id = d.Id;
    END
END;
GO