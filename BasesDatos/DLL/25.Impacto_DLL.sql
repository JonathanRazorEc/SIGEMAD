CREATE TABLE AlteracionInterrupcion (
	Id int NOT NULL IDENTITY(1,1),
	Descripcion nvarchar(100) NOT NULL,
	Borrado bit DEFAULT 0 NOT NULL,
	Editable bit DEFAULT 1 NOT NULL,
	CONSTRAINT PK_Alteracion PRIMARY KEY (Id)
);

CREATE TABLE TipoDanio (
	Id int NOT NULL IDENTITY(1,1),
	Descripcion nvarchar(100) NOT NULL,
	Borrado bit DEFAULT 0 NOT NULL,
	Editable bit NOT NULL,
	CONSTRAINT PK_TipoDanio PRIMARY KEY (Id)
);

CREATE TABLE ZonaPlanificacion (
	Id int NOT NULL IDENTITY(1,1),
	Zona nvarchar(100)  NOT NULL,
	KmInicio int NOT NULL,
	KmFin int NOT NULL,
	Borrado bit DEFAULT 0 NOT NULL,
	Editable bit NOT NULL,
	CONSTRAINT PK_ZonaPlanificacion PRIMARY KEY (Id)
);

CREATE TABLE GrupoImpacto(
	Id int NOT NULL IDENTITY(1,1),
	Descripcion nvarchar(100) NOT NULL,
	Borrado bit DEFAULT 0 NOT NULL,
	Editable bit NOT NULL,
	CONSTRAINT PK_GrupoImpacto PRIMARY KEY (Id)
);

CREATE TABLE GrIconoImpacto(
	Id int NOT NULL IDENTITY(1,1),
IdGrupoImpacto int NOT NULL,
	Descripcion nvarchar(100) NOT NULL,
	Imagen nvarchar(100) NOT NULL,
	Borrado bit DEFAULT 0 NOT NULL,
	Editable bit NOT NULL,
	CONSTRAINT PK_GrIconoImpacto PRIMARY KEY (Id),
	CONSTRAINT FK_GrIconoImpacto_GrupoImpacto FOREIGN KEY (IdGrupoImpacto) REFERENCES GrupoImpacto(Id)
);

CREATE TABLE SubgrupoImpacto(
	Id int NOT NULL IDENTITY(1,1),
IdGrIconoImpacto int NOT NULL,
	Descripcion nvarchar(100) NOT NULL,
	Borrado bit DEFAULT 0 NOT NULL,
	Editable bit NOT NULL,
	CONSTRAINT PK_SubrupoImpacto PRIMARY KEY (Id),
	CONSTRAINT FK_SubGrupoImpacto_GrIconoImpacto FOREIGN KEY (IdGrIconoImpacto) REFERENCES GrIconoImpacto(Id)
 
);

CREATE TABLE TipoImpacto(
	Id int NOT NULL IDENTITY(1,1),
	Descripcion nvarchar(100) NOT NULL,
	Borrado bit DEFAULT 0 NOT NULL,
	Editable bit DEFAULT 1 NOT NULL,
	CONSTRAINT PK_TipoImpacto PRIMARY KEY (Id)
);

CREATE TABLE CategoriaImpacto(
	Id int NOT NULL IDENTITY(1,1),
	IdSubgrupoImpacto int NOT NULL,
	Descripcion nvarchar(100) NOT NULL,
	Borrado bit DEFAULT 0 NOT NULL,
	Editable bit NOT NULL,
	CONSTRAINT PK_CategoriaImpacto PRIMARY KEY (Id),
	CONSTRAINT FK_CategoriaImpacto_SubgrupoImpacto FOREIGN KEY (IdSubgrupoImpacto) REFERENCES SubgrupoImpacto(Id)
);

CREATE TABLE ClaseImpacto(
	Id int NOT NULL IDENTITY(1,1),
	Descripcion nvarchar(100) NOT NULL,
	Borrado bit DEFAULT 0 NOT NULL,
	Editable bit NOT NULL,
	CONSTRAINT PK_ClaseImpacto PRIMARY KEY (Id)
);

CREATE TABLE TipoActuacion (
	Id int NOT NULL IDENTITY(1,1),
	Descripcion nvarchar(100) NOT NULL,
	Borrado bit DEFAULT 0 NOT NULL,
	Editable bit NOT NULL,
	CONSTRAINT PK_TipoActuacion PRIMARY KEY (Id)
);

CREATE TABLE ImpactoClasificado (
	Id int NOT NULL IDENTITY(1,1),
	IdTipoImpacto int NOT NULL,
	IdCategoriaImpacto int NOT NULL,
	Descripcion nvarchar(255) NOT NULL,
	IdClaseImpacto int NOT NULL,
	RelevanciaGeneral bit NOT NULL,
	Nuclear bit NOT NULL,
	IdTipoActuacion int NULL,
	ValorAD int NULL,
	Borrado bit DEFAULT 0 NOT NULL,
	Editable bit NOT NULL,
	CONSTRAINT PK_ImpactoClasif PRIMARY KEY (Id),
	CONSTRAINT FK_ImpactoClasif_TipoImpacto FOREIGN KEY (IdTipoImpacto) REFERENCES TipoImpacto(Id),
	CONSTRAINT FK_ImpactoClasif_CategImpacto FOREIGN KEY (IdCategoriaImpacto) REFERENCES CategoriaImpacto(Id),
	CONSTRAINT FK_ImpactoClasif_ClaseImpacto FOREIGN KEY (IdClaseImpacto) REFERENCES ClaseImpacto(Id),
	CONSTRAINT FK_ImpactoClasif_TipoActuac FOREIGN KEY (IdTipoActuacion) REFERENCES TipoActuacion(Id)
);

