CREATE TABLE OPE_HIST_Frontera (
	Id int NOT NULL,
	Nombre varchar(100) NOT NULL,
	UtmX int NOT NULL,
	UtmY int NOT NULL,
	UmbralMedio int NOT NULL,
	UmbralAlto int NOT NULL,
	UmbralMuyAlto int NOT NULL,
	
	--
	[Editable] bit DEFAULT 1 NOT NULL,
	--
	CONSTRAINT PK_OPE_HIST_Frontera PRIMARY KEY (Id)
);