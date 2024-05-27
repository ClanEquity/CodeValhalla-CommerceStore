using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hardwareECommerce.Models
{
    public class user
    {
        public int Id { get; set; }
        public string userName { get; set; }
        public string userMail { get; set; }
        public string userPassword { get; set; }
        public int userBalance { get; set; }
    }
}