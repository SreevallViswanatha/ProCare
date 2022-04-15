using System.Net.Mail;

namespace ProCare.API.PBM.Helpers
{
    public static class EmailHelper
    {
        public static void SendNotificationEmail(string hostName, string smtpPort, string emailFrom, string emailTo, string message, string subject, string emailCopyTo = "")
        {
            try
            {
                EmailConfig emailConfig = new EmailConfig()
                {
                    HostName = hostName,
                    MailFrom = emailFrom,
                    MailFromName = emailFrom,
                    SMTPPortNumber = smtpPort
                };

                Email email = new Email()
                {
                    To = emailTo,
                    CC = !string.IsNullOrWhiteSpace(emailCopyTo) ? emailCopyTo : string.Empty,
                    BCC = string.Empty,
                    Subject = subject,
                    Message = message
                };

                SendEmail(email, emailConfig);
            }
            catch { }
        }

        public static void SendEmail(Email email, EmailConfig emailConfig)
        {
            var message = new MailMessage();
            message.From = new MailAddress(emailConfig.MailFrom);
            if (!string.IsNullOrEmpty(email.To))
            {
                message.To.Add(email.To);
            }
            if (!string.IsNullOrEmpty(email.CC))
            {
                message.CC.Add(email.CC);
            }
            if (!string.IsNullOrEmpty(email.BCC))
            {
                message.Bcc.Add(email.BCC);
            }
            message.Subject = email.Subject;
            message.IsBodyHtml = true;
            message.Body = email.Message;

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Port = int.Parse(emailConfig.SMTPPortNumber);
                smtpClient.Host = emailConfig.HostName;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Send(message);
            }
        }
    }

    public class Email
    {
        public string From { get; set; }
        public string To { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }

    public class EmailConfig
    {
        public string HostName { get; set; }
        public string SMTPPortNumber { get; set; }
        public string MailFrom { get; set; }
        public string MailFromName { get; set; }
    }
}
