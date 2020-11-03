using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Models.DAO
{
    public class AboutDao
    {
        OnlineShopDbContext db = null;
        public AboutDao()
        {
            db = new OnlineShopDbContext();
        }
        public About GetActiveAbout()
        {
            return db.Abouts.Single(x => x.Status == true);
        }
        
    }
}
