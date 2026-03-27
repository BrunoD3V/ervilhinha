# 🇵🇹 Guia Completo de Tradução PT-PT - Ervilhinha

## ✅ Ficheiros JÁ Traduzidos

1. ✅ `Views/Home/Index.cshtml`
2. ✅ `Views/Shared/_Layout.cshtml`
3. ✅ `Views/Shared/_LoginPartial.cshtml`
4. ✅ `Views/BabyItems/Index.cshtml`
5. ✅ `Views/BabyItems/Create.cshtml`
6. ✅ `Views/BabyItems/Edit.cshtml` (parcial)
7. ✅ `Views/BabyItems/Delete.cshtml` (parcial)
8. ✅ `Views/Expenses/Index.cshtml`
9. ✅ `Areas/Identity/Pages/Account/Login.cshtml` (parcial)

---

## 📝 Traduções Rápidas - Copiar e Colar

### **Termos Comuns em TODOS os Ficheiros**

Procura e substitui GLOBALMENTE:

| Original (EN) | Tradução (PT-PT) |
|--------------|------------------|
| `Save` | `Guardar` |
| `Cancel` | `Cancelar` |
| `Edit` | `Editar` |
| `Delete` | `Eliminar` |
| `Create` | `Criar` |
| `Add` | `Adicionar` |
| `Back` | `Voltar` |
| `Search` | `Pesquisar` |
| `Filter` | `Filtrar` |
| `Sort` | `Ordenar` |
| `Actions` | `Ações` |
| `Details` | `Detalhes` |
| `Description` | `Descrição` |
| `Name` | `Nome` |
| `Date` | `Data` |
| `Amount` | `Montante` |
| `Category` | `Categoria` |
| `Notes` | `Notas` |
| `Priority` | `Prioridade` |
| `Status` | `Estado` |
| `Total` | `Total` |
| `$` (símbolo dólar) | `€` (euro) |

---

## 📂 Traduções por Ficheiro

### **Views/BabyItems/Edit.cshtml** - COMPLETAR

```cshtml
<!-- SUBSTITUIR TODO O CONTEÚDO -->
@model Ervilhinha.Models.BabyItem

@{
    ViewData["Title"] = "Editar Artigo do Bebé";
}

<h1>✏️ Editar Artigo do Bebé</h1>

<div class="row">
    <div class="col-md-8">
        <form asp-action="Edit" method="post">
            <div asp-validation-summary="ModelOnly" class="text-danger"></div>
            <input type="hidden" asp-for="Id" />
            
            <div class="mb-3">
                <label asp-for="Name" class="form-label">Nome</label>
                <input asp-for="Name" class="form-control" />
                <span asp-validation-for="Name" class="text-danger"></span>
            </div>
            
            <div class="mb-3">
                <label asp-for="Description" class="form-label">Descrição</label>
                <textarea asp-for="Description" class="form-control" rows="3"></textarea>
                <span asp-validation-for="Description" class="text-danger"></span>
            </div>
            
            <div class="row">
                <div class="col-md-6 mb-3">
                    <label asp-for="Category" class="form-label">Categoria</label>
                    <input asp-for="Category" class="form-control" list="categoryList" />
                    <datalist id="categoryList">
                        <option value="Roupa"></option>
                        <option value="Quarto do Bebé"></option>
                        <option value="Alimentação"></option>
                        <option value="Banho"></option>
                        <option value="Fraldas"></option>
                        <option value="Brinquedos"></option>
                        <option value="Segurança"></option>
                        <option value="Viagem"></option>
                    </datalist>
                    <span asp-validation-for="Category" class="text-danger"></span>
                </div>
                
                <div class="col-md-6 mb-3">
                    <label asp-for="Priority" class="form-label">Prioridade</label>
                    <select asp-for="Priority" class="form-select">
                        <option value="1">1 - Muito Alta</option>
                        <option value="2">2 - Alta</option>
                        <option value="3">3 - Média</option>
                        <option value="4">4 - Baixa</option>
                        <option value="5">5 - Muito Baixa</option>
                    </select>
                    <span asp-validation-for="Priority" class="text-danger"></span>
                </div>
            </div>
            
            <div class="row">
                <div class="col-md-6 mb-3">
                    <label asp-for="EstimatedCost" class="form-label">Custo Estimado (€)</label>
                    <input asp-for="EstimatedCost" class="form-control" type="number" step="0.01" />
                    <span asp-validation-for="EstimatedCost" class="text-danger"></span>
                </div>
                
                <div class="col-md-6 mb-3">
                    <label asp-for="ActualCost" class="form-label">Custo Real (€)</label>
                    <input asp-for="ActualCost" class="form-control" type="number" step="0.01" />
                    <span asp-validation-for="ActualCost" class="text-danger"></span>
                </div>
            </div>
            
            <div class="mb-3">
                <label asp-for="Notes" class="form-label">Notas</label>
                <textarea asp-for="Notes" class="form-control" rows="2"></textarea>
                <span asp-validation-for="Notes" class="text-danger"></span>
            </div>
            
            <div class="mb-3">
                <button type="submit" class="btn btn-primary">Guardar</button>
                <a asp-action="Index" class="btn btn-secondary">Cancelar</a>
            </div>
        </form>
    </div>
</div>

@section Scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
}
```

