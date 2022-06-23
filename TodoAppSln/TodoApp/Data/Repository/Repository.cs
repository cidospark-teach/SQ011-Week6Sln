using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Magazine011.Data.Repository
{
    public class Repository : IRepository
    {
        private readonly string _conStr;
        private readonly string _conStr2;

        public Repository(IConfiguration configure)
        {
            _conStr = configure.GetSection("ConStrs:Default").Value ;
            _conStr2 = configure.GetSection("ConStrs:Con2").Value;

        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_conStr) ;
        }

        public SqlConnection GetConnection2()
        {
            return new SqlConnection(_conStr2);
        }

        public bool ExecuteQuery(string statement)
        {
            var con = GetConnection();
            try
            {
                con.Open();
                using (var cmd = new SqlCommand(statement,con))
                {
                    var res = cmd.ExecuteNonQuery();
                    if (res > 0)
                        return true;
                    else
                        return false;
                }
            }
            finally
            {
                con.Close();
            }

        }

        public SqlDataReader FetchData(string statement)
        {
            var con = GetConnection();
            con.Open();
            using (var cmd = new SqlCommand(statement, con))
            {
                var res = cmd.ExecuteReader();
                return res;
            }

        }

        public bool ExecuteTransaction(List<string> statements)
        {
            var con = GetConnection();
            con.Open();
            SqlTransaction transObj = con.BeginTransaction();

            try
            {
                foreach(var statement in statements)
                {
                    using (var cmd = new SqlCommand(statement, con, transObj))
                    {
                        var res = cmd.ExecuteNonQuery();
                    }
                }
                transObj.Commit();
                return true;
            }
            catch(Exception ex)
            {
                transObj.Rollback();
                // log exception
                throw new Exception(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        public bool BuildDBObject(string statement)
        {
            var con = GetConnection2();
            try
            {
                con.Open();
                using (var cmd = new SqlCommand(statement, con))
                {
                    var res = cmd.ExecuteNonQuery();
                    if (res > 0)
                        return true;
                    else
                        return false;
                }
            }
            finally
            {
                con.Close();
            }
        }
    }
}
