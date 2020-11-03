using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class OrderDetailViewModel
    {
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public long OrderID { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
    }
}
