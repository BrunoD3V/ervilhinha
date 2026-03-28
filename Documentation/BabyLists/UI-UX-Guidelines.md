# 🎨 Guia de UI/UX - Sistema de Listas do Bebé

## 🎯 Princípios de Design

### **1. Clareza acima de tudo**
> "Um utilizador não deve precisar de pensar"

- ✅ **Labels descritivos**: "Nome da Lista" (não "Nome")
- ✅ **Placeholders sugestivos**: "Ex: Enxoval da Maria..."
- ✅ **Help text contextual**: Explicar o que cada campo faz
- ✅ **Feedback imediato**: Validação em tempo real

### **2. Agrupamento Lógico**
> "Campos relacionados devem estar juntos"

**ANTES (❌ Evitar):**
```
Nome da Lista
Tipo de Lista
Nome do Bebé
Descrição
Data Prevista
Ativar Partilha
```

**DEPOIS (✅ Melhor):**
```
[Card: Informações Básicas]
  - Nome da Lista
  - Tipo de Lista
  - Descrição

[Card: Informações do Bebé]
  - Nome do Bebé | Data Prevista (lado a lado)

[Card: Opções de Partilha]
  - Ativar Partilha
  - (Se ativo) Opções adicionais
```

### **3. Layout Responsivo Inteligente**
> "Não desperdiçar espaço, mas também não comprimir"

#### **Desktop (≥992px)**
```css
/* ❌ EVITAR - Campo de data full-width */
<div class="col-12">
    <input type="date" class="form-control" /> /* Ocupa 100%! */
</div>

/* ✅ MELHOR - Largura apropriada */
<div class="col-md-6">
    <input type="date" class="form-control" /> /* Ocupa 50% */
</div>
```

#### **Regras de Largura:**
- **Text curto** (Nome, Email): `col-md-6` (50%)
- **Text médio** (Descrição): `col-12` (100%)
- **Número/Data**: `col-md-4` ou `col-md-6` (33-50%)
- **Select**: `col-md-6` ou `col-12` (50-100%)
- **Textarea**: Sempre `col-12` (100%)

### **4. Hierarquia Visual**
> "O utilizador deve saber onde olhar primeiro"

#### **Tamanhos de Fonte:**
```css
H1 (Título da Página): 2.5rem (40px)
H2 (Card Headers): 1.5rem (24px)
H3 (Sub-secções): 1.25rem (20px)
Labels: 1rem (16px) - font-weight: 600
Body Text: 1rem (16px)
Help Text: 0.875rem (14px)
```

#### **Pesos:**
- **Títulos**: `fw-bold` (700)
- **Labels**: `fw-semibold` (600)
- **Body**: `fw-normal` (400)
- **Help Text**: `fw-light` (300)

### **5. Estados de Interação**
> "Feedback visual em cada ação"

#### **Estados de Input:**
```css
/* Normal */
.form-control {
    border: 1px solid #dee2e6;
    background: white;
}

/* Focus */
.form-control:focus {
    border-color: #A5D6A7; /* Pea green */
    box-shadow: 0 0 0 0.25rem rgba(165, 214, 167, 0.25);
}

/* Invalid */
.form-control.is-invalid {
    border-color: #dc3545; /* Red */
    background-image: url(...); /* Error icon */
}

/* Valid */
.form-control.is-valid {
    border-color: #28a745; /* Green */
    background-image: url(...); /* Check icon */
}
```

#### **Estados de Botão:**
```css
/* Primary Button */
.btn-primary {
    background: linear-gradient(135deg, #A5D6A7, #81C784);
}

.btn-primary:hover {
    background: linear-gradient(135deg, #81C784, #66BB6A);
    transform: translateY(-2px);
    box-shadow: 0 4px 8px rgba(0,0,0,0.2);
}

.btn-primary:active {
    transform: translateY(0);
}

.btn-primary:disabled {
    opacity: 0.6;
    cursor: not-allowed;
}
```

---

## 📐 Layouts Padrão

### **Layout 1: Form Simples (1 coluna)**
**Uso**: Formulários básicos, wizards

