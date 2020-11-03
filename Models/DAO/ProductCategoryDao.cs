using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Models.EF;
using PagedList;
using PagedList.Mvc;

namespace Models.DAO
{
    public class ProductCategoryDao
    {
        OnlineShopDbContext db = null;
        public ProductCategoryDao()
        {
            db = new OnlineShopDbContext();
        }
        public List<ProductCategory> ListAll()
        {
            return db.ProductCategories.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }
        public List<ProductCategory> ListNotParentID()
        {
            return db.ProductCategories.Where(x => x.Status == true && x.IsParent == false).OrderBy(x => x.DisplayOrder).ToList();
        }
        public List<ProductCategory> ListParentID()
        {
            return db.ProductCategories.Where(x => x.Status == true && x.IsParent == true).OrderBy(x => x.DisplayOrder).ToList();
        }
        public ProductCategory ViewDetail(long id)
        {
            return db.ProductCategories.Find(id);
        }
        public IEnumerable<ProductCategory> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<ProductCategory> model = db.ProductCategories;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) && x.Status == true);
            }
            if (string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Status == true);
            }

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public long Insert(ProductCategory entity)
        {
            db.ProductCategories.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public bool Update(ProductCategory entity)
        {
            try
            {
                var pdcategory = db.ProductCategories.Find(entity.ID);
                pdcategory.Name = entity.Name;
                pdcategory.MetaTitle = entity.MetaTitle;
                pdcategory.CreatedBy = entity.CreatedBy;
                pdcategory.ModifiedBy = entity.ModifiedBy;
                pdcategory.Status = entity.Status;

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
                var pdproduct = db.ProductCategories.Find(id);
                db.ProductCategories.Remove(pdproduct);
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
