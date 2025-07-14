CREATE TABLE [OPE_DatoFrontera](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdOpeFrontera] int NOT NULL FOREIGN KEY REFERENCES OPE_Frontera(Id),
	[Fecha] [datetime2](7) NOT NULL,
	[IdOpeDatoFronteraIntervaloHorario] int NOT NULL FOREIGN KEY REFERENCES OPE_DatoFronteraIntervaloHorario(Id),
	[IntervaloHorarioPersonalizado] [bit] DEFAULT(0),
	[InicioIntervaloHorarioPersonalizado] TIME ,
    [FinIntervaloHorarioPersonalizado] TIME,
	[NumeroVehiculos] int NOT NULL,
	[Afluencia] [nvarchar](255) NOT NULL,
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




ALTER TABLE [OPE_DatoFrontera]
ADD CONSTRAINT CHK_IntervaloHorarioPersonalizado_Completo
CHECK (
    ([IntervaloHorarioPersonalizado] = 0)
    OR
    ([IntervaloHorarioPersonalizado] = 1 
     AND InicioIntervaloHorarioPersonalizado IS NOT NULL 
     AND FinIntervaloHorarioPersonalizado IS NOT NULL)
);