```html
<div class="row justify-content-center">
    <div class="col-lg-6 col-xl-5">
        <form>
            <div class="mb-3">
                <label>Campo 1</label>
                <input class="form-control" />
            </div>
            <div class="mb-3">
                <label>Campo 2</label>
                <input class="form-control" />
            </div>
        </form>
    </div>
</div>
```

**Largura recomendada**: `col-lg-6 col-xl-5` (50% em large, 41% em xlarge)

---

### **Layout 2: Form Médio (2 colunas)**
**Uso**: Forms com campos relacionados

```html
<div class="row justify-content-center">
    <div class="col-lg-10 col-xl-9">
        <form>
            <div class="row">
                <div class="col-md-6 mb-3">
                    <label>Campo A</label>
                    <input class="form-control" />
                </div>
                <div class="col-md-6 mb-3">
                    <label>Campo B</label>
                    <input class="form-control" />
                </div>
            </div>
        </form>
    </div>
</div>
```

**Largura recomendada**: `col-lg-10 col-xl-9` (83-75%)

---

### **Layout 3: Dashboard (Sidebar + Main)**
**Uso**: Páginas de gestão/manage

```html
<div class="row">
    <div class="col-lg-8">
        <!-- Conteúdo principal -->
        <div class="card">...</div>
    </div>
    <div class="col-lg-4">
        <!-- Sidebar de ações -->
        <div class="card sticky-top">...</div>
    </div>
</div>
```

**Proporção recomendada**: 8-4 (66%-33%) ou 9-3 (75%-25%)

---

### **Layout 4: Grid de Cards**
**Uso**: Listas de items, galerias

```html
<div class="row">
    <div class="col-sm-6 col-md-4 col-lg-3">
        <div class="card">...</div>
    </div>
    <!-- Repetir -->
</div>
```

**Breakpoints**:
- **SM (≥576px)**: 2 colunas (`col-sm-6`)
- **MD (≥768px)**: 3 colunas (`col-md-4`)
- **LG (≥992px)**: 4 colunas (`col-lg-3`)

---

## ✨ Componentes Reutilizáveis

### **1. Card com Header Gradiente**
```html
<div class="card shadow mb-4">
    <div class="card-header bg-gradient-pea">
        <h5 class="mb-0">📝 Título da Secção</h5>
    </div>
    <div class="card-body">
        <!-- Conteúdo -->
    </div>
</div>
```

**CSS:**
```css
.bg-gradient-pea {
    background: linear-gradient(135deg, #E8F5E9 0%, #C8E6C9 100%);
}

.bg-gradient-pink {
    background: linear-gradient(135deg, #FCE4EC 0%, #F8BBD0 100%);
}

.bg-gradient-blue {
    background: linear-gradient(135deg, #E3F2FD 0%, #BBDEFB 100%);
}

.bg-gradient-yellow {
    background: linear-gradient(135deg, #FFF9C4 0%, #FFF59D 100%);
}
```

---

### **2. Toggle Switch Grande**
```html
<div class="form-check form-switch form-switch-lg">
    <input class="form-check-input" type="checkbox" 
           role="switch" id="toggleId" />
    <label class="form-check-label fw-semibold" for="toggleId">
        <span class="fs-5">🔗</span> Texto do Toggle
    </label>
</div>
```

**CSS:**
```css
.form-switch-lg .form-check-input {
    width: 3rem;
    height: 1.5rem;
}
```

---

### **3. Alert Informativo**
```html
<div class="alert alert-primary" role="alert">
    <div class="d-flex align-items-start">
        <i class="bi bi-info-circle-fill me-2 fs-5"></i>
        <div>
            <strong>Título do Aviso</strong>
            <p class="mb-0 small mt-1">
                Texto explicativo...
            </p>
        </div>
    </div>
</div>
```

**Variações**:
- `alert-primary`: Info (azul)
- `alert-success`: Sucesso (verde)
- `alert-warning`: Aviso (amarelo)
- `alert-danger`: Erro (vermelho)

---

### **4. Botão com Loading State**
```html
<button type="submit" class="btn btn-primary" id="submitBtn">
    <i class="bi bi-check-circle-fill"></i> Criar Lista
</button>
```

