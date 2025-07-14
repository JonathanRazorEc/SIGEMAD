DECLARE @sql NVARCHAR(MAX) = N'';

SELECT @sql += '
IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys 
    WHERE parent_object_id = OBJECT_ID(''' + t.name + ''') 
      AND referenced_object_id = OBJECT_ID(''AspNetUsers'')
)
BEGIN' +

    CASE WHEN EXISTS (
        SELECT 1 FROM sys.columns 
        WHERE object_id = t.object_id AND name = 'CreadoPor'
    ) THEN '
    ALTER TABLE ' + QUOTENAME(t.name) + '
    ADD CONSTRAINT FK_AspNetUsers_CreadoPor_' + t.name + '
    FOREIGN KEY (CreadoPor)
    REFERENCES AspNetUsers(id);' ELSE '' END +

    CASE WHEN EXISTS (
        SELECT 1 FROM sys.columns 
        WHERE object_id = t.object_id AND name = 'ModificadoPor'
    ) THEN '
    ALTER TABLE ' + QUOTENAME(t.name) + '
    ADD CONSTRAINT FK_AspNetUsers_ModificadoPor_' + t.name + '
    FOREIGN KEY (ModificadoPor)
    REFERENCES AspNetUsers(id);' ELSE '' END +

    CASE WHEN EXISTS (
        SELECT 1 FROM sys.columns 
        WHERE object_id = t.object_id AND name = 'EliminadoPor'
    ) THEN '
    ALTER TABLE ' + QUOTENAME(t.name) + '
    ADD CONSTRAINT FK_AspNetUsers_EliminadoPor_' + t.name + '
    FOREIGN KEY (EliminadoPor)
    REFERENCES AspNetUsers(id);' ELSE '' END + '
END;
'
FROM sys.tables t
WHERE EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = t.object_id AND name IN ('CreadoPor', 'ModificadoPor', 'EliminadoPor')
);

-- Ejecutamos el SQL generado
EXEC sp_executesql @sql;
