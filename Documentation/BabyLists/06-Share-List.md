# 🔗 Partilhar Lista - Documentação Completa

## 📍 Localização
**URL**: `/BabyLists/Share?id={listId}`  
**Autenticação**: Obrigatória  
**Permissões**: Proprietário da lista ou Gestor com permissões

---

## 🎯 Objetivo da Página

Permite partilhar a lista do bebé com família e amigos através de:
- **Código de partilha**: 9 caracteres alfanuméricos (ex: ABC123XYZ)
- **Link direto**: URL completo para acesso rápido
- **Configurações de privacidade**: Pública ou Privada

---

## 📋 Estrutura da Página

### **Layout Desktop**
```
┌────────────────────────────────────────────────────┐
│ 🔗 Partilhar Lista                                 │
│ Enxoval da Maria                                   │
├────────────────────────────────────────────────────┤
│                                                     │
│ [Card: Código de Partilha]                         │
│   📋 Código: ABC123XYZ   [📋 Copiar Código]       │
│                                                     │
│ [Card: Link Direto]                                │
│   🔗 Link: https://...    [📋 Copiar Link]        │
│                                                     │
│ [Card: Partilhar Via]                              │
│   [📱 WhatsApp]  [📧 Email]  [💬 Mensagem]        │
│                                                     │
│ [Card: Configurações de Privacidade]               │
│   Status: 🔓 Lista Pública                         │
│   [🔄 Alternar para Privada]                       │
│                                                     │
│ [Card: Como Funciona]                              │
│   ℹ️ Explicação do sistema                         │
│                                                     │
│ [← Voltar para a Lista]                            │
└────────────────────────────────────────────────────┘
```

---

## 📝 Elementos da Página

### **ELEMENTO 1: Código de Partilha**

#### **Display do Código**
**HTML**:
```html
<div class="card shadow mb-4">
    <div class="card-header bg-gradient-blue">
        <h5 class="mb-0">📋 Código de Partilha</h5>
    </div>
    <div class="card-body text-center">
        <div class="mb-3">
            <label class="form-label fw-semibold">Código Único da Lista</label>
            <div class="input-group input-group-lg">
                <input type="text" class="form-control form-control-lg text-center fw-bold fs-1" 
                       value="@Model.ShareCode" readonly id="shareCodeInput" 
                       style="letter-spacing: 0.5rem; font-family: monospace;" />
                <button class="btn btn-primary" type="button" id="copyCodeBtn">
                    <i class="bi bi-clipboard"></i> Copiar
                </button>
            </div>
            <small class="text-muted d-block mt-2">
                Partilha este código com família e amigos
            </small>
        </div>

        <div class="alert alert-success" role="alert" id="copyCodeSuccess" style="display: none;">
            <i class="bi bi-check-circle-fill"></i> Código copiado para a área de transferência!
        </div>
    </div>
</div>
```

**Características:**
- ✅ **Readonly**: Não pode ser editado
- ✅ **Monospace font**: Melhor legibilidade
- ✅ **Letter-spacing**: Destaque visual
- ✅ **Texto centralizado**: Destaque
- ✅ **Tamanho grande**: `fs-1` (3rem)

**JavaScript de Cópia:**
```javascript
const copyCodeBtn = document.getElementById('copyCodeBtn');
const shareCodeInput = document.getElementById('shareCodeInput');
const copyCodeSuccess = document.getElementById('copyCodeSuccess');

copyCodeBtn.addEventListener('click', async function() {
    try {
        await navigator.clipboard.writeText(shareCodeInput.value);
        
        // Feedback visual
        copyCodeSuccess.style.display = 'block';
        this.innerHTML = '<i class="bi bi-check-circle-fill"></i> Copiado!';
        this.classList.replace('btn-primary', 'btn-success');
        
        setTimeout(() => {
            copyCodeSuccess.style.display = 'none';
            this.innerHTML = '<i class="bi bi-clipboard"></i> Copiar';
            this.classList.replace('btn-success', 'btn-primary');
        }, 3000);
    } catch (err) {
        // Fallback para browsers antigos
        shareCodeInput.select();
        document.execCommand('copy');
    }
});
```

**Análise UI/UX:**
- ✅ **EXCELENTE**: Código destacado visualmente
- ✅ **EXCELENTE**: Botão de cópia integrado
- ✅ **EXCELENTE**: Feedback imediato ao copiar
- ✅ **BOM**: Fallback para browsers sem Clipboard API

---

### **ELEMENTO 2: Link Direto**

