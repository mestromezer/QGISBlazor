using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QGISEFApi.Models
{
    public class Building
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public Geometry Geom { get; set; }
        [Required]
        public string Address { get; set; }
        public BuildingResponse ToBuildingResponse()
        {
            return new BuildingResponse()
            {
                Id = Id,
                Geom = this.Geom.ToString(),
                Address = this.Address
            };
        }
    }
}
