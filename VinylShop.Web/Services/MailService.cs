using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using VinylShop.Web.Data.Entities;
using VinylShop.Web.Models;

namespace VinylShop.Web.Services
{
    public class MailService(IOptions<MailSettings> settings) : IMailService
    {
        private readonly MailSettings _settings = settings.Value;

        public async Task SendAsync(MailAddress sender, MailAddress receiver, string subject, string body, CancellationToken cancellationToken = default)
        {
            using SmtpClient smtpClient = new(_settings.Host, _settings.Port)
            {
                //Credentials = new NetworkCredential(_settings.Username, _settings.Password),
                //EnableSsl = _settings.EnableSsl,
            };

            using MailMessage message = new(sender, receiver)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            await smtpClient.SendMailAsync(message, cancellationToken);
        }

        public Task SendEmailConfirmationAsync(UserEntity user, IUrlHelper urlHelper, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SendResetPasswordAsync(UserEntity user, IUrlHelper urlHelper, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
