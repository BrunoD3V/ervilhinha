# 🤖 Invoice OCR - Melhorias UX

## 📋 Plano de Melhorias

### **1. Drag & Drop Upload**
Substituir input file por zona de arrastar:

```html
<div class="dropzone-modern" id="invoiceDropzone">
    <i class="bi bi-cloud-upload icon-pulse"></i>
    <h5>Arrasta fatura aqui</h5>
    <p class="text-muted">ou clica para escolher</p>
    <small>JPG, PNG, PDF (máx 10MB)</small>
</div>

<style>
.dropzone-modern {
    border: 3px dashed var(--pea-green);
    background: linear-gradient(135deg, var(--bg-cream) 0%, var(--pea-green-light) 100%);
    padding: 3rem;
    text-align: center;
    border-radius: 25px;
    cursor: pointer;
    transition: all 0.3s ease;
}

.dropzone-modern:hover {
    border-color: var(--pea-green-soft);
    background: var(--pea-green-light);
    transform: scale(1.02);
}

.dropzone-modern.dragover {
    border-color: #81C784;
    background: var(--pea-green);
    box-shadow: 0 0 30px rgba(129, 199, 132, 0.5);
}
</style>

<script>
const dropzone = document.getElementById('invoiceDropzone');
const fileInput = document.getElementById('fileInput');

dropzone.addEventListener('click', () => fileInput.click());

dropzone.addEventListener('dragover', (e) => {
    e.preventDefault();
    dropzone.classList.add('dragover');
});

dropzone.addEventListener('dragleave', () => {
    dropzone.classList.remove('dragover');
});

dropzone.addEventListener('drop', (e) => {
    e.preventDefault();
    dropzone.classList.remove('dragover');
    
    const files = e.dataTransfer.files;
    if (files.length > 0) {
        fileInput.files = files;
        uploadInvoice(files[0]);
    }
});

function uploadInvoice(file) {
    const formData = new FormData();
    formData.append('file', file);
    
    // Mostrar preview + skeleton loader
    showPreview(file);
    showSkeletonLoader();
    
    fetch('/Invoices/Upload', {
        method: 'POST',
        body: formData
    })
    .then(response => response.json())
    .then(data => {
        hideSkeletonLoader();
        showReviewForm(data);
        ErvilhinhaEpic.confetti.launch(3000);
        ErvilhinhaEpic.toast.show('✅ Fatura processada!', 'success');
    })
    .catch(error => {
        ErvilhinhaEpic.toast.show('❌ Erro ao processar', 'error');
    });
}
</script>
```

### **2. Loading Animation com Skeleton**
Enquanto OCR processa (1-3 segundos):

```html
<div id="ocrLoader" class="hidden">
    <div class="ocr-processing-card">
        <div class="spinner-modern">
            <i class="bi bi-receipt-cutoff icon-spin"></i>
        </div>
        <h5 class="mt-3">🤖 A ler fatura...</h5>
        <div class="progress-modern">
            <div class="progress-bar" style="width: 0%"></div>
        </div>
        <p class="text-muted mt-2">Estamos a extrair os dados</p>
    </div>
</div>

<style>
.icon-spin {
    animation: spin 2s linear infinite;
}

@keyframes spin {
    from { transform: rotate(0deg); }
    to { transform: rotate(360deg); }
}

.progress-bar {
    animation: loading-progress 3s ease-in-out forwards;
}

@keyframes loading-progress {
    0% { width: 0%; }
    30% { width: 50%; }
    90% { width: 95%; }
    100% { width: 100%; }
}
</style>
```

### **3. Review Form com Confiança**
Mostrar confidence score do Azure:

```html
<div class="review-card">
    <h5>🔍 Dados Extraídos</h5>
    
    <div class="extracted-field">
        <label>Fornecedor</label>
        <input type="text" value="@Model.VendorName" />
        <span class="confidence-badge high">
            <i class="bi bi-check-circle-fill"></i> 98% confiança
        </span>
    </div>
    
    <div class="extracted-field">
        <label>Total</label>
        <input type="number" step="0.01" value="@Model.TotalAmount" />
        <span class="confidence-badge medium">
            <i class="bi bi-exclamation-triangle-fill"></i> 75% confiança
        </span>
    </div>
</div>

<style>
.confidence-badge.high {
    color: var(--pea-green-soft);
}

.confidence-badge.medium {
    color: var(--baby-peach);
}

.confidence-badge.low {
    color: #EF9A9A;
}
</style>
```

### **4. Correção Inteligente**
Se utilizador corrige campo, aprender padrão:

```javascript
// Guardar correções para melhorar futuras leituras
fetch('/Invoices/ReportCorrection', {
    method: 'POST',
    body: JSON.stringify({
        invoiceId: 123,
        field: 'VendorName',
        ocrValue: 'Continnte',
        correctedValue: 'Continente',
        confidence: 0.75
    })
});
```

### **5. Preview Side-by-Side**
Mostrar imagem ao lado dos dados:

```html
<div class="review-layout">
    <div class="preview-pane">
        <img src="/uploads/invoices/@Model.FileName" alt="Fatura" />
        <button class="btn-zoom" onclick="zoomImage()">
            <i class="bi bi-zoom-in"></i> Ampliar
        </button>
    </div>
    
    <div class="form-pane">
        <!-- Form fields aqui -->
    </div>
</div>

<style>
.review-layout {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 2rem;
}

.preview-pane img {
    width: 100%;
    border-radius: 20px;
    box-shadow: 0 8px 25px rgba(0,0,0,0.1);
}

@media (max-width: 768px) {
    .review-layout {
        grid-template-columns: 1fr;
    }
}
</style>
```

