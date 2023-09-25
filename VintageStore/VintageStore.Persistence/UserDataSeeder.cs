using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VintageStore.Domain;

namespace VintageStore.Persistence
{
    public static class UserDataSeeder
    {
        public static async Task Seed(IServiceProvider service)
        {
            // Repositorio de usuarios
            var userManager = service.GetRequiredService<UserManager<VintageStoreUserIdentity>>();
            // Repositorio de Roles
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

            // Crear Roles
            var adminRole = new IdentityRole(Constantes.RolAdmin);
            var customerRole = new IdentityRole(Constantes.RolCustomer);

            if (!await roleManager.RoleExistsAsync(Constantes.RolAdmin))
                await roleManager.CreateAsync(adminRole);

            if (!await roleManager.RoleExistsAsync(Constantes.RolCustomer))
                await roleManager.CreateAsync(customerRole);

            var adminUser = new VintageStoreUserIdentity
            {
                FirstName = "Administrador",
                LastName = "del Sistema",
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                PhoneNumber = "999 999 999",
                Age = 35,
                DocumentType = DocumentTypeEnum.Dni,
                DocumentNumber = "12345678",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin1234*");
            if (result.Succeeded)
            {
                // Esto me asegura que el usuario se creó correctamente.
                adminUser = await userManager.FindByEmailAsync(adminUser.Email);
                if (adminUser is not null)
                    await userManager.AddToRoleAsync(adminUser, Constantes.RolAdmin);
            }
        }
    }
}
