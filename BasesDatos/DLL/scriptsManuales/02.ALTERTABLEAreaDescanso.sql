DECLARE @constraintName NVARCHAR(128);
DECLARE @sql NVARCHAR(MAX);

-- Obtener el nombre de la constraint
SELECT @constraintName = fk.name
FROM sys.foreign_keys fk
INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
INNER JOIN sys.columns c ON fkc.parent_object_id = c.object_id AND fkc.parent_column_id = c.column_id
WHERE fk.parent_object_id = OBJECT_ID('OPE_AreaDescanso')
  AND c.name = 'IdOpeEstadoOcupacion';

-- Si existe, armar el comando dinámico
IF @constraintName IS NOT NULL
BEGIN
    SET @sql = N'ALTER TABLE OPE_AreaDescanso DROP CONSTRAINT ' + QUOTENAME(@constraintName) + ';';
    EXEC sp_executesql @sql;

    -- Ahora eliminar la columna (ya sin FK)
    ALTER TABLE OPE_AreaDescanso DROP COLUMN IdOpeEstadoOcupacion;
END
