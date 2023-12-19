using Microsoft.Extensions.Options;
using QuickQuiz.Core.Model;
using QuickQuiz.Core.Services;
using System.Net;
using System.Net.Mail;

namespace QuickQuiz.Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        public async Task SendResetPasswordEmail(string resetPasswordEmailLink, string ToEmail)
        {
            SmtpClient smptClient = SmtpClient();
            MailMessage mailMessage = new()
            {
                From = new MailAddress(_emailSettings.Email),
                Subject = "Quick Quiz| Şifre Sıfırlama Linki",
                Body = @$"
                <h4>Şifrenizi yenilemek için aşağıdaki linke tıklayınız.</h4>
                <p><a href='{resetPasswordEmailLink}'>şifre yenileme link</a></p>",
                IsBodyHtml = true
            };
            mailMessage.To.Add(ToEmail);

            await smptClient.SendMailAsync(mailMessage);
        }
        public async Task SendAccountConfirmEmail(string url,string userName, string ToEmail)
        {
            SmtpClient smptClient = SmtpClient();
            MailMessage mailMessage = new()
            {
                From = new MailAddress(_emailSettings.Email),
                Subject = "Quick Quiz| Kayıt İşlemi",
                Body = @$"
                <h4><strong>{userName}</strong> kullanıcı adı ile www.quizck.com'a kayıt oldunuz, hesabı siz oluşturduysanız linke tıklayarak hesabınızı aktifleştirebilirsiniz.</h4>
                <p><a href='{url}'>Aktivasyon Linki</a></p>",
                IsBodyHtml = true
            };
            mailMessage.To.Add(ToEmail);
            try
            {
            await smptClient.SendMailAsync(mailMessage);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                throw;
            }
        }
        private SmtpClient SmtpClient()
        {
            return new()
            {
                Host = _emailSettings.Host,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Port = 587,
                Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password),
                EnableSsl = true
            };
        }
    }
}