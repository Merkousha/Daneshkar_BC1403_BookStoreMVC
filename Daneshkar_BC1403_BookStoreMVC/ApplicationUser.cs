using Microsoft.AspNetCore.Identity;

namespace MVC
{
    public class ApplicationUser : IdentityUser<long>

    {
        public string FullName { get; set; }

    }

}