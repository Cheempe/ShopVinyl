using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using VinylShop.Web.Data.Entities;

namespace VinylShop.Web.Services
{
    public interface IMailService
    {
        Task SendEmailConfirmationAsync(UserEntity user, IUrlHelper urlHelper, CancellationToken cancellationToken);
        Task SendResetPasswordAsync(UserEntity user, IUrlHelper urlHelper, CancellationToken cancellationToken);
        Task SendAsync(MailAddress sender, MailAddress receiver, string subject, string body, CancellationToken cancellationToken = default);
    }
}
