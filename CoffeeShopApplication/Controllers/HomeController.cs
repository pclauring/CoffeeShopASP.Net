using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoffeeShopApplication.Models;
using System.Text.RegularExpressions;

namespace CoffeeShopApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult AddNewUser(User NewUser)
        {
            CoffeeShopDBEntities ORM = new CoffeeShopDBEntities();
            if (ModelState.IsValid)
            {
                ORM.Users.Add(NewUser);
                ORM.SaveChanges();
                ViewBag.Username = NewUser.firstName;
                return View();
            }
            else
            {
                return View("Register");
            }
        }
        public ActionResult Shop()
        {
            CoffeeShopDBEntities ORM = new CoffeeShopDBEntities();

            List<Item> OutputList = ORM.Items.ToList();

            ViewBag.ItemList = OutputList;

            if (Session["Cart"] == null)
            {
                Session.Add("Cart", new List<string>());
            }

            //fetch list from session
            List<string> Cart = (List<string>)Session["Cart"];

            ViewBag.Cart = Cart;

            return View();
        }

        public ActionResult AdminStoreEdit()
        {
            CoffeeShopDBEntities ORM = new CoffeeShopDBEntities();

            List<Item> OutputList = ORM.Items.ToList();

            ViewBag.ItemList = OutputList;

            return View();
        }

        public ActionResult ItemForm()
        {
            return View();
        }

        public ActionResult SaveItem(Item NewItem)
        {
            if (ModelState.IsValid)
            {
                CoffeeShopDBEntities ORM = new CoffeeShopDBEntities();

                ORM.Items.Add(NewItem);
                ORM.SaveChanges();
                return RedirectToAction("AdminStoreEdit");
            }
            else
            {
                return View("ItemForm");
            }
        }

        public ActionResult DeleteItem(string Name)
        {
            CoffeeShopDBEntities ORM = new CoffeeShopDBEntities();

            Item ItemToBeDeleted = ORM.Items.Find(Name);

            if (ItemToBeDeleted != null)
            {
                ORM.Items.Remove(ItemToBeDeleted);
                ORM.SaveChanges();
            }

            return RedirectToAction("AdminStoreEdit");
        }

        public ActionResult EditItem(string Name)
        {
            CoffeeShopDBEntities ORM = new CoffeeShopDBEntities();

            Item SelectedItem = ORM.Items.Find(Name);

            if (SelectedItem != null)
            {
                ViewBag.SelectedItem = SelectedItem;
                return View();
            }

            return RedirectToAction("AdminStoreEdit");

        }

        public ActionResult SaveEditItem(Item EditedItem)
        {
            CoffeeShopDBEntities ORM = new CoffeeShopDBEntities();
            Item update = ORM.Items.Find(EditedItem.Name);
            update.Description = EditedItem.Description;
            update.Price = EditedItem.Price;
            update.Quantity = EditedItem.Quantity;

            ORM.Entry(update).State = System.Data.Entity.EntityState.Modified;
            ORM.SaveChanges();

            return RedirectToAction("AdminStoreEdit");
        }

        public ActionResult AddToCart(string Name)
        {
            if (Session["Cart"] == null)
            {
                Session.Add("Cart", new List<string>());
            }

            //fetch list from session
            List<string> Cart = (List<string>)Session["Cart"];
            if (!Cart.Contains(Name))
            {
                Cart.Add(Name);
            }
            //Save list back in session
            Session["Cart"] = Cart;

            return RedirectToAction("Shop");
        }

        public ActionResult SearchForItemName(string ItemName)
        {
            CoffeeShopDBEntities ORM = new CoffeeShopDBEntities();
            List<Item> OutputList = new List<Item>();

            foreach (Item i in ORM.Items.ToList())
            {
                if (i.Name != null && Regex.IsMatch(i.Name, ItemName, RegexOptions.IgnoreCase))
                {
                    OutputList.Add(i);
                }
            }

            ViewBag.ItemList = OutputList;
            return View("Shop");
        }
    }
}