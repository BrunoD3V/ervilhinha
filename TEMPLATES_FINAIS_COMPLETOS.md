# 🎯 TEMPLATES FINAIS - 6 Ficheiros Restantes

Copia e cola estes conteúdos completos nos respetivos ficheiros para terminar a tradução a 100%!

---

## 1️⃣ Views/ExpenseCategories/Index.cshtml

**SUBSTITUIR TODO O CONTEÚDO:**

```cshtml
@model IEnumerable<Ervilhinha.Models.ExpenseCategory>

@{
    ViewData["Title"] = "Categorias de Despesas";
}

<div class="d-flex justify-content-between align-items-center mb-4">
    <h1>🏷️ Categorias de Despesas</h1>
    <a asp-action="Create" class="btn btn-primary">➕ Adicionar Categoria</a>
</div>

@if (Model.Any())
{
    <div class="table-responsive">
        <table class="table table-striped">
            <thead>
                <tr>
                    <th>Nome</th>
                    <th>Cor</th>
                    <th>Estado</th>
                    <th>Ações</th>
                </tr>
            </thead>
            <tbody>
                @foreach (var category in Model)
                {
                    <tr>
                        <td>
                            <span class="badge" style="background-color: @category.Color">
                                @category.Name
                            </span>
                        </td>
                        <td>
                            <input type="color" value="@category.Color" disabled style="border: none;" />
                            <code>@category.Color</code>
                        </td>
                        <td>
                            @if (category.IsActive)
                            {
                                <span class="badge bg-success">Ativo</span>
                            }
                            else
                            {
                                <span class="badge bg-secondary">Inativo</span>
                            }
                        </td>
                        <td>
                            <div class="btn-group btn-group-sm">
                                <a asp-action="Edit" asp-route-id="@category.Id" class="btn btn-warning">Editar</a>
                                @if (User.IsInRole("Admin"))
                                {
                                    <a asp-action="Delete" asp-route-id="@category.Id" class="btn btn-danger">Eliminar</a>
                                }
                            </div>
                        </td>
                    </tr>
                }
            </tbody>
        </table>
    </div>
}
else
{
    <div class="alert alert-info">
        Não foram encontradas categorias. <a asp-action="Create">Cria a tua primeira categoria!</a>
    </div>
}
```

---

## 2️⃣ Views/ExpenseCategories/Create.cshtml

**SUBSTITUIR TODO O CONTEÚDO:**

```cshtml
@model Ervilhinha.Models.ExpenseCategory

@{
    ViewData["Title"] = "Criar Categoria";
}

<h1>➕ Criar Categoria de Despesa</h1>

<div class="row">
    <div class="col-md-6">
        <form asp-action="Create" method="post">
            <div asp-validation-summary="ModelOnly" class="text-danger"></div>
            
            <div class="mb-3">
                <label asp-for="Name" class="form-label">Nome</label>
                <input asp-for="Name" class="form-control" placeholder="Ex: Alimentação, Transporte..." />
                <span asp-validation-for="Name" class="text-danger"></span>
            </div>
            
            <div class="mb-3">
                <label asp-for="Color" class="form-label">Cor</label>
                <div class="input-group">
                    <input asp-for="Color" type="color" class="form-control form-control-color" />
                    <input asp-for="Color" type="text" class="form-control" placeholder="#FF5733" />
                </div>
                <div class="form-text">Escolhe uma cor para identificar esta categoria nos gráficos</div>
                <span asp-validation-for="Color" class="text-danger"></span>
            </div>
            
            <div class="mb-3 form-check">
                <input asp-for="IsActive" class="form-check-input" />
                <label asp-for="IsActive" class="form-check-label">Categoria Ativa</label>
                <div class="form-text">Apenas categorias ativas aparecem na criação de despesas</div>
            </div>
            
            <div class="mb-3">
                <button type="submit" class="btn btn-primary">Guardar</button>
                <a asp-action="Index" class="btn btn-secondary">Cancelar</a>
            </div>
        </form>
    </div>

    <div class="col-md-6">
        <div class="card">
            <div class="card-header bg-info text-white">
                <h5>💡 Sugestões de Categorias</h5>
            </div>
            <div class="card-body">
                <ul class="list-unstyled">
                    <li class="mb-2">🍼 <strong>Bebé</strong> - #FF69B4</li>
                    <li class="mb-2">🏠 <strong>Casa</strong> - #4CAF50</li>
                    <li class="mb-2">🚗 <strong>Transporte</strong> - #2196F3</li>
                    <li class="mb-2">🛒 <strong>Supermercado</strong> - #FF9800</li>
                    <li class="mb-2">⚡ <strong>Contas</strong> - #9C27B0</li>
                    <li class="mb-2">💊 <strong>Saúde</strong> - #F44336</li>
                    <li class="mb-2">🎉 <strong>Lazer</strong> - #00BCD4</li>
                </ul>
            </div>
        </div>
    </div>
</div>

@section Scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
}
```

