SET QUOTED_IDENTIFIER ON;

DECLARE @sql NVARCHAR(MAX) = '';

SELECT @sql += '
-- Tabla: ' + QUOTENAME(s.name) + '.' + QUOTENAME(t.name) + '
ALTER TABLE ' + QUOTENAME(s.name) + '.' + QUOTENAME(t.name) + '
ALTER COLUMN ' + QUOTENAME(c.name) + ' nvarchar(450) NULL;
'
FROM sys.columns c
JOIN sys.tables t ON c.object_id = t.object_id
JOIN sys.schemas s ON t.schema_id = s.schema_id
WHERE c.name IN ('CreadoPor', 'ModificadoPor', 'EliminadoPor')
  AND c.system_type_id = TYPE_ID('uniqueidentifier');

--select @sql;
EXEC sp_executesql @sql; -- Descomenta esta línea para ejecutar los cambios
