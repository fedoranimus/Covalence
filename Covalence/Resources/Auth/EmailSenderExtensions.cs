using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Covalence 
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(link)}'>clicking this link</a>");
        }

        public static Task SendForgotPasswordAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Reset Password",
                   $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(link)}'>clicking this link</a>");
        }

        public static Task SendConnectionRequestedAsync(this IEmailSender emailSender, string email)
        {
            return emailSender.SendEmailAsync(email, "Connection Requested",
                $"You have a pending connection request waiting for you on <a href='{HtmlEncoder.Default.Encode("www.becovalent.com")}'>Covalence</a>.");
        }

        public static Task SendConnectionAcceptedAsync(this IEmailSender emailSender, string email)
        {
            return emailSender.SendEmailAsync(email, "Connection Accepted",
                $"Your connection request has been accepted on <a href='{HtmlEncoder.Default.Encode("www.becovalent.com")}'>Covalence</a>.");
        }
    }
}