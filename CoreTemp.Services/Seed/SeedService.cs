using CoreTemp.Data.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreTemp.Services.Seed
{
    public class SeedService : ISeedService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public SeedService(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void SeedUsers()
        {
            //try
            //{
            //    _dbLog.Database.Migrate();
            //    _dbMain.Database.Migrate();
            //    _dbFinancial.Database.Migrate();
            //}
            //catch (Exception ex)
            //{
            //}

            if (!_userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Files/Json/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<IList<User>>(userData);

                var roles = new List<Role>
                {
                    new Role {Name = "Admin"},
                    new Role {Name = "User"},
                    new Role {Name = "Vip"},
                    new Role {Name = "Operator"}
                };

                foreach (var role in roles)
                {
                    _roleManager.CreateAsync(role).Wait();
                }

                foreach (var user in users)
                {
                    user.Email = user.Email.ToLower();
                    user.UserName = user.UserName.ToLower();
                    _userManager.CreateAsync(user, "123456").Wait();
                    //_userManager.AddToRoleAsync(user, "Admin").Wait();
                    _userManager.AddToRolesAsync(user, new[] { "Admin", "User", "Vip", "Operator" }).Wait();
                }
            }
        }

    }
}
