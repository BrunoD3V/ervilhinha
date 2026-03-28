# ➕ Criar Nova Lista - Documentação Completa

## 📍 Localização
**URL**: `/BabyLists/Create`  
**Autenticação**: Obrigatória  
**Permissões**: Utilizador autenticado

---

## 🎯 Objetivo da Página

Permite ao utilizador criar uma nova lista do bebé, escolhendo entre:
- **🛒 Enxoval**: Para planeamento pessoal de compras
- **🎁 Lista de Presentes**: Para partilhar com família/amigos
- **📋 Lista Geral**: Para uso misto

---

## 📋 Estrutura do Formulário

### **Layout Desktop (≥768px)**
```
┌─────────────────────────────────────────┐
│  ➕ Nova Lista do Bebé                  │
├─────────────────────────────────────────┤
│  [Informações Básicas - Card]           │
│    ┌──────────────────────────────┐     │
│    │ Nome da Lista*     [______]  │     │
│    │ Tipo de Lista*     [▼____]   │     │
│    │ Descrição          [______]  │     │
│    │                    [______]  │     │
│    └──────────────────────────────┘     │
│                                          │
│  [Informações do Bebé - Card]            │
│    ┌─────────────┬─────────────┐        │
│    │ Nome Bebé   │ Data Prev.  │        │
│    │ [________]  │ [__/__/____]│        │
│    └─────────────┴─────────────┘        │
│                                          │
│  [Opções de Partilha - Card]             │
│    ┌──────────────────────────────┐     │
│    │ ☐ Ativar Partilha            │     │
│    │                              │     │
│    │ [Se ativo: mostra opções]   │     │
│    │   ☐ Lista Pública            │     │
│    │   Co-Gestor: [__________]    │     │
│    └──────────────────────────────┘     │
│                                          │
│  [✅ Criar Lista] [Cancelar]             │
└─────────────────────────────────────────┘
```

### **Layout Mobile (<768px)**
```
┌───────────────────┐
│ ➕ Nova Lista     │
├───────────────────┤
│ Nome*             │
│ [______________]  │
│                   │
│ Tipo*             │
│ [▼_____________]  │
│                   │
│ Descrição         │
│ [______________]  │
│ [______________]  │
│                   │
│ Nome do Bebé      │
│ [______________]  │
│                   │
│ Data Prevista     │
│ [__/__/____]      │
│                   │
│ ☐ Ativar Partilha │
│                   │
│ [✅ Criar Lista]  │
│ [Cancelar]        │
└───────────────────┘
```

---

## 📝 Campos do Formulário

### **GRUPO 1: Informações Básicas**

#### **1.1 Nome da Lista*** 
**Tipo**: Text Input  
**Propriedade**: `BabyList.Name`  
**HTML**:
```html
<input asp-for="Name" class="form-control" 
       placeholder="Ex: Enxoval da Maria, Lista de Presentes para o João..." 
       autofocus required maxlength="100" />
```

**Validações:**
- ✅ **Obrigatório**: `[Required(ErrorMessage = "O nome da lista é obrigatório")]`
- ✅ **Comprimento**: Máximo 100 caracteres `[StringLength(100)]`
- ✅ **Client-side**: HTML5 `required` + `maxlength`
- ✅ **Server-side**: DataAnnotations

**Mensagens de Erro:**
- Vazio: `"O nome da lista é obrigatório"`
- Muito longo: `"O nome não pode ter mais de 100 caracteres"`

**Dicas de UX:**
- 💡 Placeholder sugestivo com exemplos
- 💡 Autofocus para começar imediatamente
- 💡 Largura full no mobile, 100% no desktop (col-12)

**Exemplos Válidos:**
- ✅ "Enxoval da Maria"
- ✅ "Presentes para o Bebé João"
- ✅ "Lista Geral - Agosto 2026"

**Exemplos Inválidos:**
- ❌ "" (vazio)
- ❌ "Lista" + 96 caracteres (>100)

---

#### **1.2 Tipo de Lista***
**Tipo**: Select/Dropdown  
**Propriedade**: `BabyList.Type` (enum `ListType`)  
**HTML**:
```html
<select asp-for="Type" class="form-select" required>
    <option value="">-- Selecione o tipo --</option>
    <option value="1" selected>🛒 Enxoval (Planeamento Pessoal)</option>
    <option value="2">🎁 Lista de Presentes (Partilhável)</option>
    <option value="3">📋 Lista Geral</option>
</select>
```

**Validações:**
- ✅ **Obrigatório**: Implícito (enum não-nullable)
- ✅ **Valores válidos**: 1 (Enxoval), 2 (Presentes), 3 (Geral)

**Mensagens de Erro:**
- Não selecionado: `"Selecione um tipo de lista"`

**Dicas de UX:**
- 💡 Emojis para identificação visual rápida
- 💡 Descrição clara entre parênteses
- 💡 "Enxoval" como padrão (selected)
- 💡 Help text abaixo explicando diferenças:
  ```html
  <small class="text-muted">
    <strong>Enxoval:</strong> Para o teu planeamento de compras<br>
    <strong>Presentes:</strong> Para partilhar com família/amigos
  </small>
  ```

**Comportamento:**
- Se selecionar "Presentes", sugerir ativar partilha (highlight no toggle)

---

#### **1.3 Descrição**
**Tipo**: Textarea  
**Propriedade**: `BabyList.Description` (nullable)  
**HTML**:
```html
<textarea asp-for="Description" class="form-control" rows="3" 
          maxlength="500"
          placeholder="Descrição opcional da lista...">
</textarea>
<small class="text-muted">Opcional • Máximo 500 caracteres</small>
```

**Validações:**
- ❌ **Não obrigatório**
- ✅ **Comprimento**: Máximo 500 caracteres `[StringLength(500)]`

**Mensagens de Erro:**
- Muito longo: `"A descrição não pode ter mais de 500 caracteres"`

**Dicas de UX:**
- 💡 Placeholder com exemplo
- 💡 Contador de caracteres (JavaScript opcional)
- 💡 Altura fixa (3 rows) para consistência

**Exemplos Válidos:**
- ✅ "" (vazio - é opcional)
- ✅ "Lista de items essenciais para o primeiro mês do bebé"
- ✅ "Presentes para o João - nascimento previsto para Outubro 2026"

---

### **GRUPO 2: Informações do Bebé**

#### **2.1 Nome do Bebé**
**Tipo**: Text Input  
**Propriedade**: `BabyList.BabyName` (nullable)  
**HTML**:
```html
<div class="col-md-6">
    <label asp-for="BabyName" class="form-label">Nome do Bebé</label>
    <input asp-for="BabyName" class="form-control" 
           maxlength="100"
           placeholder="Ex: Maria, João (opcional)" />
    <small class="text-muted">Opcional</small>
</div>
```

**Validações:**
- ❌ **Não obrigatório**
- ✅ **Comprimento**: Máximo 100 caracteres `[StringLength(100)]`

**Mensagens de Erro:**
- Muito longo: `"O nome não pode ter mais de 100 caracteres"`

**Dicas de UX:**
- 💡 Layout em coluna (col-md-6) no desktop
- 💡 Agrupa com "Data Prevista" (campos relacionados)
- 💡 Indicação clara de "opcional"

**Exemplos Válidos:**
- ✅ "" (vazio)
- ✅ "Maria"
- ✅ "João Pedro"

---

#### **2.2 Data Prevista**
**Tipo**: Date Input  
**Propriedade**: `BabyList.ExpectedDate` (nullable)  
**HTML**:
```html
<div class="col-md-6">
    <label asp-for="ExpectedDate" class="form-label">Data Prevista de Nascimento</label>
    <input asp-for="ExpectedDate" type="date" class="form-control" 
           min="@DateTime.Now.ToString("yyyy-MM-dd")" />
    <small class="text-muted">Opcional</small>
</div>
```

**Validações:**
- ❌ **Não obrigatório**
- ⚠️ **Data futura**: Recomendado (HTML5 `min="hoje"`)

**Mensagens de Erro:**
- Data no passado: `"A data deve ser futura"` (validação custom)

**Dicas de UX:**
- 💡 Layout em coluna (col-md-6) - **NÃO ocupa toda a largura**
- 💡 Formato PT: `dd/MM/yyyy` (via culture)
- 💡 Min date = hoje (previne datas passadas)
- 💡 Date picker nativo do browser

**Exemplos Válidos:**
- ✅ null (vazio)
- ✅ "15/10/2026"
- ✅ "2026-12-25"

**Análise UI/UX:**
- ✅ **BOM**: Campo de data com largura apropriada (50%)
- ❌ **EVITAR**: Input de data full-width (desperdício de espaço)

---

### **GRUPO 3: Opções de Partilha**

#### **3.1 Ativar Partilha**
**Tipo**: Checkbox Toggle  
**Propriedade**: `BabyList.IsShared`  
**HTML**:
```html
<div class="card card-blue mb-3">
    <div class="card-body">
        <div class="form-check form-switch mb-3">
            <input asp-for="IsShared" class="form-check-input" 
                   id="isSharedToggle" type="checkbox" />
            <label class="form-check-label" for="isSharedToggle">
                <strong>🔗 Ativar Partilha</strong>
            </label>
        </div>
        <small class="text-muted d-block">
            Permite partilhar esta lista com família e amigos. 
            Será gerado um código único de partilha.
        </small>
    </div>
</div>
```

**Validações:**
- ❌ **Sem validação** (boolean, padrão = false)

**Comportamento JavaScript:**
```javascript
const isSharedToggle = document.getElementById('isSharedToggle');
const shareOptions = document.getElementById('shareOptions');

isSharedToggle.addEventListener('change', function() {
    shareOptions.style.display = this.checked ? 'block' : 'none';
});
```

**Dicas de UX:**
- 💡 Toggle switch (mais visual que checkbox simples)
- 💡 Card destacado (cor diferente)
- 💡 Explicação clara do que acontece
- 💡 Mostra/esconde opções condicionalmente

---

#### **3.2 Lista Pública** (Condicional)
**Tipo**: Checkbox  
**Propriedade**: `BabyList.IsPublic`  
**Condição**: Só aparece se `IsShared = true`  
**HTML**:
```html
<div id="shareOptions" style="display: none;">
    <div class="form-check mb-3">
        <input asp-for="IsPublic" class="form-check-input" 
               id="isPublicToggle" type="checkbox" />
        <label class="form-check-label" for="isPublicToggle">
            <strong>Lista Pública</strong>
        </label>
    </div>
    <small class="text-muted d-block mb-3">
        <strong>Pública:</strong> Qualquer pessoa com o link pode ver<br>
        <strong>Privada:</strong> Apenas convidados específicos
    </small>
</div>
```

**Validações:**
- ❌ **Sem validação** (boolean, padrão = false)

**Dicas de UX:**
- 💡 Explicação clara das diferenças
- 💡 Só aparece quando relevante
- 💡 Padrão = false (privado por segurança)

---

#### **3.3 Co-Gestor Email** (Condicional)
**Tipo**: Email Input  
**Propriedade**: `coManagerEmail` (parâmetro extra, não no modelo)  
**Condição**: Só aparece se `IsShared = true`  
**HTML**:
```html
<div id="shareOptions" style="display: none;">
    <label for="coManagerEmail" class="form-label">
        Co-Gestor (opcional)
    </label>
    <input type="email" name="coManagerEmail" id="coManagerEmail" 
           class="form-control" 
           placeholder="email@exemplo.com" />
    <small class="text-muted">
        Email do parceiro/parceira para gerir a lista em conjunto
    </small>
</div>
```

**Validações:**
- ❌ **Não obrigatório**
- ✅ **Formato email**: HTML5 `type="email"`
- ✅ **Não duplicar**: Não pode ser igual ao email do criador

**Mensagens de Erro:**
- Formato inválido: `"Email inválido"`
- Email igual ao criador: `"Não podes adicionar-te como co-gestor"`

**Dicas de UX:**
- 💡 Placeholder com exemplo
- 💡 Explicação do propósito
- 💡 Type="email" para validação automática

**Exemplos Válidos:**
- ✅ "" (vazio)
- ✅ "parceira@exemplo.com"

**Exemplos Inválidos:**
- ❌ "email inválido"
- ❌ (mesmo email do utilizador autenticado)

---

## 🎬 Botões de Ação

### **Botão 1: ✅ Criar Lista** (Primary)
**Tipo**: Submit Button  
**HTML**:
```html
<button type="submit" class="btn btn-primary">
    ✅ Criar Lista
</button>
```

**Ação ao Clicar:**
1. **Validação Client-Side** (HTML5 + JavaScript)
   - Campos obrigatórios preenchidos?
   - Formatos corretos?
   
2. **Submit do Form** (POST /BabyLists/Create)

3. **Validação Server-Side** (Controller)
   ```csharp
   if (ModelState.IsValid)
   {
       // Processar
   }
   ```

4. **Processamento** (Controller Action)
   - Definir `UserId` = utilizador autenticado
   - Definir `CreatedBy` = email do utilizador
   - Se `IsShared = true`:
     - Gerar `ShareCode` único
     - Criar `BabyListManager` para criador
     - Se `coManagerEmail` preenchido:
       - Criar `BabyListManager` para co-gestor
   - Salvar na base de dados

5. **Redirecionamento**
   - Sucesso: `RedirectToAction("Manage", new { id = babyList.Id })`
   - Com mensagem: `TempData["Success"] = "✅ Lista criada com sucesso!"`
   - Se partilhada: `TempData["ShareCode"] = shareCode`

**Estados do Botão:**
- Normal: `btn btn-primary`
- Hover: Gradiente mais escuro
- Loading: (opcional) Spinner + "Criando..."
- Disabled: Se form inválido

