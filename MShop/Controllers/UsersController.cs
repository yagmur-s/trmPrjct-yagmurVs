using MShop.Models.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MShop.Models.Data;
using MShop.Models.ViewModels.Shop;
using PagedList;
using System.Data.Entity.Infrastructure;
using System.Net;
namespace MShop.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (String.IsNullOrEmpty(HttpContext.User.Identity.Name))
            {
                FormsAuthentication.SignOut();
                return View();
            }
            return Redirect("/shop/Products");
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(UsersVM model, string returnurl)
        {
            UsersDTO user = new UsersDTO();
            if (ModelState.IsValid)
            {

                using (Db db = new Db())
                {
                    user = db.Users.ToArray().Where(x => (x.UserName == model.UserName)
                    && (x.Password == model.Password) && (x.Email == model.Email)).FirstOrDefault();

                    FormsAuthentication.SetAuthCookie(model.Email, true);
                    return RedirectToAction("Index", "Home");

                }
            }
            else
            {
                ModelState.AddModelError("", "E-mail or password is invalid!");
            }
            return View(model);
        }
    }
    public ActionResult LogOff()
    {
        FormsAuthentication.SignOut();
        return RedirectToAction("Login", "Login");
    }
}