#### **Display do Link**
**HTML**:
```html
<div class="card shadow mb-4">
    <div class="card-header bg-gradient-pea">
        <h5 class="mb-0">🔗 Link Direto</h5>
    </div>
    <div class="card-body">
        <label class="form-label fw-semibold">URL Completo para Partilha</label>
        <div class="input-group input-group-lg">
            <span class="input-group-text bg-primary text-white">
                <i class="bi bi-link-45deg"></i>
            </span>
            <input type="text" class="form-control" 
                   value="@Url.ActionLink("ViewList", "BabyListPublic", new { code = Model.ShareCode }, protocol: "https")" 
                   readonly id="shareLinkInput" />
            <button class="btn btn-primary" type="button" id="copyLinkBtn">
                <i class="bi bi-clipboard"></i> Copiar Link
            </button>
        </div>
        <small class="text-muted d-block mt-2">
            <i class="bi bi-info-circle"></i> Qualquer pessoa com este link pode aceder à lista
        </small>
    </div>
</div>
```

**JavaScript de Cópia:**
```javascript
const copyLinkBtn = document.getElementById('copyLinkBtn');
const shareLinkInput = document.getElementById('shareLinkInput');

copyLinkBtn.addEventListener('click', async function() {
    try {
        await navigator.clipboard.writeText(shareLinkInput.value);
        
        this.innerHTML = '<i class="bi bi-check-circle-fill"></i> Link Copiado!';
        this.classList.replace('btn-primary', 'btn-success');
        
        setTimeout(() => {
            this.innerHTML = '<i class="bi bi-clipboard"></i> Copiar Link';
            this.classList.replace('btn-success', 'btn-primary');
        }, 3000);
    } catch (err) {
        shareLinkInput.select();
        document.execCommand('copy');
    }
});
```

**Análise UI/UX:**
- ✅ **BOM**: URL completo visível (transparência)
- ✅ **BOM**: Readonly (previne edição acidental)
- ✅ **BOM**: Ícone de link claro
- ⚠️ **MELHORAR**: Truncar URL em mobile?
  ```css
  @media (max-width: 767px) {
      #shareLinkInput {
          font-size: 0.875rem;
          overflow: hidden;
          text-overflow: ellipsis;
      }
  }
  ```

---

### **ELEMENTO 3: Partilhar Via Redes Sociais**

#### **Botões de Partilha**
**HTML**:
```html
<div class="card shadow mb-4">
    <div class="card-header bg-gradient-pink">
        <h5 class="mb-0">📱 Partilhar Via</h5>
    </div>
    <div class="card-body">
        <div class="row g-2">
            <!-- WhatsApp -->
            <div class="col-md-4">
                <a href="https://wa.me/?text=@Uri.EscapeDataString($"Olá! 👶 Quero partilhar a minha lista do bebé contigo. Podes ver os items e reservar presentes aqui: {Url.ActionLink("ViewList", "BabyListPublic", new { code = Model.ShareCode }, protocol: "https")}")" 
                   target="_blank" 
                   class="btn btn-success btn-lg w-100">
                    <i class="bi bi-whatsapp fs-4"></i>
                    <span class="d-block">WhatsApp</span>
                </a>
            </div>

            <!-- Email -->
            <div class="col-md-4">
                <a href="mailto:?subject=@Uri.EscapeDataString($"Lista do Bebé - {Model.Name}")&body=@Uri.EscapeDataString($"Olá!\n\nQuero partilhar contigo a minha lista do bebé.\n\nPodes ver os items e reservar presentes aqui:\n{Url.ActionLink("ViewList", "BabyListPublic", new { code = Model.ShareCode }, protocol: "https")}\n\nOu usa o código: {Model.ShareCode}\n\nObrigado!\n{User.Identity.Name}")" 
                   class="btn btn-primary btn-lg w-100">
                    <i class="bi bi-envelope-fill fs-4"></i>
                    <span class="d-block">Email</span>
                </a>
            </div>

            <!-- Copiar Mensagem -->
            <div class="col-md-4">
                <button type="button" class="btn btn-info btn-lg w-100" id="copyMessageBtn">
                    <i class="bi bi-chat-dots-fill fs-4"></i>
                    <span class="d-block">Copiar Mensagem</span>
                </button>
            </div>
        </div>

        <small class="text-muted d-block mt-3 text-center">
            Escolhe a forma mais conveniente para partilhar a tua lista
        </small>
    </div>
</div>
```

**JavaScript - Copiar Mensagem:**
```javascript
const copyMessageBtn = document.getElementById('copyMessageBtn');

copyMessageBtn.addEventListener('click', async function() {
    const message = `Olá! 👶

Quero partilhar contigo a minha lista do bebé.

Podes ver os items e reservar presentes aqui:
${shareLinkInput.value}

Ou usa o código: ${shareCodeInput.value}

