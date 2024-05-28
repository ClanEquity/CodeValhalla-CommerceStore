using hardwareECommerce.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace hardwareECommerce.Controllers
{
    public class HomeController : Controller
    {
        dbcontxt dbcontxt = new dbcontxt();
        product product = new product();
        user user = new user();
        cart cart = new cart();

        public static bool IsSignedIn = false;
        public static user signedAccount = new user();
        // GET: Home
        public ActionResult Index()
        {
            return View("Anasayfa",IsSignedIn);
        }

        public ActionResult Anasayfa()
        {
            return View(IsSignedIn);
        }

        public ActionResult GirisYap()
        {
            if (IsSignedIn == true)
            {
                return View("Profil",signedAccount);
            }
            else
            {
                return View("GirisYap",null , "");
            }
        }

        public ActionResult HesapOlustur()
        {
            return View();
        }

        public ActionResult CreateAccount(string userEmail, string userName, string userPassword)
        {
            user.userBalance = 0;
            user.userMail = userEmail;
            user.userPassword = userPassword;
            user.userName = userName;
            dbcontxt.userTable.Add(user);
            dbcontxt.SaveChanges();

            return View("GirisYap", null, "");
        }

        public ActionResult SignIn(string userEmail, string userPassword)
        {
            var signedUser = dbcontxt.userTable.Where(item => item.userMail == userEmail && item.userPassword == userPassword).FirstOrDefault();

            if(signedUser == null)
            {
                string errorText = "Hesap Bulunamadı veya Bilgileriniz Yanlış.";
                return View("GirisYap",null, errorText);
            }
            else
            {
                signedAccount = signedUser;
                IsSignedIn = true;
                return View("Profil", signedUser);
            }
        }

        public ActionResult LogOut()
        {
            IsSignedIn = false;
            signedAccount = null;
            return View("GirisYap", null, "");
        }

        public ActionResult Profil()
        {
            if (IsSignedIn)
            {
                return View(signedAccount);
            }
            else
            {
                return View("GirisYap", null, "");
            }
            
        }

        public ActionResult AddBal(int balance)
        {
            var user = dbcontxt.userTable.Where(item => item.Id == signedAccount.Id).FirstOrDefault();
            user.userBalance += balance;
            signedAccount.userBalance = user.userBalance;
            dbcontxt.SaveChanges();
            return View("Profil",signedAccount);
        }

        public ActionResult Hakkımızda()
        {
            return View();
        }

        public ActionResult Iletisim()
        {
            return View();
        }

        public ActionResult IadePolitikası()
        {
            return View();
        }

        public ActionResult SSS()
        {
            return View();
        }

        public ActionResult Magaza()
        {
            return View(dbcontxt.productTable.ToList());
        }

        public ActionResult addToCart(int productID)
        {
            if(IsSignedIn == false)
            {
                return View("GirisYap", null, "Sepetinize Ürün Eklemek için giriş yapın");
            }
            else
            {

                var cartItem = dbcontxt.cartTable.Where(item => item.userId == signedAccount.Id && item.productId==productID).FirstOrDefault();

                if (cartItem == null)
                {
                    cart.productId = productID;
                    var myproduct = dbcontxt.productTable.Where(item => item.Id == productID).FirstOrDefault();
                    cart.productName = myproduct.productName;
                    cart.productDescription = myproduct.productDescription;
                    cart.productType = myproduct.productType;
                    cart.productPrice = myproduct.productPrice;
                    cart.quantity = 1;
                    cart.userId = signedAccount.Id;
                    dbcontxt.cartTable.Add(cart);
                    dbcontxt.SaveChanges();
                }
                else
                {
                    cartItem.quantity++;
                    dbcontxt.SaveChanges();
                }
                return View("Magaza", dbcontxt.productTable.ToList());
            }
            
        }

        public ActionResult filtreProducts(string searchText)
        {
            return View("Magaza", dbcontxt.productTable.Where(item => item.productName.Contains(searchText)).ToList());
        }

        public ActionResult Sepet()
        {
            dynamic myModel = new ExpandoObject();
            myModel.ErrorText = "";
            myModel.Products = dbcontxt.cartTable.Where(item => item.userId == signedAccount.Id).ToList();
            myModel.Balance = signedAccount.userBalance;
            return View(myModel);
        }

        public ActionResult Pay(int totalPrice)
        {
            if (IsSignedIn == false)
            {
                return View("GirisYap", null, "İşlem yapmak için giriş yapın");
            }
            else
            {
                if (totalPrice > signedAccount.userBalance)
                {
                    dynamic myModel = new ExpandoObject();
                    myModel.ErrorText = "Bakiye Yetersiz";
                    myModel.Products = dbcontxt.cartTable.Where(item => item.userId == signedAccount.Id).ToList();
                    myModel.Balance = signedAccount.userBalance;
                    return View("Sepet",null,myModel);
                }
                else
                {
                    var user = dbcontxt.userTable.Where(item => item.Id == signedAccount.Id).FirstOrDefault();
                    user.userBalance = user.userBalance - totalPrice;
                    signedAccount.userBalance = user.userBalance;
                    dbcontxt.SaveChanges();
                    var list = dbcontxt.cartTable.Where(item => item.userId == signedAccount.Id).ToList();
                    foreach(var item in list)
                    {
                        dbcontxt.cartTable.Remove(dbcontxt.cartTable.Where(x => x.Id == item.Id).FirstOrDefault());
                    }
                    dbcontxt.SaveChanges();

                    dynamic myModel = new ExpandoObject();
                    myModel.ErrorText = "";
                    myModel.Products = dbcontxt.cartTable.Where(item => item.userId == signedAccount.Id).ToList();
                    myModel.Balance = signedAccount.userBalance;
                    return View("Sepet",null,myModel);
                }
            }
        }
    }
}