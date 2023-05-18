using Microsoft.EntityFrameworkCore;

namespace TestContainerWebApi.db
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

    }
}
