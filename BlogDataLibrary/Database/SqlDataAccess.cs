using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BlogDataLibrary.Database
{
    public class SqlDataAccess : ISqlDataAccess
    {

        /*Type in the configuration property and the constructor:*/
        private IConfiguration _config;
        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        /*For SQL commands that require return of data, type in the following inside the 
        SqlDataAccess class as another method:*/
        public List<T> LoadData<T, U>(string sqlSatement,
                                      U parameters,
                                      string connectionStringName,
                                      bool isStoredProcedure)
        {
            CommandType commandType = CommandType.Text;
            string connectionString = _config.GetConnectionString(connectionStringName);

            if (isStoredProcedure)
            {
                commandType = CommandType.StoredProcedure;
            }

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(sqlSatement, parameters, commandType: commandType).ToList();
                return rows;
            }
        }

        /*For SQL commands that require return of data, type in the following inside the 
            SqlDataAccess class as another method:*/
        public void SaveData<T>(string sqlStatement,
                                T parameters,
                                string connectionStringName,
                                bool isStoredProcedure)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);
            CommandType commandType = CommandType.Text;

            if (isStoredProcedure)
            {
                commandType = CommandType.StoredProcedure;
            }

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(sqlStatement, parameters, commandType: commandType);
            }
        }
    }
}
