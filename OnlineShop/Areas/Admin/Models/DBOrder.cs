using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace OnlineShop.Areas.Admin.Models
{
    public class DBOrder
    {
        public string Result { get; set; }
        public static List<DBOrder> Delete(int id)
        {
            using (IDbConnection connection = new SqlConnection(Parameter.connect))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                return connection.Query<DBOrder>("DELETE_Order", new { ID = id }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}