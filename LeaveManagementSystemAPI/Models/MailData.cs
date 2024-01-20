namespace LeaveManagementSystemAPI.Models
{
    public class MailData
    {
        public List<string> To { get; }
        public List<string> Bcc { get; }
        public List<string> Cc { get; }

        public string? From { get; }
        public string? DisplayName { get; }
        public string? ReplyTo { get; }
        public string? ReplyToName { get; }
        public string? Subject { get; }
        public string? Body { get; }

        public MailData(
            List<string> to,
            string? subject,
            string? body = null,
            string? displayName = null,
            string? from = null,
            List<string>? bcc = null,
            List<string>? cc = null, 
            string? replyTo = null, 
            string? replyToName = null
            
            )
        {
            To = to;
            Bcc = bcc;
            Cc = cc;
            From = from;
            DisplayName = displayName;
            ReplyTo = replyTo;
            ReplyToName = replyToName;
            Subject = subject;
            Body = body;
        }
    }
}
