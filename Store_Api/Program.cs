
using Domain.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Services;
using Services_Abstractions;
using Shared.ErrorsModels;
using Store_Api.Extentions;
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

            builder.Services.RegisterAllServices(builder.Configuration);


            var app = builder.Build();

           await app.ConfigureMiddelwares();

            app.Run();
        }
    }
}
