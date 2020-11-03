using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.EF;
using Models.ViewModel;
using PagedList;

namespace Models.DAO
{
    public class OrderDetailDao
    {
        OnlineShopDbContext db = null;
        public OrderDetailDao()
        {
            db = new OnlineShopDbContext();
        }
        public IEnumerable<OrderDetail> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<OrderDetail> model = db.OrderDetails;
            return model.OrderByDescending(x => x.OrderID).ToPagedList(page, pageSize);
        }
        public IEnumerable<OrderDetail> ListByOrderID(long? orderID)
        {
            IQueryable<OrderDetail> model = db.OrderDetails;
            return model.Where(x => x.OrderID == orderID).OrderByDescending(x => x.OrderID).ToList();
        }
        // Xem danh sách chi tiết đơn hàng theo orderID
        public IEnumerable<OrderDetailViewModel> List(long? orderID, string searchString, int page, int pageSize)
        {
            var model = (from a in db.Products
                         join b in db.OrderDetails
                         on a.ID equals b.ProductID
                         where b.OrderID == orderID
                         select new
                         {
                             OrderId = b.OrderID,
                             ProductID = b.ProductID,
                             ProductName = a.Name,
                             Quantity = b.Quantity,
                             Price = b.Price
                         }).AsEnumerable().Select(x => new OrderDetailViewModel()
                         {
                             OrderID = x.OrderId,
                             ProductID = x.ProductID,
                             ProductName = x.ProductName,
                             Quantity = x.Quantity,
                             Price = x.Price
                         });
            model.OrderByDescending(x => x.Quantity).Take(pageSize);
            return model.ToList();
            // return model.Where(x => x.OrderID == orderID).OrderByDescending(x => x.OrderID).ToPagedList(page, pageSize);
        }
    }
}
