using Microsoft.SqlServer.Types;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.Json;

namespace QGISDataApi.Services
{
    public class DbConnection: IDbConnection
    {
        private readonly string _connectionString = "Data Source = DESKTOP-PMP9UHE; Initial catalog=Project; Integrated Security=true";

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
                        var _id = reader.GetInt32(0);
                        var _geom = SqlGeometry.Deserialize(reader.GetSqlBytes(1)).ToString();
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
    }
}