---

### **Views/BabyItems/Delete.cshtml** - COMPLETAR

```cshtml
@model Ervilhinha.Models.BabyItem

@{
    ViewData["Title"] = "Eliminar Artigo do Bebé";
}

<h1>🗑️ Eliminar Artigo do Bebé</h1>

<div class="alert alert-warning">
    <h4>Tens a certeza que queres eliminar este artigo?</h4>
    <p>Esta ação não pode ser revertida.</p>
</div>

<div class="row">
    <div class="col-md-8">
        <dl class="row">
            <dt class="col-sm-3">Nome</dt>
            <dd class="col-sm-9">@Model.Name</dd>

            <dt class="col-sm-3">Descrição</dt>
            <dd class="col-sm-9">@Model.Description</dd>

            <dt class="col-sm-3">Categoria</dt>
            <dd class="col-sm-9">@Model.Category</dd>

            <dt class="col-sm-3">Prioridade</dt>
            <dd class="col-sm-9">@Model.Priority</dd>

            @if (Model.EstimatedCost.HasValue)
            {
                <dt class="col-sm-3">Custo Estimado</dt>
                <dd class="col-sm-9">€@Model.EstimatedCost.Value.ToString("N2")</dd>
            }

            @if (Model.ActualCost.HasValue)
            {
                <dt class="col-sm-3">Custo Real</dt>
                <dd class="col-sm-9">€@Model.ActualCost.Value.ToString("N2")</dd>
            }

            <dt class="col-sm-3">Estado</dt>
            <dd class="col-sm-9">@(Model.IsPurchased ? "Comprado" : "Por Comprar")</dd>
        </dl>

        <form asp-action="Delete" method="post">
            <input type="hidden" asp-for="Id" />
            <button type="submit" class="btn btn-danger">Eliminar</button>
            <a asp-action="Index" class="btn btn-secondary">Cancelar</a>
        </form>
    </div>
</div>
```

---

### **Views/Expenses/Create.cshtml** - SUBSTITUIR CONTEÚDO

