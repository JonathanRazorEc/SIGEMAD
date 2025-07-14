CREATE TABLE [OPE_LineaMaritima](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](255) NOT NULL,
	[IdOpePuertoOrigen] int NOT NULL FOREIGN KEY REFERENCES OPE_Puerto(Id),
	[IdOpePuertoDestino] int NOT NULL FOREIGN KEY REFERENCES OPE_Puerto(Id),
	[IdOpeFase] int NOT NULL FOREIGN KEY REFERENCES OPE_Fase(Id),
	[FechaValidezDesde] [datetime2](7) NOT NULL,
	[FechaValidezHasta] [datetime2](7) NULL,
	[NumeroRotaciones] int NULL,
	[NumeroPasajeros] int NULL,
	[NumeroTurismos] int NULL,
	[NumeroAutocares] int NULL,
	[NumeroCamiones] int NULL,
	[NumeroTotalVehiculos] int NULL,
	[FechaCreacion] [datetime2](7) NOT NULL,
	[CreadoPor] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime2](7) NULL,
	[ModificadoPor] [uniqueidentifier] NULL,
	[FechaEliminacion] [datetime2](7) NULL,
	[EliminadoPor] [uniqueidentifier] NULL,
	[Borrado] BIT NOT NULL DEFAULT 0,
    [Editable] BIT NOT NULL DEFAULT 1,
	
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO




