# 📦 Adicionar Item - Documentação Completa

## 📍 Localização
**URL**: `/BabyLists/AddItem?listId={id}`  
**Autenticação**: Obrigatória  
**Permissões**: Proprietário da lista ou Co-Gestor

---

## 🎯 Objetivo da Página

Permite adicionar um novo item à lista do bebé, com todas as informações necessárias para:
- Planeamento de compras (custo, timing)
- Categorização (tipo de item, prioridade)
- Referências (link de produto)

---

## 📋 Estrutura do Formulário

### **Layout Desktop**
```
┌─────────────────────────────────────────────────────┐
│ 📦 Adicionar Item                                   │
│ À lista: Enxoval da Maria                           │
├─────────────────────────────────────────────────────┤
│ [Informação Básica - Card]                          │
│   Nome do Item*     [_________________________]     │
│   Descrição         [_________________________]     │
│                     [_________________________]     │
│                                                      │
│ [Categorização - Card]                               │
│   ┌────────────────┬───────────────┐               │
│   │ Categoria*     │ Prioridade*   │               │
│   │ [▼________]    │ [▼_______]    │               │
│   └────────────────┴───────────────┘               │
│                                                      │
│ [Planeamento - Card]                                 │
│   ┌────────┬──────────┬──────────────┐             │
│   │ Qtd*   │ Custo€*  │ Timing       │             │
│   │ [___]  │ [_____]  │ [▼________]  │             │
│   └────────┴──────────┴──────────────┘             │
│                                                      │
│ [Informações Adicionais - Card]                      │
│   Link Produto  [_________________________]         │
│   Notas         [_________________________]         │
│                 [_________________________]         │
│                                                      │
│ [✅ Adicionar Item] [Cancelar]                      │
└─────────────────────────────────────────────────────┘
```

---

## 📝 Campos do Formulário

### **GRUPO 1: Informação Básica**

#### **1.1 Nome do Item*** (Obrigatório)
**Tipo**: Text Input  
**Propriedade**: `BabyListItem.Name`  
**Tamanho**: Full-width (`col-12`)

**HTML**:
```html
<input asp-for="Name" class="form-control form-control-lg" 
       placeholder="Ex: Berço de madeira, Body 0-3M branco, Carrinho de passeio..." 
       autofocus required maxlength="200" />
```

**Validações:**
- ✅ **Obrigatório**: `[Required(ErrorMessage = "O nome do item é obrigatório")]`
- ✅ **Comprimento**: Máximo 200 caracteres `[StringLength(200)]`
- ✅ **Client-side**: HTML5 `required` + `maxlength`

**Mensagens de Erro:**
- Vazio: `"O nome do item é obrigatório"`
- Muito longo: `"O nome não pode ter mais de 200 caracteres"`

**Dicas de UX:**
- 💡 **Autofocus**: Cursor inicia aqui automaticamente
- 💡 **Placeholder sugestivo**: Mostra 3 exemplos diferentes
- 💡 **Font-size maior**: `form-control-lg` (1.25rem)
- 💡 **Full-width**: Nomes podem ser longos

**Exemplos Válidos:**
- ✅ "Berço de madeira Montessori"
- ✅ "Pack 5 bodies brancos 0-3M"
- ✅ "Carrinho de passeio 3 em 1"

**Exemplos Inválidos:**
- ❌ "" (vazio)
- ❌ "A" (muito curto - não dá contexto)

**Análise Crítica UI/UX:**
- ✅ **BOM**: Primeiro campo (lógico)
- ✅ **BOM**: Tamanho grande (destaque)
- ✅ **BOM**: Placeholder ajuda a entender formato
- ⚠️ **MELHORAR**: Adicionar contador de caracteres?
  ```html
  <small class="text-muted">
      <span id="charCount">0</span> / 200 caracteres
  </small>
  ```

---

#### **1.2 Descrição** (Opcional)
**Tipo**: Textarea  
**Propriedade**: `BabyListItem.Description`  
**Tamanho**: Full-width (`col-12`)

**HTML**:
```html
<textarea asp-for="Description" class="form-control" rows="3" 
          maxlength="1000"
          placeholder="Detalhes adicionais como cor, tamanho, marca preferida..."></textarea>
<small class="text-muted">Opcional • Máximo 1000 caracteres</small>
```

**Validações:**
- ❌ **Não obrigatório**
- ✅ **Comprimento**: Máximo 1000 caracteres `[StringLength(1000)]`

**Mensagens de Erro:**
- Muito longo: `"A descrição não pode ter mais de 1000 caracteres"`

**Dicas de UX:**
- 💡 **Altura fixa**: 3 rows (consistência)
- 💡 **Placeholder útil**: Dá exemplos do que escrever
- 💡 **Indicação clara**: "Opcional"

**Exemplos Válidos:**
- ✅ "" (vazio - é opcional)
- ✅ "Cor preferida: branco ou bege. Tamanho: 120x60cm"
- ✅ "Marca: Chicco ou similar. Com função de balanço."

**Análise Crítica UI/UX:**
- ✅ **BOM**: Vem logo após o nome (contexto)
- ✅ **BOM**: Não obrigatório (não tranca o form)
- ⚠️ **MELHORAR**: Auto-resize do textarea?
  ```javascript
  textarea.addEventListener('input', function() {
      this.style.height = 'auto';
      this.style.height = this.scrollHeight + 'px';
  });
  ```

---

### **GRUPO 2: Categorização**

#### **2.1 Categoria*** (Obrigatório)
**Tipo**: Select/Dropdown  
**Propriedade**: `BabyListItem.Category` (enum `ItemCategory`)  
**Tamanho**: 50% desktop (`col-md-6`)

**HTML**:
```html
<div class="col-md-6 mb-3">
    <label asp-for="Category" class="form-label fw-semibold">
        Categoria <span class="text-danger">*</span>
    </label>
    <select asp-for="Category" class="form-select" required>
        <option value="">-- Selecione a categoria --</option>
        <option value="1">🛏️ Quarto do Bebé</option>
        <option value="2">👕 Roupa 0-3M</option>
        <option value="3">👕 Roupa 3-6M</option>
        <option value="4">👕 Roupa 6-12M</option>
        <option value="5">🛁 Higiene e Banho</option>
        <option value="6">🍼 Alimentação</option>
        <option value="7">🚗 Passeio (Carrinho, Cadeira Auto)</option>
        <option value="8">🧸 Brinquedos</option>
        <option value="9">🏥 Saúde e Segurança</option>
        <option value="10">👶 Acessórios</option>
        <option value="11">📦 Outros</option>
    </select>
    <span asp-validation-for="Category" class="text-danger small"></span>
</div>
```

**Validações:**
- ✅ **Obrigatório**: Enum não-nullable
- ✅ **Valores válidos**: 1-11

**Mensagens de Erro:**
- Não selecionado: `"Selecione uma categoria"`
- Valor inválido: `"Categoria inválida"`

**Dicas de UX:**
- 💡 **Emojis**: Identificação visual rápida
- 💡 **Agrupamento**: Roupa por idade (0-3M, 3-6M, 6-12M)
- 💡 **Descrições**: "Passeio (Carrinho, Cadeira Auto)" - explica o que inclui
- 💡 **Opção neutra**: "Outros" para casos especiais

**Análise Crítica UI/UX:**
- ✅ **BOM**: Largura 50% (não desperdiça espaço)
- ✅ **BOM**: Emojis facilitam scanning visual
- ✅ **BOM**: Ordem lógica (do mais essencial para acessórios)
- ⚠️ **MELHORAR**: Agrupar por tipo?
  ```html
  <optgroup label="Essenciais">
      <option>Quarto do Bebé</option>
      <option>Alimentação</option>
  </optgroup>
  <optgroup label="Roupa">
      <option>0-3M</option>
      <option>3-6M</option>
  </optgroup>
  ```

