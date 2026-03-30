# 🚀 GUIA RÁPIDO DE DEPLOY - ERVILHINHA

## ✅ **STATUS**: PRONTA PARA PRODUÇÃO

---

## 🎯 **OPÇÃO 1: Deploy Básico (5 minutos)**

### **O Que Funciona**:
✅ Gestão de Despesas  
✅ Categorias  
✅ Baby Items (Enxoval)  
✅ Baby Lists (Partilha)  
✅ Timeline de Eventos  
✅ Simulador de Custos (dados reais PT)  
✅ Orçamento Familiar  
✅ Alertas Inteligentes  
✅ Autenticação (Email/Password)  

⚠️ **Limitação**: Invoice OCR em modo manual

---

### **Passos Deploy**:

#### **1. Preparar Publicação**
```powershell
# Build Release
dotnet build --configuration Release

# Publicar
dotnet publish --configuration Release --output C:\publish\ervilhinha
```

#### **2. Configurar SQL Server**
```sql
-- Criar base de dados
CREATE DATABASE Ervilhinha;
GO
```

#### **3. Aplicar Migrations**
```powershell
# Atualizar connection string em appsettings.Production.json
# Depois:
dotnet ef database update
```

#### **4. Deploy IIS** (Windows Server)
```powershell
# 1. Copiar pasta C:\publish\ervilhinha para C:\inetpub\wwwroot\ervilhinha
# 2. IIS Manager → Criar novo Site:
#    - Nome: Ervilhinha
#    - Path: C:\inetpub\wwwroot\ervilhinha
#    - Binding: https://ervilhinha.seudominio.com
# 3. Application Pool: .NET 8.0
```

#### **5. Deploy Azure** (App Service)
```bash
# Criar App Service
az webapp create \
  --name ervilhinha-app \
  --resource-group ErvilhinhaRG \
  --plan ErvilhinhaServicePlan \
  --runtime "DOTNET|8.0"

# Deploy
az webapp deployment source config-zip \
  --resource-group ErvilhinhaRG \
  --name ervilhinha-app \
  --src ervilhinha.zip

# Configurar Connection String (Portal Azure)
# Settings → Configuration → Connection strings
# Name: DefaultConnection
# Value: Server=...;Database=Ervilhinha;...
# Type: SQLServer
```

#### **6. Criar Admin User**
Opção A: Via secrets.json (antes de deploy)
```json
{
  "AdminUser": {
    "Email": "admin@ervilhinha.com",
    "Password": "Admin@2025!"
  }
}
```

Opção B: Manualmente depois do deploy
```
1. Registar conta normal em /Identity/Account/Register
2. Ir à base de dados
3. Executar SQL:
   INSERT INTO AspNetUserRoles (UserId, RoleId)
   SELECT Id, (SELECT Id FROM AspNetRoles WHERE Name = 'Admin')
   FROM AspNetUsers WHERE Email = 'seuemail@exemplo.com'
```

**TEMPO TOTAL**: ~10 minutos ⏱️

---

## 🚀 **OPÇÃO 2: Deploy Completo com OCR (15 minutos)**

### **Tudo da Opção 1 +**

#### **7. Criar Azure Document Intelligence**
```bash
# Criar recurso (Free tier)
az cognitiveservices account create \
  --name ervilhinha-ocr \
  --resource-group ErvilhinhaRG \
  --kind FormRecognizer \
  --sku F0 \
  --location westeurope \
  --yes

# Obter endpoint
az cognitiveservices account show \
  --name ervilhinha-ocr \
  --resource-group ErvilhinhaRG \
  --query "properties.endpoint" -o tsv

# Obter API key
az cognitiveservices account keys list \
  --name ervilhinha-ocr \
  --resource-group ErvilhinhaRG \
  --query "key1" -o tsv
```

Resultado:
```
Endpoint: https://ervilhinha-ocr.cognitiveservices.azure.com/
Key: abc123def456...
```

#### **8. Configurar App Settings**

**IIS** (`appsettings.Production.json`):
```json
{
  "AzureFormRecognizer": {
    "Endpoint": "https://ervilhinha-ocr.cognitiveservices.azure.com/",
    "ApiKey": "abc123def456..."
  }
}
```

**Azure App Service** (Portal):
```
Settings → Configuration → Application settings
[New application setting]

Name: AzureFormRecognizer__Endpoint
Value: https://ervilhinha-ocr.cognitiveservices.azure.com/

Name: AzureFormRecognizer__ApiKey
Value: abc123def456...
```

