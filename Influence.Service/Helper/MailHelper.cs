using System.Net;
using System.Net.Mail;
using Serilog;

namespace Influence.Service.Helper
{
    public class EmailModel
    {
        public string EmailAccountHost { get; set; }
        public Nullable<int> EmailAccountPort { get; set; }
        public Nullable<bool> EmailAccountEnableSsl { get; set; }
        public Nullable<bool> EmailAccountUseDefaultCredentials { get; set; }
        public string EmailAccountUsername { get; set; }
        public string EmailAccountPassword { get; set; }

    }
    public class EmailRequestModel
    {
        public EmailRequestModel()
        {

        }
        public EmailRequestModel(string email)
        {
            this.Email = new List<string>();
            this.Email.Add(email);

        }
        public List<string> Email { get; set; }
        public List<string> Cc { get; set; }
        public string Header { get; set; }
        public string Message { get; set; }
        public bool IsHtml { get; set; }

    }

    public static class MailHelper
    {

        public static async Task<bool> SendSingleEmailAsync(EmailRequestModel model)
        {
            try
            {

                var fromEmail = "stigadevelopment@gmail.com";
                var emailHesap = new EmailModel
                {
                    EmailAccountEnableSsl = true,
                    EmailAccountHost = "smtp.gmail.com",
                    EmailAccountPassword = "stigadevelopment123",
                    EmailAccountPort = 587,
                    EmailAccountUseDefaultCredentials = false,
                    EmailAccountUsername = "stigadevelopment"
                };
                List<MailAddress> mailAddresses = new List<MailAddress>();
                mailAddresses.AddRange(model.Email.Select(x => new MailAddress(x)
                ).ToList());

                await SendEmail(emailHesap, model.Header, model.Message, new MailAddress(fromEmail), mailAddresses, new List<string>(), model.Cc, null, "", model.IsHtml);
                return (true);
            }
            catch (Exception e)
            {
                Log.Error("Mail Hata "+e.Message+ e?.InnerException?.Message);
                return (false);

            }
        }
        private static async Task SendEmail(EmailModel emailAccount, string subject, string body, MailAddress @from, List<MailAddress> to, IEnumerable<string> bcc, IEnumerable<string> cc, byte[] attachment, string attachmentName, bool isHtml = false)
        {
            var message = new MailMessage { From = @from };

            foreach (var toAddress in to)
            {
                message.To.Add(toAddress);
            }

            if (null != bcc)
            {
                foreach (var address in bcc.Where(bccValue => !string.IsNullOrWhiteSpace(bccValue)))
                {
                    message.Bcc.Add(address.Trim());
                }
            }

            if (null != cc)
            {
                foreach (var address in cc.Where(ccValue => !string.IsNullOrWhiteSpace(ccValue)))
                {
                    message.CC.Add(address.Trim());
                }
            }

            if (attachment != null && attachment.Length > 0)
            {
                var memoryStream = new MemoryStream(attachment);
                message.Attachments.Add(new Attachment(memoryStream, attachmentName));
            }

            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = isHtml;

            using (var smtpClient = new SmtpClient())
            {
                //smtpClient.UseDefaultCredentials = (bool)emailAccount.EmailAccountUseDefaultCredentials;
                smtpClient.Host = emailAccount.EmailAccountHost;
                smtpClient.Port = emailAccount.EmailAccountPort.GetValueOrDefault();
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = emailAccount.EmailAccountUseDefaultCredentials.GetValueOrDefault()
                    ? CredentialCache.DefaultNetworkCredentials
                    : new NetworkCredential(emailAccount.EmailAccountUsername, emailAccount.EmailAccountPassword);

                await smtpClient.SendMailAsync(message);

            }
        }




    }
}