---

#### **2.2 Prioridade*** (Obrigatório)
**Tipo**: Select/Dropdown  
**Propriedade**: `BabyListItem.Priority` (enum `ItemPriority`)  
**Tamanho**: 50% desktop (`col-md-6`)

**HTML**:
```html
<div class="col-md-6 mb-3">
    <label asp-for="Priority" class="form-label fw-semibold">
        Prioridade <span class="text-danger">*</span>
    </label>
    <select asp-for="Priority" class="form-select" required>
        <option value="1" selected>✅ Essencial (Comprar prioritariamente)</option>
        <option value="2">⭐ Recomendado (Muito útil)</option>
        <option value="3">💡 Opcional (Se houver orçamento)</option>
    </select>
    <span asp-validation-for="Priority" class="text-danger small"></span>
    <small class="text-muted d-block mt-1">
        Ajuda a priorizar o que comprar primeiro
    </small>
</div>
```

**Validações:**
- ✅ **Obrigatório**: Enum não-nullable
- ✅ **Padrão**: "Essencial" (selected)

**Mensagens de Erro:**
- Não selecionado: `"Selecione uma prioridade"`

**Dicas de UX:**
- 💡 **Emojis + descrição**: ✅ Essencial (Comprar prioritariamente)
- 💡 **Padrão inteligente**: "Essencial" (maioria dos items)
- 💡 **Help text**: Explica o propósito
- 💡 **Lado a lado com Categoria**: Campos relacionados

**Análise Crítica UI/UX:**
- ✅ **BOM**: Largura 50% (par com Categoria)
- ✅ **BOM**: Descrições claras
- ✅ **BOM**: Padrão "Essencial" (seguro)
- ✅ **EXCELENTE**: Help text contextual

---

### **GRUPO 3: Planeamento Financeiro**

#### **3.1 Quantidade*** (Obrigatório)
**Tipo**: Number Input  
**Propriedade**: `BabyListItem.Quantity`  
**Tamanho**: 33% desktop (`col-md-4`)

**HTML**:
```html
<div class="col-md-4 mb-3">
    <label asp-for="Quantity" class="form-label fw-semibold">
        Quantidade <span class="text-danger">*</span>
    </label>
    <input asp-for="Quantity" type="number" class="form-control" 
           min="1" max="100" value="1" required />
    <span asp-validation-for="Quantity" class="text-danger small"></span>
    <small class="text-muted">Min: 1 | Máx: 100</small>
</div>
```

**Validações:**
- ✅ **Obrigatório**: Não pode estar vazio
- ✅ **Range**: 1 a 100 `[Range(1, 100)]`
- ✅ **Client-side**: HTML5 `min="1" max="100"`
- ✅ **Padrão**: 1

**Mensagens de Erro:**
- Vazio: `"A quantidade é obrigatória"`
- Menor que 1: `"A quantidade deve ser pelo menos 1"`
- Maior que 100: `"A quantidade não pode exceder 100"`

**Dicas de UX:**
- 💡 **Tipo number**: Teclado numérico no mobile
- 💡 **Setas**: Incremento fácil (arrows)
- 💡 **Largura pequena**: 33% (número curto)
- 💡 **Limites visíveis**: "Min: 1 | Máx: 100"

**Análise Crítica UI/UX:**
- ✅ **EXCELENTE**: Largura apropriada (não 100%)
- ✅ **BOM**: Padrão = 1 (comum)
- ✅ **BOM**: Limites claros
- ⚠️ **MELHORAR**: Spinners mais visíveis?
  ```css
  input[type="number"]::-webkit-inner-spin-button {
      opacity: 1;
      height: 30px;
  }
  ```

---

#### **3.2 Custo Estimado*** (Obrigatório)
**Tipo**: Number Input (Decimal)  
**Propriedade**: `BabyListItem.EstimatedCost`  
**Tamanho**: 33% desktop (`col-md-4`)

