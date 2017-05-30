using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Covalence.Authentication
{
    public class LogoutViewModel
    {
        [BindNever]
        public string RequestId { get; set; }
    }
}