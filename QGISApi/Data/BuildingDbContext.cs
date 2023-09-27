using Microsoft.EntityFrameworkCore;
using QGISApi.Models;

namespace QGISApi.Data
{
    public class BuildingDbContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source = DESKTOP-PMP9UHE; Initial catalog=Project; Integrated Security=true;TrustServerCertificate=True",
                x => x.UseNetTopologySuite());
        }

        public DbSet<Building> Buildings { get; set; }
    }
}
