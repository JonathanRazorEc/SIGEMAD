DROP TABLE IF EXISTS dbo.TipoMovimiento;
GO
CREATE TABLE dbo.TipoMovimiento (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Descripcion VARCHAR(50) NOT NULL
);
GO