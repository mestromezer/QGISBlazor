using Microsoft.SqlServer.Types;
using QGISDirectDatabaseConnectionApi.Models;
using System.Data.SqlClient;
using NetTopologySuite.IO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace QGISDirectDatabaseConnectionApi.Services;

public class DbContext: IDbContext<Building>
{
    private readonly string _connectionString;
    public DbContext()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
       .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
       .AddJsonFile("appsettings.json")
       .Build();
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        if (_connectionString == null) throw new Exception("Connection string was null.");
    }
    public async Task<List<Building>> GetItems()
    {
        var query = "SELECT * FROM Buildings" ;

        using (var con = new SqlConnection(_connectionString))
        {
            var buildings = new List<Building>(1024);
            var cmd = new SqlCommand(query, con);
            con.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    if (reader.IsDBNull(0))
                    {
                        return null;
                    }
                    var id = reader.GetInt32(0);
                    var bytes = reader.GetSqlBytes(1).Value;
                    var geometry = SqlGeometryProcessor.SqlBytesToGeometry(bytes);
                    var addres = reader.GetString(2);
                    var newbie = new Building()
                    {
                        ID = id,
                        Geom = geometry,
                        Address = addres
                    };
                    buildings.Add(newbie);
                }

                con.Close();
                return buildings;
            }
        }
    }
    public async Task<Building> GetItem(int Id)
    {
        var query = $"SELECT * FROM Buildings WHERE Buildings.ID={Id}";

        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();
            var cmd = new SqlCommand(query, con);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                await reader.ReadAsync();
                if (reader.IsDBNull(0))
                {
                    return null;
                }
                var id = reader.GetInt32(0);
                var bytes = reader.GetSqlBytes(1).Value;
                var geometry = SqlGeometryProcessor.SqlBytesToGeometry(bytes);
                var address = reader.GetString(2);
                return new Building()
                {
                    ID = id,
                    Geom = geometry,
                    Address = address
                };
            }
        }
    }
    public async Task AddItem(Building newbie)
    {
        var query = "INSERT INTO Buildings (geom,address) VALUES(@geom, @address)";
        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();
            var cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(new SqlParameter("geom", newbie.Geom.Buffer(0).AsText()));
            cmd.Parameters.Add(new SqlParameter("address", newbie.Address));
            var number = await cmd.ExecuteNonQueryAsync();
            if (number == 0) 
            {
                throw new Exception(
                    $"Exception occured while trying to add element with id={newbie.ID}"
                    );
            }
        }
    }
    public async Task UpdateItem(Building newbie)
    {
        var query = $"UPDATE Buildings\n" +
            $"SET Buildings.geom=@geom, Buildings.address=@address\n" +
            $"WHERE Buildings.ID={newbie.ID}";
        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();
            var cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(new SqlParameter("geom", newbie.Geom.Buffer(0).AsText()));
            cmd.Parameters.Add(new SqlParameter("address", newbie.Address));
            var number = await cmd.ExecuteNonQueryAsync();
            if (number == 0)
            {
                throw new Exception(
                    $"Exception occured while trying to update element with id={newbie.ID}\n" +
                    $"No such record found\n"
                    );
            }
        }
    }
    public async Task DeleteItem(int id)
    {
        var query = $"DELETE FROM Buildings WHERE Buildings.ID={id}";
        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();
            var cmd = new SqlCommand(query, con);
            var number = await cmd.ExecuteNonQueryAsync();
            if (number == 0)
            {
                throw new Exception(
                    $"Exception occured while trying to delete element with id={id}\n"+
                    $"Could not find"
                    );
            }
        }
    }
}
