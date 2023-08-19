using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace To_do_List_Application
{
    public class dbConnect
    {
        private readonly string connectionString;

        public dbConnect(string serverName, string dbName)
        {
            connectionString = $"Data Source={serverName};Initial Catalog={dbName};Integrated Security=True";
        }

        public SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }
    }
}
