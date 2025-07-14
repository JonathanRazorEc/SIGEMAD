CREATE TABLE Auditoria_Parametro (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FechaRegistro DATETIME2 NOT NULL,
    TipoMovimiento CHAR(1) CHECK (TipoMovimiento IN ('I', 'U', 'D')) NOT NULL,
    IdParametro INT NOT NULL,
    IdRegistro INT NOT NULL,
    IdEstadoIncendio INT,
    FechaFinal DATETIME2,
    IdPlanEmergencia INT,
    IdFaseEmergencia INT,
    IdPlanSituacion INT,
    IdSituacionEquivalente INT,
	FechaHoraActualizacion DATETIME2(7) NOT NULL,
	Observaciones NVARCHAR(MAX) NULL,
    Prevision NVARCHAR(MAX) NULL,
    FechaCreacion DATETIME2 NOT NULL,
    CreadoPor UNIQUEIDENTIFIER,
    FechaModificacion DATETIME2,
    ModificadoPor UNIQUEIDENTIFIER,
    FechaEliminacion DATETIME2,
    EliminadoPor UNIQUEIDENTIFIER,
    Borrado BIT NOT NULL,
	FOREIGN KEY (IdParametro) REFERENCES dbo.Parametro(Id),
    FOREIGN KEY (IdRegistro) REFERENCES dbo.Registro(Id),
    FOREIGN KEY (IdEstadoIncendio) REFERENCES dbo.EstadoIncendio(Id),
    FOREIGN KEY (IdPlanEmergencia) REFERENCES dbo.PlanEmergencia(Id),
    FOREIGN KEY (IdFaseEmergencia) REFERENCES dbo.FaseEmergencia(Id),
    FOREIGN KEY (IdPlanSituacion) REFERENCES PlanSituacion(Id),
    FOREIGN KEY (IdSituacionEquivalente) REFERENCES SituacionEquivalente(Id)
);
GO


CREATE TRIGGER trg_Auditoria_Parametro
ON dbo.Parametro
FOR INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @FechaRegistro DATETIME2 = SYSDATETIME();
    DECLARE @TipoMovimiento CHAR(1);

    -- Para INSERT
    IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted) 
    BEGIN
        INSERT INTO Auditoria_Parametro (FechaRegistro, TipoMovimiento, IdParametro, IdRegistro, IdEstadoIncendio, FechaFinal, IdPlanEmergencia, IdFaseEmergencia, IdPlanSituacion, IdSituacionEquivalente,FechaHoraActualizacion, Observaciones,Prevision, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado)
        SELECT @FechaRegistro, 'I', Id, IdRegistro, IdEstadoIncendio, FechaFinal, IdPlanEmergencia, IdFaseEmergencia, IdPlanSituacion, IdSituacionEquivalente,FechaHoraActualizacion,Observaciones,Prevision, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado
        FROM inserted;
    END

    -- Para UPDATE Y DELETE
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        INSERT INTO Auditoria_Parametro (FechaRegistro, TipoMovimiento, IdParametro, IdRegistro, IdEstadoIncendio, FechaFinal, IdPlanEmergencia, IdFaseEmergencia, IdPlanSituacion, IdSituacionEquivalente,FechaHoraActualizacion, Observaciones,Prevision, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado)
        SELECT @FechaRegistro, CASE WHEN (i.Borrado = 1 AND d.Borrado = 0) THEN 'D' ELSE 'U' END, i.Id, i.IdRegistro, i.IdEstadoIncendio, i.FechaFinal, i.IdPlanEmergencia, i.IdFaseEmergencia, i.IdPlanSituacion, i.IdSituacionEquivalente,i.FechaHoraActualizacion,i.Observaciones,i.Prevision, i.FechaCreacion, i.CreadoPor, i.FechaModificacion, i.ModificadoPor, i.FechaEliminacion, i.EliminadoPor, i.Borrado
        FROM inserted i
        JOIN deleted d ON i.Id = d.Id;
    END
END;
GO