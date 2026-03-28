# 🎨 Guia Visual de Componentes UI/UX - Ervilhinha

## 📋 Biblioteca de Componentes Reutilizáveis

Este documento fornece **snippets de código prontos a usar** para criar páginas consistentes e modernas.

---

## 🧭 1. Breadcrumbs

### Código Base
```razor
<nav aria-label="breadcrumb">
    <ol class="breadcrumb">
        <li class="breadcrumb-item"><a asp-action="Index" asp-controller="Home">Início</a></li>
        <li class="breadcrumb-item"><a asp-action="Index">Secção Principal</a></li>
        <li class="breadcrumb-item active" aria-current="page">Página Atual</li>
    </ol>
</nav>
```

### Exemplos Específicos

#### BabyLists
```razor
<nav aria-label="breadcrumb">
    <ol class="breadcrumb">
        <li class="breadcrumb-item"><a asp-action="Index">Minhas Listas</a></li>
        <li class="breadcrumb-item"><a asp-action="Manage" asp-route-id="@Model.ListId">@Model.ListName</a></li>
        <li class="breadcrumb-item active" aria-current="page">Adicionar Item</li>
    </ol>
</nav>
```

#### Expenses
```razor
<nav aria-label="breadcrumb">
    <ol class="breadcrumb">
        <li class="breadcrumb-item"><a asp-action="Index" asp-controller="Home">Início</a></li>
        <li class="breadcrumb-item"><a asp-action="Index">Despesas</a></li>
        <li class="breadcrumb-item active" aria-current="page">Adicionar</li>
    </ol>
</nav>
```

---

## 🎨 2. Headers de Páginas

### Estrutura Completa
```razor
<div class="mb-4">
    <!-- Breadcrumbs -->
    <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <!-- ... -->
        </ol>
    </nav>
    
    <!-- Título -->
    <h1 class="page-title">🎯 Título da Página</h1>
    
    <!-- Descrição -->
    <p class="text-soft mb-0">
        <i class="bi bi-info-circle"></i> Breve descrição do que a página faz
    </p>
</div>
```

### Variações

#### Com Ação Rápida
```razor
<div class="mb-4">
    <div class="d-flex justify-content-between align-items-center">
        <div>
            <h1 class="page-title">📋 Minhas Listas</h1>
            <p class="text-soft mb-0">Gere as tuas listas do bebé</p>
        </div>
        <a asp-action="Create" class="btn btn-primary">
            <i class="bi bi-plus-circle"></i> Nova Lista
        </a>
    </div>
</div>
```

---

## 📦 3. Cards com Headers Coloridos

### Template Base
```razor
<div class="card shadow mb-4">
    <div class="card-header bg-gradient-NOME">
        <h5 class="mb-0">🎯 Título do Grupo</h5>
    </div>
    <div class="card-body">
        <!-- Conteúdo -->
    </div>
</div>
```

### Estilos Disponíveis

```css
/* Verde (Informação Geral) */
.bg-gradient-pea {
    background: linear-gradient(135deg, #E8F5E9 0%, #C8E6C9 100%);
}

/* Rosa (Bebé) */
.bg-gradient-pink {
    background: linear-gradient(135deg, #FCE4EC 0%, #F8BBD0 100%);
}

/* Azul (Categorização) */
.bg-gradient-blue {
    background: linear-gradient(135deg, #E3F2FD 0%, #BBDEFB 100%);
}

/* Amarelo (Financeiro) */
.bg-gradient-yellow {
    background: linear-gradient(135deg, #FFF9C4 0%, #FFF59D 100%);
}

/* Laranja (Alertas/Avisos) */
.bg-gradient-orange {
    background: linear-gradient(135deg, #FFF3E0 0%, #FFE0B2 100%);
}

/* Roxo (Premium/Especial) */
.bg-gradient-purple {
    background: linear-gradient(135deg, #F3E5F5 0%, #E1BEE7 100%);
}

/* Ciano (Informação Extra) */
.bg-gradient-cyan {
    background: linear-gradient(135deg, #E0F7FA 0%, #B2EBF2 100%);
}
```

### Exemplo Completo
```razor
<!-- Informações Básicas -->
<div class="card shadow mb-4">
    <div class="card-header bg-gradient-pea">
        <h5 class="mb-0">📝 Informações Básicas</h5>
    </div>
    <div class="card-body">
        <div class="mb-3">
            <label class="form-label fw-semibold">
                Nome <span class="text-danger">*</span>
            </label>
            <input type="text" class="form-control form-control-lg" 
                   placeholder="Digite o nome..." required />
        </div>
    </div>
</div>
```

