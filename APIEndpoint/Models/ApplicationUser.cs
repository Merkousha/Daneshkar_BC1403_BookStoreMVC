using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace API.Models;

public class ApplicationUser: IdentityUser<long>

{
    public string FullName { get; set; }

}
