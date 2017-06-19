using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Covalence.ViewModels
{
    public class LogoutViewModel
    {
        [BindNever]
        public string RequestId { get; set; }
    }
}