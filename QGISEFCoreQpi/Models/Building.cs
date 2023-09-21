using Microsoft.SqlServer.Types;
using System.ComponentModel.DataAnnotations;

namespace QGISEFCoreApi.Models
{
    public class Building
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int ID { get; set; }
        [Required]
        [StringLength(4096)]
        public string? Geom { get; set; }
        [StringLength(128)]
        public string? Address { get; set; }
    }
}