**HTML**:
```html
<div class="col-md-4 mb-3">
    <label asp-for="EstimatedCost" class="form-label fw-semibold">
        Custo Estimado (€) <span class="text-danger">*</span>
    </label>
    <div class="input-group">
        <span class="input-group-text">€</span>
        <input asp-for="EstimatedCost" type="number" class="form-control" 
               step="0.01" min="0" max="10000" placeholder="0.00" required />
    </div>
    <span asp-validation-for="EstimatedCost" class="text-danger small"></span>
    <small class="text-muted">Ex: 49.99</small>
</div>
```

**Validações:**
- ✅ **Obrigatório**: Não pode estar vazio
- ✅ **Range**: €0 a €10,000 `[Range(0, 10000)]`
- ✅ **Decimais**: Até 2 casas (`step="0.01"`)

**Mensagens de Erro:**
- Vazio: `"O custo estimado é obrigatório"`
- Negativo: `"O custo não pode ser negativo"`
- Muito alto: `"O custo não pode exceder €10,000"`

**Dicas de UX:**
- 💡 **Símbolo €**: Input group prefix
- 💡 **Step 0.01**: Permite cêntimos
- 💡 **Placeholder**: "0.00" (formato esperado)
- 💡 **Largura média**: 33% (valores até €10k)

**Análise Crítica UI/UX:**
- ✅ **EXCELENTE**: Largura apropriada
- ✅ **EXCELENTE**: Símbolo € visível
- ✅ **BOM**: Placeholder mostra formato
- ⚠️ **MELHORAR**: Formatar automaticamente?
  ```javascript
  input.addEventListener('blur', function() {
      this.value = parseFloat(this.value).toFixed(2);
  });
  ```

---

#### **3.3 Quando Comprar** (Opcional)
**Tipo**: Select/Dropdown  
**Propriedade**: `BabyListItem.RecommendedTiming` (enum `PurchaseTiming`)  
**Tamanho**: 33% desktop (`col-md-4`)

**HTML**:
```html
<div class="col-md-4 mb-3">
    <label asp-for="RecommendedTiming" class="form-label fw-semibold">
        Quando Comprar <span class="text-muted small">(opcional)</span>
    </label>
    <select asp-for="RecommendedTiming" class="form-select">
        <option value="">-- Quando necessário --</option>
        <option value="1">Gravidez (até 6 meses)</option>
        <option value="2">7º Mês</option>
        <option value="3" selected>8º Mês</option>
        <option value="4">Antes do Nascimento</option>
        <option value="5">Após Nascimento</option>
        <option value="6">Com 3 Meses</option>
        <option value="7">Com 6 Meses</option>
        <option value="8">Com 9 Meses</option>
        <option value="9">Quando Necessário</option>
    </select>
    <small class="text-muted">
        Recomendação de quando adquirir este item
    </small>
</div>
```

**Validações:**
- ❌ **Não obrigatório** (nullable enum)

**Dicas de UX:**
- 💡 **Padrão inteligente**: "8º Mês" (timing comum)
- 💡 **Opções lógicas**: Progressão temporal
- 💡 **Help text**: Explica o propósito
- 💡 **Agrupado**: Com Qtd e Custo (planeamento)

**Análise Crítica UI/UX:**
- ✅ **BOM**: Largura 33% (completa a linha)
- ✅ **BOM**: Opcional (nem tudo tem timing específico)
- ✅ **BOM**: Opções cronológicas
- ⚠️ **MELHORAR**: Filtrar por categoria?
  - Ex: Roupa 0-3M → Sugerir "7º ou 8º Mês"
  - Ex: Carrinho → Sugerir "Antes do Nascimento"

---

### **GRUPO 4: Informações Adicionais**

#### **4.1 Link do Produto** (Opcional)
**Tipo**: URL Input  
**Propriedade**: `BabyListItem.ProductUrl`  
**Tamanho**: Full-width (`col-12`)

