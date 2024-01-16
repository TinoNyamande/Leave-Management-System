namespace LeaveManagementSystemAPI.Models
{
    public interface IMailService
    {
        Task<string> SendAsync(MailData mailData, CancellationToken ct);
    }
}
