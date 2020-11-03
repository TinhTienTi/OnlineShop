using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class FeedBackDao
    {
        OnlineShopDbContext db = null;
        public FeedBackDao()
        {
            db = new OnlineShopDbContext();
        }
      
        public IEnumerable<Feedback> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Feedback> model = db.Feedbacks;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Phone.Contains(searchString));
            }

            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }
        public bool Delete(int id)
        {
            try
            {
                var feed = db.Feedbacks.Find(id);
                db.Feedbacks.Remove(feed);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
