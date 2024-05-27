using hardwareECommerce.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hardwareECommerce.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        dbcontxt dbcontxt = new dbcontxt();
        product product = new product();
        user user = new user();
        public ActionResult Index()
        {
            return View();
        }
        // Product Database Operations
        public ActionResult AddedData(int productID,string productName, string productDescription, int productPrice, string productType, HttpPostedFileBase image)
        {
            if(image != null)
            {
                string imagePath = Path.GetFileName(image.FileName);
                var uploadPath = Path.Combine(Server.MapPath("/assets"),imagePath);
                image.SaveAs(uploadPath);
            }


            product.productID = productID;
            product.productName = productName;
            product.productDescription = productDescription;
            product.productPrice = productPrice;
            product.productType = productType;
            product.productImage = "/assets/" + Path.GetFileName(image.FileName);

            dbcontxt.productTable.Add(product);
            dbcontxt.SaveChanges();
            return View("AddData");
        }
        public ActionResult DataView()
        {
            List<product> products = dbcontxt.productTable.ToList();
            return View(products);
        }

        public ActionResult DataViewBySearch(string searchType, string searchBar)
        {
            var searchedProducts = searchItem(searchType, searchBar);

            return View("DataView", searchedProducts);
        }

        public ActionResult AddData()
        {
            return View();
        }
        public ActionResult DeleteData()
        {
            List<product> products = dbcontxt.productTable.ToList();
            return View(products);
        }

        public ActionResult DeleteDataBySearch(string searchType, string searchBar)
        {
            var searchedProducts = searchItem(searchType, searchBar);

            return View("DeleteData", searchedProducts);
        }

        public ActionResult DeleteDataID(product productToDelete)
        {
            var productDelete = dbcontxt.productTable.Where(item => item.Id == productToDelete.Id &&
            item.productID == productToDelete.productID &&
            item.productName == productToDelete.productName &&
            item.productPrice == productToDelete.productPrice &&
            item.productDescription == productToDelete.productDescription &&
            item.productType == productToDelete.productType).FirstOrDefault();

            dbcontxt.productTable.Remove(productDelete);
            dbcontxt.SaveChanges();

            return View("DeleteData",dbcontxt.productTable.ToList());
        }
        public ActionResult UpdateData()
        {
            List<product> products = dbcontxt.productTable.ToList();
            return View(products);
        }

        public ActionResult UpdateDataBySearch(string searchType, string searchBar)
        {
            var searchedProducts = searchItem(searchType,searchBar);
            return View("UpdateData", searchedProducts);
        }
        
        public ActionResult UpdateDataID(int productId, int newId, string newName, string newDesc, int newPrice, string newType)
        {
            var productUpdate = dbcontxt.productTable.Where(item => item.Id == productId).FirstOrDefault();

            productUpdate.productID = newId;
            productUpdate.productName = newName;
            productUpdate.productDescription = newDesc;
            productUpdate.productPrice = newPrice;
            productUpdate.productType = newType;

            dbcontxt.SaveChanges();

            return View("UpdateData", dbcontxt.productTable.ToList());
        }
        // Product Database Operations End

        // User Database Operations

        public ActionResult UserAddData()
        {
            return View();
        }
        public ActionResult UserAddedData(string userName, string userMail, string userPassword, int userBalance)
        {
            user.userName = userName;
            user.userMail = userMail;
            user.userPassword = userPassword;
            user.userBalance = userBalance;

            dbcontxt.userTable.Add(user);
            dbcontxt.SaveChanges();
            return View("UserAddData");
        }
        public ActionResult UserDataView()
        {
            List<user> users = dbcontxt.userTable.ToList();
            return View(users);
        }
        public ActionResult UserDataViewBySearch(string searchBar)
        {
            var searchedProducts = UserSearchItem(searchBar);

            return View("UserDataView", searchedProducts);
        }
        public ActionResult UserDeleteData()
        {
            List<user> users = dbcontxt.userTable.ToList();
            return View(users);
        }
        public ActionResult UserDeleteDataID(user userToDelete)
        {
            var userDelete = dbcontxt.userTable.Where(item => item.Id == userToDelete.Id).FirstOrDefault();

            dbcontxt.userTable.Remove(userDelete);
            dbcontxt.SaveChanges();

            return View("UserDeleteData", dbcontxt.userTable.ToList());
        }
        public ActionResult UserDeleteDataBySearch(string searchBar)
        {
            var searchedUsers = UserSearchItem(searchBar);

            return View("UserDeleteData", searchedUsers);
        }

        public ActionResult UserUpdateData()
        {
            List<user> users = dbcontxt.userTable.ToList();
            return View(users);
        }
        public ActionResult UserUpdateDataBySearch(string searchBar)
        {
            var searchedUsers = UserSearchItem(searchBar);
            return View("UserUpdateData", searchedUsers);
        }
        public ActionResult UserUpdateDataID(int userId, string newName, string newPassword, string newMail, int newBalance)
        {
            var userUpdate = dbcontxt.userTable.Where(item => item.Id == userId).FirstOrDefault();

            userUpdate.userName = newName;
            userUpdate.userPassword = newPassword;
            userUpdate.userMail = newMail;
            userUpdate.userBalance = newBalance;

            dbcontxt.SaveChanges();

            return View("UserUpdateData", dbcontxt.userTable.ToList());
        }

        List<product> searchItem(string searchType, string searchBar)
        {
            List<product> searchedProducts;
            int deger;
            if (searchType == "Koda Göre")
            {
                bool isNumber = int.TryParse(searchBar, out int result);
                if (isNumber)
                {
                    deger = int.Parse(searchBar);
                }
                else
                {
                    deger = 0;
                }
                searchedProducts = dbcontxt.productTable.Where(item => item.productID == deger).ToList();
            }
            else if (searchType == "İsme'e Göre")
            {
                searchedProducts = dbcontxt.productTable.Where(item => item.productName.Contains(searchBar)).ToList();
            }
            else
            {
                searchedProducts = dbcontxt.productTable.Where(item => item.productType == searchBar).ToList();
            }

            return searchedProducts;
        }

        List<user> UserSearchItem(string searchBar)
        {
            List<user> searchedUsers;

            searchedUsers = dbcontxt.userTable.Where(item => item.userName.Contains(searchBar)).ToList();

            return searchedUsers;
        }
    }
}