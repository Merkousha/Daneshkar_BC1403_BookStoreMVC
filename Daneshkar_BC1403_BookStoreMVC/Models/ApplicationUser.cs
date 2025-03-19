using Microsoft.AspNetCore.Identity;

namespace Daneshkar_BC1403_BookStoreMVC.Models
{
    public class ApplicationUser : IdentityUser<long>

    {
        public string FullName { get; set; } = string.Empty;

    }
}