**HTML**:
```html
<div class="mb-3">
    <label asp-for="ProductUrl" class="form-label fw-semibold">
        Link do Produto <span class="text-muted small">(opcional)</span>
    </label>
    <div class="input-group">
        <span class="input-group-text">
            <i class="bi bi-link-45deg"></i>
        </span>
        <input asp-for="ProductUrl" type="url" class="form-control" 
               placeholder="https://www.loja.com/produto" />
    </div>
    <span asp-validation-for="ProductUrl" class="text-danger small"></span>
    <small class="text-muted">
        Link para o produto numa loja online (ex: Amazon, Chicco, Prenatal)
    </small>
</div>
```

**Validações:**
- ❌ **Não obrigatório**
- ✅ **Formato URL**: HTML5 `type="url"`
- ✅ **Comprimento**: Máximo 500 caracteres

**Mensagens de Erro:**
- Formato inválido: `"URL inválido. Use: https://exemplo.com"`

**Dicas de UX:**
- 💡 **Ícone de link**: Visual claro
- 💡 **Tipo URL**: Validação automática
- 💡 **Placeholder**: Mostra formato esperado
- 💡 **Help text**: Dá exemplos de lojas

**Análise Crítica UI/UX:**
- ✅ **BOM**: Full-width (URLs podem ser longas)
- ✅ **BOM**: Ícone ajuda a identificar
- ✅ **BOM**: Opcional (nem tudo está online)
- ⚠️ **MELHORAR**: Validar domínios conhecidos?
  ```javascript
  const validDomains = ['amazon', 'chicco', 'prenatal'];
  if (validDomains.some(d => url.includes(d))) {
      // Mostrar ícone da loja
  }
  ```

---

#### **4.2 Notas** (Opcional)
**Tipo**: Textarea  
**Propriedade**: `BabyListItem.Notes`  
**Tamanho**: Full-width (`col-12`)

**HTML**:
```html
<div class="mb-0">
    <label asp-for="Notes" class="form-label fw-semibold">
        Notas <span class="text-muted small">(opcional)</span>
    </label>
    <textarea asp-for="Notes" class="form-control" rows="2" 
              maxlength="1000"
              placeholder="Observações adicionais, preferências, alternativas..."></textarea>
    <small class="text-muted">Máximo 1000 caracteres</small>
</div>
```

**Validações:**
- ❌ **Não obrigatório**
- ✅ **Comprimento**: Máximo 1000 caracteres

**Dicas de UX:**
- 💡 **Altura menor**: 2 rows (menos importante)
- 💡 **Placeholder sugestivo**: Dá ideias do que escrever
- 💡 **Último campo**: Lógico (info extra)

**Análise Crítica UI/UX:**
- ✅ **BOM**: Opcional (não bloqueia)
- ✅ **BOM**: Altura apropriada (não domina)
- ⚠️ **MELHORAR**: Auto-expand?

---

## 🎬 Botões de Ação

### **Botão 1: ✅ Adicionar Item** (Primary)
**HTML**:
```html
<button type="submit" class="btn btn-primary btn-lg px-5">
    <i class="bi bi-plus-circle-fill"></i> Adicionar Item
</button>
```

**Ação ao Clicar:**
1. **Validação Client-Side**
   - Campos obrigatórios: Name, Category, Priority, Quantity, EstimatedCost
   - Formatos: URL válido (se preenchido)
   - Ranges: Quantidade 1-100, Custo 0-10000

2. **Submit POST** para `/BabyLists/AddItem`

3. **Processamento (Controller)**
   ```csharp
   if (ModelState.IsValid)
   {
       item.CreatedDate = DateTime.UtcNow;
       item.CreatedBy = userEmail;
       _context.Add(item);
       await _context.SaveChangesAsync();
       
       TempData["Success"] = $"✅ Item '{item.Name}' adicionado!";
       return RedirectToAction("Manage", new { id = item.BabyListId });
   }
   ```

