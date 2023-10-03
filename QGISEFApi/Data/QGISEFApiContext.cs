using Microsoft.EntityFrameworkCore;

namespace QGISEFApi.Data;

public class QGISEFApiContext : DbContext
{
    public QGISEFApiContext (DbContextOptions<QGISEFApiContext> options)
        : base(options)
    {
    }

    public DbSet<QGISEFApi.Models.Building> Buildings { get; set; } = default!;
}
