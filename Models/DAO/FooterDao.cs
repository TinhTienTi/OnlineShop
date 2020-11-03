using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.EF;
using PagedList;

namespace Models.DAO
{
    public class FooterDao
    {
        OnlineShopDbContext db = null;
        public FooterDao()
        {
            db = new OnlineShopDbContext();
        }
        public Footer GetFooter()
        {
            return db.Footers.SingleOrDefault(x => x.Status == true);
        }
        public IEnumerable<Footer> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Footer> model = db.Footers;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Content.Contains(searchString) || x.Content.Contains(searchString));
            }

            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }
        public string Insert(Footer entity)
        {
            db.Footers.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public Footer ViewDetail(long id)
        {
            return db.Footers.Find(id);
        }
        public bool Update(Footer entity)
        {
            try
            {
                var footer = db.Footers.Find(entity.ID);
                footer.Content = entity.Content;
                footer.Status = entity.Status;
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