### **6. Batch Upload**
Permitir múltiplas faturas:

```javascript
// Processar 5 faturas simultaneamente
async function uploadBatch(files) {
    const promises = Array.from(files).map(file => uploadInvoice(file));
    
    const results = await Promise.all(promises);
    
    ErvilhinhaEpic.toast.show(`✅ ${results.length} faturas processadas!`, 'success');
    ErvilhinhaEpic.confetti.launch(5000);
}
```

### **7. Auto-Categorização**
Usar AI para sugerir categoria:

```csharp
// Em InvoiceOcrService
public string SuggestCategory(InvoiceOcrResult result)
{
    var vendor = result.VendorName?.ToLower() ?? "";
    
    if (vendor.Contains("continente") || vendor.Contains("pingo doce"))
        return "Alimentação";
    
    if (vendor.Contains("farmácia") || vendor.Contains("wells"))
        return "Saúde";
    
    if (vendor.Contains("zippy") || vendor.Contains("chicco"))
        return "Roupa Bebé";
    
    // Usar Azure OpenAI para casos complexos
    return await _aiService.ClassifyExpense(result.RawText);
}
```

### **8. Histórico de Faturas**
Dashboard com estatísticas:

```razor
<div class="stats-grid">
    <div class="stat-card">
        <h3>@Model.TotalInvoices</h3>
        <p>Faturas processadas</p>
        <i class="bi bi-receipt"></i>
    </div>
    
    <div class="stat-card">
        <h3>@Model.AverageConfidence%</h3>
        <p>Precisão média</p>
        <i class="bi bi-graph-up"></i>
    </div>
    
    <div class="stat-card">
        <h3>€@Model.TotalAmount</h3>
        <p>Total extraído</p>
        <i class="bi bi-currency-euro"></i>
    </div>
</div>
```

---

## 🎯 Prioridades Sugeridas

### **Must Have** (fazer já):
1. ✅ Ativar Azure (5 min)
2. ✅ Drag & drop upload (30 min)
3. ✅ Loading animation (15 min)
4. ✅ Review form melhorada (30 min)

### **Nice to Have** (depois):
5. Preview side-by-side
6. Confidence badges
7. Auto-categorização

### **Future**:
8. Batch upload
9. Correção com machine learning
10. Dashboard de estatísticas

---

## 💡 Integração com Expenses

Atualmente:
```
Invoice → Upload → OCR → Review → Manual create Expense
```

Proposta:
```
Invoice → Upload → OCR → Auto-create Expense Draft → User approves
```

Código:
```csharp
[HttpPost]
public async Task<IActionResult> AutoCreateExpense(int invoiceId)
{
    var invoice = await _context.Invoices.FindAsync(invoiceId);
    
    var expense = new Expense
    {
        UserId = User.GetUserId(),
        Description = invoice.VendorName ?? "Fatura importada",
        Amount = invoice.TotalAmount ?? 0,
        ExpenseDate = invoice.InvoiceDate ?? DateTime.Today,
        CategoryId = SuggestCategory(invoice),
        InvoiceId = invoiceId,
        Status = ExpenseStatus.Draft // Precisa aprovação
    };
    
    _context.Expenses.Add(expense);
    await _context.SaveChangesAsync();
    
    return RedirectToAction("Edit", "Expenses", new { id = expense.Id });
}
```

---

## 🔒 Segurança & Privacidade

### **GDPR Compliance**:
- ✅ Faturas guardadas localmente (`wwwroot/uploads`)
- ✅ Azure processa mas **não guarda** imagens
- ✅ User pode apagar fatura a qualquer momento
- ⚠️ Adicionar aviso: "Dados processados na EU (Azure West Europe)"

### **Encriptação**:
```csharp
// Opcional: Encriptar faturas em disco
public async Task<string> SaveEncryptedInvoice(IFormFile file)
{
    using var aes = Aes.Create();
    aes.Key = GetEncryptionKey();
    aes.IV = GetIV();
    
    var encrypted = await EncryptFile(file, aes);
    await File.WriteAllBytesAsync(filePath, encrypted);
}
```

---

## 📈 Métricas de Sucesso

Tracking sugerido:
- **Tempo médio** de processar fatura (objetivo: <5 segundos)
- **Taxa de correções** (objetivo: <10%)
- **Adoção** (% users que usam OCR vs manual entry)
- **Economia de tempo** (estimativa: 2 min/fatura manual → 10 seg OCR)

---

## 💰 Custos Estimados

### **Azure Document Intelligence Pricing**:
| Tier | Preço | Faturas/mês | Custo mensal |
|------|-------|-------------|--------------|
| **F0 (Free)** | €0 | 500 | **€0** |
| **S0 (Standard)** | €1.00/1000 páginas | Ilimitado | Depende uso |

**Exemplo**: 
- Família média: ~50 faturas/mês
- 100 utilizadores × 50 faturas = 5000 faturas/mês
- Custo: **€5/mês** (tier free cobre primeiros 500)

**ROI**:
- Tempo poupado: 100 users × 50 faturas × 2 min = **10,000 minutos/mês**
- Equivalente: **167 horas** de trabalho manual
- Valor: €10/hora × 167h = **€1,670/mês** economizado
- **ROI: 334x** 🚀

---

## 🎓 Recursos Azure

- [Documentação oficial](https://learn.microsoft.com/en-us/azure/ai-services/document-intelligence/)
- [Pricing calculator](https://azure.microsoft.com/en-us/pricing/details/form-recognizer/)
- [Studio online](https://formrecognizer.appliedai.azure.com/) - Testar sem código