---

## 💰 4. Input Groups (Valores Monetários)

### Variação 1: Custo/Despesa (Verde)
```razor
<div class="input-group input-group-lg">
    <span class="input-group-text bg-success text-white fw-bold">€</span>
    <input type="number" class="form-control" 
           step="0.01" min="0" max="100000" 
           placeholder="0.00" required />
</div>
```

### Variação 2: Custo Estimado (Primário)
```razor
<div class="input-group input-group-lg">
    <span class="input-group-text bg-primary text-white fw-bold">€</span>
    <input type="number" class="form-control" 
           step="0.01" min="0" placeholder="0.00" />
</div>
```

### Variação 3: Despesa (Vermelho)
```razor
<div class="input-group input-group-lg">
    <span class="input-group-text bg-danger text-white fw-bold">€</span>
    <input type="number" class="form-control" 
           step="0.01" min="0" placeholder="0.00" />
</div>
```

### Com Validação e Helper
```razor
<label asp-for="EstimatedCost" class="form-label fw-semibold">
    Custo Estimado <span class="text-danger">*</span>
</label>
<div class="input-group input-group-lg">
    <span class="input-group-text bg-success text-white fw-bold">€</span>
    <input asp-for="EstimatedCost" type="number" class="form-control" 
           step="0.01" min="0" max="10000" required id="estimatedCostInput" />
</div>
<span asp-validation-for="EstimatedCost" class="text-danger small"></span>
<small class="text-muted d-block mt-1">Valor estimado do produto</small>
```

---

## 📊 5. Contadores de Caracteres

### HTML
```razor
<label asp-for="Name" class="form-label fw-semibold">
    Nome do Item <span class="text-danger">*</span>
</label>
<input asp-for="Name" class="form-control form-control-lg" 
       maxlength="200" id="nameInput" required />
<small class="text-muted d-block mt-1">
    <span id="nameCharCount">0</span> / 200 caracteres
</small>
```

### JavaScript
```javascript
const nameInput = document.getElementById('nameInput');
const nameCount = document.getElementById('nameCharCount');

nameInput.addEventListener('input', function() {
    nameCount.textContent = this.value.length;
});
```

### Versão para Textarea
```razor
<textarea asp-for="Description" class="form-control" 
          rows="3" maxlength="500" id="descriptionInput"></textarea>
<small class="text-muted d-block mt-1">
    Máximo 500 caracteres • <span id="descCharCount">0</span> usados
</small>
```

---

## 🔘 6. Toggle Switches Modernos

### HTML
```razor
<div class="form-check form-switch form-switch-lg">
    <input asp-for="IsActive" class="form-check-input" 
           type="checkbox" role="switch" id="activeToggle" />
    <label class="form-check-label fw-semibold" for="activeToggle">
        <span class="fs-5">✅</span> Item Ativo
    </label>
</div>
<small class="text-muted d-block ms-5">Descrição do que o toggle faz</small>
```

### CSS
```css
.form-switch-lg .form-check-input {
    width: 3rem;
    height: 1.5rem;
}
```

### Exemplo Completo (3 Toggles em Row)
```razor
<div class="row">
    <!-- Toggle 1 -->
    <div class="col-md-4 mb-3 mb-md-0">
        <div class="form-check form-switch form-switch-lg">
            <input asp-for="IsPurchased" class="form-check-input" 
                   type="checkbox" role="switch" id="purchasedToggle" />
            <label class="form-check-label fw-semibold" for="purchasedToggle">
                <span class="fs-5">✅</span> Já Comprado
            </label>
        </div>
        <small class="text-muted d-block ms-5">Item foi adquirido</small>
    </div>

    <!-- Toggle 2 -->
    <div class="col-md-4 mb-3 mb-md-0">
        <div class="form-check form-switch form-switch-lg">
            <input asp-for="IsGift" class="form-check-input" 
                   type="checkbox" role="switch" id="giftToggle" />
            <label class="form-check-label fw-semibold" for="giftToggle">
                <span class="fs-5">🎁</span> Foi Presente
            </label>
        </div>
        <small class="text-muted d-block ms-5">Recebido como oferta</small>
    </div>

    <!-- Toggle 3 -->
    <div class="col-md-4">
        <div class="form-check form-switch form-switch-lg">
            <input asp-for="IsShared" class="form-check-input" 
                   type="checkbox" role="switch" id="sharedToggle" />
            <label class="form-check-label fw-semibold" for="sharedToggle">
                <span class="fs-5">🔗</span> Partilhado
            </label>
        </div>
        <small class="text-muted d-block ms-5">Lista está partilhada</small>
    </div>
</div>
```

