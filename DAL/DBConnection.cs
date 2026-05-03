using System.Configuration;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class DBConnection
    {
        private readonly string _connectionString;

        public DBConnection()
        {
            // Tries to find the connection string added by Visual Studio or fallback
            _connectionString = ConfigurationManager.ConnectionStrings["template_api.Properties.Settings.valhalla"]?.ConnectionString 
                                ?? ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString
                                ?? "Data Source=localhost;Initial Catalog=valhalla_db;Integrated Security=True;Trust Server Certificate=True;";
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