**JavaScript:**
```javascript
form.addEventListener('submit', function() {
    const btn = document.getElementById('submitBtn');
    btn.disabled = true;
    btn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span> A processar...';
});
```

---

### **5. Campo com Ícone e Help Text**
```html
<div class="mb-3">
    <label class="form-label fw-semibold">
        Nome do Campo <span class="text-danger">*</span>
    </label>
    <div class="input-group">
        <span class="input-group-text">
            <i class="bi bi-person"></i>
        </span>
        <input class="form-control" placeholder="..." />
    </div>
    <small class="text-muted d-block mt-1">
        <i class="bi bi-info-circle"></i> Texto de ajuda
    </small>
</div>
```

---

## 🎨 Paleta de Cores "Little Pea"

### **Cores Primárias**
```css
--pea-green-light: #E8F5E9;
--pea-green: #C8E6C9;
--pea-green-medium: #A5D6A7;
--pea-green-dark: #81C784;

--baby-pink-light: #FCE4EC;
--baby-pink: #F8BBD0;

--baby-blue-light: #E3F2FD;
--baby-blue: #BBDEFB;

--baby-yellow-light: #FFF9C4;
--baby-yellow: #FFF59D;

--baby-peach: #FFCCBC;
```

### **Uso das Cores**
| Elemento | Cor | Código |
|---|---|---|
| **Botão Primary** | Pea Green Gradient | `#A5D6A7` → `#81C784` |
| **Botão Success** | Verde Suave | `#81C784` |
| **Botão Info** | Baby Blue | `#64B5F6` |
| **Botão Warning** | Baby Yellow | `#FFD54F` |
| **Botão Danger** | Coral Pink | `#EF9A9A` |
| **Badge Essencial** | Coral Pink | `#EF9A9A` |
| **Badge Recomendado** | Baby Blue | `#64B5F6` |
| **Badge Opcional** | Cinzento Suave | `#BDBDBD` |
| **Card Header** | Pea Green Light | `#E8F5E9` |

---

## 🔤 Tipografia

### **Fonte Principal**
```css
font-family: 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
```

### **Tamanhos e Pesos**
```css
/* Títulos */
.page-title {
    font-size: 2.5rem;
    font-weight: 700;
    color: #2E7D32;
}

/* Labels */
.form-label {
    font-size: 1rem;
    font-weight: 600;
    margin-bottom: 0.5rem;
}

/* Help Text */
.text-muted,
small {
    font-size: 0.875rem;
    font-weight: 400;
    color: #6c757d;
}

/* Required Asterisk */
.text-danger {
    color: #dc3545;
    font-weight: 700;
}
```

---

## 📏 Espaçamento

### **Margens e Paddings**
```css
/* Espaçamento entre cards */
.card + .card {
    margin-top: 1.5rem; /* 24px */
}

/* Espaçamento entre campos */
.mb-3 {
    margin-bottom: 1rem; /* 16px */
}

.mb-4 {
    margin-bottom: 1.5rem; /* 24px */
}

/* Padding interno de cards */
.card-body {
    padding: 1.5rem; /* 24px */
}

/* Gap entre botões */
.d-flex.gap-2 {
    gap: 0.5rem; /* 8px */
}

.d-flex.gap-3 {
    gap: 1rem; /* 16px */
}
```

### **Regra Geral**
- **Muito próximo**: 0.5rem (8px)
- **Próximo**: 1rem (16px)
- **Normal**: 1.5rem (24px)
- **Espaçado**: 2rem (32px)
- **Muito espaçado**: 3rem (48px)

---

## ♿ Acessibilidade (WCAG 2.1 AA)

### **Contraste de Cores**
**Mínimo**: 4.5:1 para texto normal  
**Mínimo**: 3:1 para texto grande (≥18px ou ≥14px bold)

