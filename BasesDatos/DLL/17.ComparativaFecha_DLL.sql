DROP TABLE IF EXISTS dbo.ComparativaFecha;
GO
CREATE TABLE dbo.ComparativaFecha (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Descripcion VARCHAR(50) NOT NULL
);
GO