# Script para obtener un token JWT desde la API
# Uso: .\obtener-token.ps1 -Username "admin" -Password "Admin123!" -Url "http://localhost:5252"

param(
    [string]$Username = "admin",
    [string]$Password = "admin123",
    [string]$Url = "http://localhost:5252"
)

Write-Host "=== Generador de Token JWT ===" -ForegroundColor Cyan
Write-Host ""

# URL del endpoint de login
$loginUrl = "$Url/api/auth/login"

# Datos del login
$body = @{
    username = $Username
    password = $Password
} | ConvertTo-Json

Write-Host "Conectando a: $loginUrl" -ForegroundColor Yellow
Write-Host "Usuario: $Username" -ForegroundColor Yellow
Write-Host ""

try {
    # Hacer la petición POST
    $response = Invoke-RestMethod -Uri $loginUrl -Method Post -Body $body -ContentType "application/json" -ErrorAction Stop
    
    if ($response.success -and $response.data.token) {
        Write-Host "✓ Login exitoso!" -ForegroundColor Green
        Write-Host ""
        Write-Host "TOKEN JWT:" -ForegroundColor Cyan
        Write-Host $response.data.token -ForegroundColor White
        Write-Host ""
        Write-Host "Información del token:" -ForegroundColor Cyan
        Write-Host "  - Expira en: $($response.data.expiresAt)" -ForegroundColor White
        Write-Host "  - Usuario: $($response.data.user.username)" -ForegroundColor White
        Write-Host "  - Email: $($response.data.user.email)" -ForegroundColor White
        Write-Host "  - Rol: $($response.data.user.role)" -ForegroundColor White
        Write-Host ""
        Write-Host "Para usar en Swagger, ingresa:" -ForegroundColor Yellow
        Write-Host "Bearer $($response.data.token)" -ForegroundColor White
        Write-Host ""
        
        # Guardar el token en un archivo temporal
        $tokenFile = "token.txt"
        "Bearer $($response.data.token)" | Out-File -FilePath $tokenFile -Encoding UTF8
        Write-Host "Token guardado en: $tokenFile" -ForegroundColor Green
    } else {
        Write-Host "✗ Error: No se pudo obtener el token" -ForegroundColor Red
        Write-Host $response | ConvertTo-Json -Depth 5
    }
} catch {
    Write-Host "✗ Error al conectar con la API:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    Write-Host ""
    Write-Host "Asegúrate de que:" -ForegroundColor Yellow
    Write-Host "  1. La aplicación esté corriendo en $Url" -ForegroundColor White
    Write-Host "  2. El usuario '$Username' exista en la base de datos" -ForegroundColor White
    Write-Host "  3. La contraseña sea correcta" -ForegroundColor White
    exit 1
}
