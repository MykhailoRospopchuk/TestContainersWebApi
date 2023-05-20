using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TestContainerWebApi.Models;

namespace TestContainerWebApi.db
{
    public class AppDbContext : DbContext
    {
        private IConfiguration _configuration;
        private readonly string _con_str;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            _con_str = Database.GetConnectionString();
        }

        

    }
}
