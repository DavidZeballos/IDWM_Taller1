using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using EFTEC;
using IDWM_TallerAPI.Src.Models;

namespace IDWM_TallerAPI.Src.Data
{
    public class DataSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateAsyncScope())
            {
                var service = scope.ServiceProvider;
                var context = service.GetRequiredService<DataContext>();

                if(!context.Roles.Any()){
                    context.Roles.AddRange(
                        new Role {Name = "Admin"},
                        new Role {Name = "User"}
                    );
                }

                if(!context.ProductTypes.Any()){
                    context.ProductTypes.AddRange(
                        new ProductType {Name = "Tecnología"},
                        new ProductType {Name = "Hogar y Decoración"},
                        new ProductType {Name = "Vestimenta"},
                        new ProductType {Name = "Alimentos"},
                        new ProductType {Name = "Juguetes"},
                        new ProductType {Name = "Herramientas"}
                    );
                }

                if(!context.Users.Any()){
                    
                    var user = new User {
                        Name = "David Zeballos",
                        Rut = "19.952.533-6",
                        Email = "david.zeballos@gmail.com",
                        DateOfBirth = new DateTime (1999,2,9),
                        Gender = "Masculino",
                        Password = BCrypt.Net.BCrypt.HashPassword("P4ssw0rd"),
                        RoleId = 1,
                        Status = true

                    };

                    context.Users.Add(user);

                    var genders = new[] { "Masculino", "Femenino", "Otro" };

                    var faker = new Faker<User>()
                    .RuleFor(u => u.Name, f => f.Person.FullName)
                    .RuleFor(u => u.Rut, f => RutChile.ConvierteTipoRut(RutChile.GeneraRut(1,99999999),10,true,true))
                    .RuleFor(u => u.Email, f => f.Internet.Email())
                    .RuleFor(u => u.DateOfBirth, f => f.Person.DateOfBirth)
                    .RuleFor(u => u.Gender, f => f.PickRandom(genders))
                    .RuleFor(u => u.Password, f => BCrypt.Net.BCrypt.HashPassword("password"))
                    .RuleFor(u => u.RoleId, f => 2)
                    .RuleFor(u => u.Status, f => true);

                    var users = faker.Generate(20);

                    context.Users.AddRange(users);
                }

                if(!context.Products.Any()){
                    var productFaker = new Faker<Product>()
                    .RuleFor(p => p.Id, f => f.IndexFaker + 1)
                    .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                    .RuleFor(p => p.Price, f => f.Random.Int(1000, 1000000))
                    .RuleFor(p => p.InStock, f => f.Random.Int(5, 100))
                    .RuleFor(p => p.ImageURL, f => f.Image.PicsumUrl(400, 400, true))
                    .RuleFor(p => p.ProductTypeId, f => f.Random.Int(1, 6));

                    var products = productFaker.Generate(5);
                    context.Products.AddRange(products);
                }

                context.SaveChanges();
            }
        }
    }
}