CREATE TABLE ValidacionImpactoClasificado (
	Id int NOT NULL IDENTITY(1,1),
	IdImpactoClasificado int NOT NULL,
	Campo nvarchar(100) NOT NULL,
	TipoCampo nvarchar(100) NOT NULL,
	EsObligatorio bit NOT NULL,
	Etiqueta nvarchar(100)NOT NULL,
	Orden bit NOT NULL,
	Borrado bit DEFAULT 0 NOT NULL,
	Editable bit DEFAULT 1 NOT NULL,
	CONSTRAINT PK_ValidImpClasif PRIMARY KEY (Id),
	CONSTRAINT FK_ValidImpaClasif_ImpaClasif FOREIGN KEY (IdImpactoClasificado) REFERENCES ImpactoClasificado(Id)
);


CREATE TABLE TipoImpactoEvolucion (
	Id int IDENTITY(1,1) NOT NULL,
	IdRegistro int NOT NULL,
	IdTipoImpacto int NOT NULL,
	Estimado int NULL,
	FechaCreacion datetime2 DEFAULT sysdatetime() NOT NULL,
	CreadoPor uniqueidentifier NULL,
	FechaModificacion datetime2 NULL,
	ModificadoPor uniqueidentifier NULL,
	FechaEliminacion datetime2 NULL,
	EliminadoPor uniqueidentifier NULL,
	Borrado bit DEFAULT 0 NOT NULL,
CONSTRAINT PK_TipoImpactoEv PRIMARY KEY (Id),
	CONSTRAINT FK_ImpactoEv_Registro FOREIGN KEY (IdRegistro) REFERENCES Registro(Id),
    CONSTRAINT FK_ImpactoEv_TipoImpacto FOREIGN KEY (IdTipoImpacto) REFERENCES TipoImpacto(Id)
);

CREATE TABLE ImpactoEvolucion (
	Id int IDENTITY(1,1) NOT NULL,
	IdTipoImpactoEvolucion int NOT NULL,
	IdImpactoClasificado int NOT NULL,
	IdAlteracionInterrupcion int NULL,
	Causa nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Fecha date NULL,
	FechaHora datetime NULL,
	FechaHoraInicio datetime NULL,
	FechaHoraFin datetime NULL,
	Numero int NULL,
	NumeroGraves int NULL,
	NumeroIntervinientes int NULL,
	NumeroLocalidades int NULL,
	NumeroServicios int NULL,
	NumeroUsuarios int NULL,
	Observaciones nvarchar(MAX) NULL,
	IdTipoDanio int NULL,
	IdZonaPlanificacion int NULL,
	IdProvincia int NULL,
	IdMunicipio int NULL,

	ExtraFechaHora1 datetime NULL,
	ExtraFechaHora2 datetime NULL,
	ExtraNumerico1 int NULL,
	ExtraNumerico2 int NULL,
	ExtraNumerico3 int NULL,
	ExtraCaracter1 nvarchar(255) NULL,
	FechaCreacion datetime2 DEFAULT sysdatetime() NOT NULL,
	CreadoPor uniqueidentifier NULL,
	FechaModificacion datetime2 NULL,
	ModificadoPor uniqueidentifier NULL,
	FechaEliminacion datetime2 NULL,
	EliminadoPor uniqueidentifier NULL,
	Borrado bit DEFAULT 0 NOT NULL,
	CONSTRAINT PK_ImpactoEv PRIMARY KEY (Id),
	CONSTRAINT FK_ImpactoEv_AltInt FOREIGN KEY (IdAlteracionInterrupcion) REFERENCES AlteracionInterrupcion(Id),
	CONSTRAINT FK_ImpactoEv_ImpClasif FOREIGN KEY (IdImpactoClasificado) REFERENCES ImpactoClasificado(Id),
	CONSTRAINT FK_ImpactoEv_TipoImpactoEv FOREIGN KEY (IdTipoImpactoEvolucion) REFERENCES TipoImpactoEvolucion(Id),
	CONSTRAINT FK_ImpactoEv_TipDanyo FOREIGN KEY (IdTipoDanio) REFERENCES TipoDanio(Id),
	CONSTRAINT FK_ImpactoEv_ZonaPlan FOREIGN KEY (IdZonaPlanificacion) REFERENCES ZonaPlanificacion(Id),
	CONSTRAINT FK_ImpactoEv_Provincia FOREIGN KEY (IdProvincia) REFERENCES Provincia (Id),
	CONSTRAINT FK_ImpactoEv_Municipio FOREIGN KEY (IdMunicipio) REFERENCES Municipio (Id)
);
