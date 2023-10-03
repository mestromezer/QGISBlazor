using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System.Data.SqlTypes;

namespace QGISDirectDatabaseConnectionApi.Services;

public static class SqlGeometryProcessor
{
    public static Geometry SqlBytesToGeometry(byte[] bytes)
    {
        var geometryReader = new SqlServerBytesReader { IsGeography = true };
        return geometryReader.Read(bytes);
    }
}