**Verificar**:
- ✅ Pea Green (#A5D6A7) em texto branco: ✅ 1.5:1 (FALHA - usar texto escuro)
- ✅ Pea Green Dark (#81C784) em texto branco: ✅ 2:1 (FALHA - usar texto escuro)
- ✅ Texto escuro (#2E7D32) em branco: ✅ 8:1 (PASSA)

### **Navegação por Teclado**
```html
<!-- Tab order lógico -->
<input tabindex="1" />
<input tabindex="2" />
<button tabindex="3">Submit</button>
```

### **ARIA Labels**
```html
<!-- Switch com role -->
<input type="checkbox" role="switch" 
       aria-label="Ativar partilha da lista" />

<!-- Botão com estado -->
<button aria-pressed="false">Toggle</button>

<!-- Alert com role -->
<div role="alert" aria-live="assertive">
    Mensagem de erro
</div>
```

### **Focus States Visíveis**
```css
*:focus {
    outline: 2px solid #A5D6A7;
    outline-offset: 2px;
}

.btn:focus {
    box-shadow: 0 0 0 0.25rem rgba(165, 214, 167, 0.5);
}
```

---

## 📱 Responsividade

### **Breakpoints Bootstrap 5**
```css
/* Extra Small (mobile) */
@media (max-width: 575.98px) { }

/* Small (landscape phones) */
@media (min-width: 576px) and (max-width: 767.98px) { }

/* Medium (tablets) */
@media (min-width: 768px) and (max-width: 991.98px) { }

/* Large (desktops) */
@media (min-width: 992px) and (max-width: 1199.98px) { }

/* Extra Large (large desktops) */
@media (min-width: 1200px) { }
```

### **Adaptações Mobile**
```css
/* Mobile-first: Padrão para mobile */
.btn {
    width: 100%;
    font-size: 1.125rem; /* Maior para touch */
    padding: 0.75rem 1.5rem; /* Área maior */
}

/* Desktop: Largura automática */
@media (min-width: 768px) {
    .btn {
        width: auto;
        font-size: 1rem;
        padding: 0.5rem 1rem;
    }
}
```

---

## ⚡ Performance e Otimização

### **Lazy Loading de Imagens**
```html
<img src="placeholder.jpg" 
     data-src="imagem-real.jpg" 
     loading="lazy" 
     alt="Descrição" />
```

### **Minificação de CSS**
```bash
# Produção
npm run build:css
```

### **Debounce em Inputs**
```javascript
// Evitar validação em cada tecla
let timeout;
input.addEventListener('input', function() {
    clearTimeout(timeout);
    timeout = setTimeout(() => {
        validate(this.value);
    }, 500); // 500ms delay
});
```

---

## 🧪 Checklist de UI/UX

### **Antes de Commit**
- [ ] Campos obrigatórios marcados com `*`
- [ ] Validações client-side funcionam
- [ ] Mensagens de erro claras em PT
- [ ] Layout responsivo testado (mobile/tablet/desktop)
- [ ] Botões têm feedback visual (hover/focus/active)
- [ ] Loading states implementados
- [ ] Help text em campos complexos
- [ ] Tab order lógico
- [ ] Contraste de cores adequado (WCAG AA)
- [ ] Testado com screen reader (NVDA/JAWS)

### **Antes de Deploy**
- [ ] Todos os formulários validados (client + server)
- [ ] Erros de API tratados com mensagens amigáveis
- [ ] Performance testada (Lighthouse score >90)
- [ ] Cross-browser testado (Chrome, Firefox, Safari, Edge)
- [ ] Mobile testado em dispositivos reais
- [ ] Animações suaves (<16ms)
- [ ] Imagens otimizadas (<100KB)
- [ ] CSS/JS minificados

---

## 📚 Recursos

### **Ferramentas de Design**
- [Figma](https://figma.com) - Prototipagem
- [Coolors](https://coolors.co) - Paletas de cores
- [WebAIM Contrast Checker](https://webaim.org/resources/contrastchecker/) - Contraste WCAG

### **Ícones**
- [Bootstrap Icons](https://icons.getbootstrap.com/)
- [Emojis Unicode](https://unicode.org/emoji/charts/full-emoji-list.html)

### **Validação**
- [W3C HTML Validator](https://validator.w3.org/)
- [WAVE Web Accessibility](https://wave.webaim.org/)
- [Lighthouse](https://developers.google.com/web/tools/lighthouse)

---

**Navegação:**
- [← Visão Geral](01-Overview.md)
- [Criar Lista →](02-Create-List.md)
