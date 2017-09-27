using Covalence.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Covalence 
{
    public static class UrlHelperExtensions 
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ConfirmEmail),
                controller: "account",
                values: new { userId, code },
                protocol: scheme
            );
        }

        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ResetPassword),
                controller: "account",
                values: new { userId, code },
                protocol: scheme
            );
        }
    }
}