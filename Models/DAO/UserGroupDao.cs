using System;
using System.Collections.Generic;
using System.Linq;
using Models.EF;

namespace Models.DAO
{
    public class UserGroupDao
    {
        OnlineShopDbContext db = null;
        public UserGroupDao()
        {
            db = new OnlineShopDbContext();
        }
        public List<UserGroup> ListAll()
        {
            return db.UserGroups.ToList();
        }
    }
}
