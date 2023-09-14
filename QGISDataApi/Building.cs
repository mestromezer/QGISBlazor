using Microsoft.SqlServer.Types;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlTypes;

namespace QGISDataApi
{
    public class Building
    {
        [Required]
        [Range(0,Int32.MaxValue)]
        public int ID { get; set; }
        [Required]
        public string? Geom { get; set; }
        [StringLength(128)]
        public string? Address { get; set; }
    }
}
