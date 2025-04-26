using Domain.Contracts;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Persistence.Identity;
using Services;
using Shared.ErrorsModels;
using Store_Api.Middelware;

namespace Store_Api.Extentions
{
    public static class Extentions
    {
        public static IServiceCollection RegisterAllServices (this IServiceCollection services, IConfiguration configuration)
        {

           

            services.AddBuiltInServices();
            services.AddSwaggerServices();
            services.ConfigureServices();



            services.AddInfrastructureServices(configuration);
            services.AddIdentityServices();


            services.AddApplicationServices();

            return services;
        }

        private static IServiceCollection AddBuiltInServices(this IServiceCollection services)
        {

            services.AddControllers();
           

            return services;
        }
        private static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {

            services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<StoreIdentityDbContext>();

            return services;
        }


        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();


            return services;
        }
        private static IServiceCollection ConfigureServices(this IServiceCollection services)
        {

            services.Configure<ApiBehaviorOptions>(config =>

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


            return services;
        }

        public static async Task<WebApplication> ConfigureMiddelwares (this WebApplication app)
        {

            await app.InitalizeDatabaseAsunc();

            app.UseGlobalErrorHandling();

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

            return app;
        }

        private static async Task<WebApplication> InitalizeDatabaseAsunc(this WebApplication app)
        { 
          
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>(); // Ask CLR Create Object From Iniaitiazer
            await dbInitializer.InitializerAsync();
            await dbInitializer.InitializerIdentityAsync();

            return app;
        }

        private static WebApplication UseGlobalErrorHandling(this WebApplication app)
        {

            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            return app;
        }

    }
}
