using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace hardwareECommerce.Models
{
    public class dbcontxt:DbContext
    {
        public DbSet<product> productTable { get; set; }
        public DbSet<user> userTable { get; set; }
        public DbSet<cart> cartTable { get; set; }
    }
}