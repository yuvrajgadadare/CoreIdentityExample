using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Models
{
    public class DapperContext
    {
        //private readonly IConfiguration _configuration;
        //private readonly string _connectionString;
        public DapperContext()
        {
          //  _configuration = configuration;
            //_connectionString = _configuration.GetConnectionString("SqlConnection");
        }
        public IDbConnection CreateConnection() => new SqlConnection(DatabaseOperations.ConnectionString);
    }
}
