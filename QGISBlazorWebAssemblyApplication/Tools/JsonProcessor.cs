using NetTopologySuite.IO;
using Newtonsoft.Json;

namespace QGISBlazorWebAssemblyApplication.Tools;
public static class JsonProcessor<T>
{
    public static string ConvertToGeoJson(T data)
    {
        var serializer = GeoJsonSerializer.Create();
        using (var stringWriter = new StringWriter())
        using (var jsonWriter = new JsonTextWriter(stringWriter))
        {
            serializer.Serialize(jsonWriter, data);
            return stringWriter.ToString();
        }
    }
    public static T Parse(string data)
    {
        var serializer = GeoJsonSerializer.Create();
        using (var stringReader = new StringReader(data))
        using (var jsonReader = new JsonTextReader(stringReader))
        {
            return serializer.Deserialize<T>(jsonReader);
        }
    }
}