---

## 🔢 7. Select Boxes com Optgroups

### Simples
```razor
<select asp-for="Priority" class="form-select form-select-lg" required>
    <option value="">-- Selecione a Prioridade --</option>
    <option value="1">✅ Essencial</option>
    <option value="2">⭐ Recomendado</option>
    <option value="3">💡 Opcional</option>
</select>
```

### Com Optgroups
```razor
<select asp-for="Category" class="form-select form-select-lg" required>
    <option value="">-- Selecione a Categoria --</option>
    
    <optgroup label="Essenciais">
        <option value="1">🛏️ Quarto do Bebé</option>
        <option value="6">🍼 Alimentação</option>
        <option value="5">🛁 Higiene e Banho</option>
    </optgroup>
    
    <optgroup label="Roupa por Idade">
        <option value="2">👶 Roupa 0-3 Meses</option>
        <option value="3">👕 Roupa 3-6 Meses</option>
        <option value="4">👔 Roupa 6-12 Meses</option>
    </optgroup>
    
    <optgroup label="Outros">
        <option value="8">🧸 Brinquedos</option>
        <option value="10">✨ Acessórios</option>
    </optgroup>
</select>
```

---

## 🎯 8. Botões de Ação

### Layout Padrão (Cancelar | Guardar)
```razor
<div class="d-flex gap-2 justify-content-between align-items-center">
    <a asp-action="Index" class="btn btn-outline-secondary btn-lg" id="cancelBtn">
        <i class="bi bi-x-circle"></i> Cancelar
    </a>
    <button type="submit" class="btn btn-primary btn-lg px-5" id="submitBtn">
        <i class="bi bi-check-circle-fill"></i> Guardar
    </button>
</div>

<!-- Helper Text -->
<div class="text-center mt-3">
    <small class="text-muted">
        <i class="bi bi-shield-check"></i> As alterações serão guardadas imediatamente
    </small>
</div>
```

### Variações de Texto

#### Criar
```razor
<button type="submit" class="btn btn-primary btn-lg px-5">
    <i class="bi bi-plus-circle-fill"></i> Criar Lista
</button>
```

#### Atualizar
```razor
<button type="submit" class="btn btn-primary btn-lg px-5">
    <i class="bi bi-check-circle-fill"></i> Guardar Alterações
</button>
```

#### Calcular
```razor
<button type="submit" class="btn btn-primary btn-lg px-5">
    <i class="bi bi-calculator-fill"></i> Calcular Custos
</button>
```

#### Eliminar (Destrutivo)
```razor
<button type="submit" class="btn btn-danger btn-lg px-5">
    <i class="bi bi-trash-fill"></i> Eliminar Permanentemente
</button>
```

---

## ⚙️ 9. JavaScript - Loading States

### Botão de Submit
```javascript
const form = document.getElementById('formId');
const submitBtn = document.getElementById('submitBtn');

form.addEventListener('submit', function() {
    if (form.checkValidity()) {
        submitBtn.disabled = true;
        submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span> A guardar...';
    }
});
```

### Variações de Mensagens

```javascript
// Criar
submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span> A criar...';

// Atualizar
submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span> A atualizar...';

// Calcular
submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span> A calcular...';

// Eliminar
submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span> A eliminar...';

// Upload
submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span> A enviar ficheiro...';
```

---

## ⚠️ 10. Confirmação de Cancelamento

### Versão Simples
```javascript
const cancelBtn = document.getElementById('cancelBtn');

cancelBtn.addEventListener('click', function(e) {
    const nameInput = document.getElementById('nameInput').value.trim();
    
    if (nameInput !== '') {
        if (!confirm('Tens dados não guardados. Tens a certeza que queres cancelar?')) {
            e.preventDefault();
        }
    }
});
```

### Versão Completa (Detecta Alterações)
```javascript
const form = document.getElementById('formId');
const cancelBtn = document.getElementById('cancelBtn');
const originalData = new FormData(form);

cancelBtn.addEventListener('click', function(e) {
    const currentData = new FormData(form);
    let isModified = false;
    
    for (let [key, value] of currentData.entries()) {
        if (originalData.get(key) !== value) {
            isModified = true;
            break;
        }
    }
    
    if (isModified) {
        if (!confirm('Tens alterações não guardadas. Tens a certeza que queres cancelar?')) {
            e.preventDefault();
        }
    }
});
```

