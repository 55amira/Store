
using Domain.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Services;
using Services_Abstractions;
using Shared.ErrorsModels;
using Store_Api.Middelware;
using System.Threading.Tasks;
using AssemblyMapping= Services.AssemblyReference;

namespace Store_Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                //options.UseSqlServer(builder.Configuration["ConnectionStrings:DefultConnection"]);
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefultConnection"));
            });
            builder.Services.AddScoped<IDbInitializer,Iniaitiazer>();   // Alloe DI For IDbInitializer
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
            builder.Services.AddAutoMapper(typeof(AssemblyMapping).Assembly);
            builder.Services.AddScoped<IServiceManager,ServiceManager>();

            builder.Services.Configure<ApiBehaviorOptions>(config =>

               config.InvalidModelStateResponseFactory = (ActionContext) =>
               {
                   var errors = ActionContext.ModelState.Where(m => m.Value.Errors.Any())
                   .Select(m => new ValidationError()
                   {
                       Field = m.Key,
                       Errors = m.Value.Errors.Select(errors => errors.ErrorMessage)
                   });

                   var response = new ValidationerrorResponse()
                   {
                       Errors = errors
                   };
                  
                   return new BadRequestObjectResult(response);
               }
            );

            var app = builder.Build();

            #region Seeding 
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>(); // Ask CLR Create Object From Iniaitiazer
            await dbInitializer.InitializerAsync(); 
            #endregion

            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
