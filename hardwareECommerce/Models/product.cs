using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hardwareECommerce.Models
{
    public class product
    {
        public int Id { get; set; }
        public int productID { get; set; }
        public string productName { get; set; }
        public string productDescription { get; set; }
        public string productType { get; set; }
        public int productPrice { get; set; }
        public string productImage { get; set; }
    }
}