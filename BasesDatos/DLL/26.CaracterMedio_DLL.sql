CREATE TABLE dbo.CaracterMedio (
	Id int PRIMARY KEY IDENTITY(1,1),
	Descripcion varchar(250) NOT NULL,
	Obsoleto BIT NOT NULL
);