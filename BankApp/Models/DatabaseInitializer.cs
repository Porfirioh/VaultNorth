using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BankApp.Models
{
    public class DatabaseInitializer
	{
		public void Initialize(BankAppDataContext context, UserManager<IdentityUser> userManager)

		{
			context.Database.Migrate();

			context.Database.EnsureCreated();

			AddRole(context, "Admin");

			AddRole(context, "Cashier");

			string admin = "Admin";

			string cashier = "Cashier";

			AddUser(userManager, "admin@admin.se", admin);

			AddUser(userManager, "cashier@cashier.se", cashier);
		}

		private void AddRole(BankAppDataContext context, string role)
		{
			if (context.Roles.Any(r => r.Name == role))
				return;

			context.Roles.Add(new IdentityRole { Name = role, NormalizedName = role });

			context.SaveChanges();
		}

		private void AddUser(UserManager<IdentityUser> userManager, string user, string role)
		{
			if (userManager.FindByEmailAsync(user).Result == null)
			{
				var identityUser = new IdentityUser
				{
					UserName = user,
					Email = user,
					EmailConfirmed = true
				};

				var result = userManager.CreateAsync(identityUser, "Abc123!").Result;

				if (result.Succeeded)
				{
					userManager.AddToRoleAsync(identityUser, role).Wait();

				}
			}

		}
	}
}