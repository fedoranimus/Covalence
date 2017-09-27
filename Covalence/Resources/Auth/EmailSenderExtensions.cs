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
    }
}