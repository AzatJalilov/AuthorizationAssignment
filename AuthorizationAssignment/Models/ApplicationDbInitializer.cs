using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;

namespace AuthorizationAssignment.Models
{

    public class ApplicationDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        public static void InitializeIdentityForEF(ApplicationDbContext db)
        {

            if (!db.Users.Any())
            {
                var roleStore = new RoleStore<IdentityRole>(db);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var userStore = new UserStore<ApplicationUser>(db);
                var userManager = new UserManager<ApplicationUser>(userStore);

                // Add missing roles
                var role = roleManager.FindByName("Master");
                if (role == null)
                {
                    role = new IdentityRole("Master");
                    roleManager.Create(role);
                }

                // Create master users
                var user = userManager.FindByName("master");
                if (user == null)
                {
                    var newUser = new ApplicationUser()
                    {
                        UserName = "master@master.net",
                        Email = "master@master.net"
                    };
                    userManager.Create(newUser, "CorrectHorseBatteryStaple");
                    userManager.SetLockoutEnabled(newUser.Id, false);
                    userManager.AddToRole(newUser.Id, "Master");
                }
            }
        }
    }
}