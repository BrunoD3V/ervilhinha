# 📋 Visão Geral - Sistema de Listas do Bebé

## 🎯 Objetivo

O **Sistema de Listas do Bebé** é uma solução unificada que combina duas funcionalidades essenciais:

1. **🛒 Enxoval (Planeamento Pessoal)**
   - Organização de compras para o bebé
   - Tracking de orçamento (estimado vs real)
   - Recomendações de timing (quando comprar cada item)
   - Categorização detalhada (11 categorias)

2. **🎁 Lista de Presentes (Partilhável)**
   - Partilha com família e amigos
   - Sistema de reservas (evita presentes duplicados)
   - Gestores múltiplos (pai + mãe)
   - Visibilidade pública ou privada

---

## 🗺️ Arquitetura

### **Modelos de Dados**

#### **BabyList** (Lista Principal)
```csharp
- Name: Nome da lista (obrigatório, máx 100 chars)
- Description: Descrição opcional (máx 500 chars)
- Type: Enxoval | Presentes | Geral
- BabyName: Nome do bebé (opcional, máx 100 chars)
- ExpectedDate: Data prevista de nascimento (opcional)
- IsShared: Se true, ativa sistema de partilha
- ShareCode: Código único de 9 caracteres (auto-gerado)
- IsPublic: Se true, qualquer pessoa com link pode ver
```

#### **BabyListItem** (Item da Lista)
```csharp
- Name: Nome do item (obrigatório, máx 200 chars)
- Description: Descrição (opcional, máx 1000 chars)
- Category: 11 categorias (QuartoBebe, Roupa0-3M, etc.)
- Priority: Essencial | Recomendado | Opcional
- EstimatedCost: Custo estimado em € (0-10000)
- ActualCost: Custo real pago (opcional)
- Quantity: Quantidade desejada (1-100)
- ReservedQuantity: Quantidade já reservada
- AcquiredQuantity: Quantidade já comprada
- RecommendedTiming: Quando comprar (8 opções)
- IsPurchased: Se true, item já foi comprado
- PurchaseDate: Data da compra (opcional)
- IsGift: Se true, item foi presente
- ProductUrl: Link para produto online (opcional)
- Notes: Notas adicionais (opcional, máx 1000 chars)
```

#### **BabyListManager** (Gestores)
```csharp
- ManagerEmail: Email do gestor (obrigatório)
- ManagerName: Nome do gestor (máx 100 chars)
- Role: "Criador" | "Co-Gestor"
- CanManageManagers: Se pode adicionar outros gestores
```

#### **BabyListReservation** (Reservas)
```csharp
- ReservedBy: Email de quem reservou (obrigatório)
- ReservedByName: Nome de quem reservou (obrigatório)
- Quantity: Quantidade reservada (1-100)
- Message: Mensagem opcional para os pais (máx 500 chars)
- ContactEmail: Email para contacto (opcional)
```

---

## 🔄 Fluxos de Utilizador

### **Fluxo 1: Criar Lista Privada (Enxoval)**
```
1. Index → Clicar "➕ Nova Lista"
2. Create Form:
   - Nome da Lista* (ex: "Enxoval da Maria")
   - Tipo: "🛒 Enxoval"
   - Nome do Bebé (ex: "Maria")
   - Data Prevista (ex: "15/10/2026")
   - IsShared: false (padrão)
3. Submit → Manage
4. Adicionar Items → AddItem Form
5. Ver estatísticas e progresso
```

### **Fluxo 2: Criar Lista Partilhada (Presentes)**
```
1. Index → Clicar "➕ Nova Lista"
2. Create Form:
   - Nome da Lista* (ex: "Presentes para o João")
   - Tipo: "🎁 Lista de Presentes"
   - IsShared: true ✓
   - IsPublic: true ✓
   - Co-Gestor Email (opcional)
3. Submit → ShareCode gerado (ex: "ABC123XYZ")
4. Manage → Adicionar Items
5. Share → Copiar link/código
6. Enviar para família/amigos
```

### **Fluxo 3: Ativar Partilha em Lista Existente**
```
1. Manage → Botão "🔗 Ativar Partilha"
2. ShareCode gerado automaticamente
3. Toggle "🔓 Tornar Pública" (se necessário)
4. Share → Ver código e opções de partilha
```

### **Fluxo 4: Visitante Reserva Presente**
```
1. Recebe link: /BabyListPublic/ViewList?code=ABC123XYZ
2. Ver lista completa com disponibilidade
3. Clicar "🎁 Reservar" em item
4. Reserve Form:
   - Nome* (ex: "Avó Maria")
   - Email* (ex: "avo@exemplo.com")
   - Quantidade* (ex: 2)
   - Mensagem (ex: "Com muito carinho!")
5. Submit → Confirmation
6. Ver confirmação e link para MyReservations
```

---

## 📊 Funcionalidades por Página

