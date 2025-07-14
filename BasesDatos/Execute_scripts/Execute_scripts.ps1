function Get-NumericSortKey {
    param($name)

    if ($name -match '^(\d+(?:\.\d+)*)') {
        # Dividir en partes y convertir cada una a número
        $parts = $matches[1] -split '\.' | ForEach-Object { "{0:D4}" -f [int]$_ }
        return [string]::Join('.', $parts)
    } else {
        return '9999.9999'  # archivos sin prefijo numérico van al final
    }
}

# Función para leer archivos .ini
function Get-IniContent {
    param(
        [string]$iniPath
    )

    $iniContent = @{}
    $currentSection = ""
    
    foreach ($line in Get-Content $iniPath) {
        # Ignorar comentarios y líneas vacías
        if ($line -match "^\s*;") { continue }
        if ($line -match "^\s*$") { continue }
        
        # Detectar nuevas secciones
        if ($line -match "^\[(.+)\]$") {
            $currentSection = $matches[1]
            $iniContent[$currentSection] = @{}
        } elseif ($line -match "^\s*(.+?)\s*=\s*(.+?)\s*$") {
            # Añadir claves y valores a la sección actual
            $key = $matches[1]
            $value = $matches[2]
            $iniContent[$currentSection][$key] = $value
        }
    }

    return $iniContent
}

# Función para verificar la conexión a la base de datos
function Test-DatabaseConnection {
    param(
        [string]$dbServer,
        [string]$dbName,
        [string]$useWindowsAuth,
        [string]$dbUser,
        [string]$dbPassword,
        [string]$logFile
    )

    $testQuery = "SELECT 1"
    # Construir el comando sqlcmd dependiendo del valor de USE_WINDOWS_AUTH
    if ($useWindowsAuth -eq "true") {
        $sqlcmd = "sqlcmd -S $dbServer -d $dbName -Q `"$testQuery`" -E"
    } else {
        #$sqlcmd = "sqlcmd -S $dbServer -d $dbName -U $dbUser -P $dbPassword -Q `"$testQuery`""
        $sqlcmd = "sqlcmd -S $dbServer -d $dbName -U `"$dbUser`" -P `"$dbPassword`" -C -N -Q `"$testQuery`""
    }


    # $sqlcmd = "sqlcmd -S $dbServer -d $dbName -Q `"$testQuery`" -E"
    Write-Host "Verificando conexión a la base de datos... ($sqlcmd)"
    Add-Content -Path $logFile -Value "Verificando conexión a la base de datos ($sqlcmd) ..."
    $result = Invoke-Expression $sqlcmd

    if ($LASTEXITCODE -ne 0) {
        Write-Host "Error: No se pudo conectar a la base de datos $dbName en el servidor $dbServer ($result)" -ForegroundColor Red
        Add-Content -Path $logFile -Value "Error: No se pudo conectar a la base de datos $dbName en el servidor $dbServer ($result)"
        exit 1  # Salir si no se puede conectar a la base de datos
    } else {
        Write-Host "Conexión a la base de datos verificada correctamente."
        Add-Content -Path $logFile -Value "Conexión a la base de datos verificada correctamente."
    }
}

# Función para ejecutar los scripts en una carpeta
function Execute-Scripts {
    param(
        [string]$folder,
        [string]$dbServer,
        [string]$dbName,
        [string]$useWindowsAuth,
        [string]$dbUser,
        [string]$dbPassword,
        [string]$logFile
    )

    # Obtener todos los archivos .sql en la carpeta y ordenarlos por nombre
    #$files = Get-ChildItem -Path $folder -Filter *.sql | Sort-Object Name
	
	## EJECUCIÓN POR Nº
	$files = Get-ChildItem -Path $folder -Filter *.sql | Sort-Object { Get-NumericSortKey $_.BaseName }
	## FIN EJECUCIÓN POR Nº

    # Indicar la carpeta actual en el log
    Write-Host "$1`r`nEjecutando scripts en la carpeta: $folder"
    Add-Content -Path $logFile -Value "$1`r`nEjecutando scripts en la carpeta: $folder"

    # Iterar sobre cada archivo SQL y ejecutarlo
    $allScriptsExecutedSuccessfully = $true
    foreach ($file in $files) {
        $scriptName = $file.Name
        Write-Host "Ejecutando script: $scriptName"
        Add-Content -Path $logFile -Value "$1`r`nEjecutando script: $scriptName"

        # Ejecutar el script usando sqlcmd con codificación UTF-8
        $startTime = Get-Date
        #$sqlcmd = "sqlcmd -S $dbServer -d $dbName -i `"$($file.FullName)`" -f 65001 -E -o output.log"

        # Construir el comando sqlcmd dependiendo del valor de USE_WINDOWS_AUTH
        if ($useWindowsAuth -eq "true") {
            $sqlcmd = "sqlcmd -S $dbServer -d $dbName -i `"$($file.FullName)`" -f 65001 -E -o output.log"
        } else {
            $sqlcmd = "sqlcmd -S $dbServer -d $dbName -U $dbUser -P $dbPassword -i `"$($file.FullName)`" -f 65001 -o output.log"
        }

        Write-Host "Comando: $sqlcmd"  # Para depuración
        Add-Content -Path $logFile -Value "Comando: $sqlcmd"
        $result = Invoke-Expression $sqlcmd
        $endTime = Get-Date

        # Verificar si hubo un error
        if ($LASTEXITCODE -ne 0) {
            Write-Host "Error ejecutando el script: $scriptName" -ForegroundColor Red
            Add-Content -Path $logFile -Value "Error ejecutando el script: $scriptName. Código de salida: $LASTEXITCODE"
            $allScriptsExecutedSuccessfully = $false
        } else {
            $duration = $endTime - $startTime
            Write-Host "Script ejecutado correctamente: $scriptName"
            Write-Host "Duración de ejecución: $($duration.TotalSeconds) segundos"
            Add-Content -Path $logFile -Value "Script ejecutado correctamente: $scriptName"
            Add-Content -Path $logFile -Value "Duración de ejecución: $($duration.TotalSeconds) segundos"
            
            # Leer el archivo de salida para detalles adicionales
            $outputDetails = Get-Content -Path "output.log"
            Add-Content -Path $logFile -Value "Detalles del script:"
            Add-Content -Path $logFile -Value $outputDetails

            # Analizar el archivo de salida para obtener detalles sobre los registros afectados
            $rowsAffected = ($outputDetails -match "\(\d+ rows? affected\)").Count
            Add-Content -Path $logFile -Value "Filas afectadas: $rowsAffected"
        }
    }

    return $allScriptsExecutedSuccessfully
}

