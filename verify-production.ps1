# ERVILHINHA - Production Verification Script

Write-Host ""
Write-Host "========================================"
Write-Host " ERVILHINHA - Verificacao de Producao"
Write-Host "========================================"
Write-Host ""

# 1. Procurar Mock Data
Write-Host "[1/4] A procurar mock data..." -ForegroundColor Cyan
$mockResults = Get-ChildItem -Path . -Recurse -Include *.cs | Select-String -Pattern "GetMockResult" 2>$null | Where-Object { $_.Path -notlike "*\obj\*" -and $_.Path -notlike "*\bin\*" }

if ($mockResults) {
    Write-Host "   AVISO: Mock data encontrado" -ForegroundColor Yellow
    $issuesFound = 1
} else {
    Write-Host "   OK: Sem mock data" -ForegroundColor Green
    $issuesFound = 0
}

Write-Host ""

# 2. Build
Write-Host "[2/4] A fazer build..." -ForegroundColor Cyan
$null = dotnet build --configuration Release --verbosity quiet 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "   OK: Build sucesso" -ForegroundColor Green
} else {
    Write-Host "   ERRO: Build falhou" -ForegroundColor Red
    $issuesFound++
}

Write-Host ""

# 3. Ficheiros
Write-Host "[3/4] A verificar ficheiros..." -ForegroundColor Cyan
$files = @("wwwroot\css\forms-modern.css", "wwwroot\js\epic-effects.js", "Services\InvoiceOcrService.cs")
$allOk = $true
foreach ($f in $files) {
    if (-not (Test-Path $f)) {
        Write-Host "   FALTA: $f" -ForegroundColor Red
        $allOk = $false
        $issuesFound++
    }
}
if ($allOk) { Write-Host "   OK: Ficheiros presentes" -ForegroundColor Green }

Write-Host ""

# 4. User Secrets
Write-Host "[4/4] A verificar secrets..." -ForegroundColor Cyan
$secretsPath = Join-Path $env:APPDATA "Microsoft\UserSecrets\aspnet-Ervilhinha-9909016f-d58c-476e-8a22-c1ed48f5e12b\secrets.json"
if (Test-Path $secretsPath) {
    Write-Host "   OK: User Secrets encontrado" -ForegroundColor Green
    $secrets = Get-Content $secretsPath -Raw | ConvertFrom-Json
    if ($secrets.AzureFormRecognizer.Endpoint) {
        Write-Host "   INFO: Azure OCR ativo" -ForegroundColor Cyan
    } else {
        Write-Host "   INFO: Azure OCR desativo (modo manual)" -ForegroundColor Cyan
    }
} else {
    Write-Host "   AVISO: User Secrets nao encontrado" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "========================================"

# Resumo
if ($issuesFound -eq 0) {
    Write-Host " PRONTA PARA PRODUCAO!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Proximos passos:" -ForegroundColor Cyan
    Write-Host "1. Deploy para servidor"
    Write-Host "2. Aplicar migrations"
    Write-Host "3. Criar admin user"
} else {
    Write-Host " $issuesFound problemas encontrados" -ForegroundColor Yellow
}

Write-Host ""
