# ✅ Checklist de Qualidade UI/UX - Ervilhinha

## 📋 Guia de Validação para Novas Páginas

Use esta checklist antes de publicar ou fazer commit de uma nova página de formulário.

---

## 🎯 Checklist Principal

### **1. Estrutura e Navegação**

- [ ] **Breadcrumbs** implementados corretamente
  - [ ] Níveis corretos (Home → Secção → Página)
  - [ ] Links funcionais
  - [ ] Último item não clicável (active)
  
- [ ] **Header da Página** completo
  - [ ] Título com ícone emoji
  - [ ] Subtítulo/descrição informativa
  - [ ] Ícone de informação `<i class="bi bi-info-circle"></i>`

- [ ] **Responsividade** testada
  - [ ] Desktop (>= 992px)
  - [ ] Tablet (768px - 991px)
  - [ ] Mobile (< 768px)

---

### **2. Formulário e Campos**

- [ ] **Validação ASP.NET Core**
  - [ ] `asp-validation-summary="ModelOnly"` presente
  - [ ] `asp-validation-for` em todos os campos obrigatórios
  - [ ] Classes `text-danger small` nas mensagens de erro

- [ ] **Labels e Placeholders**
  - [ ] Todos os campos têm labels
  - [ ] Asterisco vermelho `*` nos obrigatórios
  - [ ] Placeholders informativos
  - [ ] Helper text quando aplicável

- [ ] **Campos Obrigatórios**
  - [ ] Atributo `required` nos inputs HTML
  - [ ] `[Required]` no Model
  - [ ] Indicação visual clara (asterisco)

- [ ] **Tipos de Input Corretos**
  - [ ] `type="email"` para emails
  - [ ] `type="url"` para URLs
  - [ ] `type="number"` para números
  - [ ] `type="date"` para datas
  - [ ] `step="0.01"` em valores monetários

- [ ] **Constraints de Validação**
  - [ ] `maxlength` definido
  - [ ] `min` e `max` quando aplicável
  - [ ] `pattern` para formatos específicos (se necessário)

---

### **3. Cards e Agrupamento**

- [ ] **Organização Lógica**
  - [ ] Informação agrupada por tema
  - [ ] Mínimo de 2 cards, máximo de 6
  - [ ] Ordem lógica (básico → específico → ações)

- [ ] **Headers dos Cards**
  - [ ] Gradiente colorido aplicado
  - [ ] Título com ícone emoji
  - [ ] Tag `<h5>` utilizada
  - [ ] Margin bottom zero `mb-0`

- [ ] **Cores dos Gradientes**
  - [ ] Verde (`.bg-gradient-pea`) - Info Geral
  - [ ] Rosa (`.bg-gradient-pink`) - Bebé
  - [ ] Azul (`.bg-gradient-blue`) - Categorização
  - [ ] Amarelo (`.bg-gradient-yellow`) - Financeiro
  - [ ] Laranja (`.bg-gradient-orange`) - Alertas
  - [ ] Outros conforme contexto

---

### **4. Interatividade JavaScript**

- [ ] **Contadores de Caracteres**
  - [ ] Implementados em campos com `maxlength`
  - [ ] Atualização em tempo real
  - [ ] Formato: `X / Y caracteres`

- [ ] **Auto-resize Textareas**
  - [ ] `textarea` cresce com o conteúdo
  - [ ] Listener `input` implementado

- [ ] **Formatação Automática**
  - [ ] Valores monetários com 2 decimais
  - [ ] Listener `blur` nos inputs de valor

- [ ] **Confirmação de Cancelamento**
  - [ ] Deteta se formulário foi modificado
  - [ ] `confirm()` antes de cancelar
  - [ ] Previne navegação se utilizador cancela

- [ ] **Loading State no Submit**
  - [ ] Botão desativado após submit
  - [ ] Spinner + mensagem de loading
  - [ ] Apenas se `form.checkValidity()` passar

- [ ] **Validação Client-Side**
  - [ ] Scripts de validação incluídos
  - [ ] `_ValidationScriptsPartial` no `@section Scripts`

---

### **5. Componentes Visuais**

