# ✅ Guia Completo de Validações - Sistema BabyLists

## 📋 Índice por Modelo

- [BabyList (Lista Principal)](#babylist)
- [BabyListItem (Item da Lista)](#babylistitem)
- [BabyListManager (Gestores)](#babylistmanager)
- [BabyListShare (Partilhas)](#babylistshare)
- [BabyListReservation (Reservas)](#babylistreservation)

---

## 🗂️ BabyList

### **Name** - Nome da Lista
| Propriedade | Valor |
|---|---|
| **Tipo** | `string` |
| **Obrigatório** | ✅ Sim |
| **Comprimento** | Máximo 100 caracteres |
| **Validação Client-Side** | HTML5 `required` + `maxlength="100"` |
| **Validação Server-Side** | `[Required]` + `[StringLength(100)]` |
| **Mensagem de Erro (vazio)** | "O nome da lista é obrigatório" |
| **Mensagem de Erro (longo)** | "O nome não pode ter mais de 100 caracteres" |
| **Placeholder** | "Ex: Enxoval da Maria, Lista de Presentes para o João..." |
| **Exemplos Válidos** | "Enxoval da Maria", "Presentes João 2026" |
| **Exemplos Inválidos** | "" (vazio), "Lista com mais de 100 caracteres..." (excede limite) |

---

### **Description** - Descrição
| Propriedade | Valor |
|---|---|
| **Tipo** | `string?` (nullable) |
| **Obrigatório** | ❌ Não |
| **Comprimento** | Máximo 500 caracteres |
| **Validação Client-Side** | `maxlength="500"` |
| **Validação Server-Side** | `[StringLength(500)]` |
| **Mensagem de Erro** | "A descrição não pode ter mais de 500 caracteres" |
| **Placeholder** | "Descrição opcional da lista..." |
| **Exemplos Válidos** | null, "", "Lista de items essenciais..." |
| **Exemplos Inválidos** | (texto com >500 caracteres) |

---

### **Type** - Tipo de Lista
| Propriedade | Valor |
|---|---|
| **Tipo** | `ListType` (enum: 1, 2, 3) |
| **Obrigatório** | ✅ Sim (enum não-nullable) |
| **Valores Válidos** | 1 (Enxoval), 2 (Presentes), 3 (Geral) |
| **Validação Client-Side** | `required` |
| **Validação Server-Side** | Implícita (enum) |
| **Padrão** | 1 (Enxoval) |
| **Mensagem de Erro** | "Selecione um tipo de lista" |
| **Display** | 🛒 Enxoval / 🎁 Presentes / 📋 Geral |

---

### **BabyName** - Nome do Bebé
| Propriedade | Valor |
|---|---|
| **Tipo** | `string?` (nullable) |
| **Obrigatório** | ❌ Não |
| **Comprimento** | Máximo 100 caracteres |
| **Validação Client-Side** | `maxlength="100"` |
| **Validação Server-Side** | `[StringLength(100)]` |
| **Mensagem de Erro** | "O nome não pode ter mais de 100 caracteres" |
| **Placeholder** | "Ex: Maria, João (opcional)" |
| **Exemplos Válidos** | null, "", "Maria", "João Pedro" |

---

### **ExpectedDate** - Data Prevista
| Propriedade | Valor |
|---|---|
| **Tipo** | `DateTime?` (nullable) |
| **Obrigatório** | ❌ Não |
| **Formato** | `dd/MM/yyyy` (cultura PT) |
| **Validação Client-Side** | `type="date"` + `min="hoje"` |
| **Validação Server-Side** | (opcional) Data futura |
| **Mensagem de Erro** | "A data deve ser futura" |
| **Exemplos Válidos** | null, "2026-12-25", "2027-01-15" |
| **Exemplos Inválidos** | "2020-01-01" (no passado) |

---

### **IsShared** - Ativar Partilha
| Propriedade | Valor |
|---|---|
| **Tipo** | `bool` |
| **Obrigatório** | N/A (boolean) |
| **Padrão** | `false` |
| **Validação** | Nenhuma |
| **Comportamento** | Se `true`, gera `ShareCode` automaticamente |

---

### **ShareCode** - Código de Partilha
| Propriedade | Valor |
|---|---|
| **Tipo** | `string?` (nullable) |
| **Obrigatório** | ❌ Não (gerado automaticamente) |
| **Formato** | 9 caracteres alfanuméricos (ex: "ABC123XYZ") |
| **Comprimento** | Máximo 50 caracteres |
| **Validação Server-Side** | `[StringLength(50)]` + Único na BD |
| **Geração** | `ShareCodeGenerator.Generate()` |
| **Mensagem de Erro** | "Código de partilha inválido" |
| **Exemplos Válidos** | "ABC123XYZ", "XYZ789ABC" |
| **Exemplos Inválidos** | "ABC" (muito curto), "123" (duplicado) |

---

### **IsPublic** - Lista Pública
| Propriedade | Valor |
|---|---|
| **Tipo** | `bool` |
| **Obrigatório** | N/A (boolean) |
| **Padrão** | `false` |
| **Validação** | Nenhuma |
| **Comportamento** | Só relevante se `IsShared = true` |
| **Significado** | `true` = Qualquer pessoa com link; `false` = Só convidados |

---

## 📦 BabyListItem

### **Name** - Nome do Item
| Propriedade | Valor |
|---|---|
| **Tipo** | `string` |
| **Obrigatório** | ✅ Sim |
| **Comprimento** | Máximo 200 caracteres |
| **Validação Client-Side** | `required` + `maxlength="200"` |
| **Validação Server-Side** | `[Required]` + `[StringLength(200)]` |
| **Mensagem de Erro (vazio)** | "O nome do item é obrigatório" |
| **Mensagem de Erro (longo)** | "O nome não pode ter mais de 200 caracteres" |
| **Placeholder** | "Ex: Berço de madeira, Pack 5 bodies 0-3M..." |
| **Exemplos Válidos** | "Berço Montessori", "Carrinho 3 em 1" |
| **Exemplos Inválidos** | "", "A" (muito vago) |
| **UI Enhancement** | Contador de caracteres em tempo real |

---

### **Description** - Descrição do Item
| Propriedade | Valor |
|---|---|
| **Tipo** | `string?` (nullable) |
| **Obrigatório** | ❌ Não |
| **Comprimento** | Máximo 1000 caracteres |
| **Validação Client-Side** | `maxlength="1000"` |
| **Validação Server-Side** | `[StringLength(1000)]` |
| **Mensagem de Erro** | "A descrição não pode ter mais de 1000 caracteres" |
| **Placeholder** | "Detalhes como cor, tamanho, marca preferida..." |
| **UI Enhancement** | Auto-resize textarea + contador |

---

### **Category** - Categoria
| Propriedade | Valor |
|---|---|
| **Tipo** | `ItemCategory` (enum: 1-11) |
| **Obrigatório** | ✅ Sim |
| **Valores Válidos** | 1 (Quarto), 2-4 (Roupa), 5 (Higiene), 6 (Alimentação), 7 (Passeio), 8 (Brinquedos), 9 (Saúde), 10 (Acessórios), 11 (Outros) |
| **Validação Client-Side** | `required` |
| **Validação Server-Side** | Implícita (enum) |
| **Mensagem de Erro** | "Selecione uma categoria" |
| **UI Enhancement** | Emojis + optgroups (Essenciais, Roupa, Outros) |
| **Smart Suggestion** | Auto-detecta categoria baseado no nome |

**Mapeamento de Categorias:**
| ID | Nome | Emoji | Uso |
|---|---|---|---|
| 1 | Quarto do Bebé | 🛏️ | Berço, cómoda, móbile |
| 2 | Roupa 0-3M | 👶 | Body, babygrow, meias |
| 3 | Roupa 3-6M | 👕 | Roupa para 3-6 meses |
| 4 | Roupa 6-12M | 👔 | Roupa para 6-12 meses |
| 5 | Higiene e Banho | 🛁 | Fraldas, toalhas, banheira |
| 6 | Alimentação | 🍼 | Biberões, esterilizador |
| 7 | Passeio | 🚗 | Carrinho, cadeira auto |
| 8 | Brinquedos | 🧸 | Brinquedos educativos |
| 9 | Saúde e Segurança | 🏥 | Termómetro, monitor |
| 10 | Acessórios | ✨ | Chupetas, babetes |
| 11 | Outros | 📦 | Não se encaixa |

---

### **Priority** - Prioridade
| Propriedade | Valor |
|---|---|
| **Tipo** | `ItemPriority` (enum: 1, 2, 3) |
| **Obrigatório** | ✅ Sim |
| **Valores Válidos** | 1 (Essencial), 2 (Recomendado), 3 (Opcional) |
| **Padrão** | 1 (Essencial) |
| **Validação Client-Side** | `required` |
| **Validação Server-Side** | Implícita (enum) |
| **Mensagem de Erro** | "Selecione uma prioridade" |
| **Display** | ✅ Essencial / ⭐ Recomendado / 💡 Opcional |

---

### **EstimatedCost** - Custo Estimado
| Propriedade | Valor |
|---|---|
| **Tipo** | `decimal` |
| **Obrigatório** | ✅ Sim |
| **Range** | €0 - €10,000 |
| **Decimais** | Até 2 casas |
| **Validação Client-Side** | `required` + `min="0"` + `max="10000"` + `step="0.01"` |
| **Validação Server-Side** | `[Range(0, 10000)]` |
| **Mensagem de Erro (vazio)** | "O custo estimado é obrigatório" |
| **Mensagem de Erro (negativo)** | "O custo não pode ser negativo" |
| **Mensagem de Erro (alto)** | "O custo não pode exceder €10,000" |
| **Placeholder** | "0.00" |
| **UI Enhancement** | Símbolo € visível + formatação automática em blur |
| **Exemplos Válidos** | 0, 0.01, 49.99, 299.99 |
| **Exemplos Inválidos** | -10, 15000 |

---

### **ActualCost** - Custo Real
| Propriedade | Valor |
|---|---|
| **Tipo** | `decimal?` (nullable) |
| **Obrigatório** | ❌ Não |
| **Range** | €0 - €10,000 |
| **Validação** | Igual a `EstimatedCost` (opcional) |
| **UI** | Só aparece se `IsPurchased = true` |

---

### **Quantity** - Quantidade
| Propriedade | Valor |
|---|---|
| **Tipo** | `int` |
| **Obrigatório** | ✅ Sim |
| **Range** | 1 - 100 |
| **Padrão** | 1 |
| **Validação Client-Side** | `required` + `min="1"` + `max="100"` |
| **Validação Server-Side** | `[Range(1, 100)]` |
| **Mensagem de Erro (vazio)** | "A quantidade é obrigatória" |
| **Mensagem de Erro (<1)** | "A quantidade deve ser pelo menos 1" |
| **Mensagem de Erro (>100)** | "A quantidade não pode exceder 100" |
| **UI Enhancement** | Spinners visíveis + cálculo automático de custo total |
| **Exemplos Válidos** | 1, 5, 12, 100 |
| **Exemplos Inválidos** | 0, -5, 150 |

---

### **RecommendedTiming** - Quando Comprar
| Propriedade | Valor |
|---|---|
| **Tipo** | `PurchaseTiming?` (nullable enum) |
| **Obrigatório** | ❌ Não |
| **Valores Válidos** | 1-9 (ver mapeamento) |
| **Padrão** | 3 (8º Mês) |
| **Validação** | Nenhuma |
| **UI Enhancement** | Emojis 📅 + descrições claras |

**Mapeamento de Timing:**
| ID | Nome | Emoji | Quando |
|---|---|---|---|
| 1 | Gravidez (até 6M) | 📅 | Início da gravidez |
| 2 | 7º Mês | 📅 | 7º mês de gravidez |
| 3 | 8º Mês | 📅 | 8º mês (padrão) |
| 4 | Antes do Nascimento | 📅 | Última semana |
| 5 | Após Nascimento | 👶 | Primeiros dias |
| 6 | Com 3 Meses | 👶 | Bebé com 3M |
| 7 | Com 6 Meses | 👶 | Bebé com 6M |
| 8 | Com 9 Meses | 👶 | Bebé com 9M |
| 9 | Quando Necessário | ⏰ | Sem pressa |

---

### **IsPurchased** - Já Comprado
| Propriedade | Valor |
|---|---|
| **Tipo** | `bool` |
| **Padrão** | `false` |
| **Validação** | Nenhuma |
| **Comportamento** | Se `true`, mostrar campos `PurchaseDate` e `ActualCost` |

---

### **PurchaseDate** - Data da Compra
| Propriedade | Valor |
|---|---|
| **Tipo** | `DateTime?` (nullable) |
| **Obrigatório** | ❌ Não |
| **Validação** | Data no passado ou hoje |
| **UI** | Só aparece se `IsPurchased = true` |

---

### **IsGift** - Foi Presente
| Propriedade | Valor |
|---|---|
| **Tipo** | `bool` |
| **Padrão** | `false` |
| **Validação** | Nenhuma |
| **UI** | Checkbox simples |

---

### **ProductUrl** - Link do Produto
| Propriedade | Valor |
|---|---|
| **Tipo** | `string?` (nullable) |
| **Obrigatório** | ❌ Não |
| **Comprimento** | Máximo 500 caracteres |
| **Formato** | URL válido (HTTP/HTTPS) |
| **Validação Client-Side** | `type="url"` |
| **Validação Server-Side** | `[StringLength(500)]` |
| **Mensagem de Erro** | "URL inválido. Use: https://exemplo.com" |
| **Placeholder** | "https://www.loja.com/produto" |
| **UI Enhancement** | Ícone 🔗 + validação de domínio |
| **Exemplos Válidos** | "https://amazon.pt/produto", "https://chicco.pt" |
| **Exemplos Inválidos** | "google", "http://", "ftp://invalid" |

---

### **Notes** - Notas
| Propriedade | Valor |
|---|---|
| **Tipo** | `string?` (nullable) |
| **Obrigatório** | ❌ Não |
| **Comprimento** | Máximo 1000 caracteres |
| **Validação** | `[StringLength(1000)]` |
| **UI** | Textarea com auto-resize |

---

## 👥 BabyListManager

### **ManagerEmail** - Email do Gestor
| Propriedade | Valor |
|---|---|
| **Tipo** | `string` |
| **Obrigatório** | ✅ Sim |
| **Formato** | Email válido (RFC 5322) |
| **Validação Client-Side** | `type="email"` + `required` |
| **Validação Server-Side** | `[Required]` + `[EmailAddress]` |
| **Mensagem de Erro (vazio)** | "O email é obrigatório" |
| **Mensagem de Erro (inválido)** | "Email inválido" |
| **Mensagem de Erro (duplicado)** | "Este email já é gestor desta lista" |
| **Validação Custom** | Não pode ser igual ao criador da lista |

---

### **ManagerName** - Nome do Gestor
| Propriedade | Valor |
|---|---|
| **Tipo** | `string` |
| **Obrigatório** | ✅ Recomendado |
| **Comprimento** | Máximo 100 caracteres |
| **Validação** | `[StringLength(100)]` |

---

## 🔗 BabyListShare

### **SharedWithEmail** - Email do Convidado
| Propriedade | Valor |
|---|---|
| **Tipo** | `string` |
| **Obrigatório** | ✅ Sim |
| **Formato** | Email válido |
| **Validação** | `[Required]` + `[EmailAddress]` |
| **Validação Custom** | Não pode ser igual ao criador |

---

### **InviteMessage** - Mensagem de Convite
| Propriedade | Valor |
|---|---|
| **Tipo** | `string?` (nullable) |
| **Obrigatório** | ❌ Não |
| **Comprimento** | Máximo 500 caracteres |
| **Validação** | `[StringLength(500)]` |

---

## 🎁 BabyListReservation

### **ReservedBy** - Email de Quem Reservou
| Propriedade | Valor |
|---|---|
| **Tipo** | `string` |
| **Obrigatório** | ✅ Sim |
| **Formato** | Email válido |
| **Validação** | `[Required]` + `[EmailAddress]` |

---

### **ReservedByName** - Nome de Quem Reservou
| Propriedade | Valor |
|---|---|
| **Tipo** | `string` |
| **Obrigatório** | ✅ Sim |
| **Comprimento** | Máximo 100 caracteres |
| **Validação** | `[Required]` + `[StringLength(100)]` |
| **Mensagem de Erro** | "O nome é obrigatório" |

---

### **Quantity** - Quantidade Reservada
| Propriedade | Valor |
|---|---|
| **Tipo** | `int` |
| **Obrigatório** | ✅ Sim |
| **Range** | 1 até `Item.AvailableQuantity` |
| **Validação Custom** | Não pode exceder disponível |
| **Mensagem de Erro** | "Quantidade indisponível. Disponível: X" |

---

### **Message** - Mensagem para os Pais
| Propriedade | Valor |
|---|---|
| **Tipo** | `string?` (nullable) |
| **Obrigatório** | ❌ Não |
| **Comprimento** | Máximo 500 caracteres |
| **Validação** | `[StringLength(500)]` |

---

## 🔍 Validações Customizadas (Server-Side)

### **Validação 1: ShareCode Único**
```csharp
// No controller
while (await _context.BabyLists.AnyAsync(l => l.ShareCode == code))
{
    code = ShareCodeGenerator.Generate();
}
```

### **Validação 2: Quantidade Disponível**
```csharp
var item = await _context.BabyListItems.FindAsync(id);
if (reservation.Quantity > item.AvailableQuantity)
{
    ModelState.AddModelError("Quantity", 
        $"Apenas {item.AvailableQuantity} unidades disponíveis");
}
```

### **Validação 3: Email Não Duplicado em Managers**
```csharp
if (await _context.BabyListManagers
    .AnyAsync(m => m.BabyListId == listId && m.ManagerEmail == email))
{
    ModelState.AddModelError("ManagerEmail", 
        "Este email já é gestor desta lista");
}
```

### **Validação 4: Co-Gestor Diferente do Criador**
```csharp
if (coManagerEmail == User.Identity.Name)
{
    ModelState.AddModelError("coManagerEmail", 
        "Não podes adicionar-te como co-gestor");
}
```

---

## 📊 Resumo de Validações

| Campo | Obrigatório | Min | Max | Tipo | Mensagem Erro |
|---|---|---|---|---|---|
| `BabyList.Name` | ✅ | - | 100 chars | string | "O nome da lista é obrigatório" |
| `BabyList.Description` | ❌ | - | 500 chars | string? | - |
| `BabyList.BabyName` | ❌ | - | 100 chars | string? | - |
| `BabyList.ShareCode` | ❌ | - | 50 chars | string? | (gerado auto) |
| `BabyListItem.Name` | ✅ | - | 200 chars | string | "O nome do item é obrigatório" |
| `BabyListItem.Description` | ❌ | - | 1000 chars | string? | - |
| `BabyListItem.EstimatedCost` | ✅ | 0 | 10000 | decimal | "O custo estimado é obrigatório" |
| `BabyListItem.Quantity` | ✅ | 1 | 100 | int | "A quantidade é obrigatória" |
| `BabyListItem.ProductUrl` | ❌ | - | 500 chars | string? | "URL inválido" |
| `BabyListItem.Notes` | ❌ | - | 1000 chars | string? | - |
| `BabyListManager.ManagerEmail` | ✅ | - | - | string | "O email é obrigatório" |
| `BabyListReservation.ReservedBy` | ✅ | - | - | string | "O email é obrigatório" |
| `BabyListReservation.ReservedByName` | ✅ | - | 100 chars | string | "O nome é obrigatório" |
| `BabyListReservation.Quantity` | ✅ | 1 | Disponível | int | "Quantidade indisponível" |

---

**Navegação:**
- [← Resumo UI/UX](00-Summary-UI-UX-Improvements.md)
- [Voltar ao Índice →](../README.md)