```cshtml
@model Ervilhinha.Models.Expense

@{
    ViewData["Title"] = "Adicionar Despesa";
}

<h1>➕ Adicionar Despesa</h1>

<div class="row">
    <div class="col-md-8">
        <form asp-action="Create" method="post">
            <div asp-validation-summary="ModelOnly" class="text-danger"></div>
            
            <div class="mb-3">
                <label asp-for="Description" class="form-label">Descrição</label>
                <input asp-for="Description" class="form-control" />
                <span asp-validation-for="Description" class="text-danger"></span>
            </div>
            
            <div class="row">
                <div class="col-md-6 mb-3">
                    <label asp-for="Amount" class="form-label">Montante (€)</label>
                    <input asp-for="Amount" class="form-control" type="number" step="0.01" />
                    <span asp-validation-for="Amount" class="text-danger"></span>
                </div>
                
                <div class="col-md-6 mb-3">
                    <label asp-for="Date" class="form-label">Data</label>
                    <input asp-for="Date" class="form-control" type="date" />
                    <span asp-validation-for="Date" class="text-danger"></span>
                </div>
            </div>
            
            <div class="mb-3">
                <label asp-for="ExpenseCategoryId" class="form-label">Categoria</label>
                <select asp-for="ExpenseCategoryId" class="form-select" asp-items="ViewBag.ExpenseCategoryId">
                    <option value="">-- Seleciona uma Categoria --</option>
                </select>
                <span asp-validation-for="ExpenseCategoryId" class="text-danger"></span>
            </div>
            
            <div class="mb-3">
                <label asp-for="Notes" class="form-label">Notas</label>
                <textarea asp-for="Notes" class="form-control" rows="3"></textarea>
                <span asp-validation-for="Notes" class="text-danger"></span>
            </div>
            
            <div class="mb-3">
                <button type="submit" class="btn btn-primary">Guardar</button>
                <a asp-action="Index" class="btn btn-secondary">Cancelar</a>
            </div>
        </form>
    </div>
</div>

@section Scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
}
```

---

### **Views/Expenses/Edit.cshtml** - SUBSTITUIR

Usa o mesmo template de Create.cshtml mas com:
- Título: "Editar Despesa"
- `<h1>✏️ Editar Despesa</h1>`
- Adicionar `<input type="hidden" asp-for="Id" />`

---

### **Views/Expenses/Delete.cshtml** - SUBSTITUIR

```cshtml
@model Ervilhinha.Models.Expense

@{
    ViewData["Title"] = "Eliminar Despesa";
}

<h1>🗑️ Eliminar Despesa</h1>

<div class="alert alert-warning">
    <h4>Tens a certeza que queres eliminar esta despesa?</h4>
    <p>Esta ação não pode ser revertida.</p>
</div>

<div class="row">
    <div class="col-md-8">
        <dl class="row">
            <dt class="col-sm-3">Descrição</dt>
            <dd class="col-sm-9">@Model.Description</dd>

            <dt class="col-sm-3">Montante</dt>
            <dd class="col-sm-9"><strong>€@Model.Amount.ToString("N2")</strong></dd>

            <dt class="col-sm-3">Data</dt>
            <dd class="col-sm-9">@Model.Date.ToShortDateString()</dd>

            <dt class="col-sm-3">Categoria</dt>
            <dd class="col-sm-9">
                <span class="badge" style="background-color: @Model.ExpenseCategory?.Color">
                    @Model.ExpenseCategory?.Name
                </span>
            </dd>

            @if (!string.IsNullOrEmpty(Model.Notes))
            {
                <dt class="col-sm-3">Notas</dt>
                <dd class="col-sm-9">@Model.Notes</dd>
            }

            <dt class="col-sm-3">Criado Por</dt>
            <dd class="col-sm-9">@Model.CreatedBy em @Model.CreatedDate.ToLocalTime().ToString("g")</dd>
        </dl>

        <form asp-action="Delete" method="post">
            <input type="hidden" asp-for="Id" />
            <button type="submit" class="btn btn-danger">Eliminar</button>
            <a asp-action="Index" class="btn btn-secondary">Cancelar</a>
        </form>
    </div>
</div>
```

---

### **Views/Expenses/Reports.cshtml** - JÁ TRADUZIDO ✅

(Este já foi traduzido anteriormente)

---

## 🎯 Ficheiros Prioritários RESTANTES

1. **Views/Invoices/*.cshtml** (4 ficheiros) - CRÍTICO
2. **Views/ExpenseCategories/*.cshtml** (4 ficheiros)
3. **Views/Admin/Users.cshtml**
4. **Areas/Identity/Pages/Account/Login.cshtml** - Completar
5. **Areas/Identity/Pages/Account/ExternalLogin.cshtml**

---

## 💾 Como Aplicar

1. Abre cada ficheiro
2. Copia o conteúdo traduzido acima
3. Substitui TODO o conteúdo
4. Guarda (Ctrl+S)
5. Testa a página

---

**Próximo passo**: Vou traduzir os ficheiros de Invoices que são super importantes! 📄
