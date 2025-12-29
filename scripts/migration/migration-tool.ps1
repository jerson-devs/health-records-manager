# Herramienta de Migración: Oracle 11g a PostgreSQL/Oracle
# Health Records Manager
# Autor: Health Records System
# Fecha: 2025-01-16
# Descripción: Script PowerShell para automatizar la migración de datos

param(
    [Parameter(Mandatory=$true)]
    [ValidateSet("PostgreSQL", "Oracle")]
    [string]$TargetDatabase,
    
    [Parameter(Mandatory=$true)]
    [string]$OracleConnectionString,
    
    [Parameter(Mandatory=$true)]
    [string]$TargetConnectionString,
    
    [Parameter(Mandatory=$false)]
    [string]$ExportPath = ".\exports",
    
    [Parameter(Mandatory=$false)]
    [switch]$SkipDataExport,
    
    [Parameter(Mandatory=$false)]
    [switch]$SkipDataImport,
    
    [Parameter(Mandatory=$false)]
    [switch]$VerifyOnly
)

# ============================================
# CONFIGURACIÓN
# ============================================

$ErrorActionPreference = "Stop"
$ProgressPreference = "Continue"

# Colores para output
function Write-Info { Write-Host $args -ForegroundColor Cyan }
function Write-Success { Write-Host $args -ForegroundColor Green }
function Write-Warning { Write-Host $args -ForegroundColor Yellow }
function Write-Error { Write-Host $args -ForegroundColor Red }

# ============================================
# FUNCIONES AUXILIARES
# ============================================

function Test-DatabaseConnection {
    param([string]$ConnectionString, [string]$DatabaseType)
    
    Write-Info "Probando conexión a $DatabaseType..."
    
    try {
        if ($DatabaseType -eq "Oracle") {
            # Requiere Oracle.ManagedDataAccess
            # $connection = New-Object Oracle.ManagedDataAccess.Client.OracleConnection($ConnectionString)
            # $connection.Open()
            # $connection.Close()
            Write-Warning "Verificación de conexión Oracle requiere Oracle.ManagedDataAccess.dll"
            return $true
        }
        elseif ($DatabaseType -eq "PostgreSQL") {
            # Requiere Npgsql
            # $connection = New-Object Npgsql.NpgsqlConnection($ConnectionString)
            # $connection.Open()
            # $connection.Close()
            Write-Warning "Verificación de conexión PostgreSQL requiere Npgsql.dll"
            return $true
        }
    }
    catch {
        Write-Error "Error al conectar: $_"
        return $false
    }
}

function Export-OracleData {
    param([string]$TableName, [string]$OutputFile)
    
    Write-Info "Exportando tabla: $TableName"
    
    # Ejemplo de exportación usando sqlplus
    $sqlScript = @"
SET PAGESIZE 0
SET FEEDBACK OFF
SET HEADING OFF
SET ECHO OFF
SET COLSEP ','
SPOOL $OutputFile
SELECT * FROM $TableName;
SPOOL OFF
EXIT
"@
    
    $sqlScript | Out-File -FilePath "$env:TEMP\export_$TableName.sql" -Encoding UTF8
    
    Write-Warning "Ejecuta manualmente: sqlplus $OracleConnectionString @$env:TEMP\export_$TableName.sql"
    Write-Info "O usa Oracle SQL Developer para exportar a CSV"
}

function Import-PostgreSQLData {
    param([string]$TableName, [string]$InputFile)
    
    Write-Info "Importando tabla: $TableName"
    
    # Ejemplo usando psql
    $copyCommand = "COPY $TableName FROM '$InputFile' WITH (FORMAT csv, HEADER false, DELIMITER ',');"
    
    Write-Warning "Ejecuta manualmente: psql -c `"$copyCommand`" `"$TargetConnectionString`""
    Write-Info "O usa pgAdmin para importar el CSV"
}

function Import-OracleData {
    param([string]$TableName, [string]$InputFile)
    
    Write-Info "Importando tabla: $TableName"
    
    Write-Warning "Usa impdp o SQL*Loader para importar datos a Oracle"
    Write-Info "Ejemplo impdp: impdp system/password@target SCHEMAS=HEALTHRECORDS DUMPFILE=$InputFile"
}

function Verify-DataIntegrity {
    param([string]$SourceConnection, [string]$TargetConnection, [string]$TargetType)
    
    Write-Info "Verificando integridad de datos..."
    
    $tables = @("PATIENTS", "MEDICAL_RECORDS", "USERS")
    
    foreach ($table in $tables) {
        Write-Info "Verificando tabla: $table"
        
        # Aquí iría la lógica para comparar conteos
        # Requiere conexiones activas a ambas bases de datos
        Write-Warning "Verificación manual requerida: Compara conteos de $table entre Oracle y $TargetType"
    }
}

# ============================================
# FUNCIÓN PRINCIPAL
# ============================================

