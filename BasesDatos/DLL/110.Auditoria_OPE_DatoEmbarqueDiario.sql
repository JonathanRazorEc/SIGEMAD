CREATE TABLE [Auditoria_OPE_DatoEmbarqueDiario](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FechaRegistro] datetime2 NOT NULL,
	[TipoMovimiento] char(1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[IdOpeDatoEmbarqueDiario] int NOT NULL  FOREIGN KEY REFERENCES OPE_DatoEmbarqueDiario(Id),
	[IdOpeLineaMaritima] int NOT NULL  FOREIGN KEY REFERENCES OPE_LineaMaritima(Id),
	[Fecha] [datetime2](7) NOT NULL,
	[NumeroRotaciones] int NOT NULL,
	[NumeroPasajeros] int NOT NULL,
	[NumeroTurismos] int NOT NULL,
	[NumeroAutocares] int NOT NULL,
	[NumeroCamiones] int NOT NULL,
	[NumeroTotalVehiculos] int NOT NULL,
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

ALTER TABLE Auditoria_OPE_DatoEmbarqueDiario WITH NOCHECK ADD CONSTRAINT CK__Auditoria__Ope_DatoEmbarqueDiario_TipoM__06ED0088 CHECK (([TipoMovimiento]='D' OR [TipoMovimiento]='U' OR [TipoMovimiento]='I'));