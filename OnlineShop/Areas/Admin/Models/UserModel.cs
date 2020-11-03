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
    public class UserModel
    {
        public int Result { get; set; }
        public static List<UserModel> Delete(int Id)
        {
            using (IDbConnection conn = new SqlConnection(Parameter.connect))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                return conn.Query<UserModel>("Delete_User", new { ID = Id }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
    public class UserNameModel
    {
        public int Result { get; set; }
        public static int GetUserName(string User)
        {
            using (IDbConnection conn = new SqlConnection(Parameter.connect))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                return conn.Query<int>("Get_UserName", new { UserName = User }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
    }
    public class UserGroupModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public static List<UserGroupModel> Get()
        {
            using (IDbConnection conn = new SqlConnection(Parameter.connect))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                return conn.Query<UserGroupModel>("Get_GroupUser", commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}