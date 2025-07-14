CREATE TABLE Auditoria_ActivacionPlanEmergencia (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FechaRegistro DATETIME2 NOT NULL,
    TipoMovimiento CHAR(1) CHECK (TipoMovimiento IN ('I', 'U', 'D')) NOT NULL,
    IdActivacionPlanEmergencia INT NOT NULL,
    IdRegistro INT NOT NULL,
    IdTipoPlan INT,
    IdPlanEmergencia INT,
    TipoPlanPersonalizado NVARCHAR(510),
    PlanEmergenciaPersonalizado NVARCHAR(510),
    FechaHoraInicio DatetimeOffset NOT NULL,
    FechaHoraFin DatetimeOffset,
    Autoridad NVARCHAR(510) NOT NULL,
    Observaciones NVARCHAR(MAX),
    IdArchivo UNIQUEIDENTIFIER,
    FechaCreacion DATETIME2 NOT NULL,
    CreadoPor UNIQUEIDENTIFIER,
    FechaModificacion DATETIME2,
    ModificadoPor UNIQUEIDENTIFIER,
    FechaEliminacion DATETIME2,
    EliminadoPor UNIQUEIDENTIFIER,
    Borrado BIT NOT NULL,
	FOREIGN KEY (IdActivacionPlanEmergencia) REFERENCES dbo.ActivacionPlanEmergencia(Id),
    FOREIGN KEY (IdRegistro) REFERENCES dbo.Registro(Id),
    FOREIGN KEY (IdTipoPlan) REFERENCES dbo.TipoPlan(Id),
    FOREIGN KEY (IdPlanEmergencia) REFERENCES dbo.PlanEmergencia(Id),
    FOREIGN KEY (IdArchivo) REFERENCES dbo.Archivo(Id)
);
GO

CREATE TRIGGER trg_Auditoria_Activacion_ActivacionPlanEmergencia
ON dbo.ActivacionPlanEmergencia
FOR INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @FechaRegistro DATETIME2 = SYSDATETIME();
    DECLARE @TipoMovimiento CHAR(1);

    -- Para INSERT
    IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted) 
    BEGIN
        INSERT INTO Auditoria_ActivacionPlanEmergencia (FechaRegistro, TipoMovimiento, IdActivacionPlanEmergencia, IdRegistro, IdTipoPlan, IdPlanEmergencia, TipoPlanPersonalizado, PlanEmergenciaPersonalizado, FechaHoraInicio, FechaHoraFin, Autoridad, Observaciones, IdArchivo, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado)
        SELECT @FechaRegistro, 'I', Id, IdRegistro, IdTipoPlan, IdPlanEmergencia, TipoPlanPersonalizado, PlanEmergenciaPersonalizado, FechaHoraInicio, FechaHoraFin, Autoridad, Observaciones, IdArchivo, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado
        FROM inserted;
    END

    -- Para UPDATE Y DELETE
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        INSERT INTO Auditoria_ActivacionPlanEmergencia (FechaRegistro, TipoMovimiento, IdActivacionPlanEmergencia, IdRegistro, IdTipoPlan, IdPlanEmergencia, TipoPlanPersonalizado, PlanEmergenciaPersonalizado, FechaHoraInicio, FechaHoraFin, Autoridad, Observaciones, IdArchivo, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado)
        SELECT @FechaRegistro, CASE WHEN (i.Borrado = 1 AND d.Borrado = 0) THEN 'D' ELSE 'U' END, i.Id, i.IdRegistro, i.IdTipoPlan, i.IdPlanEmergencia, i.TipoPlanPersonalizado, i.PlanEmergenciaPersonalizado, i.FechaHoraInicio, i.FechaHoraFin, i.Autoridad, i.Observaciones, i.IdArchivo, i.FechaCreacion, i.CreadoPor, i.FechaModificacion, i.ModificadoPor, i.FechaEliminacion, i.EliminadoPor, i.Borrado
        FROM inserted i
        JOIN deleted d ON i.Id = d.Id;
    END
END;
GO