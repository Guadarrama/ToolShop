using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using toolShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace toolShop.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            //test if user is in session then redirect to the dashboard:
            int? Sess = HttpContext.Session.GetInt32("LoggedIn");
            if (Sess != null)
            {
                return Redirect("Dashboard");
            }
            //try catch
            try
            {
                 ViewBag.AllProducts = dbContext.Products
                .Include(seller => seller.Seller)
                .ToList(); 
            }
            catch
            {
                Console.WriteLine("AllProducts exception!");
            }

            return View();
        }
        [HttpGet("LogReg")]
        public IActionResult LogReg()
        {
            return View();
        }

        [HttpPost("regCheck")]
        public IActionResult regCheck(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email","This email already has an existing account");
                    return View("LogReg");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                dbContext.Add(newUser);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("LoggedIn", newUser.UserId);
                return Redirect("Dashboard");
            }
            return View("LogReg");
        }

        [HttpPost("logCheck")]
        public IActionResult logCheck(LUser CheckUser)
        {
            if(ModelState.IsValid)
            {
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == CheckUser.LEmail);

                if(userInDb == null)
                {
                    ModelState.AddModelError("LEmail", "Invalid Email or Password");
                    return View("LogReg");
                }
                //initialize hasher object
                var hasher = new PasswordHasher<LUser>();
                //verify provided password against hash stored in database
                var result = hasher.VerifyHashedPassword(CheckUser, userInDb.Password, CheckUser.LPassword);
                //result can be compared to the number 0 for failure
                if(result == 0)
                {
                    ModelState.AddModelError("LEmail", "Invalid Email or Password");
                    return View("LogReg");
                }
                HttpContext.Session.SetInt32("LoggedIn", userInDb.UserId);
                return Redirect("Dashboard");
            }
            return View("LogReg");
        }
        //logout controller
        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            //test if user is in session:
            int? Sess = HttpContext.Session.GetInt32("LoggedIn");
            if (Sess == null)
            {
                HttpContext.Session.Clear();
                return Redirect("/");
            }
            ViewBag.CurrentUserId = (int)Sess;
            ViewBag.ThisUser = dbContext.Users
                .FirstOrDefault(u => u.UserId == (int)Sess);
            ViewBag.AllProducts = dbContext.Products
                .Include(seller => seller.Seller)
                .ToList();

            return View();
        }

        [HttpGet("MyProfile")]
        public IActionResult MyProfile()
        {
            //test if user is in session:
            int? Sess = HttpContext.Session.GetInt32("LoggedIn");
            if (Sess == null)
            {
                HttpContext.Session.Clear();
                return Redirect("/");
            }
            ViewBag.PageName = "MyProfile";
            ViewBag.CurrentUserId = (int)Sess;
            ViewBag.ThisUser = dbContext.Users
                .Include(s => s.Stock)
                .Include(pi => pi.PurchasedItems)
                .ThenInclude(ipurchased => ipurchased.ItemPurchased)
                .FirstOrDefault(u => u.UserId == (int)Sess);
            return View();
        }

        [HttpGet("MyCart")]
        public IActionResult MyCart()
        {
            //test if user is in session:
            int? Sess = HttpContext.Session.GetInt32("LoggedIn");
            if (Sess == null)
            {
                HttpContext.Session.Clear();
                return Redirect("/");
            }
            ViewBag.CurrentUserId = (int)Sess;
            ViewBag.ThisUser = dbContext.Users
                .Include(c => c.CartItems)
                .ThenInclude(p => p.ItemInCart)
                .FirstOrDefault(u => u.UserId == (int)Sess);
            return View();
        }

        [HttpGet("AddItem")]
        public IActionResult AddItem()
        {
            //test if user is in session:
            int? Sess = HttpContext.Session.GetInt32("LoggedIn");
            if (Sess == null)
            {
                HttpContext.Session.Clear();
                return Redirect("/");
            }
            
            ViewBag.CurrentUserId = (int)Sess;
            ViewBag.ThisUser = dbContext.Users
                .FirstOrDefault(u => u.UserId == (int)Sess);
            return View();
        }
        
        [HttpPost("newProduct")]
        public IActionResult AddNewProduct(Product newProduct)
        {
            if(ModelState.IsValid)
            {
                //get user in session
                int? Sess = HttpContext.Session.GetInt32("LoggedIn");
                newProduct.UserId = (int)Sess;
                dbContext.Add(newProduct);
                dbContext.SaveChanges();   
                return Redirect("MyProfile");       
            }
            return View("AddItem");
        }

        [HttpGet("EditProduct/{ProductId}")]
        public IActionResult EditProduct(int ProductId)
        {
            int? Sess = HttpContext.Session.GetInt32("LoggedIn");
            if (Sess == null)
            {
                HttpContext.Session.Clear();
                return Redirect("/");
            }
            ViewBag.CurrentUserId = (int)Sess;
            ViewBag.PageName = "EditProduct";
            ViewBag.ProductToDisplay = dbContext.Products
                    .Where(e => e.UserId == (int)Sess)
                    .FirstOrDefault(p => p.ProductId == ProductId);

            return View();
        }

        //EDIT PRODUCT
        [HttpPost("editProduct/{ProductId}")]
        public IActionResult editProduct(Product productEdit, int ProductId)
        {
            
            int? Sess = HttpContext.Session.GetInt32("LoggedIn");
            if (Sess == null)
            {
                HttpContext.Session.Clear();
                return Redirect("/");
            }
            if (ModelState.IsValid)
            {
                
                Console.WriteLine(ProductId);
                //get product from database
                Product productFromDb = dbContext.Products.Where(u=>u.ProductId == ProductId).SingleOrDefault();
                //change any changed values
                productFromDb.Name = productEdit.Name;
                productFromDb.Description = productEdit.Description;
                productFromDb.Price = productEdit.Price;
                productFromDb.Quantity = productEdit.Quantity;

                //Update and save changes to database
                dbContext.Products.Update(productFromDb);
                dbContext.SaveChanges();

                return RedirectToAction("MyProfile");

            }
            ViewBag.CurrentUserId = (int)Sess;
            ViewBag.ProductToDisplay = dbContext.Products
                .Where(e => e.UserId == (int)Sess)
                .FirstOrDefault(p => p.ProductId == ProductId); 

            return View("EditProduct");
        }
        
        //add item to cart
        [HttpPost("addItemToCart")]
        public IActionResult addItemToCart(UserCart cartItem)
        {
            int? Sess = HttpContext.Session.GetInt32("LoggedIn");
            if (Sess == null)
            {
                HttpContext.Session.Clear();
                return Redirect("/");
            }
            if(dbContext.UserCarts.Where(u=> u.UserId==(int)Sess && u.ProductId==cartItem.ProductId).SingleOrDefault() !=null)
            {
                
                int AmountToAdd = cartItem.Amount;
                Console.WriteLine(cartItem.Amount);

                cartItem = dbContext.UserCarts.Where(u=> u.UserId==(int)Sess && u.ProductId==cartItem.ProductId).SingleOrDefault();
                
                Product thisProduct = dbContext.Products.Where(p =>p.ProductId == cartItem.ProductId).SingleOrDefault();

                cartItem.Amount += AmountToAdd;
                if(cartItem.Amount > thisProduct.Quantity)
                {
                    ModelState.AddModelError("Amount", "MAX STOCK REACHED");
                    return Redirect("Dashboard");
                }
                Console.WriteLine(cartItem.Amount);
                dbContext.UserCarts.Update(cartItem);
                dbContext.SaveChanges();

            }
            else
            {
                cartItem.UserId = (int)Sess;
                dbContext.Add(cartItem);
                dbContext.SaveChanges();
                return Redirect("Dashboard");
            }
            return Redirect("Dashboard");
        }
        [HttpGet("editAmountInCart/{ProductId}/{newAmount}")]
        public IActionResult editAmountInCart(int ProductId, int newAmount)
        {
            int? Sess = HttpContext.Session.GetInt32("LoggedIn");
            if (Sess == null)
            {
                HttpContext.Session.Clear();
                return Redirect("/");
            }

            UserCart thisCartItem = dbContext.UserCarts.Where(u => u.UserId == (int)Sess && u.ProductId == ProductId).SingleOrDefault();

            thisCartItem.Amount = newAmount;

            dbContext.UserCarts.Update(thisCartItem);
            dbContext.SaveChanges();

            return RedirectToAction("MyCart");
        }

        [HttpGet("removeFromCart/{ProductId}")]
        public IActionResult removeFromCart(int ProductId)
        {
            int? Sess = HttpContext.Session.GetInt32("LoggedIn");
            if(Sess == null)
            {
                HttpContext.Session.Clear();
                return Redirect("/");
            }
            //Get UserCarts where UserId == (int)Sess && ProductId == ProductId
            //Delete UserCart then savechanges
            
            UserCart UserCartToRemove = dbContext.UserCarts
                .Where(thisUser => thisUser.UserId == (int)Sess)
                .FirstOrDefault(productInCart => productInCart.ProductId == ProductId);

            dbContext.UserCarts.Remove(UserCartToRemove);
            dbContext.SaveChanges();

            return RedirectToAction("MyCart");
        }
        
        [HttpGet("checkoutForm")]
        public IActionResult CheckoutForm()
        {
            //test if user is in session:
            int? Sess = HttpContext.Session.GetInt32("LoggedIn");
            if (Sess == null)
            {
                HttpContext.Session.Clear();
                return Redirect("/");
            }
            ViewBag.CurrentUserId = (int)Sess;
            ViewBag.ThisUser = dbContext.Users
                .Include(c => c.CartItems)
                .ThenInclude(p => p.ItemInCart)
                .FirstOrDefault(u => u.UserId == (int)Sess);
            return View();
        }

        [HttpPost("cartCheckout")]
        public IActionResult cartCheckout()
        {
            //item(#)[0] = itemId, item(#)[1] = amount
            int? Sess = HttpContext.Session.GetInt32("LoggedIn");
            if (Sess == null)
            {
                HttpContext.Session.Clear();
                return Redirect("/");
            }
            //after user confirmation, run code
            Console.WriteLine("*******************************************************");
            string orderCheckoutString = Request.Form["cartTxt"]; //jsonString from form
            int itemCount = Convert.ToInt32(Request.Form["cartCount"]); //item quant from form
            Console.WriteLine("*        Amount of products to checkout: {0}          *", itemCount);
            Console.WriteLine("*******************************************************");

            JObject checkoutItems = JObject.Parse(orderCheckoutString);

            for (int i = 1; i <= itemCount; i++)
            {
                string tokenToReference = "item" + Convert.ToString(i);
                JToken itemData = checkoutItems.SelectToken(tokenToReference);
                var itemDataArray = itemData?.ToObject<int[]>(); // JToken to array
                Console.WriteLine("++++++++++++++++++++++++");
                Console.WriteLine("Reference Text: {0}", tokenToReference);
                Console.WriteLine("++++++++++++++++++++++++");
                // Console.WriteLine("itemId: {0}, amount: {1}", itemDataArray[0], itemDataArray[1]); -- delete comment
                //1. check if there's enough items to sell, check product
                //if(Quantity-AmountSold > 0)
                //add data to new item
                //check if UserPurchases exists, if not create ne UserPurchases:
                //if item exists
                int thisUserId = (int)Sess;
                if(
                    dbContext.UserPurchases
                        .Where(up => up.UserId==(int)Sess && up.ProductId== itemDataArray[0])
                        .SingleOrDefault() !=null
                )
                {
                    //check if item exists
                    Console.WriteLine("Purchasing {0} of (itemId): {1}, EXISTING", itemDataArray[1], itemDataArray[0]);
                    UserPurchase thisUserPurchase = dbContext.UserPurchases
                        .Where(up => up.UserId == thisUserId && up.ProductId == itemDataArray[0])
                        .SingleOrDefault();
                    //amount to add
                    thisUserPurchase.Quantity += itemDataArray[1];
                    //get Product data
                    Product productCheck = dbContext.Products
                        .Where(p => p.ProductId == thisUserPurchase.ProductId)
                        .SingleOrDefault();
                    productCheck.AmountSold += itemDataArray[1];
                    //add here: error catch!
                    dbContext.Products.Update(productCheck);
                    dbContext.UserPurchases.Update(thisUserPurchase);
                }
                else //if item doesnt exist
                {
                    UserPurchase newPurchase = new UserPurchase();
                    newPurchase.UserId = thisUserId;
                    newPurchase.ProductId = itemDataArray[0];
                    newPurchase.Quantity = itemDataArray[1];
                    //modify amount sold on Product table in database
                    Product thisProduct = dbContext.Products
                        .Where(p => p.ProductId == itemDataArray[0])
                        .SingleOrDefault();
                    //have to test if there are enough items to sell/modify "AmountSold"
                    thisProduct.AmountSold += itemDataArray[1];

                    //console print test:
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine("-------Confirm data to add: -------");
                    Console.WriteLine("newPurchase: UserId: {0}, ProductID: {1}, Quantity: {2} | Product: Amount Sold: {3}", 
                        newPurchase.UserId, newPurchase.ProductId, newPurchase.Quantity, thisProduct.AmountSold
                    );
                    Console.WriteLine("-----------------------------------");
                    //add here: error catch!
                    /**/
                    dbContext.Products.Update(thisProduct);
                    dbContext.Add(newPurchase);
                    /**/
                }

                UserCart UserCartToRemove = dbContext.UserCarts
                    .Where(thisUser => thisUser.UserId == (int)Sess)
                    .FirstOrDefault(productInCart => productInCart.ProductId == itemDataArray[0]);

                dbContext.UserCarts.Remove(UserCartToRemove);
                dbContext.SaveChanges();
            }


            return Redirect("MyProfile");
        }
    }
}