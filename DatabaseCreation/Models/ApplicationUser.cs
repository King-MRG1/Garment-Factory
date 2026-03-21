using Microsoft.AspNetCore.Identity;
namespace DatabaseCreation.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Full_name { get; set; }
        List<RefreshTokenStore> RefreshTokens { get; set; }
    }
}
