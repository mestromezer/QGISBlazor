using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace QGISEFApi.Models
{
    public class Building
    {
        [Key]
        public int Id { get; set; }
        public Geometry? Geom { get; set; }
        public string? Address { get; set; }
    }
}