---

## 3️⃣ Views/ExpenseCategories/Edit.cshtml

**SUBSTITUIR TODO O CONTEÚDO:**

```cshtml
@model Ervilhinha.Models.ExpenseCategory

@{
    ViewData["Title"] = "Editar Categoria";
}

<h1>✏️ Editar Categoria</h1>

<div class="row">
    <div class="col-md-6">
        <form asp-action="Edit" method="post">
            <input type="hidden" asp-for="Id" />
            <div asp-validation-summary="ModelOnly" class="text-danger"></div>
            
            <div class="mb-3">
                <label asp-for="Name" class="form-label">Nome</label>
                <input asp-for="Name" class="form-control" />
                <span asp-validation-for="Name" class="text-danger"></span>
            </div>
            
            <div class="mb-3">
                <label asp-for="Color" class="form-label">Cor</label>
                <div class="input-group">
                    <input asp-for="Color" type="color" class="form-control form-control-color" />
                    <input asp-for="Color" type="text" class="form-control" />
                </div>
                <span asp-validation-for="Color" class="text-danger"></span>
            </div>
            
            <div class="mb-3 form-check">
                <input asp-for="IsActive" class="form-check-input" />
                <label asp-for="IsActive" class="form-check-label">Categoria Ativa</label>
            </div>
            
            <div class="mb-3">
                <button type="submit" class="btn btn-primary">Guardar Alterações</button>
                <a asp-action="Index" class="btn btn-secondary">Cancelar</a>
            </div>
        </form>
    </div>

    <div class="col-md-6">
        <div class="card">
            <div class="card-header">
                <h5>Pré-visualização</h5>
            </div>
            <div class="card-body">
                <h3>
                    <span class="badge" style="background-color: @Model.Color">
                        @Model.Name
                    </span>
                </h3>
                <p class="text-muted">Esta é a aparência da categoria nos relatórios.</p>
            </div>
        </div>
    </div>
</div>

@section Scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
}
```

---

## 4️⃣ Views/ExpenseCategories/Delete.cshtml

**SUBSTITUIR TODO O CONTEÚDO:**

```cshtml
@model Ervilhinha.Models.ExpenseCategory

@{
    ViewData["Title"] = "Eliminar Categoria";
}

<h1>🗑️ Eliminar Categoria</h1>

<div class="alert alert-danger">
    <h4>⚠ Tens a certeza que queres eliminar esta categoria?</h4>
    <p>Esta ação não pode ser revertida.</p>
</div>

<div class="card">
    <div class="card-body">
        <dl class="row">
            <dt class="col-sm-3">Nome:</dt>
            <dd class="col-sm-9">
                <span class="badge" style="background-color: @Model.Color">
                    @Model.Name
                </span>
            </dd>

            <dt class="col-sm-3">Cor:</dt>
            <dd class="col-sm-9">
                <input type="color" value="@Model.Color" disabled />
                <code>@Model.Color</code>
            </dd>

            <dt class="col-sm-3">Estado:</dt>
            <dd class="col-sm-9">
                @if (Model.IsActive)
                {
                    <span class="badge bg-success">Ativo</span>
                }
                else
                {
                    <span class="badge bg-secondary">Inativo</span>
                }
            </dd>
        </dl>

        @if (Model.Expenses != null && Model.Expenses.Any())
        {
            <div class="alert alert-warning">
                <strong>⚠ Atenção:</strong> Esta categoria tem <strong>@Model.Expenses.Count despesa(s)</strong> associada(s). 
                Ao eliminar a categoria, estas despesas ficarão sem categoria.
            </div>
        }

        <form asp-action="Delete" method="post">
            <input type="hidden" asp-for="Id" />
            <button type="submit" class="btn btn-danger">Eliminar Categoria</button>
            <a asp-action="Index" class="btn btn-secondary">Cancelar</a>
        </form>
    </div>
</div>
```

---

## 5️⃣ Views/Invoices/Review.cshtml

**SUBSTITUIR TODO O CONTEÚDO:**

