# 📚 Documentação do Sistema Ervilhinha

Bem-vindo à documentação completa do **Ervilhinha** - o sistema inteligente de gestão de bebés e finanças familiares.

## 🗂️ Estrutura da Documentação

### **1. Sistema de Listas do Bebé (BabyLists)** ⭐ NOVO
Sistema unificado que combina planeamento de enxoval e listas de presentes.

#### **📖 Guias de Utilizador**
- [📋 00 - Resumo de Melhorias UI/UX](BabyLists/00-Summary-UI-UX-Improvements.md) ⭐ **LEITURA OBRIGATÓRIA**
- [📋 01 - Visão Geral do Sistema](BabyLists/01-Overview.md)
- [➕ 02 - Criar Nova Lista](BabyLists/02-Create-List.md) ✅ **View melhorada**
- [⚙️ 03 - Gerir Lista](BabyLists/03-Manage-List.md)
- [📦 04 - Adicionar Item](BabyLists/04-Add-Item.md) ✅ **View melhorada**
- [✏️ 05 - Editar Item](BabyLists/05-Edit-Item.md)
- [🔗 06 - Partilhar Lista](BabyLists/06-Share-List.md)
- [👁️ 07 - Visualização Pública](BabyLists/07-Public-View.md)
- [🎁 08 - Reservar Item](BabyLists/08-Reserve-Item.md)

#### **🎨 Guias Técnicos**
- [🎨 Guia Completo de UI/UX](BabyLists/UI-UX-Guidelines.md) ⭐ **Design System**
- [✅ Guia de Validações](BabyLists/09-Validation-Guide.md) 🆕 **Validações completas**

### **2. Planeamento Financeiro**
- Simulador de Custos do Bebé
- Orçamento Familiar
- Timeline do Bebé
- Alertas Inteligentes

### **3. Gestão de Despesas**
- Despesas Mensais
- Faturas e OCR
- Relatórios

### **4. Artigos do Bebé**
- Catálogo de Produtos
- Gestão de Inventário

---

## 🎨 Princípios de Design

### **Tema "Little Pea" (Ervilhinha)**
- **Cores Pastel Suaves**: Pea green, baby pink, baby blue, baby yellow
- **Tipografia Clara**: Sans-serif, tamanhos hierárquicos
- **Espaçamento Generoso**: Respiração entre elementos
- **Iconografia Consistente**: Emojis + ícones Bootstrap

### **Responsividade**
- **Mobile-First**: Design adaptativo
- **Breakpoints**: SM (576px), MD (768px), LG (992px), XL (1200px)
- **Touch-Friendly**: Botões grandes, áreas clicáveis amplas

### **Acessibilidade**
- **WCAG 2.1 AA**: Contraste mínimo 4.5:1
- **Navegação por Teclado**: Tab order lógico
- **Screen Readers**: ARIA labels apropriados
- **Mensagens de Erro**: Clara e contextual

---

## 📊 Convenções de Validação

### **Campos Obrigatórios**
- Marcados com **asterisco (*)** vermelho
- Validação client-side (HTML5) + server-side (DataAnnotations)
- Mensagem de erro em português claro

### **Formato de Dados**
- **Datas**: `dd/MM/yyyy` (formato PT)
- **Moeda**: `€ 0.00` (formato PT-PT)
- **Email**: Validação RFC 5322
- **URLs**: Validação HTTP/HTTPS

### **Limites e Ranges**
- **Texto Curto**: Máximo 100 caracteres
- **Texto Médio**: Máximo 500 caracteres
- **Texto Longo**: Máximo 1000 caracteres
- **Valores Monetários**: €0 - €10,000
- **Quantidades**: 1 - 100 unidades

---

## 🔄 Fluxos de Utilizador

### **Fluxo Principal: Criar Lista do Bebé**
```
1. Index → Ver todas as listas
2. Create → Criar nova lista (privada/partilhada)
3. Manage → Adicionar items
4. Share → Gerar código de partilha (opcional)
5. Public View → Visitantes veem e reservam
```

### **Fluxo de Visitante: Reservar Presente**
```
1. Receber link/código
2. Public View → Ver lista pública
3. Reserve → Escolher item e quantidade
4. Confirmation → Ver confirmação
5. MyReservations → Consultar reservas
```

---

## 🆘 Suporte

### **Mensagens de Erro Comuns**
- **Campo obrigatório**: "O [nome do campo] é obrigatório"
- **Formato inválido**: "Formato inválido. Use: [exemplo]"
- **Valor fora do range**: "O valor deve estar entre [min] e [max]"
- **Email duplicado**: "Este email já está registado"

### **Feedback Visual**
- ✅ **Sucesso**: Alert verde com ícone de check
- ⚠️ **Aviso**: Alert amarelo com ícone de atenção
- ❌ **Erro**: Alert vermelho com ícone de X
- ℹ️ **Info**: Alert azul com ícone de i

---

## 📱 Contacto

Para sugestões ou reportar bugs:
- **GitHub Issues**: [Ervilhinha Issues](https://github.com/BrunoD3V/Ervilhinha/issues)
- **Email**: bruno@ervilhinha.pt

---

**Última Atualização**: 28 de Março de 2026  
**Versão**: 1.0.0
