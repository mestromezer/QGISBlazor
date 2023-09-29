﻿using Microsoft.SqlServer.Types;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QGISDirectDatabaseConnectionApi.Models;
using Microsoft.Extensions.Options;
using NetTopologySuite.Geometries;

namespace QGISDirectDatabaseConnectionApi.Services
{
    public class DbContext: IDbContext
    {
        private readonly string _connectionString;
        public DbContext()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json")
           .Build();
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<List<Building>> GetItems()
        {
            string Query = "SELECT * FROM Buildings" ;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                List<Building> _buildings = new List<Building>(1024);
                SqlCommand cmd = new SqlCommand(Query, con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        if (reader.IsDBNull(0))
                        {
                            return null;
                        }
                        var _id = reader.GetInt32(0);
                        var _geom =  SqlGeometry.Deserialize(reader.GetSqlBytes(1)).ToString();
                        var _addres = reader.GetString(2);
                        var newbie = new Building()
                        {
                            ID = _id,
                            Geom = _geom,
                            Address = _addres
                        };
                        _buildings.Add(newbie);
                    }

                    con.Close();
                    return _buildings;
                }
            }
        }
        public async Task<Building> GetItem(int id)
        {
            string query = $"SELECT * FROM Buildings WHERE Buildings.ID={id}";

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
                    var _id = reader.GetInt32(0);
                    var _geom = SqlGeometry.Deserialize(reader.GetSqlBytes(1)).ToString();
                    var _address = reader.GetString(2);
                    return new Building()
                    {
                        ID = _id,
                        Geom = _geom,
                        Address = _address
                    };
                }
            }
        }
        public async Task AddItem(Building newbie)
        {
            string query = $"DECLARE @g geometry;   \r\nSET @g = geometry::Parse(\'{newbie.Geom}\')\n" +
                $"INSERT INTO Buildings (geom,address) VALUES(@g,\'{newbie.Address}\')";
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                var command = new SqlCommand(query, con);
                int number = await command.ExecuteNonQueryAsync();
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
            string query = $"DELETE FROM Buildings WHERE Buildings.ID={id}";
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                var command = new SqlCommand(query, con);
                int number = await command.ExecuteNonQueryAsync();
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
            string query= $"DECLARE @g geometry;   \r\nSET @g = geometry::Parse('{newbie.Geom}')\n" +
                $"UPDATE Buildings\n" +
                $"SET Buildings.geom=@g, Buildings.address=\'{newbie.Address}\'\n" +
                $"WHERE Buildings.ID={newbie.ID}";
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                var exec_query = new SqlCommand(query, con);
                int number = await exec_query.ExecuteNonQueryAsync();
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
}
