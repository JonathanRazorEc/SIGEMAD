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


--Añadimos las relaciones a la tabla registro

-- FK para creadoPor
IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys 
    WHERE parent_object_id = OBJECT_ID('registro') And referenced_object_id = OBJECT_ID('AspNetUsers')
)
BEGIN
    ALTER TABLE registro
    ADD CONSTRAINT FK_AspNetUsers_CreadoPor
    FOREIGN KEY (creadoPor)
    REFERENCES AspNetUsers(id);

	ALTER TABLE registro
    ADD CONSTRAINT FK_AspNetUsers_Modificado
    FOREIGN KEY (ModificadoPor)
    REFERENCES AspNetUsers(id);

	ALTER TABLE registro
    ADD CONSTRAINT FK_AspNetUsers_Eliminado
    FOREIGN KEY (EliminadoPor)
    REFERENCES AspNetUsers(id);
END;
