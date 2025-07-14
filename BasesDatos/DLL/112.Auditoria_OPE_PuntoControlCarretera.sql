CREATE TABLE [Auditoria_OPE_PuntoControlCarretera](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FechaRegistro] datetime2 NOT NULL,
	[TipoMovimiento] char(1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[IdOpePuntoControlCarretera] int NOT NULL  FOREIGN KEY REFERENCES OPE_PuntoControlCarretera(Id),
	[Nombre] [nvarchar](255) NOT NULL,
	[IdCcaa] int NULL FOREIGN KEY REFERENCES CCAA(Id),
	[IdProvincia] int NULL FOREIGN KEY REFERENCES Provincia(Id),
	[IdMunicipio] int NULL FOREIGN KEY REFERENCES Municipio(Id),
	[Carretera] [nvarchar](255) NOT NULL,
	[PK] [int] NOT NULL,
	[CoordenadaUTM_X] [int] NOT NULL,
	[CoordenadaUTM_Y] [int] NOT NULL,
	[TransitoMedioVehiculos] int NOT NULL,
	[TransitoAltoVehiculos] int NOT NULL,
	[FechaCreacion] [datetime2](7)  NOT NULL,
	[CreadoPor] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime2](7) NULL,
	[ModificadoPor] [uniqueidentifier] NULL,
	[FechaEliminacion] [datetime2](7) NULL,
	[EliminadoPor] [uniqueidentifier] NULL,
	[Borrado] [bit] DEFAULT 0 NOT NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE Auditoria_OPE_PuntoControlCarretera WITH NOCHECK ADD CONSTRAINT CK__Auditoria__Ope_PuntoControlCarretera_TipoM__06ED0088 CHECK (([TipoMovimiento]='D' OR [TipoMovimiento]='U' OR [TipoMovimiento]='I'));