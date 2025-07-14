-- Crear la tabla AuditoriaIncendio
CREATE TABLE Auditoria_Incendio (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FechaRegistro DATETIME2 NOT NULL,
    TipoMovimiento CHAR(1) CHECK (TipoMovimiento IN ('I', 'U', 'D')) NOT NULL,
    IdIncendio INT NOT NULL,
    IdSuceso INT NOT NULL,
    IdTerritorio INT NOT NULL,
    IdClaseSuceso INT NOT NULL,
    IdEstadoSuceso INT,
    IdPais INT NOT NULL,
    EsLimitrofe BIT NOT NULL,
    IdDistrito INT,
    IdMunicipioExtranjero INT,
    IdProvincia INT,
    IdMunicipio INT,
    FechaInicio DATETIME2 NOT NULL ,
    Ubicacion NVARCHAR(510),
    Denominacion NVARCHAR(510) NOT NULL,
    NotaGeneral NVARCHAR(MAX),
    RutaMapaRiesgo NVARCHAR(MAX),
    UTM_X DECIMAL(18,9),
    UTM_Y DECIMAL(18,9),
    Huso INT,
    GeoPosicion GEOMETRY,
    FechaCreacion DATETIME2 NOT NULL,
    CreadoPor UNIQUEIDENTIFIER,
    FechaModificacion DATETIME2,
    ModificadoPor UNIQUEIDENTIFIER,
    FechaEliminacion DATETIME2,
    EliminadoPor UNIQUEIDENTIFIER,
    Borrado BIT NOT NULL,
	FOREIGN KEY (IdIncendio) REFERENCES dbo.Incendio(Id),
    FOREIGN KEY (IdSuceso) REFERENCES dbo.Suceso(Id),
    FOREIGN KEY (IdTerritorio) REFERENCES dbo.Territorio(Id),
    FOREIGN KEY (IdClaseSuceso) REFERENCES dbo.ClaseSuceso(Id),
    FOREIGN KEY (IdEstadoSuceso) REFERENCES dbo.EstadoSuceso(Id),
    FOREIGN KEY (IdPais) REFERENCES dbo.Pais(Id),
    FOREIGN KEY (IdDistrito) REFERENCES dbo.Distrito(Id),
    FOREIGN KEY (IdMunicipioExtranjero) REFERENCES dbo.MunicipioExtranjero(Id),
    FOREIGN KEY (IdProvincia) REFERENCES dbo.Provincia(Id),
    FOREIGN KEY (IdMunicipio) REFERENCES dbo.Municipio(Id)
);
GO

-- Crear el trigger trg_Auditoria_Incendio
CREATE TRIGGER trg_Auditoria_Incendio
ON dbo.Incendio
FOR INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @FechaRegistro DATETIME2 = SYSDATETIME();
    DECLARE @TipoMovimiento CHAR(1);

    -- Para INSERT
    IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted) 
    BEGIN
        INSERT INTO Auditoria_Incendio (FechaRegistro, TipoMovimiento, IdIncendio, IdSuceso, IdTerritorio, IdClaseSuceso, IdEstadoSuceso, IdPais, EsLimitrofe, IdDistrito, IdMunicipioExtranjero, IdProvincia, IdMunicipio, FechaInicio, Ubicacion, Denominacion, NotaGeneral, RutaMapaRiesgo, UTM_X, UTM_Y, Huso, GeoPosicion, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado)
        SELECT @FechaRegistro, 'I', Id, IdSuceso, IdTerritorio, IdClaseSuceso, IdEstadoSuceso, IdPais, EsLimitrofe, IdDistrito, IdMunicipioExtranjero, IdProvincia, IdMunicipio, FechaInicio, Ubicacion, Denominacion, NotaGeneral, RutaMapaRiesgo, UTM_X, UTM_Y, Huso, GeoPosicion, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado
        FROM inserted;
    END

    -- Para UPDATE Y DELETE
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN

        INSERT INTO Auditoria_Incendio (FechaRegistro, TipoMovimiento, IdIncendio, IdSuceso, IdTerritorio, IdClaseSuceso, IdEstadoSuceso, IdPais, EsLimitrofe, IdDistrito, IdMunicipioExtranjero, IdProvincia, IdMunicipio, FechaInicio, Ubicacion, Denominacion, NotaGeneral, RutaMapaRiesgo, UTM_X, UTM_Y, Huso, GeoPosicion, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, FechaEliminacion, EliminadoPor, Borrado)
        SELECT @FechaRegistro, CASE WHEN (i.Borrado = 1 AND d.Borrado = 0) THEN 'D' ELSE 'U' END, i.Id, i.IdSuceso, i.IdTerritorio, i.IdClaseSuceso, i.IdEstadoSuceso, i.IdPais, i.EsLimitrofe, i.IdDistrito, i.IdMunicipioExtranjero, i.IdProvincia, i.IdMunicipio, i.FechaInicio, i.Ubicacion, i.Denominacion, i.NotaGeneral, i.RutaMapaRiesgo, i.UTM_X, i.UTM_Y, i.Huso, i.GeoPosicion, i.FechaCreacion, i.CreadoPor, i.FechaModificacion, i.ModificadoPor, i.FechaEliminacion, i.EliminadoPor, i.Borrado
        FROM inserted i
        JOIN deleted d ON i.Id = d.Id;
    END
END;
GO