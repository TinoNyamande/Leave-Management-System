using LeaveManagementSystemAPI.Models;

namespace LeaveManagementSystemAPI.MyServices
{
    public interface IEmailService
    {
        Task<string> SendAsync(MailData mailData, CancellationToken ct);
    }
}