Obrigado!`;

    try {
        await navigator.clipboard.writeText(message);
        
        this.innerHTML = '<i class="bi bi-check-circle-fill fs-4"></i><span class="d-block">Mensagem Copiada!</span>';
        this.classList.replace('btn-info', 'btn-success');
        
        setTimeout(() => {
            this.innerHTML = '<i class="bi bi-chat-dots-fill fs-4"></i><span class="d-block">Copiar Mensagem</span>';
            this.classList.replace('btn-success', 'btn-info');
        }, 3000);
    } catch (err) {
        alert('Não foi possível copiar. Tenta copiar o link manualmente.');
    }
});
```

**Análise UI/UX:**
- ✅ **EXCELENTE**: Múltiplas opções de partilha
- ✅ **EXCELENTE**: Mensagens pré-formatadas
- ✅ **BOM**: Ícones grandes e claros
- ✅ **BOM**: Cores distintivas (WhatsApp verde, Email azul)
- ⚠️ **MELHORAR**: Adicionar Facebook/Messenger?

---

### **ELEMENTO 4: Configurações de Privacidade**

#### **Toggle Público/Privado**
**HTML**:
```html
<div class="card shadow mb-4">
    <div class="card-header bg-gradient-yellow">
        <h5 class="mb-0">🔒 Configurações de Privacidade</h5>
    </div>
    <div class="card-body">
        <div class="row align-items-center">
            <div class="col-md-8">
                <h6 class="mb-2">Status Atual</h6>
                @if (Model.IsPublic)
                {
                    <div class="alert alert-success mb-0">
                        <i class="bi bi-unlock-fill fs-3"></i>
                        <div class="d-inline-block ms-2">
                            <strong>Lista Pública</strong>
                            <p class="mb-0 small">Qualquer pessoa com o link ou código pode ver esta lista</p>
                        </div>
                    </div>
                }
                else
                {
                    <div class="alert alert-warning mb-0">
                        <i class="bi bi-lock-fill fs-3"></i>
                        <div class="d-inline-block ms-2">
                            <strong>Lista Privada</strong>
                            <p class="mb-0 small">Apenas pessoas que convidares especificamente podem aceder</p>
                        </div>
                    </div>
                }
            </div>
            <div class="col-md-4 text-end">
                <form asp-action="TogglePublic" method="post">
                    <input type="hidden" name="id" value="@Model.Id" />
                    <button type="submit" class="btn btn-lg @(Model.IsPublic ? "btn-warning" : "btn-success")">
                        @if (Model.IsPublic)
                        {
                            <i class="bi bi-lock-fill"></i>
                            <span>Tornar Privada</span>
                        }
                        else
                        {
                            <i class="bi bi-unlock-fill"></i>
                            <span>Tornar Pública</span>
                        }
                    </button>
                </form>
            </div>
        </div>

        <hr class="my-3">

        <div class="row">
            <div class="col-md-6">
                <h6>🔓 Lista Pública</h6>
                <ul class="small text-muted">
                    <li>Qualquer pessoa pode ver</li>
                    <li>Não precisa de aprovação</li>
                    <li>Mais fácil de partilhar</li>
                    <li>Ideal para grupos grandes</li>
                </ul>
            </div>
            <div class="col-md-6">
                <h6>🔒 Lista Privada</h6>
                <ul class="small text-muted">
                    <li>Só convidados podem ver</li>
                    <li>Mais controlo sobre acesso</li>
                    <li>Lista de convidados visível</li>
                    <li>Ideal para grupos pequenos</li>
                </ul>
            </div>
        </div>
    </div>
</div>
```

**Controller Action:**
```csharp
[HttpPost]
public async Task<IActionResult> TogglePublic(int id)
{
    var list = await _context.BabyLists.FindAsync(id);
    if (list == null || list.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
    {
        return NotFound();
    }

    list.IsPublic = !list.IsPublic;
    await _context.SaveChangesAsync();

    TempData["Success"] = list.IsPublic 
        ? "✅ Lista agora é pública!" 
        : "✅ Lista agora é privada!";

    return RedirectToAction("Share", new { id });
}
```

**Análise UI/UX:**
- ✅ **EXCELENTE**: Status visual claro (ícone + cor)
- ✅ **EXCELENTE**: Explicação das diferenças
- ✅ **BOM**: Botão grande e claro
- ✅ **BOM**: Feedback imediato (TempData)
- ⚠️ **MELHORAR**: Confirmação antes de tornar pública?
  ```javascript
  form.addEventListener('submit', function(e) {
      if (!isPublic) { // Vai tornar pública
          if (!confirm('Tem a certeza? Qualquer pessoa poderá ver a lista.')) {
              e.preventDefault();
          }
      }
  });
  ```