- [ ] **Input Groups Monetários**
  - [ ] Símbolo `€` no prefix
  - [ ] Cor de fundo apropriada (success, primary, danger)
  - [ ] Text branco no símbolo
  - [ ] Tamanho `input-group-lg`

- [ ] **Toggle Switches**
  - [ ] Class `form-switch form-switch-lg`
  - [ ] `role="switch"` no input
  - [ ] Ícone emoji no label
  - [ ] Helper text abaixo (`ms-5`)

- [ ] **Select Boxes**
  - [ ] Primeira opção: "-- Selecione --" com `value=""`
  - [ ] Optgroups quando aplicável
  - [ ] Ícones emoji nas opções
  - [ ] Tamanho `form-select-lg`

- [ ] **Botões de Ação**
  - [ ] Layout `justify-content-between`
  - [ ] Cancelar: `btn-outline-secondary btn-lg`
  - [ ] Guardar: `btn-primary btn-lg px-5`
  - [ ] Ícones Bootstrap Icons
  - [ ] IDs únicos para JavaScript

- [ ] **Alerts e Avisos**
  - [ ] `role="alert"` para acessibilidade
  - [ ] Ícones apropriados
  - [ ] Cores semânticas (info, success, warning, danger)

---

### **6. Acessibilidade (A11Y)**

- [ ] **ARIA Attributes**
  - [ ] `aria-label` em breadcrumbs
  - [ ] `aria-current="page"` no item ativo
  - [ ] `role="alert"` em mensagens
  - [ ] `role="switch"` em toggles

- [ ] **Navegação por Teclado**
  - [ ] Ordem de `tabindex` lógica
  - [ ] `autofocus` no primeiro campo
  - [ ] Enter submete o formulário
  - [ ] Esc fecha modals (se aplicável)

- [ ] **Semântica HTML**
  - [ ] `<label>` associado a `<input>` (for/id)
  - [ ] `<button type="submit">` para submit
  - [ ] `<a>` para links, não botões de navegação
  - [ ] Headers hierárquicos (h1 → h5)

- [ ] **Contraste de Cores**
  - [ ] Ratio mínimo 4.5:1 para texto normal
  - [ ] Ratio mínimo 3:1 para texto grande
  - [ ] Testado com ferramenta (ex: Wave, Axe)

---

### **7. Performance**

- [ ] **JavaScript Otimizado**
  - [ ] Event listeners adicionados uma vez
  - [ ] Sem leaks de memória
  - [ ] `debounce` em inputs frequentes (se aplicável)

- [ ] **CSS Otimizado**
  - [ ] Estilos inline apenas se necessário
  - [ ] Classes reutilizáveis
  - [ ] Sem !important desnecessários

- [ ] **Imagens e Ícones**
  - [ ] Bootstrap Icons via CSS (não inline SVG)
  - [ ] Lazy loading se aplicável
  - [ ] Tamanhos apropriados

---

### **8. Responsividade Detalhada**

- [ ] **Layout Mobile**
  - [ ] Cards em coluna única
  - [ ] Botões full-width ou stacked
  - [ ] Padding reduzido
  - [ ] Sidebar abaixo do form (não ao lado)

- [ ] **Layout Tablet**
  - [ ] Rows de 2 colunas onde faz sentido
  - [ ] Sidebar visível
  - [ ] Espaçamentos adequados

- [ ] **Layout Desktop**
  - [ ] Form centralizado (`col-lg-10 col-xl-9`)
  - [ ] Sidebar ao lado (se aplicável)
  - [ ] Máxima largura definida

- [ ] **Breakpoints Bootstrap**
  - [ ] `col-md-*` para tablets
  - [ ] `col-lg-*` para desktops
  - [ ] `col-xl-*` para ecrãs grandes
  - [ ] Classes `mb-3 mb-md-0` para margens responsivas

---

### **9. Documentação e Código**

- [ ] **Comentários**
  - [ ] Blocos de código comentados (HTML)
  - [ ] Funções JavaScript documentadas
  - [ ] CSS complexo explicado

- [ ] **Nomenclatura**
  - [ ] IDs únicos e descritivos
  - [ ] Classes semânticas
  - [ ] Variáveis JavaScript em camelCase

