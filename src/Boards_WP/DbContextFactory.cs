using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Boards_WP.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

           
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-GFA6UNJ\\SQLEXPRESS;Initial Catalog=Communities;Integrated Security=True;Encrypt=False;TrustServerCertificate=True");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}