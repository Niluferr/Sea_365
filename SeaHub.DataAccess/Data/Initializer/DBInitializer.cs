using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SeaHub.Models;
using SeaHub.Utility;
using System;
using System.Linq;

namespace SeaHub.DataAccess.Data.Initializer
{
    public class DBInitializer : IDBInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _rolManager;

        public DBInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> rolManager)
        {
            _db = db;
            _userManager = userManager;
            _rolManager = rolManager;
        }

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {
                throw;
            }

            if (_db.Roles.Any(r => r.Name == SD.Admin)) return;

            _rolManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();
            _rolManager.CreateAsync(new IdentityRole(SD.Manager)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                Name = "Erdal Demiryol"
            }, "Admin123!").GetAwaiter().GetResult();

            ApplicationUser user = _db.ApplicationUser.Where(u => u.Email == "admin@gmail.com").FirstOrDefault();
            _userManager.AddToRoleAsync(user, SD.Admin).GetAwaiter().GetResult();

        }
    }
}
