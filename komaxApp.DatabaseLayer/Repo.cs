using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komaxApp.DatabaseLayer
{
    public class Repo
    {
        public static (int StatusCode , string StatusDetails, int LabelCount, string labelStatus) ExecuteStoredProcedureWithOutputs(object model)
        {
            using (var connection = new SqlConnection(DbConnections.getCon().ConnectionString))
            {
                connection.Open();
                var parameters = new DynamicParameters();

                // Add model properties as parameters
                foreach (var property in model.GetType().GetProperties())
                {
                    parameters.Add("@" + property.Name, property.GetValue(model) ?? DBNull.Value);
                }

                // Add output parameters
                parameters.Add("@StatusDetails", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);
                parameters.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@LabelCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@LabelStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 99);
                // Execute the stored procedure
                connection.Execute("sp_InsertLoadRecord", parameters, commandType: CommandType.StoredProcedure);

                // Retrieve output parameters
                string statusDetails = parameters.Get<string>("@StatusDetails");
                int statusCode = parameters.Get<int>("@Status");
                int labelCount = parameters.Get<int>("@LabelCount");
                string labelStatus = parameters.Get<string>("@LabelStatus");
                return (statusCode, statusDetails, labelCount, labelStatus);
            }
        }
    }
}
