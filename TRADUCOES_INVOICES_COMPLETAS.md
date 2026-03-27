# 🚀 TRADUÇÕES FINAIS - COPIAR E COLAR

## Views/Invoices/Upload.cshtml - COMPLETO

```cshtml
@{
    ViewData["Title"] = "Carregar Fatura";
}

<h1>📸 Carregar Fatura</h1>

<div class="row">
    <div class="col-md-8">
        <div class="card">
            <div class="card-header bg-primary text-white">
                <h4>Scanner de Faturas IA</h4>
            </div>
            <div class="card-body">
                <div class="alert alert-info">
                    <h5>📷 Como funciona:</h5>
                    <ol>
                        <li>Tira uma foto à fatura ou carrega um PDF</li>
                        <li>A IA extrai automaticamente o fornecedor, data e montante</li>
                        <li>Revê os dados extraídos</li>
                        <li>A despesa é criada automaticamente!</li>
                    </ol>
                </div>

                <form asp-action="Upload" method="post" enctype="multipart/form-data">
                    @Html.AntiForgeryToken()
                    <div asp-validation-summary="All" class="text-danger"></div>

                    <div class="mb-3">
                        <label for="file" class="form-label">Seleciona Imagem ou PDF da Fatura</label>
                        <input type="file" 
                               name="file" 
                               id="file" 
                               class="form-control" 
                               accept="image/*,application/pdf"
                               capture="environment"
                               required />
                        <div class="form-text">
                            Formatos suportados: JPG, PNG, PDF. Podes usar a tua câmara para tirar uma foto!
                        </div>
                    </div>

                    <div class="mb-3">
                        <div id="preview" class="mt-3"></div>
                    </div>

                    <div class="d-grid gap-2">
                        <button type="submit" class="btn btn-primary btn-lg">
                            🤖 Carregar & Processar com IA
                        </button>
                        <a asp-action="Index" class="btn btn-secondary">Cancelar</a>
                    </div>
                </form>
            </div>
        </div>
    </div>

    <div class="col-md-4">
        <div class="card">
            <div class="card-header bg-success text-white">
                <h5>✨ Dicas para Melhores Resultados</h5>
            </div>
            <div class="card-body">
                <ul class="list-unstyled">
                    <li class="mb-2">✓ Garante boa iluminação</li>
                    <li class="mb-2">✓ Captura a fatura inteira</li>
                    <li class="mb-2">✓ Mantém a fatura plana</li>
                    <li class="mb-2">✓ Evita sombras e reflexos</li>
                    <li class="mb-2">✓ Certifica-te que o texto está legível</li>
                </ul>

                <hr />

                <h6>🔒 Privacidade & Segurança</h6>
                <p class="small text-muted">
                    As tuas faturas são processadas de forma segura e armazenadas apenas na tua conta.
                    @if (string.IsNullOrEmpty(ViewBag.AzureConfigured))
                    {
                        <br /><br />
                        <strong>Nota:</strong> <text>Azure Form Recognizer não está configurado. O sistema usará dados de demonstração.</text>
                    }
                </p>
            </div>
        </div>
    </div>
</div>

@section Scripts {
    <script>
        document.getElementById('file').addEventListener('change', function(e) {
            const file = e.target.files[0];
            const preview = document.getElementById('preview');
            
            if (file) {
                const reader = new FileReader();
                reader.onload = function(e) {
                    if (file.type.startsWith('image/')) {
                        preview.innerHTML = `
                            <div class="card">
                                <div class="card-header">Pré-visualização</div>
                                <div class="card-body text-center">
                                    <img src="${e.target.result}" class="img-fluid" style="max-height: 400px;" />
                                </div>
                            </div>
                        `;
                    } else {
                        preview.innerHTML = `
                            <div class="alert alert-success">
                                <strong>📄 Ficheiro PDF selecionado:</strong> ${file.name}
                            </div>
                        `;
                    }
                };
                reader.readAsDataURL(file);
            }
        });
    </script>
}
```

---

## Views/Invoices/Review.cshtml - TRADUZIR ESTE FICHEIRO

Abre o ficheiro `Views/Invoices/Review.cshtml` e substitui TODO o conteúdo por:

