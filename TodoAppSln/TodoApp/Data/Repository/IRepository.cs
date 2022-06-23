using Microsoft.AspNetCore.Connections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Magazine011.Data.Repository
{
    public interface IRepository
    {
      
        public bool ExecuteQuery(string statement);
        public SqlConnection GetConnection();
        public SqlDataReader FetchData(string statement);
        public bool ExecuteTransaction(List<string> statements);
        public bool BuildDBObject(string statements);
    }
}
