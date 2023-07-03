
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TestContainerWebApi.Auth;
using TestContainerWebApi.db;

namespace TestContainerWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            string connection = builder.Configuration.GetConnectionString("DbConnectionString");

            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection));
            builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(connection));
            builder.Services.AddDbContext<AdoDbContext>(options => options.UseSqlServer(connection));


            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "TestAuth";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Set the expiration time for the cookie
                options.LoginPath = "/Account/Login"; // Redirect to the login page if unauthenticated
                options.AccessDeniedPath = "/Account/Login"; // Redirect to the access denied page if unauthorized
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}