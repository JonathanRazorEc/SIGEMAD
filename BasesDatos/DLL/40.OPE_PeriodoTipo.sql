CREATE TABLE [OPE_PeriodoTipo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](255) NOT NULL,
	[Borrado] [bit] NOT NULL DEFAULT 0,
	[Editable] bit DEFAULT 1 NOT NULL,
	
PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
    
    -- Agregar restricción UNIQUE para la columna Nombre
    CONSTRAINT UC_OPE_PeriodoTipo_Nombre UNIQUE ([Nombre])
) ON [PRIMARY]
GO






