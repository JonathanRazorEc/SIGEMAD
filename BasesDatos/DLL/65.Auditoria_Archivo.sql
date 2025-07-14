CREATE TABLE Auditoria_Archivo (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FechaRegistro DATETIME2 NOT NULL,
    TipoMovimiento CHAR(1) CHECK (TipoMovimiento IN ('I', 'U', 'D')) NOT NULL,
    IdArchivo UNIQUEIDENTIFIER NOT NULL,
    NombreOriginal NVARCHAR(510) NOT NULL,
    RutaDeAlmacenamiento NVARCHAR(510) NOT NULL,
    NombreUnico NVARCHAR(510) NOT NULL,
    Tipo NVARCHAR(510) NOT NULL,
    Extension NVARCHAR(510) NOT NULL,
    PesoEnBytes BIGINT NOT NULL,
    FechaCreacion DATETIME2 NOT NULL,
    CreadoPor UNIQUEIDENTIFIER,
    FechaModificacion DATETIME2,
    ModificadoPor UNIQUEIDENTIFIER,
    FechaEliminacion DATETIME2,
    EliminadoPor UNIQUEIDENTIFIER,
    Borrado BIT NOT NULL,
    FOREIGN KEY (IdArchivo) REFERENCES dbo.Archivo(Id)
);
GO

CREATE TRIGGER trg_Auditoria_Archivo
ON dbo.Archivo
FOR INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @FechaRegistro DATETIME2 = SYSDATETIME();
    DECLARE @TipoMovimiento CHAR(1);

    -- Para INSERT
    IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted) 
    BEGIN
        INSERT INTO Auditoria_Archivo (FechaRegistro, TipoMovimiento, IdArchivo, NombreOriginal, RutaDeAlmacenamiento, NombreUnico, Tipo, Extension, PesoEnBytes, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado)
        SELECT @FechaRegistro, 'I', Id, NombreOriginal, RutaDeAlmacenamiento, NombreUnico, Tipo, Extension, PesoEnBytes, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado
        FROM inserted;
    END

    -- Para UPDATE Y DELETE
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        INSERT INTO Auditoria_Archivo (FechaRegistro, TipoMovimiento, IdArchivo, NombreOriginal, RutaDeAlmacenamiento, NombreUnico, Tipo, Extension, PesoEnBytes, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado)
        SELECT @FechaRegistro, CASE WHEN (i.Borrado = 1 AND d.Borrado = 0) THEN 'D' ELSE 'U' END, i.Id, i.NombreOriginal, i.RutaDeAlmacenamiento, i.NombreUnico, i.Tipo, i.Extension, i.PesoEnBytes, i.FechaCreacion, i.CreadoPor, i.FechaModificacion, i.ModificadoPor, i.FechaEliminacion, i.EliminadoPor, i.Borrado
        FROM inserted i
        JOIN deleted d ON i.Id = d.Id;
    END
END;
GO