---

### **ELEMENTO 5: Como Funciona**

#### **Explicação do Sistema**
**HTML**:
```html
<div class="card shadow mb-4">
    <div class="card-header">
        <h5 class="mb-0">ℹ️ Como Funciona a Partilha</h5>
    </div>
    <div class="card-body">
        <div class="row">
            <div class="col-md-4 mb-3 mb-md-0 text-center">
                <div class="mb-3">
                    <i class="bi bi-send-fill text-primary" style="font-size: 3rem;"></i>
                </div>
                <h6>1. Partilha</h6>
                <p class="small text-muted">
                    Envia o código ou link para família/amigos via WhatsApp, Email, etc.
                </p>
            </div>

            <div class="col-md-4 mb-3 mb-md-0 text-center">
                <div class="mb-3">
                    <i class="bi bi-eye-fill text-success" style="font-size: 3rem;"></i>
                </div>
                <h6>2. Visualização</h6>
                <p class="small text-muted">
                    Eles abrem o link e veem todos os items da lista com disponibilidade
                </p>
            </div>

            <div class="col-md-4 text-center">
                <div class="mb-3">
                    <i class="bi bi-gift-fill text-danger" style="font-size: 3rem;"></i>
                </div>
                <h6>3. Reserva</h6>
                <p class="small text-muted">
                    Escolhem um item e reservam a quantidade desejada (evita duplicados!)
                </p>
            </div>
        </div>

        <hr class="my-3">

        <div class="alert alert-info mb-0">
            <strong>💡 Dica:</strong> Podes desativar a partilha a qualquer momento na página de gestão da lista.
        </div>
    </div>
</div>
```

**Análise UI/UX:**
- ✅ **EXCELENTE**: Visualização clara do fluxo
- ✅ **EXCELENTE**: Ícones grandes e coloridos
- ✅ **BOM**: Numeração lógica (1-2-3)
- ✅ **BOM**: Dica adicional útil

---

## 🎬 Botões de Ação

### **Botão: ← Voltar para a Lista**
**HTML**:
```html
<div class="text-center">
    <a asp-action="Manage" asp-route-id="@Model.Id" class="btn btn-outline-secondary btn-lg">
        <i class="bi bi-arrow-left-circle"></i> Voltar para Gerir Lista
    </a>
</div>
```

**Ação:**
- Redireciona para `/BabyLists/Manage/{id}`
- **SEM confirmação** (nada a salvar)

---

## 📊 Fluxo Completo

```mermaid
graph TD
    A[Manage] -->|Clicar "Partilhar"| B[Share Page]
    B -->|Ver código| C[Copiar Código]
    B -->|Ver link| D[Copiar Link]
    B -->|Partilhar| E[WhatsApp/Email]
    E --> F[Visitante recebe]
    F --> G[Acede à lista pública]
    G --> H[Reserva item]
    
    B -->|Toggle| I{Público?}
    I -->|Sim| J[Tornar Privada]
    I -->|Não| K[Tornar Pública]
    J --> B
    K --> B
```

---

## 🔐 Segurança

### **Validações:**
- ✅ **Autenticação**: Só owner/gestores acedem
- ✅ **ShareCode único**: Não pode haver duplicados
- ✅ **HTTPS obrigatório**: Links sempre seguros
- ✅ **Rate limiting**: Prevenir spam (futuro)

### **Proteções:**
```csharp
// Verificar ownership
if (list.UserId != currentUserId && !list.Managers.Any(m => m.ManagerEmail == currentEmail))
{
    return Forbid();
}

// ShareCode sempre único
while (_context.BabyLists.Any(l => l.ShareCode == code))
{
    code = ShareCodeGenerator.Generate();
}
```

---

## 🧪 Cenários de Teste

### **Cenário 1: Copiar Código**
1. Clicar "Copiar Código"
2. ✅ Código copiado para clipboard
3. ✅ Feedback visual (verde + check)
4. ✅ Botão volta ao normal após 3s

### **Cenário 2: Partilhar via WhatsApp**
1. Clicar "WhatsApp"
2. ✅ Abre app/web WhatsApp
3. ✅ Mensagem pré-preenchida
4. ✅ Link funcional

### **Cenário 3: Toggle Público→Privado**
1. Lista é Pública
2. Clicar "Tornar Privada"
3. ✅ Status muda para Privada
4. ✅ Feedback "Lista agora é privada!"
5. ✅ Visitantes precisam de convite

---

**Navegação:**
- [← Editar Item](05-Edit-Item.md)
- [Visualização Pública →](07-Public-View.md)
