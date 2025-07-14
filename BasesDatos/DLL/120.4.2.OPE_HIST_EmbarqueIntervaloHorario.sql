CREATE TABLE OPE_HIST_EmbarqueIntervaloHorario (
	Id int NOT NULL IDENTITY(1,1),
	FechaHoraInicio datetime2 NOT NULL,
	FechaHoraFin datetime2 NOT NULL,
	[Nombre] AS FORMAT([FechaHoraInicio], 'hh\:mm') + ' - ' + FORMAT([FechaHoraFin], 'hh\:mm'),
	IdIntervalo int NOT NULL,
	--
	[Borrado] [bit] DEFAULT 0 NOT NULL,
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_EmbarqueDiario PRIMARY KEY (Id),
	CONSTRAINT FK_OPE_HIST_TipoHoraPuntoControlOPE_HIST_EmbarqueIntervaloHorario FOREIGN KEY (IdIntervalo) REFERENCES OPE_HIST_TipoHoraPuntoControl(Id) 
	ON DELETE NO ACTION 
    ON UPDATE NO ACTION
);