- [ ] **Organização**
  - [ ] HTML limpo e indentado
  - [ ] JavaScript no `@section Scripts`
  - [ ] CSS no `<style>` da section ou ficheiro separado

---

### **10. Testes Manuais**

- [ ] **Fluxo Completo**
  - [ ] Criar novo item com dados válidos
  - [ ] Tentar submeter com dados inválidos
  - [ ] Cancelar sem alterações
  - [ ] Cancelar com alterações (confirmar prompt)
  - [ ] Editar item existente

- [ ] **Validação**
  - [ ] Campos obrigatórios impedem submit
  - [ ] Mensagens de erro aparecem
  - [ ] Validação client-side funciona
  - [ ] Validação server-side funciona

- [ ] **Interatividade**
  - [ ] Contadores atualizam
  - [ ] Cálculos dinâmicos corretos
  - [ ] Toggles funcionam
  - [ ] Selects populam corretamente

- [ ] **Browsers**
  - [ ] Chrome/Edge
  - [ ] Firefox
  - [ ] Safari (se possível)
  - [ ] Mobile browsers

---

## 🎨 Checklist de Estilo Visual

### **Cores**

- [ ] Headers dos cards com gradientes
- [ ] Botão primário em azul
- [ ] Botão secundário em cinza
- [ ] Botão de perigo em vermelho
- [ ] Texto de erro em vermelho
- [ ] Ícones em cores apropriadas

### **Tipografia**

- [ ] `page-title` no H1
- [ ] `fw-semibold` nos labels principais
- [ ] `small` ou `text-muted` em helper texts
- [ ] Tamanhos de fonte consistentes

### **Espaçamento**

- [ ] `mb-4` entre cards
- [ ] `mb-3` entre fields
- [ ] `px-5` em botões principais
- [ ] `gap-2` entre botões
- [ ] `mt-3` no helper text final

### **Sombras e Bordas**

- [ ] `shadow` nos cards
- [ ] `rounded` (padrão Bootstrap)
- [ ] Sem bordas duras desnecessárias

---

## 🚀 Checklist de Deploy

Antes de fazer commit/push:

- [ ] Build sem erros (`dotnet build`)
- [ ] Testes unitários passam (se aplicável)
- [ ] Página funciona em dev environment
- [ ] Console sem erros JavaScript
- [ ] Validação ASP.NET funcional
- [ ] Migrations aplicadas (se necessário)
- [ ] README atualizado (se nova feature)

---

## 📊 Score de Qualidade

Conta quantos items tens marcados:

- **90-100%**: ⭐⭐⭐⭐⭐ Excelente! Pronto para produção
- **80-89%**: ⭐⭐⭐⭐ Muito bom, pequenos ajustes
- **70-79%**: ⭐⭐⭐ Bom, mas precisa de melhorias
- **60-69%**: ⭐⭐ Aceitável, várias melhorias necessárias
- **< 60%**: ⭐ Precisa de trabalho significativo

---

## 🎯 Checklist Rápida (Mínimo Aceitável)

Se tens pouco tempo, garante pelo menos:

✅ Breadcrumbs  
✅ Header com título e descrição  
✅ Cards organizados (mín. 2)  
✅ Validação client + server  
✅ Botões de ação com ícones  
✅ Loading state no submit  
✅ Responsivo (mobile-first)  
✅ Sem erros de console  
✅ Funciona em 2+ browsers  

---

## 📝 Template de Review

Usa este template ao fazer code review:

```markdown
## Code Review - [Nome da Página]

### ✅ Aprovado
- Item 1
- Item 2

### ⚠️ Melhorias Sugeridas
- Item 1
- Item 2

### ❌ Bloqueadores
- Item 1 (precisa de correção antes de merge)

### 📊 Score: X/100
```

---

## 🎉 Conclusão

Uma página que passa em **todos** os pontos desta checklist está pronta para:

- ✅ Produção
- ✅ Ser usada como referência
- ✅ Servir de template para outras páginas
- ✅ Proporcionar excelente UX

**Qualidade não é acidente, é escolha!** 🚀

---

**Última atualização:** @DateTime.Now.ToString("dd/MM/yyyy")  
**Versão:** 1.0
