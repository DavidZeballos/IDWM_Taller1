using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using EFTEC;
using IDWM_TallerAPI.Src.Models;
using Microsoft.AspNetCore.Identity;

namespace IDWM_TallerAPI.Src.Data
{
    public class DataSeeder
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateAsyncScope())
            {
                var service = scope.ServiceProvider;
                var context = service.GetRequiredService<DataContext>();
                var userManager = service.GetRequiredService<UserManager<User>>();
                var roleManager = service.GetRequiredService<RoleManager<IdentityRole<int>>>();

                // Crear roles si no existen
                if (!context.Roles.Any())
                {
                    await roleManager.CreateAsync(new IdentityRole<int> { Name = "Admin" });
                    await roleManager.CreateAsync(new IdentityRole<int> { Name = "User" });
                }

                // Crear tipos de productos si no existen
                if (!context.ProductTypes.Any())
                {
                    context.ProductTypes.AddRange(
                        new ProductType { Name = "Poleras" },
                        new ProductType { Name = "Gorros" },
                        new ProductType { Name = "Juguetería" },
                        new ProductType { Name = "Alimentación" },
                        new ProductType { Name = "Libros" }
                    );
                }

                // Crear usuarios si no existen
                if (!context.Users.Any())
                {
                    // Crear usuario administrador
                    var adminUser = new User
                    {
                        UserName = GenerateValidName("Ignacio Mancilla"),
                        Rut = "20.416.699-4",
                        Email = "admin@idwm.cl",
                        DateOfBirth = new DateTime(2000, 10, 25),
                        Gender = "Masculino",
                        Status = true
                    };

                    var adminPassword = "P4ssw0rd";
                    var createAdminResult = await userManager.CreateAsync(adminUser, adminPassword);

                    if (createAdminResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                    else
                    {
                        throw new Exception("No se pudo crear el usuario administrador: " +
                            string.Join(", ", createAdminResult.Errors.Select(e => e.Description)));
                    }

                    // Crear usuarios de prueba
                    var genders = new[] { "Masculino", "Femenino", "Otro" };

                    var faker = new Faker<User>()
                        .RuleFor(u => u.UserName, f => GenerateValidName(f.Name.FullName()))
                        .RuleFor(u => u.Rut, f => RutChile.ConvierteTipoRut(RutChile.GeneraRut(1, 99999999), 10, true, true))
                        .RuleFor(u => u.Email, f => f.Internet.Email())
                        .RuleFor(u => u.DateOfBirth, f => f.Date.Past(30))
                        .RuleFor(u => u.Gender, f => f.PickRandom(genders))
                        .RuleFor(u => u.Status, f => true);

                    var users = faker.Generate(20);

                    foreach (var user in users)
                    {
                        var result = await userManager.CreateAsync(user, "usuarionormal123");
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(user, "User");
                        }
                        else
                        {
                            throw new Exception("No se pudo crear el usuario de prueba: " +
                                                string.Join(", ", result.Errors.Select(e => e.Description)));
                        }
                    }
                }

                // Crear productos si no existen
                if (!context.Products.Any())
                {
                    var productFaker = new Faker<Product>()
                        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                        .RuleFor(p => p.Price, f => f.Random.Int(1000, 1000000))
                        .RuleFor(p => p.InStock, f => f.Random.Int(5, 100))
                        .RuleFor(p => p.ImageURL, f => f.Image.PicsumUrl(400, 400, true))
                        .RuleFor(p => p.ProductTypeId, f => f.Random.Int(1, 5));

                    var products = productFaker.Generate(5);
                    context.Products.AddRange(products);
                }

                await context.SaveChangesAsync();
            }
        }

        // Método para generar nombres válidos (de acuerdo a los requerimientos)
        private static string GenerateValidName(string name)
        {
            // Eliminar caracteres no alfabéticos
            var cleanedName = new string(name.Where(c => char.IsLetter(c) || char.IsWhiteSpace(c)).ToArray());

            // Verificar y ajustar longitud
            if (cleanedName.Length < 10)
            {
                cleanedName = cleanedName.PadRight(10, 'a');
            }
            else if (cleanedName.Length > 64)
            {
                cleanedName = cleanedName.Substring(0, 64);
            }

            return cleanedName;
        }
    }
}
