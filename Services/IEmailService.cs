namespace Ervilhinha.Services
{
    /// <summary>
    /// Interface para serviço de envio de emails
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Envia convite para partilhar wishlist/lista
        /// </summary>
        Task<bool> SendListInviteAsync(string toEmail, string listName, string shareLink, string senderName, string listType = "Lista do Bebé");

        /// <summary>
        /// Notifica owner quando alguém reserva um item
        /// </summary>
        Task<bool> SendReservationNotificationAsync(string toEmail, string listName, string itemName, string reserverName, string reserverEmail);

        /// <summary>
        /// Envia alerta de orçamento excedido
        /// </summary>
        Task<bool> SendBudgetAlertAsync(string toEmail, string alertTitle, string alertMessage, decimal? amount = null);

        /// <summary>
        /// Envia alerta de evento próximo na timeline
        /// </summary>
        Task<bool> SendTimelineReminderAsync(string toEmail, string eventTitle, DateTime eventDate, string? recommendation = null);

        /// <summary>
        /// Envia relatório semanal de despesas
        /// </summary>
        Task<bool> SendWeeklyReportAsync(string toEmail, string userName, decimal totalExpenses, decimal budget, int alertCount);

        /// <summary>
        /// Email genérico para qualquer notificação
        /// </summary>
        Task<bool> SendNotificationAsync(string toEmail, string subject, string htmlBody, string? plainTextBody = null);

        /// <summary>
        /// Verifica se o serviço de email está configurado
        /// </summary>
        bool IsConfigured();
    }
}
