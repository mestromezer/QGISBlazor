using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QGISEFApi.Models;

namespace QGISEFApi.Data
{
    public class QGISEFApiContext : DbContext
    {
        public QGISEFApiContext (DbContextOptions<QGISEFApiContext> options)
            : base(options)
        {
        }

        public DbSet<QGISEFApi.Models.Building> Buildings { get; set; } = default!;
    }
}
