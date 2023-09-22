using Microsoft.SqlServer.Types;
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QGISEFCoreApi.Models
{
    public class Building
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Range(0, int.MaxValue)]
        public int ID { get; set; }
        [Required]
        public SqlGeometry Geom { get; set; }
        [StringLength(128)]
        public string Address { get; set; }
    }
}
