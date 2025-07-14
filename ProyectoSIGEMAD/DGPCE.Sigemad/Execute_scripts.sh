#!/bin/bash
set -e

# ------------------------------------------------------------
# Script de inicialización de la base de datos según la rama
# ------------------------------------------------------------

# Contador de ejecuciones
EXECUTION_COUNT_FILE="/app/execution_count.txt"
if [ ! -f "$EXECUTION_COUNT_FILE" ]; then
    echo "1" > "$EXECUTION_COUNT_FILE"
else
    current_count=$(cat "$EXECUTION_COUNT_FILE")
    echo $((current_count + 1)) > "$EXECUTION_COUNT_FILE"
fi
echo "Número de ejecuciones del script: $(cat "$EXECUTION_COUNT_FILE")"

# ------------------------------------------------------------
# Determinar la rama sobre la que estamos trabajando.
# Tekton debería inyectar esta variable en el Task via params.
# ------------------------------------------------------------
BRANCH="${GIT_BRANCH:-}"
if [ -z "$BRANCH" ]; then
  echo "❌ Error: no se ha recibido la variable GIT_BRANCH."
  exit 1
fi

# Extraer el nombre corto (p. ej. 'develop' de 'refs/heads/develop')
SHORT_BRANCH=$(basename "$BRANCH")

# ------------------------------------------------------------
# Configuración del servidor de base de datos según la rama
# ------------------------------------------------------------
case "$SHORT_BRANCH" in
  develop)
    DB_SERVER="sqlserver.ns-sigemad.svc.cluster.local,1433"
    ;;
  release)
    DB_SERVER="sqlserver.ns-sigemad-release.svc.cluster.local,1433"
    ;;
  *)
    echo "⚠️  La rama '$SHORT_BRANCH' no está configurada para actualización de BD. Saliendo sin cambios."
    exit 0
    ;;
esac

DB_USER="sa"
DB_PASSWORD='P@s$w0rd'
DLL_FOLDER="/app/DLL"
DATOS_FOLDER="/app/Datos"
DB_NAME="Sigemad"

echo "→ Rama detectada: $SHORT_BRANCH"
echo "→ Conectando a SQL Server: $DB_SERVER"

# ------------------------------------------------------------
# Lógica de recreación de base de datos
# ------------------------------------------------------------
echo "Eliminando base de datos $DB_NAME si existe..."
/opt/mssql-tools18/bin/sqlcmd \
  -S "$DB_SERVER" \
  -U "$DB_USER" \
  -P "$DB_PASSWORD" \
  -Q "IF EXISTS (SELECT name FROM sys.databases WHERE name = '$DB_NAME')
      BEGIN
        ALTER DATABASE [$DB_NAME] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
        DROP DATABASE [$DB_NAME];
      END" -C

echo "Creando base de datos $DB_NAME..."
/opt/mssql-tools18/bin/sqlcmd \
  -S "$DB_SERVER" \
  -U "$DB_USER" \
  -P "$DB_PASSWORD" \
  -Q "CREATE DATABASE [$DB_NAME]" -C

# ------------------------------------------------------------
# Función para ejecutar scripts .sql ordenados por prefijo numérico
# ------------------------------------------------------------
execute_scripts_in_folder() {
  local folder="$1"
  echo "→ Ejecutando scripts en carpeta: $folder"

  find "$folder" -type f -name '*.sql' \
    | awk -F/ '
      {
        file=$NF;
        if (match(file, /^[0-9]+/)) {
          num=substr(file, RSTART, RLENGTH)+0;
          print num "\t" $0;
        }
      }' \
    | sort -n \
    | cut -f2- \
    | while read -r script; do
        echo "→ Ejecutando: $script"
        /opt/mssql-tools18/bin/sqlcmd \
          -S "$DB_SERVER" \
          -U "$DB_USER" \
          -P "$DB_PASSWORD" \
          -d "$DB_NAME" \
          -i "$script" -C
    done
}

# Ejecutar scripts en orden
execute_scripts_in_folder "$DLL_FOLDER"
execute_scripts_in_folder "$DATOS_FOLDER"

echo "✅ Proceso de inicialización de base de datos completado para la rama '$SHORT_BRANCH'."
