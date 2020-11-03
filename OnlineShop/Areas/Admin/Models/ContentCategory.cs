using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data.SqlClient;
using System.Data;
namespace OnlineShop.Areas.Admin.Models
{
    public class ContentCategory
    {
        public int Result { get; set; }
        public static List<ContentCategory> Delete(int Id)
        {
            using (IDbConnection conn = new SqlConnection(Parameter.connect))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                return conn.Query<ContentCategory>("Delete_ContenCate", new { ID = Id }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}