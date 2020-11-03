using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data.SqlClient;
using System.Data;
namespace OnlineShop.Areas.Admin.Models
{
    public class ProductModel
    {
        public int Result { get; set; }
        public static List<ProductModel> Delete (int Id)
        {
            using(IDbConnection conn = new SqlConnection(Parameter.connect))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                return conn.Query<ProductModel>("Delete_Product", new { ID = Id }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
    public class ProductNameModel
    {
        public static int GetProductName(string prodName)
        {
            using (IDbConnection conn = new SqlConnection(Parameter.connect))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                return conn.Query<int>("Get_ProductName", new { ProductName = prodName}, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
    }
}