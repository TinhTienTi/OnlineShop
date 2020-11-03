using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Configuration;

namespace OnlineShop.Models
{
    public class ProductQuantity
    {
        public int Quantiy { get; set; }
        public static int Get(long Id)
        {
            using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["OnlineShopDbConnect"].ToString()))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                return conn.Query<int>("GETQUANTITYPRODUCT", new { ID = Id }, commandType: CommandType.StoredProcedure).First();
            }
        }
    }
    public class UpdateProductQuantity
    {
        public static void Update(long Id, int? quantity)
        {
            using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["OnlineShopDbConnect"].ToString()))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                conn.Query("UpdateQuantityProduct", new { ID = Id, Quantity = quantity }, commandType: CommandType.StoredProcedure);
            }
        }
    }
    public class ProductViewCount
    {
        public static void Update(long ID)
        {
            using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["OnlineShopDbConnect"].ToString()))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
               conn.Query("Update_ViewCountProduct", new { Id = ID}, commandType: CommandType.StoredProcedure);
            }
        }
    }
}