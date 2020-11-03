using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.EF;
using PagedList;

namespace Models.DAO
{
    public class CategoryDao
    {
        OnlineShopDbContext db = null;
        public CategoryDao()
        {
            db = new OnlineShopDbContext();
        }
        public long Insert(Category category)
        {
            db.Categories.Add(category);
            db.SaveChanges();
            return category.ID;
        }

        public List<Category> ListAll()
        {
            return db.Categories.Where(x => x.Status == true).ToList();
        }
        public List<Category> ListAll1()
        {
            return db.Categories.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }
        public object ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Category> model = db.Categories;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public ProductCategory ViewDetail(long id)
        {
            return db.ProductCategories.Find(id);
        }
        public Category ViewDetail1(long id)
        {
            return db.Categories.Find(id);
        }
        public bool Update(Category entity)
        {
            try
            {
                var category = db.Categories.Find(entity.ID);
                category.Name = entity.Name;
                category.Status = entity.Status;
                category.DisplayOrder = entity.DisplayOrder;
                category.CreatedBy = entity.CreatedBy;
                category.ModifiedBy = entity.ModifiedBy;
                category.MetaTitle = entity.MetaTitle;
                category.ParentID = entity.ParentID;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                var category = db.Categories.Find(id);
                db.Categories.Remove(category);
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