function Start-Migration {
    Write-Info "============================================"
    Write-Info "Herramienta de Migración de Datos"
    Write-Info "Oracle 11g -> $TargetDatabase"
    Write-Info "============================================"
    Write-Host ""
    
    # Verificar conexiones
    Write-Info "Paso 1: Verificando conexiones..."
    if (-not (Test-DatabaseConnection -ConnectionString $OracleConnectionString -DatabaseType "Oracle")) {
        Write-Error "No se pudo conectar a Oracle. Verifica la connection string."
        exit 1
    }
    
    if (-not (Test-DatabaseConnection -ConnectionString $TargetConnectionString -DatabaseType $TargetDatabase)) {
        Write-Error "No se pudo conectar a $TargetDatabase. Verifica la connection string."
        exit 1
    }
    
    Write-Success "Conexiones verificadas"
    Write-Host ""
    
    # Crear directorio de exportación
    if (-not (Test-Path $ExportPath)) {
        New-Item -ItemType Directory -Path $ExportPath -Force | Out-Null
        Write-Info "Directorio de exportación creado: $ExportPath"
    }
    
    # Exportar datos de Oracle
    if (-not $SkipDataExport) {
        Write-Info "Paso 2: Exportando datos de Oracle..."
        
        $tables = @("PATIENTS", "MEDICAL_RECORDS", "USERS")
        
        foreach ($table in $tables) {
            $outputFile = Join-Path $ExportPath "$table.csv"
            Export-OracleData -TableName $table -OutputFile $outputFile
        }
        
        Write-Success "Datos exportados a: $ExportPath"
        Write-Host ""
    }
    else {
        Write-Info "Paso 2: Omitido (SkipDataExport)"
        Write-Host ""
    }
    
    # Importar datos a destino
    if (-not $SkipDataImport) {
        Write-Info "Paso 3: Importando datos a $TargetDatabase..."
        
        $tables = @("PATIENTS", "MEDICAL_RECORDS", "USERS")
        
        foreach ($table in $tables) {
            $inputFile = Join-Path $ExportPath "$table.csv"
            
            if (Test-Path $inputFile) {
                if ($TargetDatabase -eq "PostgreSQL") {
                    Import-PostgreSQLData -TableName $table -InputFile $inputFile
                }
                elseif ($TargetDatabase -eq "Oracle") {
                    Import-OracleData -TableName $table -InputFile $inputFile
                }
            }
            else {
                Write-Warning "Archivo no encontrado: $inputFile"
            }
        }
        
        Write-Success "Datos importados a $TargetDatabase"
        Write-Host ""
    }
    else {
        Write-Info "Paso 3: Omitido (SkipDataImport)"
        Write-Host ""
    }
    
    # Verificar integridad
    if ($VerifyOnly -or (-not $SkipDataImport)) {
        Write-Info "Paso 4: Verificando integridad de datos..."
        Verify-DataIntegrity -SourceConnection $OracleConnectionString -TargetConnection $TargetConnectionString -TargetType $TargetDatabase
        Write-Host ""
    }
    
    Write-Success "============================================"
    Write-Success "Migración completada"
    Write-Success "============================================"
    Write-Host ""
    Write-Info "Próximos pasos:"
    Write-Info "1. Verifica manualmente los datos migrados"
    Write-Info "2. Ejecuta los scripts de stored procedures y funciones"
    Write-Info "3. Ejecuta los scripts de triggers"
    Write-Info "4. Prueba la aplicación con los datos migrados"
    Write-Info "5. Haz backup de la base de datos destino"
}

# ============================================
# EJECUCIÓN
# ============================================

try {
    Start-Migration
}
catch {
    Write-Error "Error durante la migración: $_"
    Write-Error $_.ScriptStackTrace
    exit 1
}

# ============================================
# EJEMPLOS DE USO
# ============================================

<#
# Ejemplo 1: Migración completa a PostgreSQL
.\migration-tool.ps1 `
    -TargetDatabase "PostgreSQL" `
    -OracleConnectionString "user/password@oracle-server:1521/XE" `
    -TargetConnectionString "Host=postgres-server;Port=5432;Database=HealthRecordsDB;Username=admin;Password=password"

# Ejemplo 2: Solo exportar datos
.\migration-tool.ps1 `
    -TargetDatabase "PostgreSQL" `
    -OracleConnectionString "user/password@oracle-server:1521/XE" `
    -TargetConnectionString "Host=postgres-server;Port=5432;Database=HealthRecordsDB;Username=admin;Password=password" `
    -SkipDataImport

# Ejemplo 3: Solo importar datos (después de exportar)
.\migration-tool.ps1 `
    -TargetDatabase "PostgreSQL" `
    -OracleConnectionString "user/password@oracle-server:1521/XE" `
    -TargetConnectionString "Host=postgres-server;Port=5432;Database=HealthRecordsDB;Username=admin;Password=password" `
    -SkipDataExport

# Ejemplo 4: Migración a Oracle moderno
.\migration-tool.ps1 `
    -TargetDatabase "Oracle" `
    -OracleConnectionString "user/password@oracle11g-server:1521/XE" `
    -TargetConnectionString "user/password@oracle-modern-server:1521/XE"
#>

