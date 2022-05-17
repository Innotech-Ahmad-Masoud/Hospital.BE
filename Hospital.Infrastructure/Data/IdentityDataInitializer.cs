using Microsoft.AspNetCore.Identity;

namespace Hospital.Infrastructure.Data
{
    public class IdentityDataInitializer
    {
        public static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            if (userManager.FindByNameAsync("admin").Result != null) return;
            var user = new IdentityUser
            {
                UserName = "admin",
                Email = "admin@admin.com"
            };

            var result = userManager.CreateAsync(user, "Password1@").Result;
        }
    }
}