**Análise UX:**
- ✅ **BOM**: Emoji + texto claro
- ✅ **BOM**: Cor primária (ação principal)
- ✅ **BOM**: Feedback visual ao clicar

---

### **Botão 2: Cancelar** (Secondary)
**Tipo**: Link Button  
**HTML**:
```html
<a asp-action="Index" class="btn btn-outline-secondary">
    Cancelar
</a>
```

**Ação ao Clicar:**
1. **Navegação** para `/BabyLists/Index`
2. **SEM salvar** dados do formulário
3. **SEM confirmação** (dados não salvos são perdidos)

**Análise UX:**
- ✅ **BOM**: Outline (ação secundária)
- ⚠️ **MELHORAR**: Adicionar confirmação se form preenchido:
  ```javascript
  if (formHasChanges) {
      if (confirm("Tens alterações não guardadas. Continuar?")) {
          location.href = '/BabyLists/Index';
      }
  }
  ```

---

## 🎨 Melhorias de UI/UX Implementadas

### **✅ Agrupamento Lógico**
- ✅ Campos relacionados em cards separados
- ✅ Labels descritivas e claras
- ✅ Help text contextual

### **✅ Layout Responsivo**
- ✅ Desktop: Campos em colunas (2-3 por linha)
- ✅ Mobile: Single-column, full-width
- ✅ Campos de data: **50% largura** (não 100%)

### **✅ Validação Visual**
- ✅ Asterisco (*) vermelho em obrigatórios
- ✅ Borda vermelha em campos inválidos
- ✅ Mensagem de erro abaixo do campo
- ✅ Ícone de erro (❌) ao lado

### **✅ Feedback ao Utilizador**
- ✅ Placeholders sugestivos
- ✅ Help text ("Opcional", limites)
- ✅ Estados hover/focus
- ✅ Loading states

### **✅ Acessibilidade**
- ✅ Labels associados (`for="id"`)
- ✅ ARIA labels em toggles
- ✅ Tab order lógico
- ✅ Contraste WCAG AA

---

## 📊 Fluxo Completo de Criação

```mermaid
graph TD
    A[Index] -->|Clicar "Nova Lista"| B[Create Form]
    B -->|Preencher dados| C{Validação}
    C -->|Erro| D[Mostrar erros]
    D --> B
    C -->|Sucesso| E[Criar BabyList]
    E -->|IsShared?| F{Partilha Ativa?}
    F -->|Não| G[Salvar]
    F -->|Sim| H[Gerar ShareCode]
    H --> I[Criar Managers]
    I --> G
    G --> J[Manage + Feedback]
    J -->|TempData Success| K[Ver lista criada]
```

---

## 🐛 Erros Comuns e Soluções

### **Erro 1: "O nome da lista é obrigatório"**
**Causa**: Campo vazio  
**Solução**: Preencher nome da lista

### **Erro 2: "Email inválido"**
**Causa**: Co-gestor email com formato errado  
**Solução**: Usar formato `nome@dominio.com`

### **Erro 3: ShareCode não gerado**
**Causa**: `IsShared = false`  
**Solução**: Ativar toggle "Ativar Partilha"

### **Erro 4: Dados perdidos ao cancelar**
**Causa**: Clicar "Cancelar" sem guardar  
**Solução**: Adicionar confirmação JavaScript

---

## 🔍 Testes de QA

### **Cenário 1: Criar Lista Privada Simples**
1. Nome: "Enxoval da Maria"
2. Tipo: Enxoval
3. IsShared: false
4. Clicar "Criar Lista"
5. ✅ Resultado: Lista criada, redireciona para Manage

### **Cenário 2: Criar Lista Pública com Co-Gestor**
1. Nome: "Presentes para o João"
2. Tipo: Presentes
3. BabyName: "João"
4. ExpectedDate: "15/10/2026"
5. IsShared: true
6. IsPublic: true
7. CoManager: "pai@exemplo.com"
8. Clicar "Criar Lista"
9. ✅ Resultado: Lista criada, ShareCode gerado, 2 managers

### **Cenário 3: Validação de Erros**
1. Deixar nome vazio
2. Clicar "Criar Lista"
3. ✅ Resultado: Erro "O nome da lista é obrigatório"

---

## 📸 Screenshots Esperados

### **Desktop - Form Completo**
![Create Desktop](screenshots/create-desktop.png)
- 3 cards agrupados
- Campos em colunas
- Botões alinhados à esquerda

### **Mobile - Form Empilhado**
![Create Mobile](screenshots/create-mobile.png)
- Single-column
- Campos full-width
- Botões full-width

---

**Navegação:**
- [← Visão Geral](01-Overview.md)
- [Gerir Lista →](03-Manage-List.md)
