using Microsoft.SqlServer.Types;
using QGISDirectDatabaseConnectionApi.Models;
using System.Data.SqlClient;

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
                    var geom =  SqlGeometry.Deserialize(reader.GetSqlBytes(1)).ToString();
                    var addres = reader.GetString(2);
                    var newbie = new Building()
                    {
                        ID = id,
                        Geom = geom,
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
            var command = new SqlCommand(query, con);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                await reader.ReadAsync();
                if (reader.IsDBNull(0))
                {
                    return null;
                }
                var id = reader.GetInt32(0);
                var geom = SqlGeometry.Deserialize(reader.GetSqlBytes(1)).ToString();
                var address = reader.GetString(2);
                return new Building()
                {
                    ID = id,
                    Geom = geom,
                    Address = address
                };
            }
        }
    }
    public async Task AddItem(Building newbie)
    {
        var query = $"DECLARE @g geometry;   \r\nSET @g = geometry::Parse(\'{newbie.Geom}\')\n" +
            $"INSERT INTO Buildings (geom,address) VALUES(@g,\'{newbie.Address}\')";
        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();
            var command = new SqlCommand(query, con);
            var number = await command.ExecuteNonQueryAsync();
            if (number == 0) 
            {
                throw new Exception(
                    $"Exception occured while trying to add element with id={newbie.ID}"
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
            var command = new SqlCommand(query, con);
            var number = await command.ExecuteNonQueryAsync();
            if (number == 0)
            {
                throw new Exception(
                    $"Exception occured while trying to delete element with id={id}\n"+
                    $"Could not find"
                    );
            }
        }
    }
    public async Task UpdateItem(Building newbie)
    {
        var query= $"DECLARE @g geometry;   \r\nSET @g = geometry::Parse('{newbie.Geom}')\n" +
            $"UPDATE Buildings\n" +
            $"SET Buildings.geom=@g, Buildings.address=\'{newbie.Address}\'\n" +
            $"WHERE Buildings.ID={newbie.ID}";
        using (var con = new SqlConnection(_connectionString))
        {
            con.Open();
            var exec_query = new SqlCommand(query, con);
            var number = await exec_query.ExecuteNonQueryAsync();
            if (number == 0)
            {
                throw new Exception(
                    $"Exception occured while trying to update element with id={newbie.ID}\n" +
                    $"No such record found\n"
                    );
            }
        }
    }
}
