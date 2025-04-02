using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace DACS.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "nguyenthuan3399@gmail.com";
            var pw = "kjwz utwl dgzm ihso";

            var client = new SmtpClient("smtp.gmail.com")
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, pw),
                Port = 587
            };
            return client.SendMailAsync(
                    new MailMessage(from: mail,
                                    to: email,
                                    subject,
                                    message));
        }


    }
}