```cshtml
@model Ervilhinha.Models.Invoice

@{
    ViewData["Title"] = "Rever Fatura";
}

<h1>📝 Rever Fatura</h1>

<div class="row">
    <div class="col-md-5">
        <div class="card">
            <div class="card-header bg-info text-white">
                <h5>📄 Imagem da Fatura</h5>
            </div>
            <div class="card-body">
                @if (!string.IsNullOrEmpty(Model.FilePath))
                {
                    <img src="@Model.FilePath" alt="Fatura" class="img-fluid" />
                }
                else
                {
                    <p class="text-muted">Imagem não disponível</p>
                }
                
                <hr />
                
                <dl class="row">
                    <dt class="col-sm-5">Nome do Ficheiro:</dt>
                    <dd class="col-sm-7">@Model.FileName</dd>
                    
                    <dt class="col-sm-5">Carregado em:</dt>
                    <dd class="col-sm-7">@Model.UploadDate.ToLocalTime().ToString("g")</dd>
                    
                    <dt class="col-sm-5">Carregado por:</dt>
                    <dd class="col-sm-7">@Model.UploadedBy</dd>
                </dl>
            </div>
        </div>
    </div>

    <div class="col-md-7">
        <div class="card">
            <div class="card-header bg-primary text-white">
                <h5>🤖 Dados Extraídos pela IA</h5>
            </div>
            <div class="card-body">
                @if (Model.HasErrors)
                {
                    <div class="alert alert-warning">
                        <strong>⚠ Atenção:</strong> A IA não conseguiu extrair todos os dados. Por favor, revê e corrige manualmente.
                    </div>
                }

                <form asp-action="ApproveInvoice" method="post">
                    @Html.AntiForgeryToken()
                    <input type="hidden" name="invoiceId" value="@Model.Id" />
                    
                    <div class="mb-3">
                        <label for="vendorName" class="form-label">Nome do Fornecedor</label>
                        <input type="text" 
                               id="vendorName" 
                               name="vendorName" 
                               class="form-control" 
                               value="@Model.VendorName" 
                               required />
                    </div>
                    
                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <label for="totalAmount" class="form-label">Montante Total (€)</label>
                            <input type="number" 
                                   id="totalAmount" 
                                   name="totalAmount" 
                                   class="form-control" 
                                   step="0.01" 
                                   value="@Model.TotalAmount" 
                                   required />
                        </div>
                        
                        <div class="col-md-6 mb-3">
                            <label for="invoiceDate" class="form-label">Data da Fatura</label>
                            <input type="date" 
                                   id="invoiceDate" 
                                   name="invoiceDate" 
                                   class="form-control" 
                                   value="@Model.InvoiceDate?.ToString("yyyy-MM-dd")" 
                                   required />
                        </div>
                    </div>

                    <div class="mb-3">
                        <label for="categoryId" class="form-label">Categoria da Despesa</label>
                        <select id="categoryId" name="categoryId" class="form-select" required>
                            <option value="">-- Seleciona uma Categoria --</option>
                            @foreach (var cat in ViewBag.Categories)
                            {
                                <option value="@cat.Id">@cat.Name</option>
                            }
                        </select>
                    </div>

                    <div class="mb-3">
                        <label for="notes" class="form-label">Notas (opcional)</label>
                        <textarea id="notes" 
                                  name="notes" 
                                  class="form-control" 
                                  rows="2"
                                  placeholder="Adiciona notas sobre esta fatura..."></textarea>
                    </div>

                    @if (Model.IsProcessed && !Model.IsReviewed)
                    {
                        <div class="alert alert-info">
                            <strong>ℹ️ Fatura processada:</strong> Revê os dados acima e aprova para criar a despesa.
                        </div>
                    }

                    @if (Model.IsReviewed && Model.Expense != null)
                    {
                        <div class="alert alert-success">
                            <strong>✓ Fatura já revista!</strong> Despesa criada com sucesso.
                            <br />
                            <a asp-controller="Expenses" asp-action="Index" class="alert-link">Ver Despesas</a>
                        </div>
                    }

                    <div class="d-grid gap-2">
                        @if (!Model.IsReviewed)
                        {
                            <button type="submit" class="btn btn-success btn-lg">
                                ✓ Aprovar e Criar Despesa
                            </button>
                        }
                        <a asp-action="Index" class="btn btn-secondary">
                            @if (Model.IsReviewed)
                            {
                                <text>Voltar à Lista</text>
                            }
                            else
                            {
                                <text>Cancelar</text>
                            }
                        </a>
                    </div>
                </form>
            </div>
        </div>
    </div>
</div>
```

---

## Views/Invoices/Delete.cshtml - TRADUZIR ESTE FICHEIRO

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

Continua no próximo documento...
