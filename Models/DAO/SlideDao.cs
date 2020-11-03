using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.EF;
using PagedList;

namespace Models.DAO
{
    public class SlideDao
    {
        OnlineShopDbContext db = null;
        public SlideDao()
        {
            db = new OnlineShopDbContext();
        }
        public List<Slide>ListAll()
        {
            return db.Slides.Where(x => x.Status == true).OrderBy(y => y.DisplayOrder).ToList();
        }
        public IEnumerable<Slide> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Slide> model = db.Slides;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Link.Contains(searchString) || x.Description.Contains(searchString));
            }

            return model.OrderByDescending(x => x.DisplayOrder).ToPagedList(page, pageSize);
        }
        public long Insert(Slide entity)
        {
            db.Slides.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public Slide ViewDetail(long id)
        {
            return db.Slides.Find(id);
        }
        public bool Update(Slide entity)
        {
            try
            {
                var slide = db.Slides.Find(entity.ID);
                slide.Image = entity.Image;
                slide.DisplayOrder = entity.DisplayOrder;
                slide.Link = entity.Link;
                slide.Description = entity.Description;
                slide.CreatedDate = DateTime.Now;
                slide.ModifiedDate = DateTime.Now;
                slide.Status = entity.Status;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                //logging
                return false;
            }

        }
        public bool Delete(int id)
        {
            try
            {
                var slide = db.Slides.Find(id);
                db.Slides.Remove(slide);
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