---

## 📊 11. Cálculos em Tempo Real

### Custo Total
```javascript
const quantityInput = document.getElementById('quantityInput');
const costInput = document.getElementById('estimatedCostInput');
const totalDisplay = document.getElementById('totalCostDisplay');

function updateTotalCost() {
    const quantity = parseFloat(quantityInput.value) || 0;
    const cost = parseFloat(costInput.value) || 0;
    const total = quantity * cost;
    
    totalDisplay.textContent = total.toFixed(2);
}

quantityInput.addEventListener('input', updateTotalCost);
costInput.addEventListener('input', updateTotalCost);
```

### Com Alerta Visual
```razor
<div class="alert alert-info" role="alert" id="totalCostAlert">
    <div class="d-flex align-items-center justify-content-between">
        <strong>💰 Custo Total:</strong>
        <span class="fs-4 fw-bold text-primary">
            € <span id="totalCostDisplay">0.00</span>
        </span>
    </div>
</div>
```

---

## 📝 12. Auto-Resize Textarea

```javascript
const textarea = document.getElementById('descriptionInput');

textarea.addEventListener('input', function() {
    this.style.height = 'auto';
    this.style.height = this.scrollHeight + 'px';
});
```

---

## 💡 13. Alerts Informativos

### Info
```razor
<div class="alert alert-info" role="alert">
    <i class="bi bi-info-circle-fill"></i>
    <strong>Informação:</strong> Mensagem informativa aqui.
</div>
```

### Success
```razor
<div class="alert alert-success" role="alert">
    <i class="bi bi-check-circle-fill"></i>
    <strong>Sucesso!</strong> Operação concluída com sucesso.
</div>
```

### Warning
```razor
<div class="alert alert-warning" role="alert">
    <i class="bi bi-exclamation-triangle-fill"></i>
    <strong>Atenção!</strong> Verifica esta informação.
</div>
```

### Danger/Error
```razor
<div class="alert alert-danger" role="alert">
    <i class="bi bi-x-circle-fill"></i>
    <strong>Erro!</strong> Algo correu mal.
</div>
```

### Alert Dinâmico (Oculto por Padrão)
```razor
<div class="alert alert-success" role="alert" 
     id="savingsAlert" style="display:none;">
    <strong><i class="bi bi-piggy-bank-fill"></i> Poupança:</strong>
    <span id="savingsText"></span>
</div>
```

```javascript
const alert = document.getElementById('savingsAlert');
const text = document.getElementById('savingsText');

function showSavings(amount) {
    text.textContent = `€${amount.toFixed(2)}/mês`;
    alert.style.display = 'block';
}
```

---

## 🎨 14. Sidebar de Ajuda

```razor
<div class="col-lg-4 col-xl-3 mt-4 mt-lg-0">
    <div class="card shadow sticky-top" style="top: 1rem;">
        <div class="card-header bg-info bg-opacity-10">
            <h6 class="mb-0 text-info">
                <i class="bi bi-lightbulb-fill"></i> Dicas Rápidas
            </h6>
        </div>
        <div class="card-body">
            <ul class="small mb-0">
                <li class="mb-2">
                    <strong>Dica 1:</strong> Descrição da dica
                </li>
                <li class="mb-2">
                    <strong>Dica 2:</strong> Outra informação útil
                </li>
                <li>
                    <strong>Dica 3:</strong> Mais uma sugestão
                </li>
            </ul>

            <hr>

            <div class="text-center">
                <a asp-action="Help" class="btn btn-sm btn-outline-info w-100">
                    <i class="bi bi-question-circle"></i> Ajuda Completa
                </a>
            </div>
        </div>
    </div>
</div>
```

---

## 🔗 15. Link de Produto com Ícone

```razor
<div class="mb-3">
    <label asp-for="ProductUrl" class="form-label fw-semibold">
        Link do Produto <span class="text-muted small">(opcional)</span>
    </label>
    <div class="input-group">
        <span class="input-group-text bg-primary text-white">
            <i class="bi bi-link-45deg"></i>
        </span>
        <input asp-for="ProductUrl" type="url" class="form-control" 
               placeholder="https://www.loja.com/produto" maxlength="500" />
    </div>
    <span asp-validation-for="ProductUrl" class="text-danger small"></span>
</div>
```

---

## ✅ Template Completo de Página

