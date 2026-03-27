# 🎁 LISTA DE DESEJOS DO BEBÉ - PLANO COMPLETO

## 📊 VISÃO GERAL

Sistema completo de Lista de Desejos partilhável para bebés, permitindo que pais criem listas, convidem familiares/amigos, e estes possam reservar items como presentes.

---

## 🎯 FUNCIONALIDADES PRINCIPAIS

### **1. Para os Pais (Gestores)**
- ✅ Criar lista de desejos
- ✅ Adicionar co-gestor (pai/mãe)
- ✅ Adicionar/editar/remover items
- ✅ Categorizar items (Roupa, Brinquedos, Quarto, etc.)
- ✅ Definir prioridades
- ✅ Adicionar links de produtos
- ✅ Ver quem reservou cada item
- ✅ Marcar items como adquiridos
- ✅ Partilhar lista (link ou email)
- ✅ Gerir convites

### **2. Para Convidados**
- ✅ Ver lista através de link partilhado
- ✅ Filtrar por categoria/prioridade
- ✅ Reservar items (total ou parcial)
- ✅ Deixar mensagem aos pais
- ✅ Ver items já reservados (sem ver quem)
- ✅ Cancelar reserva

---

## 🗄️ ESTRUTURA DE DADOS

### **Models Criados:**

1. **`BabyWishlist`** - Lista principal
   ```csharp
   - Id, Name, Description
   - ExpectedDate, BabyName
   - ShareCode (único)
   - IsPublic
   - CreatedBy, CreatedDate
   ```

2. **`WishlistManager`** - Gestores (pai/mãe)
   ```csharp
   - WishlistId
   - ManagerEmail, ManagerName
   - Role (Pai/Mãe/Gestor)
   - CanManageManagers
   ```

3. **`WishlistItem`** - Items da lista
   ```csharp
   - Name, Description, Category
   - Priority (1-3)
   - EstimatedPrice
   - Quantity, ReservedQuantity, AcquiredQuantity
   - ProductUrl, ImageUrl
   - AvailableQuantity (computed)
   ```

4. **`WishlistShare`** - Partilhas/Convites
   ```csharp
   - SharedWithEmail, SharedWithName
   - InviteMessage
   - IsAccepted, AcceptedDate
   - Permission (View/Reserve)
   ```

5. **`WishlistReservation`** - Reservas
   ```csharp
   - WishlistItemId
   - ReservedBy, ReservedByName
   - Quantity
   - Message (para os pais)
   - IsDelivered
   ```

---

## 🎨 PÁGINAS A CRIAR

### **Para Gestores (Pais):**

1. **`/Wishlist/MyLists`** - Minhas Listas
   - Listagem das listas criadas/geridas
   - Botão criar nova lista
   - Ver estatísticas (total items, % reservado)

2. **`/Wishlist/Create`** - Criar Lista
   - Nome da lista
   - Data prevista do bebé
   - Nome do bebé (opcional)
   - Adicionar co-gestor (email)

3. **`/Wishlist/Manage/{id}`** - Gerir Lista
   - Ver todos os items
   - Adicionar/Editar/Remover items
   - Ver reservas
   - Marcar como adquirido
   - Partilhar lista

4. **`/Wishlist/Share/{id}`** - Partilhar Lista
   - Gerar link público
   - Enviar convites por email
   - Ver quem aceitou
   - Gerir permissões

### **Para Convidados:**

5. **`/Wishlist/View/{shareCode}`** - Ver Lista (Pública)
   - Ver items disponíveis
   - Filtros (categoria, prioridade)
   - Botão "Reservar"
   - Items já reservados (sem detalhes)

6. **`/Wishlist/Reserve/{itemId}`** - Reservar Item
   - Escolher quantidade
   - Deixar mensagem
   - Confirmação

7. **`/Wishlist/MyReservations`** - Minhas Reservas
   - Ver items reservados
   - Cancelar reserva
   - Estado da entrega

---

## 🛠️ IMPLEMENTAÇÃO

### **Fase 1: Backend** ✅ FEITO
- ✅ Models criados
- ✅ DbContext atualizado
- ⏳ Migração (PRÓXIMO PASSO)

### **Fase 2: Controllers**
- ⏳ `WishlistController` - Gestão de listas
- ⏳ `WishlistItemController` - Gestão de items
- ⏳ `WishlistShareController` - Partilhas
- ⏳ `WishlistPublicController` - Visualização pública

### **Fase 3: Views**
- ⏳ Layout moderno com scroll contínuo
- ⏳ Cards full-width com imagens
- ⏳ Interface de gestão
- ⏳ Interface pública

### **Fase 4: Features Extra**
- ⏳ Notificações por email
- ⏳ QR Code para partilha
- ⏳ Estatísticas visuais
- ⏳ Exportar para PDF

---

## 🎨 DESIGN MODERNO

### **Homepage com Scroll Contínuo:**

```
┌─────────────────────────────────────────┐
│  🌿 Ervilhinha - Navbar                 │
├─────────────────────────────────────────┤
│                                         │
│  [Hero Section - Bem-vindo]            │
│                                         │
├─────────────────────────────────────────┤
│                                         │
│  [Section 1 - Lista de Desejos]        │
│  Full Width + Imagem Apelativa         │
│                                         │
├─────────────────────────────────────────┤
│                                         │
│  [Section 2 - Planeamento Bebé]        │
│  Full Width + Imagem Apelativa         │
│                                         │
├─────────────────────────────────────────┤
│                                         │
│  [Section 3 - Finanças]                │
│  Full Width + Imagem Apelativa         │
│                                         │
├─────────────────────────────────────────┤
│  Footer                                 │
└─────────────────────────────────────────┘
```

---

## 🚀 PRÓXIMOS PASSOS

### **Agora:**
1. ✅ Models criados
2. ✅ DbContext atualizado
3. **⏳ Criar migração**
4. **⏳ Aplicar migração**

### **Depois:**
5. Controller `WishlistController`
6. Views de gestão
7. Controller público
8. Views públicas
9. Homepage redesenhada
10. Emails de convite

---

## 📝 COMANDOS PARA MIGRAÇÃO

```powershell
# Criar migração
Add-Migration AddBabyWishlistSystem

# Aplicar migração
Update-Database
```

---

## 💡 FEATURES ESPECIAIS

### **Sistema de Partilha:**
- Link único por lista: `https://ervilhinha.com/wishlist/ABC123XYZ`
- Convites por email com mensagem personalizada
- QR Code para partilha fácil
- Lista pública ou privada (apenas convidados)

### **Gestão Inteligente:**
- 2 gestores podem editar simultaneamente
- Notificações quando alguém reserva
- Sugestões de items baseadas em categorias
- Importar items de outras listas

### **Experiência do Convidado:**
- Ver lista sem login (se pública)
- Reservar com nome/email
- Mensagem aos pais
- Acompanhar estado da reserva

---

## 🎯 RESULTADO FINAL

Uma plataforma COMPLETA onde:
- ✅ Pais criam listas de desejos
- ✅ Convidam familiares/amigos
- ✅ Convidados reservam presentes
- ✅ Tudo atualizado em tempo real
- ✅ Interface moderna e intuitiva
- ✅ Mobile-friendly

---

**ESTADO ATUAL**: Models criados, DB atualizado ✅  
**PRÓXIMO PASSO**: Criar migração e continuar implementação! 🚀

Queres que continue? 😊
