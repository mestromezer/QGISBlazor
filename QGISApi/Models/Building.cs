using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace QGISApi.Models
{
    public class Building
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int ID { get; set; }
        [Required]
        [StringLength(4096)]
        public Geometry? Geom { get; set; }
        [StringLength(128)]
        public string? Address { get; set; }
    }
}
