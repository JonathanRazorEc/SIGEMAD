-- PASO 1: Obtener el nombre de la FK que apunta a IdOpeEstadoOcupacion

DECLARE @fk_name NVARCHAR(128);
SELECT TOP 1 @fk_name = f.name
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc ON f.object_id = fc.constraint_object_id
INNER JOIN sys.columns AS c ON fc.parent_object_id = c.object_id AND fc.parent_column_id = c.column_id
WHERE OBJECT_NAME(f.parent_object_id) = 'Auditoria_OPE_AreaDescanso'
  AND c.name = 'IdOpeEstadoOcupacion';

PRINT 'Nombre de la FK: ' + ISNULL(@fk_name, '(no encontrada)');

-- PASO 2: Si se encontró la FK, genera y ejecuta el DROP CONSTRAINT
IF @fk_name IS NOT NULL
BEGIN
    DECLARE @sql_drop_fk NVARCHAR(MAX);
    SET @sql_drop_fk = N'
        ALTER TABLE Auditoria_OPE_AreaDescanso
        DROP CONSTRAINT [' + @fk_name + ']';
    PRINT 'Ejecutando: ' + @sql_drop_fk;
    EXEC sp_executesql @sql_drop_fk;
END
ELSE
BEGIN
    PRINT 'No se encontró ninguna FK para la columna IdOpeEstadoOcupacion. Quizá ya fue eliminada.';
END

-- PASO 3: Verificar si la columna existe antes de intentar borrarla
IF EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID('Auditoria_OPE_AreaDescanso')
      AND name = 'IdOpeEstadoOcupacion'
)
BEGIN
    DECLARE @sql_drop_column NVARCHAR(MAX) = N'
        ALTER TABLE Auditoria_OPE_AreaDescanso
        DROP COLUMN IdOpeEstadoOcupacion';
    PRINT 'Ejecutando: ' + @sql_drop_column;
    EXEC sp_executesql @sql_drop_column;
END
ELSE
BEGIN
    PRINT 'La columna IdOpeEstadoOcupacion ya no existe. Nada que borrar.';
END
