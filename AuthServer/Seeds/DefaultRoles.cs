using AuthServer.Models;
using Microsoft.AspNetCore.Identity;
using AuthServer.Enums;

namespace AuthServer.Seeds;

public static class DefaultRoles
{
  public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
  {
    //Seed Roles
    await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
    await roleManager.CreateAsync(new IdentityRole(Roles.Customer.ToString()));
    await roleManager.CreateAsync(new IdentityRole(Roles.Seller.ToString()));
  }
}
