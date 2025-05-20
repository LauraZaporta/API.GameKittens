using System;
using API.GameKittens.Context;
using API.GameKittens.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Build delay
        await Task.Delay(3000);


        // Add services to the container.

        //Afegim DbContext
        var connectionString = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
        object value = builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

        // Identity
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            // Configuraci� de contrasenyes
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = true;

            // Configuraci� del correu electr�nic
            options.User.RequireUniqueEmail = true;

            // Configuraci� de lockout (bloqueig d�usuari)
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // Configuraci� del login
            options.SignIn.RequireConfirmedEmail = false; // true si vols que es confirmi el correu
        })
                 .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();


        builder.Services.AddRazorPages();
        builder.Services.AddHttpContextAccessor();

        // Swagger
        builder.Services.AddSwaggerGen();

        // Controladors
        builder.Services.AddControllers();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();



        /*--------------*/
        var app = builder.Build();
        /*--------------*/


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();


        app.Run();
    }
}