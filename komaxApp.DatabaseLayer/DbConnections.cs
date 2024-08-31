using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komaxApp.DatabaseLayer
{
    public static class DbConnections
    {
        public static SqlConnection getCon()
        {
            return new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["KomaxDbCon"].ConnectionString);
        }
        public static string getConnection()
        {
            return ConfigurationManager.ConnectionStrings["KomaxDbCon"].ToString();
        }
    }
}
