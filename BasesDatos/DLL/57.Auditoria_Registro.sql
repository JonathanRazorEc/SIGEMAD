CREATE TABLE Auditoria_Registro (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FechaRegistro DATETIME2 NOT NULL,
    TipoMovimiento CHAR(1) CHECK (TipoMovimiento IN ('I', 'U', 'D')) NOT NULL,
    IdRegistro INT NOT NULL,
    IdSuceso INT NOT NULL,
    FechaHoraEvolucion DATETIME2,
    IdEntradaSalida INT,
    IdMedio INT,
    FechaCreacion DATETIME2 NOT NULL,
    CreadoPor UNIQUEIDENTIFIER,
    FechaModificacion DATETIME2,
    ModificadoPor UNIQUEIDENTIFIER,
    FechaEliminacion DATETIME2,
    EliminadoPor UNIQUEIDENTIFIER,
    Borrado BIT NOT NULL,
	FOREIGN KEY (IdRegistro) REFERENCES dbo.Registro(Id),
    FOREIGN KEY (IdSuceso) REFERENCES dbo.Suceso(Id),
    FOREIGN KEY (IdEntradaSalida) REFERENCES dbo.EntradaSalida(Id),
    FOREIGN KEY (IdMedio) REFERENCES dbo.Medio(Id)
);
GO

CREATE TRIGGER trg_Auditoria_Registro
ON dbo.Registro
FOR INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @FechaRegistro DATETIME2 = SYSDATETIME();
    DECLARE @TipoMovimiento CHAR(1);

    -- Para INSERT
    IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted) 
    BEGIN
        INSERT INTO Auditoria_Registro (FechaRegistro, TipoMovimiento, IdRegistro, IdSuceso, FechaHoraEvolucion, IdEntradaSalida, IdMedio, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado)
        SELECT @FechaRegistro, 'I', Id, IdSuceso, FechaHoraEvolucion, IdEntradaSalida, IdMedio, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado
        FROM inserted;
    END

    -- Para UPDATE Y DELETE
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        INSERT INTO Auditoria_Registro (FechaRegistro, TipoMovimiento, IdRegistro, IdSuceso, FechaHoraEvolucion, IdEntradaSalida, IdMedio, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado)
        SELECT @FechaRegistro, CASE WHEN (i.Borrado = 1 AND d.Borrado = 0) THEN 'D' ELSE 'U' END, i.Id, i.IdSuceso, i.FechaHoraEvolucion, i.IdEntradaSalida, i.IdMedio, i.FechaCreacion, i.CreadoPor, i.FechaModificacion, i.ModificadoPor, i.FechaEliminacion, i.EliminadoPor, i.Borrado
        FROM inserted i
        JOIN deleted d ON i.Id = d.Id;
    END
END;
GO


