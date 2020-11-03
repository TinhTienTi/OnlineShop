using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.EF;
using PagedList;

namespace Models.DAO
{
    public class MenuDao
    {
        OnlineShopDbContext db = null;
        public MenuDao()
        {
            db = new OnlineShopDbContext();
        }
        public List<Menu> ListByGroupID(int groupID)
        {
            return db.Menus.Where(x => x.TypeID == groupID && x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }
        public IEnumerable<Menu> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Menu> model = db.Menus;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Text.Contains(searchString) || x.Text.Contains(searchString));
            }

            return model.OrderByDescending(x => x.DisplayOrder).ToPagedList(page, pageSize);
        }
        public long Insert(Menu entity)
        {
            db.Menus.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public Menu ViewDetail(long id)
        {
            return db.Menus.Find(id);
        }
        public bool Update(Menu entity)
        {
            try
            {
                var menu = db.Menus.Find(entity.ID);
                menu.Text = entity.Text;
                menu.Link = entity.Link;
                menu.DisplayOrder = entity.DisplayOrder;
                menu.TypeID = entity.TypeID;
                menu.Status = entity.Status;
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
                var menu = db.Menus.Find(id);
                db.Menus.Remove(menu);
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