# Ruta al archivo de configuración
$configFile = "config.ini"

# Leer contenido del archivo ini
$config = Get-IniContent $configFile

# Parámetros de conexión obtenidos desde el archivo de configuración
$DB_SERVER = $config['DatabaseSettings']['DB_SERVER']
$DB_NAME = $config['DatabaseSettings']['DB_NAME']
$USE_WINDOWS_AUTH = $config['DatabaseSettings']['USE_WINDOWS_AUTH']
$DB_USER = $config['DatabaseSettings']['DB_USER']
$DB_PASSWORD = $config['DatabaseSettings']['DB_PASSWORD']
$DLL_FOLDER = $config['DatabaseSettings']['DLL_FOLDER']  # Ruta a los scripts DLL
$DATOS_FOLDER = $config['DatabaseSettings']['DATOS_FOLDER']  # Ruta a los scripts de Datos

# Archivo de log
$logFile = "execution_log_$(Get-Date -Format yyyyMMddHHmmss).log"

# Verificar la conexión a la base de datos
#Test-DatabaseConnection -dbServer $DB_SERVER -dbName $DB_NAME -logFile $logFile
Test-DatabaseConnection -dbServer $DB_SERVER -dbName $DB_NAME -useWindowsAuth $USE_WINDOWS_AUTH -dbUser $DB_USER -dbPassword $DB_PASSWORD -logFile $logFile

# Ejecutar scripts en la carpeta DLL
Write-Host "==========================================="
Write-Host "Ejecutando scripts en la carpeta DLL..."
Write-Host "==========================================="

Add-Content -Path $logFile -Value "$1`r`n==========================================="
Add-Content -Path $logFile -Value "Ejecutando scripts en la carpeta DLL..."
Add-Content -Path $logFile -Value "$1`r`n==========================================="
#$dllExecutionResult = Execute-Scripts -folder $DLL_FOLDER -dbServer $DB_SERVER -dbName $DB_NAME -logFile $logFile
$dllExecutionResult = Execute-Scripts -folder $DLL_FOLDER -dbServer $DB_SERVER -dbName $DB_NAME -useWindowsAuth $USE_WINDOWS_AUTH -dbUser $DB_USER -dbPassword $DB_PASSWORD -logFile $logFile

# Verificar si los scripts de DLL se ejecutaron correctamente
if ($dllExecutionResult) {
    Write-Host "Scripts de DLL ejecutados correctamente."
    Write-Host "$1`r`n==========================================="
    Write-Host "Ejecutando scripts en la carpeta DATOS..."
    Write-Host "==========================================="
    
    Add-Content -Path $logFile -Value "Scripts de DLL ejecutados correctamente."
    Add-Content -Path $logFile -Value "$1`r`n==========================================="
    Add-Content -Path $logFile -Value "Ejecutando scripts en la carpeta DATOS..."
    Add-Content -Path $logFile -Value "$1`r`n==========================================="
    
    # Ejecutar scripts en la carpeta Datos
    #$datosExecutionResult = Execute-Scripts -folder $DATOS_FOLDER -dbServer $DB_SERVER -dbName $DB_NAME -logFile $logFile
    $datosExecutionResult = Execute-Scripts -folder $DATOS_FOLDER -dbServer $DB_SERVER -dbName $DB_NAME -useWindowsAuth $USE_WINDOWS_AUTH -dbUser $DB_USER -dbPassword $DB_PASSWORD -logFile $logFile

    if ($datosExecutionResult) {
        Write-Host "Todos los scripts de Datos se ejecutaron correctamente."
        Add-Content -Path $logFile -Value "Todos los scripts de Datos se ejecutaron correctamente."
    } else {
        Write-Host "Error al ejecutar los scripts en la carpeta Datos." -ForegroundColor Red
        Add-Content -Path $logFile -Value "Error al ejecutar los scripts en la carpeta Datos."
        Add-Content -Path $logFile -Value "Proceso finalizado con errores."
        exit 1  # Salir si hay un error en los scripts de Datos
    }
} else {
    Write-Host "Error al ejecutar los scripts en la carpeta DLL. No se ejecutarán los scripts de Datos." -ForegroundColor Red
    Add-Content -Path $logFile -Value "Error al ejecutar los scripts en la carpeta DLL. No se ejecutarán los scripts de Datos."
    Add-Content -Path $logFile -Value "Proceso finalizado con errores."
    exit 1  # Salir si hay un error en los scripts de DLL
}



Write-Host "Log de ejecución generado: $logFile"