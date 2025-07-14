CREATE TABLE [OPE_Pais](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdPais] int NOT NULL FOREIGN KEY REFERENCES Pais(Id),
	[Extranjero] [bit] NOT NULL,
	[OpePuertos] [bit] NOT NULL,
	[OpeDatosAsistencias] [bit] NOT NULL,
	[RutaImagen] varchar(255) NULL,
	[Borrado] BIT NOT NULL DEFAULT 0,
    [Editable] BIT NOT NULL DEFAULT 1,
	
PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
    
    -- Agregar restricción UNIQUE para el pais
    CONSTRAINT UC_OPE_Pais_IdPais UNIQUE ([IdPais])
) ON [PRIMARY]
GO






