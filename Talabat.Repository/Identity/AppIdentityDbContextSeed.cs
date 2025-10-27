using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> _userManger)
        {
            if (_userManger.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    DisplayName = "Mohamed Sayed",
                    Email = "mohamedsayed@gmail.com",
                    UserName = "mohamed_sayed309",
                    PhoneNumber = "1234567890",

                };
                await _userManger.CreateAsync(user, "112001Ms!@");
            }
        }
    }
}
