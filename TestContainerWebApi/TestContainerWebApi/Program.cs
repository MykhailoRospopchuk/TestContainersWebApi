
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            string connection = builder.Configuration.GetConnectionString("DbConnectionString");

            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection));
            builder.Services.AddDbContext<AdoDbContext>(options => options.UseSqlServer(connection));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}