using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
   public  class TagDao
    {
        OnlineShopDbContext db = null;
        public TagDao()
        {
            db = new OnlineShopDbContext();
        }
        public List<Tag> ListAll()
        {
            return db.Tags.ToList();
        }
        public Tag GetByID(long id)
        {
            return db.Tags.Find(id);
        }
    }
}
