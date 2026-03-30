# 📧 FEATURES FUTURAS - Email Notifications

## 🎯 Status: PLANEJADO (Não Implementado)

---

## 📍 **TODOs Identificados no Código**

### **1. WishlistController.cs:319**
```csharp
// TODO: Enviar email com convite
// Contexto: Quando user partilha wishlist com alguém
// Comportamento atual: TempData confirma convite mas email não é enviado
```

**Prioridade**: 🟡 Média  
**Impacto**: UX - User acha que email foi enviado mas precisa copiar link manualmente  
**Workaround**: Link de partilha fica disponível em /Wishlist/Share

---

### **2. WishlistPublicController.cs:150**
```csharp
// TODO: Enviar notificação aos gestores
// Contexto: Quando alguém reserva item em wishlist pública
// Comportamento atual: Reserva é guardada mas owner não recebe notificação
```

**Prioridade**: 🟡 Média  
**Impacto**: UX - Owner precisa verificar manualmente se há reservas  
**Workaround**: Dashboard mostra reservas em /Wishlist/Manage

---

## 🚀 **Plano de Implementação (Futuro)**

### **Opção 1: SendGrid (Recomendado)**
✅ **Free tier**: 100 emails/dia  
✅ **Fácil integração**: Pacote NuGet oficial  
✅ **Templates HTML**: Suporta emails bonitos  
💰 **Custo**: Grátis até 40k/mês depois €15/mês

```csharp
// Instalar
dotnet add package SendGrid

// Services/EmailService.cs
public class SendGridEmailService : IEmailService
{
    private readonly string _apiKey;
    
    public async Task SendWishlistInvite(string toEmail, string wishlistName, string shareLink)
    {
        var client = new SendGridClient(_apiKey);
        var msg = new SendGridMessage()
        {
            From = new EmailAddress("noreply@ervilhinha.com", "Ervilhinha"),
            Subject = $"🎁 {wishlistName} - Convite para lista de bebé",
            HtmlContent = $@"
                <h2>Foste convidado!</h2>
                <p>Alguém partilhou contigo uma lista de bebé.</p>
                <a href='{shareLink}'>Ver Lista</a>
            "
        };
        msg.AddTo(toEmail);
        
        await client.SendEmailAsync(msg);
    }
}
```

---

### **Opção 2: SMTP (Gmail/Outlook)**
✅ **Grátis**  
⚠️ **Complexo**: Requer configuração SMTP  
⚠️ **Rate limits**: Gmail limita a 500/dia

```csharp
// appsettings.json
{
  "Smtp": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "Username": "ervilhinha@gmail.com",
    "Password": "app-password",
    "EnableSsl": true
  }
}

// Services/SmtpEmailService.cs
public async Task SendEmail(string to, string subject, string body)
{
    using var client = new SmtpClient(_host, _port);
    client.Credentials = new NetworkCredential(_username, _password);
    client.EnableSsl = true;
    
    var message = new MailMessage("noreply@ervilhinha.com", to, subject, body);
    message.IsBodyHtml = true;
    
    await client.SendMailAsync(message);
}
```

---

### **Opção 3: Azure Communication Services**
✅ **Integra com Azure**  
✅ **Emails + SMS**  
💰 **Custo**: €0.90/1000 emails

```bash
# Criar recurso
az communication create --name ervilhinha-email --resource-group ErvilhinhaRG
```

---

## 📝 **Templates de Email Planejados**

### **1. Convite para Wishlist**
```html
<!DOCTYPE html>
<html>
<head>
    <style>
        body { font-family: 'Segoe UI', sans-serif; background: #FFFEF7; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; }
        .header { background: linear-gradient(135deg, #C8E6C9, #A5D6A7); padding: 30px; border-radius: 20px; }
        .button { background: #81C784; color: white; padding: 15px 30px; text-decoration: none; border-radius: 25px; }
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <h1>🎁 Foste Convidado!</h1>
            <p>{SenderName} partilhou contigo a lista de bebé "{WishlistName}"</p>
        </div>
        <div style="padding: 30px; background: white; border-radius: 20px; margin-top: 20px;">
            <p>Podes ver os items e reservar presentes diretamente na plataforma.</p>
            <a href="{ShareLink}" class="button">Ver Lista do Bebé</a>
        </div>
        <p style="color: #8D6E63; font-size: 12px; text-align: center; margin-top: 20px;">
            Enviado por Ervilhinha - Planeamento Familiar Inteligente
        </p>
    </div>
</body>
</html>
```

