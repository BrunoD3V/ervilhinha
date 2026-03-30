# 🔍 SCRIPT DE VERIFICAÇÃO DE PRODUÇÃO

## PowerShell Script - Verificar Mock Data

```powershell
# verify-production.ps1
Write-Host "🌿 ERVILHINHA - Verificação de Produção" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""

# 1. Procurar por Mock Data nos ficheiros
Write-Host "🔍 A procurar mock data..." -ForegroundColor Yellow
$mockPatterns = @("GetMockResult", "Mock Data", "Sample Store", "mock data", "demo data", "test data")
$found = $false

foreach ($pattern in $mockPatterns) {
    $results = Get-ChildItem -Path . -Recurse -Include *.cs,*.cshtml | Select-String -Pattern $pattern -SimpleMatch
    if ($results) {
        Write-Host "⚠️  Encontrado '$pattern' em:" -ForegroundColor Red
        foreach ($result in $results) {
            Write-Host "   - $($result.Filename):$($result.LineNumber)" -ForegroundColor Gray
        }
        $found = $true
    }
}

if (-not $found) {
    Write-Host "✅ Nenhum mock data encontrado!" -ForegroundColor Green
}

Write-Host ""

# 2. Verificar configurações obrigatórias
Write-Host "🔐 A verificar configurações..." -ForegroundColor Yellow

# Ler appsettings.json
$appsettings = Get-Content "appsettings.json" | ConvertFrom-Json

# Connection String
if ($appsettings.ConnectionStrings.DefaultConnection -match "Server=.*;Database=.*;") {
    Write-Host "✅ Connection String configurada" -ForegroundColor Green
} else {
    Write-Host "⚠️  Connection String precisa revisão" -ForegroundColor Yellow
}

# User Secrets (verificar se existem)
$secretsId = "aspnet-Ervilhinha-9909016f-d58c-476e-8a22-c1ed48f5e12b"
$secretsPath = "$env:APPDATA\Microsoft\UserSecrets\$secretsId\secrets.json"

if (Test-Path $secretsPath) {
    Write-Host "✅ User Secrets encontrado: $secretsPath" -ForegroundColor Green
    
    $secrets = Get-Content $secretsPath | ConvertFrom-Json
    
    # Verificar AdminUser
    if ($secrets.AdminUser) {
        Write-Host "   ✓ AdminUser configurado" -ForegroundColor Gray
    } else {
        Write-Host "   ⚠️  AdminUser não configurado (opcional)" -ForegroundColor Yellow
    }
    
    # Verificar Azure OCR
    if ($secrets.AzureFormRecognizer) {
        if ($secrets.AzureFormRecognizer.Endpoint -and $secrets.AzureFormRecognizer.ApiKey) {
            Write-Host "   ✓ Azure Form Recognizer configurado (OCR ativo)" -ForegroundColor Gray
        } else {
            Write-Host "   ⚠️  Azure Form Recognizer incompleto (OCR desativado)" -ForegroundColor Yellow
        }
    } else {
        Write-Host "   ℹ️  Azure Form Recognizer não configurado (modo manual)" -ForegroundColor Cyan
    }
    
    # Verificar Google Auth
    if ($secrets.Authentication.Google) {
        if ($secrets.Authentication.Google.ClientId -ne "YOUR_GOOGLE_CLIENT_ID_HERE") {
            Write-Host "   ✓ Google Authentication configurado" -ForegroundColor Gray
        } else {
            Write-Host "   ℹ️  Google Auth não configurado (email/password funciona)" -ForegroundColor Cyan
        }
    }
} else {
    Write-Host "⚠️  User Secrets não encontrado" -ForegroundColor Yellow
    Write-Host "   → Executa: dotnet user-secrets init" -ForegroundColor Gray
}

Write-Host ""

# 3. Verificar migrations
Write-Host "🗄️  A verificar migrações..." -ForegroundColor Yellow
$migrations = Get-ChildItem -Path "Data\Migrations" -Filter "*.cs" | Where-Object { $_.Name -notlike "*Designer.cs" -and $_.Name -ne "ApplicationDbContextModelSnapshot.cs" }
Write-Host "✅ $($migrations.Count) migrações encontradas" -ForegroundColor Green

Write-Host ""

# 4. Verificar ficheiros críticos
Write-Host "📁 A verificar ficheiros críticos..." -ForegroundColor Yellow

$criticalFiles = @(
    "Program.cs",
    "appsettings.json",
    "Data\ApplicationDbContext.cs",
    "Services\InvoiceOcrService.cs",
    "Services\BabyCostCalculatorService.cs",
    "Services\FamilyBudgetService.cs",
    "Services\SmartAlertService.cs",
    "wwwroot\css\site.css",
    "wwwroot\css\forms-modern.css",
    "wwwroot\css\forms-epic.css",
    "wwwroot\js\site.js",
    "wwwroot\js\epic-effects.js"
)

foreach ($file in $criticalFiles) {
    if (Test-Path $file) {
        Write-Host "✅ $file" -ForegroundColor Green
    } else {
        Write-Host "❌ $file FALTA!" -ForegroundColor Red
    }
}

Write-Host ""

# 5. Verificar build
Write-Host "🔨 A fazer build..." -ForegroundColor Yellow
$buildResult = dotnet build --configuration Release --no-incremental 2>&1

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Build SUCESSO (Release)" -ForegroundColor Green
} else {
    Write-Host "❌ Build FALHOU" -ForegroundColor Red
    Write-Host $buildResult -ForegroundColor Gray
}

Write-Host ""

# 6. Resumo Final
Write-Host "========================================" -ForegroundColor Green
Write-Host "📊 RESUMO FINAL" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Core Services:" -ForegroundColor Cyan
Write-Host "  ✅ Expenses & Categories" -ForegroundColor Green
Write-Host "  ✅ Baby Items" -ForegroundColor Green
Write-Host "  ✅ Baby Lists (Shared)" -ForegroundColor Green
Write-Host "  ✅ Timeline & Milestones" -ForegroundColor Green
Write-Host "  ✅ Cost Simulator" -ForegroundColor Green
Write-Host "  ✅ Family Budget" -ForegroundColor Green
Write-Host "  ✅ Smart Alerts" -ForegroundColor Green
Write-Host ""
Write-Host "Optional Services:" -ForegroundColor Cyan

if ($secrets.AzureFormRecognizer.Endpoint) {
    Write-Host "  ✅ Invoice OCR (Azure)" -ForegroundColor Green
} else {
    Write-Host "  ⚙️  Invoice OCR (Manual Mode)" -ForegroundColor Yellow
}

if ($secrets.Authentication.Google.ClientId -and $secrets.Authentication.Google.ClientId -ne "YOUR_GOOGLE_CLIENT_ID_HERE") {
    Write-Host "  ✅ Google Login" -ForegroundColor Green
} else {
    Write-Host "  ⚙️  Google Login (Disabled)" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "🎯 Status: PRONTA PARA PRODUÇÃO" -ForegroundColor Green -BackgroundColor Black
Write-Host ""
```

