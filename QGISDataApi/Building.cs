using System.ComponentModel.DataAnnotations;

namespace QGISDataApi
{
    public class Building
    {
        [Required]
        [Range(0,Int32.MaxValue)]
        public int ID { get; set; }
        [Required]
        [StringLength(1024)]
        public string? Geom { get; set; }
        [StringLength(128)]
        public string? Address { get; set; }
    }
}