```cshtml
@model Ervilhinha.Models.Invoice

@{
    ViewData["Title"] = "Rever Fatura";
}

<h1>📝 Rever Fatura</h1>

<div class="row">
    <div class="col-md-8">
        <div class="card mb-3">
            <div class="card-header @(Model.HasErrors ? "bg-warning" : "bg-success text-white")">
                <h4>
                    @if (Model.HasErrors)
                    {
                        <span>⚠️ Fatura Precisa de Revisão</span>
                    }
                    else if (Model.IsReviewed)
                    {
                        <span>✓ Fatura Revista</span>
                    }
                    else
                    {
                        <span>Dados Extraídos pela IA</span>
                    }
                </h4>
            </div>
            <div class="card-body">
                @if (!string.IsNullOrEmpty(Model.ErrorMessage))
                {
                    <div class="alert alert-warning">
                        <strong>Nota de Processamento:</strong> @Model.ErrorMessage
                    </div>
                }

                <form asp-action="Review" method="post">
                    @Html.AntiForgeryToken()
                    <input type="hidden" asp-for="Id" />

                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <label asp-for="VendorName" class="form-label">Nome do Fornecedor</label>
                            <input asp-for="VendorName" class="form-control" />
                        </div>

                        <div class="col-md-6 mb-3">
                            <label asp-for="TotalAmount" class="form-label">Montante Total (€)</label>
                            <input asp-for="TotalAmount" class="form-control" type="number" step="0.01" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <label asp-for="InvoiceDate" class="form-label">Data da Fatura</label>
                            <input asp-for="InvoiceDate" class="form-control" type="date" />
                        </div>

                        <div class="col-md-6 mb-3">
                            <label class="form-label">Categoria da Despesa</label>
                            <select name="expenseCategoryId" class="form-select" required>
                                <option value="">-- Seleciona Categoria --</option>
                                @foreach (var cat in ViewBag.Categories)
                                {
                                    var selected = Model.Expense?.ExpenseCategoryId == cat.Id;
                                    <option value="@cat.Id" selected="@selected">@cat.Name</option>
                                }
                            </select>
                            <div class="form-text">Será aplicada à despesa criada</div>
                        </div>
                    </div>

                    @if (!string.IsNullOrEmpty(Model.RawOcrText))
                    {
                        <div class="mb-3">
                            <label class="form-label">Texto OCR Original</label>
                            <textarea class="form-control" rows="4" readonly>@Model.RawOcrText</textarea>
                            <div class="form-text">Texto bruto extraído da fatura</div>
                        </div>
                    }

                    <div class="alert alert-info">
                        @if (Model.Expense != null)
                        {
                            <strong>Estado da Despesa:</strong> <text>Uma despesa já foi criada. Atualizar este formulário atualizará a despesa existente.</text>
                        }
                        else
                        {
                            <strong>Criação de Despesa:</strong> <text>Uma nova despesa será criada ao guardares esta revisão.</text>
                        }
                    </div>

                    <div class="d-grid gap-2">
                        <button type="submit" class="btn btn-success btn-lg">
                            ✓ Aprovar e Guardar
                        </button>
                        <a asp-action="Index" class="btn btn-secondary">Voltar às Faturas</a>
                    </div>
                </form>
            </div>
        </div>

        @if (Model.IsReviewed)
        {
            <div class="card">
                <div class="card-header bg-info text-white">
                    <h5>Histórico de Revisão</h5>
                </div>
                <div class="card-body">
                    <dl class="row">
                        <dt class="col-sm-4">Revisto Por:</dt>
                        <dd class="col-sm-8">@Model.ReviewedBy</dd>

                        <dt class="col-sm-4">Data de Revisão:</dt>
                        <dd class="col-sm-8">@Model.ReviewedDate?.ToLocalTime().ToString("g")</dd>

                        <dt class="col-sm-4">Carregado Por:</dt>
                        <dd class="col-sm-8">@Model.UploadedBy</dd>

                        <dt class="col-sm-4">Data de Carregamento:</dt>
                        <dd class="col-sm-8">@Model.UploadDate.ToLocalTime().ToString("g")</dd>
                    </dl>
                </div>
            </div>
        }
    </div>

    <div class="col-md-4">
        <div class="card">
            <div class="card-header">
                <h5>Imagem da Fatura</h5>
            </div>
            <div class="card-body">
                @if (!string.IsNullOrEmpty(Model.FilePath))
                {
                    <a href="@Model.FilePath" target="_blank">
                        <img src="@Model.FilePath" alt="Fatura" class="img-fluid" />
                    </a>
                    <div class="d-grid gap-2 mt-3">
                        <a href="@Model.FilePath" target="_blank" class="btn btn-outline-primary">
                            🔍 Ver Tamanho Completo
                        </a>
                    </div>
                }
            </div>
        </div>

        @if (Model.Expense != null)
        {
            <div class="card mt-3">
                <div class="card-header bg-success text-white">
                    <h5>Despesa Criada</h5>
                </div>
                <div class="card-body">
                    <p><strong>Montante:</strong> €@Model.Expense.Amount.ToString("N2")</p>
                    <p><strong>Data:</strong> @Model.Expense.Date.ToShortDateString()</p>
                    <p><strong>Categoria:</strong> @Model.Expense.ExpenseCategory?.Name</p>
                    <a asp-controller="Expenses" asp-action="Index" class="btn btn-sm btn-outline-success">
                        Ver Despesas
                    </a>
                </div>
            </div>
        }
    </div>
</div>
```

