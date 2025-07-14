CREATE TABLE Auditoria_ActivacionSistema (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FechaRegistro DATETIME2 NOT NULL,
    TipoMovimiento CHAR(1) CHECK (TipoMovimiento IN ('I', 'U', 'D')) NOT NULL,
    IdActivacionSistema INT NOT NULL,
    IdRegistro INT NOT NULL,
    IdTipoSistemaEmergencia INT NOT NULL,
    FechaHoraActivacion DATETIME2,
    FechaHoraActualizacion DATETIME2,
    Autoridad NVARCHAR(510),
    DescripcionSolicitud NVARCHAR(MAX),
    Observaciones NVARCHAR(MAX),
    IdModoActivacion INT,
    FechaActivacion DATE,
    Codigo NVARCHAR(30),
    Nombre NVARCHAR(300),
    UrlAcceso NVARCHAR(MAX),
    FechaHoraPeticion DATETIME2,
    FechaAceptacion DATE,
    Peticiones NVARCHAR(MAX),
    MediosCapacidades NVARCHAR(MAX),
    FechaCreacion DATETIME2 NOT NULL,
    CreadoPor UNIQUEIDENTIFIER,
    FechaModificacion DATETIME2,
    ModificadoPor UNIQUEIDENTIFIER,
    FechaEliminacion DATETIME2,
    EliminadoPor UNIQUEIDENTIFIER,
    Borrado BIT NOT NULL,
	FOREIGN KEY (IdActivacionSistema) REFERENCES ActivacionSistema(Id),
    FOREIGN KEY (IdRegistro) REFERENCES Registro(Id),
    FOREIGN KEY (IdModoActivacion) REFERENCES ModoActivacion(Id),
    FOREIGN KEY (IdTipoSistemaEmergencia) REFERENCES TipoSistemaEmergencia(Id)
);
GO

CREATE TRIGGER trg_Auditoria_ActivacionSistema
ON ActivacionSistema
FOR INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @FechaRegistro DATETIME2 = SYSDATETIME();
    DECLARE @TipoMovimiento CHAR(1);

    -- Para INSERT
    IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted) 
    BEGIN
        INSERT INTO Auditoria_ActivacionSistema (FechaRegistro, TipoMovimiento, IdActivacionSistema, IdRegistro, IdTipoSistemaEmergencia, FechaHoraActivacion, FechaHoraActualizacion, Autoridad, DescripcionSolicitud, Observaciones, IdModoActivacion, FechaActivacion, Codigo, Nombre, UrlAcceso, FechaHoraPeticion, FechaAceptacion, Peticiones, MediosCapacidades, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado)
        SELECT @FechaRegistro, 'I', Id, IdRegistro, IdTipoSistemaEmergencia, FechaHoraActivacion, FechaHoraActualizacion, Autoridad, DescripcionSolicitud, Observaciones, IdModoActivacion, FechaActivacion, Codigo, Nombre, UrlAcceso, FechaHoraPeticion, FechaAceptacion, Peticiones, MediosCapacidades, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado
        FROM inserted;
    END

    -- Para UPDATE Y DELETE
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        INSERT INTO Auditoria_ActivacionSistema (FechaRegistro, TipoMovimiento, IdActivacionSistema, IdRegistro, IdTipoSistemaEmergencia, FechaHoraActivacion, FechaHoraActualizacion, Autoridad, DescripcionSolicitud, Observaciones, IdModoActivacion, FechaActivacion, Codigo, Nombre, UrlAcceso, FechaHoraPeticion, FechaAceptacion, Peticiones, MediosCapacidades, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado)
        SELECT @FechaRegistro, CASE WHEN (i.Borrado = 1 AND d.Borrado = 0) THEN 'D' ELSE 'U' END, i.Id, i.IdRegistro, i.IdTipoSistemaEmergencia, i.FechaHoraActivacion, i.FechaHoraActualizacion, i.Autoridad, i.DescripcionSolicitud, i.Observaciones, i.IdModoActivacion, i.FechaActivacion, i.Codigo, i.Nombre, i.UrlAcceso, i.FechaHoraPeticion, i.FechaAceptacion, i.Peticiones, i.MediosCapacidades, i.FechaCreacion, i.CreadoPor, i.FechaModificacion, i.ModificadoPor, i.FechaEliminacion, i.EliminadoPor, i.Borrado
        FROM inserted i
        JOIN deleted d ON i.Id = d.Id;
    END
END;
GO