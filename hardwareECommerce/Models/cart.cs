using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hardwareECommerce.Models
{
    public class cart
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public int productId { get; set; }
        public int quantity { get; set; }

        public string productName { get; set; }
        public string productDescription { get; set; }
        public string productType { get; set; }
        public int productPrice { get; set; }
    }
}