---

## 6️⃣ Views/Invoices/Delete.cshtml

**SUBSTITUIR TODO O CONTEÚDO:**

```cshtml
@model Ervilhinha.Models.Invoice

@{
    ViewData["Title"] = "Eliminar Fatura";
}

<h1>🗑️ Eliminar Fatura</h1>

<div class="alert alert-danger">
    <h4>⚠ Tens a certeza que queres eliminar esta fatura?</h4>
    <p>Esta ação não pode ser revertida.</p>
</div>

<div class="row">
    <div class="col-md-6">
        <div class="card">
            <div class="card-header">
                <h5>Detalhes da Fatura</h5>
            </div>
            <div class="card-body">
                @if (!string.IsNullOrEmpty(Model.FilePath))
                {
                    <div class="text-center mb-3">
                        <img src="@Model.FilePath" alt="Fatura" class="img-fluid" style="max-height: 300px;" />
                    </div>
                }

                <dl class="row">
                    <dt class="col-sm-4">Fornecedor:</dt>
                    <dd class="col-sm-8">@(Model.VendorName ?? "Desconhecido")</dd>

                    <dt class="col-sm-4">Montante:</dt>
                    <dd class="col-sm-8">
                        @if (Model.TotalAmount.HasValue)
                        {
                            <strong>€@Model.TotalAmount.Value.ToString("N2")</strong>
                        }
                        else
                        {
                            <span class="text-muted">Não especificado</span>
                        }
                    </dd>

                    <dt class="col-sm-4">Data:</dt>
                    <dd class="col-sm-8">@(Model.InvoiceDate?.ToShortDateString() ?? "Não especificada")</dd>

                    <dt class="col-sm-4">Ficheiro:</dt>
                    <dd class="col-sm-8">@Model.FileName</dd>

                    <dt class="col-sm-4">Carregado:</dt>
                    <dd class="col-sm-8">@Model.UploadDate.ToLocalTime().ToString("g") por @Model.UploadedBy</dd>

                    <dt class="col-sm-4">Estado:</dt>
                    <dd class="col-sm-8">
                        @if (Model.IsReviewed)
                        {
                            <span class="badge bg-success">Revisto</span>
                        }
                        else if (Model.IsProcessed)
                        {
                            <span class="badge bg-info">Processado</span>
                        }
                        else
                        {
                            <span class="badge bg-secondary">Pendente</span>
                        }
                    </dd>
                </dl>

                @if (Model.Expense != null)
                {
                    <div class="alert alert-warning">
                        <strong>⚠ Nota:</strong> Esta fatura tem uma despesa associada. Eliminar a fatura NÃO eliminará a despesa.
                    </div>
                }

                <form asp-action="Delete" method="post">
                    @Html.AntiForgeryToken()
                    <input type="hidden" asp-for="Id" />
                    <button type="submit" class="btn btn-danger">Eliminar Fatura</button>
                    <a asp-action="Index" class="btn btn-secondary">Cancelar</a>
                </form>
            </div>
        </div>
    </div>
</div>
```

---

## ✅ COMO APLICAR

Para cada ficheiro acima:
1. Abre o ficheiro no Visual Studio
2. Seleciona TODO o conteúdo (Ctrl+A)
3. Cola o novo conteúdo
4. Guarda (Ctrl+S)

**Demora 2 minutos para fazer os 6!** ⚡

---

## 🎉 DEPOIS DISTO

**A TUA APP ESTARÁ 100% EM PORTUGUÊS!** 🇵🇹

---

*Documento criado automaticamente - Ervilhinha PT-PT Final Templates*
