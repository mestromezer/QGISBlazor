using NetTopologySuite.Geometries;
using System.Text;
using System.Text.Json.Serialization;

namespace QGISBlazorWebAssemblyApplication.Models
{
    public class Building
    {
        public int ID { get; set; }
        public Geometry? Geom { get; set; }
        public string? Address { get; set; }
    }
}
