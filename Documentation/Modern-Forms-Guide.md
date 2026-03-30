# ✨ FORMULÁRIOS MODERNOS - GUIA DE USO

## 🎨 Visão Geral

Os **formulários modernos Ervilhinha** são "living on the edge" - únicos, com personalidade e animações suaves, mantendo a nossa querida paleta pastel! 🌿

---

## 🚀 Novidades

✨ **Floating Labels** - Labels que flutuam ao focar  
🎭 **Ícones Animados** - Ícones que ganham vida  
💎 **Glass Morphism** - Efeitos de vidro com blur  
⚡ **Micro-interações** - Levitação, rotação, brilho  
✓ **Validação Visual** - Checkmarks animados e shake  
🎯 **Bolhas Flutuantes** - Elementos decorativos  
✨ **Estrelas Brilhantes** - Headers decorados  

---

## 📚 CLASSES DISPONÍVEIS

### 1️⃣ **Inputs Básicos Melhorados** (AUTOMÁTICO!)

Todos os `.form-control` e `.form-select` agora têm:
- Glass morphism background
- Efeito de levitação ao focar (translateY -3px)
- Borda colorida ao focar
- Glow verde suave
- Transições suaves

```html
<!-- Uso normal Bootstrap - JÁ FICA MODERNO! -->
<div class="mb-3">
    <label class="form-label">Nome</label>
    <input type="text" class="form-control" placeholder="Digite seu nome" />
</div>
```

**Labels agora têm estrela ✨ automática!**

---

### 2️⃣ **Floating Labels System** (ULTRA MODERNO!)

Labels que **sobem** quando você foca ou digita.

```html
<div class="form-floating-modern">
    <input type="text" 
           id="name" 
           placeholder=" " 
           class="form-control" />
    <label for="name">Nome da Lista</label>
</div>
```

**⚠️ IMPORTANTE**: Use `placeholder=" "` (espaço) para a animação funcionar!

---

### 3️⃣ **Inputs com Ícones Integrados** (COM VIDA!)

Ícones que **giram** e **crescem** ao focar!

```html
<div class="input-with-icon">
    <input type="text" 
           id="babyName" 
           class="form-control" 
           placeholder="Nome do bebé" />
    <span class="input-icon">👶</span>
</div>
```

**Ícones sugeridos**:
- 👶 Nome do bebé
- 📝 Descrição
- 📅 Data
- 📧 Email
- 🏷️ Categoria
- 💰 Valor
- 🎁 Item

---

### 4️⃣ **Floating Labels + Ícones** (COMBO COMPLETO!)

```html
<div class="form-floating-modern input-with-icon">
    <input type="email" 
           id="email" 
           placeholder=" " 
           class="form-control" />
    <label for="email">Email</label>
    <span class="input-icon">📧</span>
</div>
```

---

### 5️⃣ **Cards de Formulário com Bolhas**

Cards com **bolhas flutuantes** de fundo!

```html
<div class="card form-card-modern">
    <div class="card-body">
        <div class="form-header-decorated">
            <h4>✨ Criar Nova Lista ✨</h4>
        </div>
        
        <!-- Seus inputs aqui -->
    </div>
</div>
```

**Elementos incluídos**:
- Bolha verde (canto superior direito)
- Bolha rosa (canto inferior esquerdo)
- Borda glass morphism
- Sombra profunda

---

### 6️⃣ **Headers Decorados com Estrelas**

```html
<div class="form-header-decorated">
    <h4>Criar Nova Lista</h4>
</div>
```

**Efeitos automáticos**:
- ✨ Estrelas nos lados (desktop)
- Linha decorativa abaixo
- Estrelas brilham com animação

---

### 7️⃣ **Botões Modernos com Ripple**

#### Botão Primary com onda ao clicar:
```html
<button type="submit" class="btn-modern-primary">
    💾 Guardar Lista
</button>
```

#### Botão Outline com shimmer:
```html
<button type="button" class="btn-modern-outline">
    ❌ Cancelar
</button>
```