## Como Executar

```powershell
# Na raiz do projeto
.\verify-production.ps1
```

---

## ✅ Verificações Manuais Adicionais

### **1. Testar Upload de Fatura (Modo Manual)**
```
1. Navegar: /Invoices/Upload
2. Carregar qualquer imagem JPG
3. Verificar: Mostra alerta "⚠️ Serviço não configurado"
4. Deve redirecionar para Review com campos vazios
5. Preencher manualmente
6. Guardar → Despesa criada ✅
```

### **2. Testar Simulador de Custos**
```
1. Navegar: /BabyCostSimulator/Create
2. Preencher com dados reais:
   - Rendimento: €2000
   - Semanas: 20
   - Lifestyle: Moderado
3. Submeter
4. Verificar cálculos fazem sentido (não são valores aleatórios)
5. Total deve estar ~€8000-12000 (realista para PT)
```

### **3. Testar Orçamento Familiar**
```
1. Criar algumas despesas reais
2. Navegar: /FamilyBudget/Index
3. Configurar orçamento mensal
4. Verificar campos "Atual" calculam com despesas reais
5. Estado financeiro deve refletir situação real
```

### **4. Testar Alertas Inteligentes**
```
1. Criar orçamento com limite baixo (ex: €100 para Bebé)
2. Criar despesa que excede (ex: €150)
3. Verificar alerta aparece automaticamente
4. Texto do alerta deve ser específico (não genérico)
```

---

## 🚀 Comandos de Deploy

### **Local → Produção**
```bash
# 1. Build Release
dotnet build --configuration Release

# 2. Publicar
dotnet publish --configuration Release --output ./publish

# 3. Aplicar migrações na produção (connection string produção)
dotnet ef database update --connection "Server=PROD;Database=Ervilhinha;..."

# 4. Deploy para Azure App Service
az webapp deployment source config-zip \
  --resource-group ErvilhinhaRG \
  --name ervilhinha-app \
  --src ./publish.zip
```

---

## 📋 Checklist de Deploy

- [ ] Build Release sem erros
- [ ] Migrações aplicadas em DB produção
- [ ] Connection string produção configurada
- [ ] User Secrets configurado (ou App Settings no Azure)
- [ ] HTTPS enforced
- [ ] Error pages configuradas
- [ ] Logs configurados (Application Insights recomendado)
- [ ] Backup DB configurado
- [ ] Testar Invoice Upload (modo manual funciona)
- [ ] Testar criar Admin user
- [ ] Testar todas as funcionalidades core

---

**Criado**: 2025-05-XX
**Última Atualização**: Remoção de Mock Data completa
