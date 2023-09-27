using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
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