**Container de ações**:
```html
<div class="form-actions-modern">
    <button type="button" class="btn-modern-outline">Cancelar</button>
    <button type="submit" class="btn-modern-primary">Guardar</button>
</div>
```

---

### 8️⃣ **Switch Toggle Moderno**

```html
<div class="switch-modern-container">
    <div class="switch-modern">
        <input type="checkbox" id="isPrivate" />
        <span class="switch-slider"></span>
    </div>
    <label for="isPrivate" class="switch-label">Lista Privada</label>
</div>
```

**Efeitos**:
- Bola desliza suavemente
- Glow verde ao ativar
- Bounce effect

---

### 9️⃣ **Checkbox/Radio Modernos**

#### Checkbox:
```html
<label class="form-check-modern">
    <input type="checkbox" />
    <span class="checkmark"></span>
    Aceito os termos
</label>
```

#### Radio:
```html
<label class="form-check-modern radio">
    <input type="radio" name="type" value="1" />
    <span class="checkmark"></span>
    Lista Pública
</label>
```

**Efeitos**:
- Checkmark animado com desenho progressivo
- Radio com bounce effect
- Glow ao selecionar

---

### 🔟 **File Upload Moderno**

```html
<div class="file-input-modern">
    <input type="file" id="fileUpload" accept="image/*" />
    <label for="fileUpload" class="file-input-label">
        <span class="file-input-icon">📎</span>
        <span class="file-input-text">Arraste ficheiros ou clique</span>
        <span class="file-input-hint">PNG, JPG até 5MB</span>
    </label>
</div>
```

**Efeitos**:
- Borda dashed→solid ao hover
- Ícone roda e cresce
- Levitação suave

---

### 1️⃣1️⃣ **Validação Visual Arrojada**

#### Válido:
```html
<input type="email" class="form-control is-valid" value="email@exemplo.com" />
<div class="valid-feedback">Email válido!</div>
```

**Efeitos**: ✓ Checkmark pop + glow verde pulsante

#### Inválido:
```html
<input type="email" class="form-control is-invalid" value="email" />
<div class="invalid-feedback">Email inválido</div>
```

**Efeitos**: ⚠ Shake dramático + warning pulsante

---

### 1️⃣2️⃣ **Hint Text Decorativo**

```html
<input type="text" class="form-control" />
<div class="form-hint">Use apenas letras e números</div>
```

**Ícone automático**: 💡 Lâmpada

---

### 1️⃣3️⃣ **Loading State**

```html
<input type="text" class="form-control loading" value="A carregar..." readonly />
```

**Efeito**: Spinner verde a rodar

---

### 1️⃣4️⃣ **Input Group Moderno**

```html
<div class="input-group-modern">
    <input type="number" class="form-control" placeholder="Valor" />
    <span class="input-group-text">€</span>
</div>
```

---

## 🎯 EXEMPLO COMPLETO - Modal com Tudo!

```html
<div class="modal-dialog modal-dialog-centered">
    <div class="modal-content form-card-modern">
        <div class="modal-body">
            <div class="form-header-decorated">
                <h4>Criar Nova Lista</h4>
            </div>

            <form method="post">
                <!-- Input com ícone e floating label -->
                <div class="form-floating-modern input-with-icon">
                    <input type="text" 
                           id="ListName" 
                           name="ListName" 
                           placeholder=" " 
                           class="form-control" 
                           required />
                    <label for="ListName">Nome da Lista</label>
                    <span class="input-icon">📝</span>
                </div>

                <!-- Select com ícone -->
                <div class="input-with-icon mb-3">
                    <label class="form-label">Tipo de Lista</label>
                    <select id="ListType" name="ListType" class="form-select">
                        <option value="">Escolha...</option>
                        <option value="Chá de Bebé">Chá de Bebé</option>
                        <option value="Enxoval">Enxoval</option>
                    </select>
                    <span class="input-icon">🎁</span>
                </div>

                <!-- Textarea com floating label -->
                <div class="form-floating-modern input-with-icon">
                    <textarea id="Description" 
                              name="Description" 
                              placeholder=" " 
                              class="form-control"></textarea>
                    <label for="Description">Descrição</label>
                    <span class="input-icon">💬</span>
                </div>

                <!-- Switch toggle -->
                <div class="switch-modern-container">
                    <div class="switch-modern">
                        <input type="checkbox" id="IsPrivate" name="IsPrivate" />
                        <span class="switch-slider"></span>
                    </div>
                    <label for="IsPrivate" class="switch-label">Lista Privada</label>
                </div>

                <!-- Botões modernos -->
                <div class="form-actions-modern">
                    <button type="button" class="btn-modern-outline" data-bs-dismiss="modal">
                        ❌ Cancelar
                    </button>
                    <button type="submit" class="btn-modern-primary">
                        💾 Criar Lista
                    </button>
                </div>
            </form>
        </div>
    </div>
</div>
```

