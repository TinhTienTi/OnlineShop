using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Configuration;

namespace OnlineShop.Areas.Admin.Models
{
    public class ReportRevenue
    {
        public Int64 ProductID { get; set; }
        public string ProductName { get; set; }
        public int Revenue { get; set; }
        public static List<ReportRevenue> Get()
        {
            using (IDbConnection conn = new SqlConnection(Parameter.connect))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                return conn.Query<ReportRevenue>("ReportTop5ProductSelling", commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
    public class ReportTop5ProductQuantity
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public static List<ReportTop5ProductQuantity> Get()
        {
            using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["OnlineShopDbConnect"].ToString()))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                return conn.Query<ReportTop5ProductQuantity>("Top5ProductQuantity", commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}