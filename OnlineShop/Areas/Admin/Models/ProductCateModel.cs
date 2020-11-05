using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace OnlineShop.Areas.Admin.Models
{
    public class ProductCateModel
    {
        public int Result { get; set; }
        public static List<ProductCateModel> Delete(int Id)
        {
            using (IDbConnection conn = new SqlConnection(Parameter.connect))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                return conn.Query<ProductCateModel>("Delete_ProductCate", new { ID = Id }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
    public class GetProductCateName
    {
        public int Result { get; set; }
        public static int GetProdCateName(string name)
        {
            using (IDbConnection conn = new SqlConnection(Parameter.connect))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                return conn.Query<int>("Get_ProductCateName", new { Name = name }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
    }
}