### **Index.cshtml** - Lista de Todas as Listas
**Elementos:**
- Cards com resumo de cada lista
- Estatísticas: Total items, Comprados, Custo
- Progress bar de conclusão
- Badges de status (Privada/Partilhada/Pública)
- Botão "Ver e Gerir Lista"

**Ações:**
- ✅ **Criar Nova Lista**: Redireciona para Create
- ✅ **Ver Detalhes**: Redireciona para Manage

---

### **Create.cshtml** - Criar Nova Lista
**Grupos de Campos:**

**1. Informações Básicas**
- Nome da Lista* (text, 100 chars)
- Tipo de Lista* (select: Enxoval/Presentes/Geral)
- Descrição (textarea, 500 chars)

**2. Informações do Bebé**
- Nome do Bebé (text, 100 chars)
- Data Prevista (date)

**3. Opções de Partilha**
- Toggle "Ativar Partilha" (checkbox)
  - Se ativo: Mostra opções de partilha
    - Toggle "Lista Pública" (checkbox)
    - Co-Gestor Email (email)

**Ações:**
- ✅ **Criar Lista**: Valida e cria lista → Redireciona para Manage
- ❌ **Cancelar**: Volta para Index (sem salvar)

---

### **Manage.cshtml** - Gerir Lista
**Seções:**

**1. Header**
- Título da lista + Emoji de tipo
- Nome do bebé (se definido)
- Descrição
- Data prevista

**2. Estatísticas (Cards)**
- Total Items
- Comprados
- Custo Estimado vs Gasto
- Progress bar geral
- (Se partilhada) Reservados/Adquiridos

**3. Ações Rápidas (Sidebar)**
- ➕ Adicionar Item
- ✏️ Editar Lista
- 🔗 Ativar/Ver Partilha
- 🔓/🔒 Toggle Público/Privado
- 🗑️ Eliminar Lista

**4. Tabela de Items**
Colunas:
- Item (nome + descrição)
- Categoria (badge)
- Quantidade
- (Se partilhada) Reservado
- Custo
- Prioridade (badge)
- (Se privada) Timing
- Estado (badge: Comprado/Reservado/Pendente)
- Ações (Editar/Toggle/Eliminar)

**Ações:**
- ✅ **Adicionar Item**: Redireciona para AddItem
- ✅ **Editar Item**: Redireciona para EditItem
- ✅ **Toggle Comprado**: POST → Marca/desmarca
- ✅ **Eliminar Item**: POST → Confirma e elimina
- ✅ **Editar Lista**: Redireciona para Edit
- ✅ **Ativar Partilha**: POST → Gera ShareCode
- ✅ **Toggle Público**: POST → Alterna visibilidade
- ✅ **Ver Partilha**: Redireciona para Share

---

### **AddItem.cshtml** - Adicionar Item
**Grupos de Campos:**

**1. Informação Básica**
- Nome do Item* (text, 200 chars)
- Descrição (textarea, 1000 chars)

**2. Categorização**
- Categoria* (select, 11 opções)
- Prioridade* (select: Essencial/Recomendado/Opcional)

**3. Planeamento Financeiro**
- Quantidade* (number, 1-100)
- Custo Estimado* (number, €0-€10000)
- Quando Comprar (select, 9 opções)

**4. Informações Adicionais**
- Link do Produto (url)
- Notas (textarea, 1000 chars)

**Ações:**
- ✅ **Adicionar Item**: Valida e adiciona → Volta para Manage
- ❌ **Cancelar**: Volta para Manage (sem salvar)

---

### **EditItem.cshtml** - Editar Item
**Grupos de Campos:** (igual a AddItem +)

**5. Status de Compra**
- ✓ Já Comprado? (checkbox)
- Data da Compra (date, só se IsPurchased)
- Custo Real (number, €0-€10000)
- ✓ Foi Presente? (checkbox)

**Ações:**
- ✅ **Guardar Alterações**: Valida e atualiza → Volta para Manage
- ❌ **Cancelar**: Volta para Manage (sem salvar)

---

### **Share.cshtml** - Partilhar Lista
**Seções:**

**1. Código de Partilha**
- Input readonly com código (ex: "ABC123XYZ")
- Botão "📋 Copiar" → Copia para clipboard

**2. Link Direto**
- Input readonly com URL completo
- Botão "📋 Copiar"
- Botões de partilha:
  - 📱 WhatsApp (abre app)
  - 📧 Email (abre cliente email)
  - 💬 Copiar Mensagem (template pronto)

**3. Configurações de Privacidade**
- Status atual (Pública/Privada)
- Botão Toggle visibilidade
- Explicação de como funciona

**Ações:**
- ✅ **Copiar Código**: JavaScript → Clipboard API
- ✅ **Copiar Link**: JavaScript → Clipboard API
- ✅ **WhatsApp**: Abre URL com texto pré-preenchido
- ✅ **Email**: Abre mailto: com assunto e corpo
- ✅ **Toggle Público**: POST → Alterna IsPublic
- ❌ **Voltar**: Redireciona para Manage

