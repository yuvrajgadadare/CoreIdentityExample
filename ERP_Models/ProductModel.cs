using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Models
{
    public class ProductModel
    {
        public int product_id { get; set; }
        public string product_name { get; set; }
        public float rate { get; set; }
        public float gst { get; set; }
        public int stock_quantity { get; set; }
    }
}