### **2. Notificação de Reserva**
```html
<h2>🎉 Alguém reservou um item!</h2>
<p>A tua lista "{WishlistName}" tem uma nova reserva:</p>
<ul>
    <li><strong>Item:</strong> {ItemName}</li>
    <li><strong>Reservado por:</strong> {ReserverName}</li>
    <li><strong>Data:</strong> {ReservationDate}</li>
</ul>
<a href="{ManageLink}">Ver Todas as Reservas</a>
```

### **3. Alerta de Orçamento**
```html
<h2>⚠️ Orçamento Excedido</h2>
<p>As tuas despesas com o bebé este mês excederam o orçamento:</p>
<ul>
    <li><strong>Orçado:</strong> {BudgetAmount}€</li>
    <li><strong>Gasto:</strong> {ActualAmount}€</li>
    <li><strong>Excesso:</strong> {ExcessAmount}€</li>
</ul>
```

---

## 🔧 **Implementação Planejada**

### **Fase 1: Interface**
```csharp
// Services/IEmailService.cs
public interface IEmailService
{
    Task SendWishlistInviteAsync(string toEmail, string wishlistName, string shareLink, string senderName);
    Task SendReservationNotificationAsync(string toEmail, string wishlistName, string itemName, string reserverName);
    Task SendBudgetAlertAsync(string toEmail, decimal budgetAmount, decimal actualAmount);
    Task SendWeeklyReportAsync(string toEmail, WeeklyReport report);
}
```

### **Fase 2: Configuração**
```json
// User Secrets
{
  "SendGrid": {
    "ApiKey": "SG.xxxxxx",
    "FromEmail": "noreply@ervilhinha.com",
    "FromName": "Ervilhinha"
  }
}
```

### **Fase 3: Injetar nos Controllers**
```csharp
public WishlistController(
    ApplicationDbContext context,
    IEmailService emailService) // ← Adicionar
{
    _context = context;
    _emailService = emailService;
}

// Substituir TODO
// TODO: Enviar email com convite
await _emailService.SendWishlistInviteAsync(email, wishlist.Name, shareLink, userEmail);
```

---

## 📊 **Estimativa de Custos (SendGrid)**

| Emails/Mês | Custo | Cenário |
|-----------|-------|---------|
| 0 - 3,000 | **GRÁTIS** | 100 users × 30 emails = 3000/mês |
| 3,000 - 40,000 | **GRÁTIS** | Até 1300 users |
| 40,000+ | €15/mês | Escala enterprise |

**ROI**: 
- Engagement +40% (users recebem notificações)
- Retenção +25% (lembretes de orçamento)
- Custo: €0 até 1300 users

---

## ✅ **Status Atual (Sem Email)**

### **Funcionalidades que ainda funcionam**:
✅ Partilha por link (copiar/colar)  
✅ Reservas são guardadas na DB  
✅ Dashboard mostra reservas  
✅ Alertas aparecem na app  

### **O que falta**:
📧 Email automático de convite  
📧 Email de notificação de reserva  
📧 Email de alertas críticos  
📧 Relatório semanal por email  

---

## 🎯 **Prioridade de Implementação**

1. **🟢 Alta**: Notificação de reserva (owner precisa saber)
2. **🟡 Média**: Convite por email (workaround: copiar link)
3. **🟡 Média**: Alertas financeiros críticos
4. **🔵 Baixa**: Relatório semanal (nice to have)

---

## 📋 **Checklist de Implementação**

Quando decidires implementar:

- [ ] Escolher provider (SendGrid recomendado)
- [ ] Criar conta/recurso
- [ ] Instalar pacote NuGet
- [ ] Criar `IEmailService` interface
- [ ] Implementar `SendGridEmailService`
- [ ] Criar templates HTML em `Views/EmailTemplates/`
- [ ] Registar serviço no `Program.cs`
- [ ] Substituir TODOs nos controllers
- [ ] Adicionar toggle "Email Notifications" no perfil user
- [ ] Testar com emails reais

---

## 🔐 **Considerações GDPR**

- [ ] Pedir consentimento para receber emails
- [ ] Permitir opt-out (unsubscribe)
- [ ] Não guardar emails de terceiros sem consentimento
- [ ] Incluir link de unsubscribe em todos os emails
- [ ] Política de privacidade atualizada

---

**Conclusão**: Emails são **feature futura planejada**, não são mock data nem bloqueiam produção.  
**App funciona 100%** sem emails - notificações aparecem na interface web.

---

**Criado**: 2025-05-XX  
**Status**: 📋 Planejado (não implementado)  
**Prioridade**: 🟡 Média (nice to have, não essencial)
