using Domain.Contracts;
using Domain.Models;
using Domain.Models.Identity;
using Domain.Models.OrderModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence
{
    public class Iniaitiazer : IDbInitializer
    {
        private readonly StoreDbContext _context;
        private readonly StoreIdentityDbContext _identityDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public Iniaitiazer(StoreDbContext context,
            StoreIdentityDbContext identityDbContext,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager) 
        {
            _context = context;
            _identityDbContext = identityDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task InitializerAsync()
        {
            // Create Database if it doesn`t Exists && Apply any Pending Migrations   
            if(_context.Database.GetPendingMigrations().Any())
            {
                await _context.Database.MigrateAsync();

            }

            // Data Seeding 

            // Seeding ProductTypes From Json File
            if(!_context.ProductTypes.Any())
            {
                // 1. Read All Data from Types json file as string
                 var TypesData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\types.json");

                // 2. Transform string to C# Objects [List<ProductTypes>]
                 var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);

                // 3. Add List<ProductTypes> To Database
                if( Types is not null && Types.Any())
                {
                     await _context.ProductTypes.AddRangeAsync(Types);
                     await _context.SaveChangesAsync();

                }
            }


            //Seeding ProductBrands From Json File
            if (!_context.ProductBrands.Any())
            {
                // 1. Read All Data from Types json file as string
                var BrandData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\brands.json");

                // 2. Transform string to C# Objects [List<ProductBrands>]
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandData);

                // 3. Add List<ProductBrands> To Database
                if (brands is not null && brands.Any())
                {
                    await _context.ProductBrands.AddRangeAsync(brands);
                    await _context.SaveChangesAsync();

                }
            }
            //Seeding Products From Json File
            if (!_context.Products.Any())
            {
                // 1. Read All Data from Products json file as string
                var ProductData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\products.json");

                // 2. Transform string to C# Objects [List<Product>]
                var products = JsonSerializer.Deserialize<List<Product>>(ProductData);

                // 3. Add List<Product> To Database
                if (products is not null && products.Any())
                {
                    await _context.Products.AddRangeAsync(products);
                    await _context.SaveChangesAsync();

                }
            }
            if (!_context.DeliveryMethods.Any())
            {
                // 1. Read All Data from delivery json file as string
                var deliveryData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\delivery.json");

                // 2. Transform string to C# Objects [List<DeliveryMethod>]
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);

                // 3. Add List<delivery> To Database
                if (deliveryMethods is not null && deliveryMethods.Any())
                {
                    await _context.DeliveryMethods.AddRangeAsync(deliveryMethods);
                    await _context.SaveChangesAsync();

                }
            }

        }

        public async Task InitializerIdentityAsync()
        {
            // Create Database if it doesn`t Exists && Apply any Pending Migrations   
            if (_identityDbContext.Database.GetPendingMigrations().Any())
            {
                await _identityDbContext.Database.MigrateAsync();

            }
            if(!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = "Admin"
                });

                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = "SuperAdmin"
                });
            }
            if (!_userManager.Users.Any())
            {
                var superAdminUser = new AppUser()
                {
                    DisplayName = "Super Admin",
                    Email = "SuperAdmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "0123456789"
                };
                var adminUser = new AppUser()
                {
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    UserName = "Admin",
                    PhoneNumber = "0123456789"
                };


                await _userManager.CreateAsync(superAdminUser, password: "P@ssW0rd");
                await _userManager.CreateAsync(adminUser, password: "P@ssW0rd");

                await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
//