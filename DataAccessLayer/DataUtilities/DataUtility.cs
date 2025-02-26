using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataUtilities
{
    public class DataUtility
    {
        private readonly string? _connection;
        public DataUtility(IConfiguration configuration)
        {
            _connection = configuration.GetConnectionString("DBConnection");
        }

        //for insert/update/delete
        public int ExecuteSql(string ProcedureName, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connection))
                {
                    SqlCommand command = new SqlCommand(ProcedureName, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    if(parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return result;                    
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //for fetching table (DataTable)
        public DataTable GetDataTable(string ProcedureName, SqlParameter[] parameters = null)
        {
            try
            {
                DataTable dataTable = new DataTable();
                using (SqlConnection connection = new SqlConnection(_connection))
                {
                    SqlCommand command = new SqlCommand(ProcedureName, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    
                    using (SqlDataAdapter adpater = new SqlDataAdapter(command))
                    {
                        adpater.Fill(dataTable);
                    }
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
