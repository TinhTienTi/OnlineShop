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
    public class OrderDetailModel
    {
        public static void Insert(long prodID, long orderId, int? quantity, decimal? price)
        {
            using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["OnlineShopDbConnect"].ToString()))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                conn.Query("InsertOrderDetail", new { ProductId = prodID, OrderId = orderId, Quantity = quantity, Price = price}, commandType: CommandType.StoredProcedure);
            }
        }
    }
}