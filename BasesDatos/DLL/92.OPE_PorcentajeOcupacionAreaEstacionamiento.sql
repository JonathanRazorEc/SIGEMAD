CREATE TABLE [OPE_PorcentajeOcupacionAreaEstacionamiento](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdOpeOcupacion] int NOT NULL FOREIGN KEY REFERENCES OPE_Ocupacion(Id),
	[PorcentajeInferior] int NOT NULL,
	[PorcentajeSuperior] int NOT NULL,
	[Nombre] AS CAST([PorcentajeInferior] AS VARCHAR(3)) + ' - ' + CAST([PorcentajeSuperior] AS VARCHAR(3)),
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