4. **Redirecionamento**
   - Sucesso: Volta para `Manage` com mensagem de sucesso
   - Erro: Permanece na página com erros visíveis

**Estados:**
- Normal: Verde com gradiente
- Hover: Gradiente mais escuro + lift
- Loading: Spinner + "A adicionar..."
- Disabled: Opacidade 60%

---

### **Botão 2: Cancelar** (Secondary)
**HTML**:
```html
<a asp-action="Manage" asp-route-id="@ViewBag.ListId" 
   class="btn btn-outline-secondary btn-lg">
    <i class="bi bi-x-circle"></i> Cancelar
</a>
```

**Ação ao Clicar:**
1. Navegação para `/BabyLists/Manage/{listId}`
2. **SEM salvar** dados do formulário
3. **COM confirmação** se form preenchido (JavaScript)

**JavaScript de Confirmação:**
```javascript
cancelBtn.addEventListener('click', function(e) {
    const hasData = form.querySelector('input[name="Name"]').value.trim() !== '';
    if (hasData) {
        if (!confirm('Dados não guardados serão perdidos. Continuar?')) {
            e.preventDefault();
        }
    }
});
```

---

## 🎨 Melhorias de UI/UX Implementadas

### **✅ Agrupamento em Cards**
- Card 1: Informação Básica (Nome, Descrição)
- Card 2: Categorização (Categoria, Prioridade)
- Card 3: Planeamento (Qtd, Custo, Timing)
- Card 4: Adicionais (Link, Notas)

### **✅ Layout Responsivo Inteligente**
```css
/* Desktop: 3 colunas */
.col-md-4 { /* Qtd, Custo, Timing - 33% cada */ }

/* Tablet: 2 colunas */
@media (min-width: 768px) and (max-width: 991px) {
    .col-md-4 { width: 50%; }
}

/* Mobile: 1 coluna */
@media (max-width: 767px) {
    .col-md-4 { width: 100%; }
}
```

### **✅ Validação Visual Clara**
```css
/* Campo inválido */
.form-control.is-invalid {
    border-color: #dc3545;
    background-image: url(...erro icon...);
}

/* Campo válido */
.form-control.is-valid {
    border-color: #28a745;
    background-image: url(...check icon...);
}
```

### **✅ Feedback Contextual**
- Help text em todos os campos complexos
- Mensagens de erro específicas (não genéricas)
- Placeholders sugestivos
- Contadores de caracteres

---

## 📊 Fluxo Completo

```mermaid
graph TD
    A[Manage] -->|Clicar "Adicionar Item"| B[AddItem Form]
    B -->|Preencher| C{Validação}
    C -->|Erro| D[Mostrar erros]
    D --> B
    C -->|Sucesso| E[Criar BabyListItem]
    E --> F[Salvar BD]
    F --> G[Manage + Feedback]
    G -->|TempData Success| H[Ver item na lista]
```

---

## 🧪 Cenários de Teste

### **Cenário 1: Item Essencial Completo**
- Nome: "Berço de madeira 120x60cm"
- Descrição: "Cor branca, com colchão incluído"
- Categoria: Quarto do Bebé
- Prioridade: Essencial
- Quantidade: 1
- Custo: 299.99
- Timing: 8º Mês
- Link: "https://www.chicco.pt/berco-next2me"
- Notas: "Verificar se cabe no quarto"
- ✅ **Resultado**: Item criado com sucesso

### **Cenário 2: Item Mínimo (só obrigatórios)**
- Nome: "Pack 5 bodies"
- Categoria: Roupa 0-3M
- Prioridade: Essencial
- Quantidade: 1
- Custo: 19.99
- ✅ **Resultado**: Item criado (resto null)

### **Cenário 3: Validação de Erros**
- Nome: "" (vazio)
- Clicar "Adicionar"
- ✅ **Resultado**: Erro "O nome do item é obrigatório"

---

**Navegação:**
- [← Gerir Lista](03-Manage-List.md)
- [Editar Item →](05-Edit-Item.md)
