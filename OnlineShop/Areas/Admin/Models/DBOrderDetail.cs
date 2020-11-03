using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data;
using System.Data.SqlClient;
namespace OnlineShop.Areas.Admin.Models
{
    public class DBOrderDetail
    {
        public string  ProductName { get; set; }
        public int OrderID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public static List<DBOrderDetail> GetProductByOrderID(int orderID)
        {
            using(IDbConnection connection = new SqlConnection(Parameter.connect))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                return connection.Query<DBOrderDetail>("GetOrderDetailByOrderID", new { OrderID = orderID }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}