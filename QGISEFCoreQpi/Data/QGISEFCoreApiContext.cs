using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QGISEFCoreApi.Models;

namespace QGISEFCoreApi.Data
{
    public class QGISEFCoreApiContext : DbContext
    {
        public QGISEFCoreApiContext (DbContextOptions<QGISEFCoreApiContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<QGISEFCoreApi.Models.Building> Building { get; set; } = default!;
    }
}