```razor
@model YourModel

@{
    ViewData["Title"] = "Título da Página";
}

<div class="mb-4">
    <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a asp-action="Index" asp-controller="Home">Início</a></li>
            <li class="breadcrumb-item"><a asp-action="Index">Secção</a></li>
            <li class="breadcrumb-item active" aria-current="page">Página Atual</li>
        </ol>
    </nav>
    <h1 class="page-title">🎯 Título Principal</h1>
    <p class="text-soft mb-0">
        <i class="bi bi-info-circle"></i> Descrição breve da funcionalidade
    </p>
</div>

<div class="row justify-content-center">
    <div class="col-lg-9 col-xl-8">
        <form asp-action="ActionName" method="post" id="mainForm">
            <input type="hidden" asp-for="Id" />
            <div asp-validation-summary="ModelOnly" class="alert alert-danger" role="alert"></div>

            <!-- CARD 1: Informações Básicas -->
            <div class="card shadow mb-4">
                <div class="card-header bg-gradient-pea">
                    <h5 class="mb-0">📝 Informações Básicas</h5>
                </div>
                <div class="card-body">
                    <div class="mb-3">
                        <label asp-for="Name" class="form-label fw-semibold">
                            Nome <span class="text-danger">*</span>
                        </label>
                        <input asp-for="Name" class="form-control form-control-lg" 
                               placeholder="Digite o nome..." 
                               autofocus required maxlength="200" id="nameInput" />
                        <span asp-validation-for="Name" class="text-danger small"></span>
                        <small class="text-muted d-block mt-1">
                            <span id="nameCharCount">0</span> / 200 caracteres
                        </small>
                    </div>
                </div>
            </div>

            <!-- CARD 2: Informações Adicionais -->
            <div class="card shadow mb-4">
                <div class="card-header bg-gradient-blue">
                    <h5 class="mb-0">ℹ️ Informações Adicionais</h5>
                </div>
                <div class="card-body">
                    <!-- Campos adicionais -->
                </div>
            </div>

            <!-- Botões de Ação -->
            <div class="d-flex gap-2 justify-content-between align-items-center">
                <a asp-action="Index" class="btn btn-outline-secondary btn-lg" id="cancelBtn">
                    <i class="bi bi-x-circle"></i> Cancelar
                </a>
                <button type="submit" class="btn btn-primary btn-lg px-5" id="submitBtn">
                    <i class="bi bi-check-circle-fill"></i> Guardar
                </button>
            </div>

            <div class="text-center mt-3">
                <small class="text-muted">
                    <i class="bi bi-shield-check"></i> Texto de ajuda/informação
                </small>
            </div>
        </form>
    </div>

    <!-- Sidebar (Opcional) -->
    <div class="col-lg-3 col-xl-3 mt-4 mt-lg-0">
        <div class="card shadow sticky-top" style="top: 1rem;">
            <div class="card-header bg-info bg-opacity-10">
                <h6 class="mb-0 text-info">
                    <i class="bi bi-lightbulb-fill"></i> Dicas
                </h6>
            </div>
            <div class="card-body">
                <ul class="small mb-0">
                    <li class="mb-2">Dica 1</li>
                    <li>Dica 2</li>
                </ul>
            </div>
        </div>
    </div>
</div>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
    
    <script>
        // Character counter
        const nameInput = document.getElementById('nameInput');
        const nameCount = document.getElementById('nameCharCount');
        nameInput.addEventListener('input', () => {
            nameCount.textContent = nameInput.value.length;
        });

        // Cancel confirmation
        const form = document.getElementById('mainForm');
        const cancelBtn = document.getElementById('cancelBtn');
        cancelBtn.addEventListener('click', function(e) {
            if (nameInput.value.trim() !== '') {
                if (!confirm('Tens dados não guardados. Tens a certeza?')) {
                    e.preventDefault();
                }
            }
        });

        // Submit loading
        form.addEventListener('submit', function() {
            if (form.checkValidity()) {
                const submitBtn = document.getElementById('submitBtn');
                submitBtn.disabled = true;
                submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span> A guardar...';
            }
        });
    </script>

    <style>
        .bg-gradient-pea {
            background: linear-gradient(135deg, #E8F5E9 0%, #C8E6C9 100%);
        }
        .bg-gradient-blue {
            background: linear-gradient(135deg, #E3F2FD 0%, #BBDEFB 100%);
        }
    </style>
}
```

---

## 🎉 Pronto a Usar!

Copia e adapta estes componentes conforme necessário. Todos seguem os **padrões estabelecidos** para garantir consistência em toda a aplicação.

**Happy Coding!** 🚀
