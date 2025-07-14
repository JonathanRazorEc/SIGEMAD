-- Tabla de Tipos de Registro de Actualización
CREATE TABLE TipoRegistroActualizacion (
    Id INT NOT NULL PRIMARY KEY,
    Nombre NVARCHAR(255) NOT NULL UNIQUE  -- Ejemplo: "Evolución", "Dirección y Coordinación"
);

-- Tabla de Apartados dentro de un Tipo de Registro
CREATE TABLE ApartadoRegistro (
    Id INT NOT NULL PRIMARY KEY,
    IdTipoRegistroActualizacion INT NOT NULL FOREIGN KEY REFERENCES TipoRegistroActualizacion(Id),
    Nombre NVARCHAR(255) NOT NULL,  -- Ejemplo: "Dirección", "Coordinación PMA", "Área Afectada",
    Orden INT NOT NULL DEFAULT 0  -- Orden de aparición en la interfaz
);

-- Tabla de estados
CREATE TABLE EstadoRegistro (
    Id INT PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL
);

-- Tabla de Registros de Actualización (Sesión de cambios)
CREATE TABLE RegistroActualizacion (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdSuceso INT NOT NULL FOREIGN KEY REFERENCES Suceso(Id),
    IdTipoRegistroActualizacion INT NOT NULL FOREIGN KEY REFERENCES TipoRegistroActualizacion(Id),
    IdReferencia INT NOT NULL,  -- ID de la cabecera del registro (DireccionCoordinacionEmergencia, etc.)
    TipoEntidad NVARCHAR(50) NOT NULL, -- 'Direccion', 'CoordinacionCecopi', 'CoordinacionPMA'
    FechaHora DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
    --
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
    CreadoPor UNIQUEIDENTIFIER NULL,
    FechaModificacion DATETIME2(7) NULL,
    ModificadoPor UNIQUEIDENTIFIER NULL,
    FechaEliminacion DATETIME2(7) NULL,
    EliminadoPor UNIQUEIDENTIFIER NULL,
    Borrado BIT NOT NULL DEFAULT 0
);

-- Relación entre RegistroActualizacion y ApartadoRegistro
CREATE TABLE RegistroApartado (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdRegistroActualizacion INT NOT NULL FOREIGN KEY REFERENCES RegistroActualizacion(Id),
    IdApartadoRegistro INT NOT NULL FOREIGN KEY REFERENCES ApartadoRegistro(Id)
);

-- Tabla de Registro Detalle (Rastreo de cambios en registros específicos)
CREATE TABLE DetalleRegistroActualizacion (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdRegistroActualizacion INT NOT NULL FOREIGN KEY REFERENCES RegistroActualizacion(Id),
    IdApartadoRegistro INT NOT NULL FOREIGN KEY REFERENCES ApartadoRegistro(Id), 
    IdReferencia INT NOT NULL,  -- ID del registro en Direccion, CoordinacionCecopi, CoordinacionPMA
    IdEstadoRegistro INT NOT NULL FOREIGN KEY REFERENCES EstadoRegistro(Id), 
    Ambito NVARCHAR(50) NULL,
    Descripcion NVARCHAR(MAX) NULL,
    --
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
    CreadoPor UNIQUEIDENTIFIER NULL,
    FechaModificacion DATETIME2(7) NULL,
    ModificadoPor UNIQUEIDENTIFIER NULL,
    FechaEliminacion DATETIME2(7) NULL,
    EliminadoPor UNIQUEIDENTIFIER NULL,
    Borrado BIT NOT NULL DEFAULT 0
);

-- Tabla de Historial de Cambios (Registro de qué campos se modificaron)
CREATE TABLE HistorialCambios (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdDetalleRegistroActualizacion INT NOT NULL FOREIGN KEY REFERENCES DetalleRegistroActualizacion(Id),
    IdReferencia INT NOT NULL,  -- ID de la tabla modificada (Direccion, CoordinacionCecopi, CoordinacionPMA)
    TablaModificada NVARCHAR(255) NOT NULL, -- Nombre de la tabla afectada
    CampoModificado NVARCHAR(255) NOT NULL, -- Nombre del campo modificado
    ValorAnterior NVARCHAR(MAX) NULL, -- Valor antes del cambio
    ValorNuevo NVARCHAR(MAX) NOT NULL, -- Valor después del cambio
    FechaModificacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
    ModificadoPor UNIQUEIDENTIFIER NULL
);

CREATE TABLE dbo.Registro (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdSuceso INT NOT NULL FOREIGN KEY REFERENCES Suceso(Id),
    FechaHoraEvolucion DATETIME2(7) NULL,
    IdEntradaSalida int NULL FOREIGN KEY REFERENCES EntradaSalida(Id),
    IdMedio int NULL FOREIGN KEY REFERENCES Medio(Id),
    ---
    FechaCreacion DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
	CreadoPor nvarchar(450) NULL FOREIGN KEY REFERENCES AspNetUsers(Id),
	FechaModificacion DATETIME2(7) NULL,
	ModificadoPor nvarchar(450) NULL FOREIGN KEY REFERENCES AspNetUsers(Id),
	FechaEliminacion DATETIME2(7) NULL,
	EliminadoPor nvarchar(450) NULL FOREIGN KEY REFERENCES AspNetUsers(Id),
	Borrado BIT NOT NULL DEFAULT 0
);