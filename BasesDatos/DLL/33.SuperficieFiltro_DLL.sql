CREATE TABLE dbo.TipoFiltro (
Id INT PRIMARY KEY,
Expresion NVARCHAR(50) NOT NULL
);
Go

CREATE TABLE dbo.SuperficieFiltro (
Id INT NOT NULL,
Descripcion VARCHAR(255) NOT NULL,
IdTipoFiltro INT NOT NULL,
Valor INT NOT NULL,
Borrado BIT DEFAULT 0 NOT NULL,
Editable BIT DEFAULT 1 NOT NULL,
CONSTRAINT PK_SuperficieFiltro PRIMARY KEY (Id),
CONSTRAINT FK_SuperficieFiltro_TipoFiltro FOREIGN KEY (IdTipoFiltro) REFERENCES TipoFiltro(Id)
);
Go