---

## 🎨 COMBINAÇÕES RECOMENDADAS

### ⭐ **Nível 1 - Básico Melhorado**
Use os estilos padrão (já aplicados automaticamente):
- `.form-control`
- `.form-label`
- Labels têm ✨ automático

### ⭐⭐ **Nível 2 - Moderno**
Adicione ícones:
- `.input-with-icon` + `.input-icon`

### ⭐⭐⭐ **Nível 3 - Ultra Moderno**
Use floating labels + ícones:
- `.form-floating-modern` + `.input-with-icon`
- `.form-card-modern` nos cards

### ⭐⭐⭐⭐ **Nível 4 - LIVING ON THE EDGE!**
Combine TUDO:
- `.form-card-modern` (card com bolhas)
- `.form-header-decorated` (header com estrelas)
- `.form-floating-modern` + `.input-with-icon` (inputs completos)
- `.btn-modern-primary` (botões com ripple)
- `.switch-modern` (toggles modernos)

---

## 📋 CHECKLIST DE CONVERSÃO

Para modernizar um formulário existente:

1. ✅ Adicionar `.form-card-modern` ao card pai
2. ✅ Adicionar `.form-header-decorated` ao header
3. ✅ Trocar `.mb-3` por `.form-floating-modern`
4. ✅ Adicionar `.input-with-icon` e `<span class="input-icon">🎯</span>`
5. ✅ Adicionar `placeholder=" "` aos inputs
6. ✅ Trocar `.btn-primary` por `.btn-modern-primary`
7. ✅ Usar `.form-actions-modern` no footer

---

## 🔥 PÁGINAS PRIORITÁRIAS PARA MODERNIZAR

1. **Views/BabyLists/Index.cshtml** - Modal "Nova Lista"
2. **Views/BabyLists/AddItem.cshtml** - Adicionar item
3. **Views/BabyLists/Create.cshtml** - Criar lista
4. **Views/Expenses/Create.cshtml** - Nova despesa
5. **Views/BabyItems/Create.cshtml** - Novo item
6. **Areas/Identity/Pages/Account/Login.cshtml** - Login
7. **Areas/Identity/Pages/Account/Register.cshtml** - Registo

---

## 💡 DICAS

### Para Modais HTMX:
- Use `.form-card-modern` no `.modal-content`
- Não precisa de card interno adicional

### Para Páginas Completas:
- Use `.card.form-card-modern` como container
- Adicione `.form-header-decorated` no topo

### Mobile:
- Tudo é responsivo automaticamente!
- Estrelas desaparecem em mobile (< 768px)
- Botões ficam full-width

### Performance:
- Todas as animações usam `transform` e `opacity` (GPU accelerated)
- Backdrop-filter pode não funcionar em browsers antigos

---

## 🎯 PRÓXIMOS PASSOS

1. Testar os novos estilos num formulário existente
2. Ver a mágica acontecer! ✨
3. Converter gradualmente outros formulários
4. Personalizar ícones por contexto

---

## ⚡ COMEÇAR AGORA

Vou criar um exemplo prático no **Modal de Nova Lista** (BabyLists/Index.cshtml) para veres os efeitos em ação!

---

**Criado em**: 2025  
**Tema**: Ervilhinha 🌿  
**Objetivo**: Formulários "living on the edge" com personalidade!