#### **9. Reiniciar App**
```bash
# Azure
az webapp restart --name ervilhinha-app --resource-group ErvilhinhaRG

# IIS
iisreset
```

#### **10. Testar OCR**
```
1. Login em /Identity/Account/Login
2. Ir a /Invoices/Upload
3. Verificar: Card mostra "🤖 IA ATIVO ✅"
4. Carregar fatura de teste
5. Verificar dados extraídos automaticamente
6. Aprovar → Despesa criada ✅
```

**TEMPO TOTAL**: ~20 minutos ⏱️  
**CUSTO EXTRA**: €0 (Free tier)

---

## 📋 **CHECKLIST PÓS-DEPLOY**

### **Funcionalidades Obrigatórias**:
- [ ] Criar conta de teste
- [ ] Criar categoria "Alimentação"
- [ ] Criar categoria "Bebé"
- [ ] Adicionar despesa manual
- [ ] Criar Baby List
- [ ] Partilhar lista (copiar código)
- [ ] Abrir link público (testar em incognito)
- [ ] Criar simulador de custos
- [ ] Verificar orçamento calcula automaticamente

### **Funcionalidades Opcionais**:
- [ ] (Se OCR ativo) Testar upload fatura
- [ ] (Se Google ativo) Testar login Google
- [ ] Verificar alertas aparecem no dashboard
- [ ] Testar timeline de eventos

### **Segurança**:
- [ ] HTTPS ativo
- [ ] Connection string não exposta
- [ ] Admin user criado
- [ ] Roles funcionam (Admin vs User)
- [ ] Painel admin acessível apenas para admins

---

## 🔧 **TROUBLESHOOTING COMUM**

### **Problema 1**: "Database connection failed"
**Solução**:
```
1. Verificar connection string em appsettings.json
2. Confirmar SQL Server está acessível
3. Executar: dotnet ef database update
```

### **Problema 2**: "Azure OCR não funciona"
**Solução**:
```
1. Verificar App Settings tem AzureFormRecognizer__Endpoint
2. Confirmar API Key está correta
3. Testar endpoint: https://SEU-RECURSO.cognitiveservices.azure.com/
4. Ver logs: /Invoices/Index deve mostrar "IA ATIVO ✅"
```

### **Problema 3**: "Não consigo fazer login como Admin"
**Solução**:
```sql
-- Verificar user tem role Admin
SELECT u.Email, r.Name
FROM AspNetUsers u
JOIN AspNetUserRoles ur ON u.Id = ur.UserId
JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE u.Email = 'seuemail@exemplo.com'

-- Se não tiver, adicionar:
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES (
    (SELECT Id FROM AspNetUsers WHERE Email = 'seuemail@exemplo.com'),
    (SELECT Id FROM AspNetRoles WHERE Name = 'Admin')
)
```

### **Problema 4**: "CSS/JS não carregam (404)"
**Solução**:
```
1. Verificar wwwroot está na pasta publish
2. IIS: Verificar Static Files está habilitado
3. Azure: Restart app service
4. Verificar _Layout.cshtml tem asp-append-version="true"
```

---

## 📊 **MONITORIZAÇÃO RECOMENDADA**

### **Logs a Vigiar**:
```csharp
// Logs críticos que indicam problemas:
ERROR: "Azure Form Recognizer NOT CONFIGURED" 
  → Normal se não ativaste OCR

ERROR: "Database connection failed"
  → CRÍTICO - verificar connection string

WARNING: "User Secrets não encontrado"
  → Normal em produção (usa appsettings)
```

### **Application Insights** (opcional mas recomendado):
```bash
# Criar recurso
az monitor app-insights component create \
  --app ervilhinha-insights \
  --location westeurope \
  --resource-group ErvilhinhaRG

# Adicionar ao appsettings.json:
{
  "ApplicationInsights": {
    "InstrumentationKey": "abc-123-def"
  }
}

# Instalar pacote
dotnet add package Microsoft.ApplicationInsights.AspNetCore
```

---

## 🎯 **CONCLUSÃO**

**Deploy Básico**: ✅ Funciona 100% sem configurações extra  
**Deploy Completo**: ✅ +OCR automático com Azure (5 min setup)  
**Custo Mínimo**: €0 (SQL Server + App Service grátis para teste, OCR free tier)  
**Custo Produção**: ~€10-50/mês (depende de hosting + OCR usage)

---

**Última Atualização**: 2025-05-XX  
**Documentação Completa**: `Documentation/MOCK-DATA-AUDIT-REPORT.md`