---

### **ViewList.cshtml** (Público) - Ver Lista Pública
**Elementos:**
- Header com nome da lista e bebé
- Descrição
- Data prevista
- Items agrupados por categoria
- Cards de items com:
  - Nome e descrição
  - Prioridade (badge)
  - Custo estimado
  - Progress bar disponibilidade
  - Botão "🎁 Reservar" (ou "✅ Reservado")
  - Link "Ver Produto" (se tiver)

**Ações:**
- ✅ **Reservar Item**: Redireciona para Reserve
- ✅ **Ver Produto**: Abre link em nova tab

---

### **Reserve.cshtml** (Público) - Reservar Item
**Grupos de Campos:**

**1. Suas Informações**
- Nome* (text, 100 chars)
- Email* (email, para contacto)

**2. Detalhes da Reserva**
- Quantidade* (number, 1 até AvailableQuantity)
- Mensagem para os Pais (textarea, 500 chars)

**3. Como Funciona** (Alert Info)
- Explicação do sistema de reservas
- Garantia de não duplicação
- Possibilidade de cancelamento

**Ações:**
- ✅ **Confirmar Reserva**: Valida e reserva → Confirmation
- ❌ **Cancelar**: Volta para ViewList

---

### **Confirmation.cshtml** (Público) - Confirmação
**Elementos:**
- Ícone de sucesso (✅ grande)
- Mensagem de agradecimento
- Card com detalhes da reserva:
  - Item reservado
  - Quantidade
  - Data da reserva
  - Mensagem (se tiver)
- Alert com próximos passos
- Link para ver todas as reservas

**Ações:**
- ✅ **Voltar à Lista**: Redireciona para ViewList
- ✅ **Ver Minhas Reservas**: Redireciona para MyReservations

---

## 🎨 Código de Cores

### **Status de Lista**
- 🔒 **Privada**: Badge cinzento (`bg-secondary`)
- 🔗 **Partilhada**: Badge azul (`bg-info`)
- 🔓 **Pública**: Badge verde (`bg-success`)

### **Prioridade de Item**
- ✅ **Essencial**: Badge vermelho (`bg-danger`)
- ⭐ **Recomendado**: Badge azul (`bg-info`)
- 💡 **Opcional**: Badge cinzento (`bg-secondary`)

### **Estado de Item**
- ✅ **Comprado**: Badge verde (`bg-success`)
- ⏳ **Reservado**: Badge amarelo (`bg-warning`)
- 📋 **Pendente**: Badge cinzento (`bg-secondary`)

---

## 📐 Layouts Responsivos

### **Desktop (≥992px)**
- Sidebar de ações (col-md-4)
- Conteúdo principal (col-md-8)
- Forms em colunas (2-3 campos por linha)
- Campos de data com largura apropriada (col-md-6)

### **Tablet (768px-991px)**
- Sidebar empilha sobre conteúdo
- Forms em 2 colunas
- Botões full-width

### **Mobile (<768px)**
- Layout single-column
- Forms full-width
- Botões grandes e touch-friendly
- Tabelas com scroll horizontal

---

## 🔐 Permissões e Segurança

### **Autenticado**
- ✅ Ver suas próprias listas
- ✅ Ver listas onde é gestor
- ✅ Criar novas listas
- ✅ Editar/eliminar suas listas
- ✅ Adicionar co-gestores

### **Visitante (com ShareCode)**
- ✅ Ver lista pública
- ✅ Reservar items disponíveis
- ✅ Ver suas reservas (por email)
- ❌ Editar lista
- ❌ Ver listas privadas (sem convite)

### **Visitante (sem ShareCode)**
- ❌ Acesso negado

---

## 📊 Regras de Negócio

### **Disponibilidade de Item**
```
AvailableQuantity = Quantity - ReservedQuantity - AcquiredQuantity

Se AvailableQuantity <= 0:
  - Botão "Reservar" desativado
  - Badge "✅ Totalmente Reservado"
```

### **ShareCode**
- Gerado automaticamente quando `IsShared = true`
- Formato: 9 caracteres alfanuméricos (ex: "ABC123XYZ")
- Único em toda a base de dados
- Não pode ser editado manualmente

### **Managers**
- Criador é sempre o primeiro manager
- Criador tem `CanManageManagers = true`
- Co-gestores podem ter permissões limitadas

---

## 🚀 Próximas Funcionalidades

- [ ] Upload de imagens de produtos
- [ ] Notificações por email (nova reserva)
- [ ] Export para PDF/Excel
- [ ] Integração com lojas online
- [ ] Wishlist templates pré-preenchidos
- [ ] Analytics de reservas
- [ ] Chat entre gestores e reservadores

---

**Navegação:**
- [← Voltar ao Índice](../README.md)
- [Criar Nova Lista →](02-Create-List.md)
