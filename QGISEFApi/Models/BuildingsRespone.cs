using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace QGISEFApi.Models
{
    public class BuildingResponse
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Geom { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
