CREATE TABLE [OPE_DatoFronteraIntervaloHorario](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Inicio] TIME NOT NULL,
    [Fin] TIME NOT NULL,
	[Nombre] AS FORMAT([Inicio], 'hh\:mm') + ' - ' + FORMAT([Fin], 'hh\:mm'),
	[Borrado] BIT NOT NULL DEFAULT 0,
    [Editable] BIT NOT NULL DEFAULT 1,
	
PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
    
    -- Agregar restricción UNIQUE para los intervalos
    CONSTRAINT UQ_OPE_DatoFronteraIntervaloHorario_InicioFin UNIQUE ([Inicio], [Fin]), -- esta línea evita duplicados de intervalos
	-- Asegura que el inicio sea menor al fin
    CONSTRAINT CK_OPE_DatoFronteraIntervaloHorario_ValidInterval CHECK ([Inicio] < [Fin])
) ON [PRIMARY]
GO






