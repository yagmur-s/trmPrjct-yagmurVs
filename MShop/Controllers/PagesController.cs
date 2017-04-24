using MShop.Models.Data;
using MShop.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MShop.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            //Declare list of PageVM
            List<PageVM> pagesList;

            //Init the list
            using (Db db = new Db())
            {
                pagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
                //constructor // x dto // lambda expression
            }

            //SomeFunction (x)  { return x.Id != id; }
            //Return view the list 

           
            return View(pagesList);
        }
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            //Check model state 
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {            
            //Declare slug
                string slug;
                //Init dto
                PagesDTO dto = new PagesDTO();
                //DTO title
                dto.Title = model.Title;  // same as the name of view
               //Check slug
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower(); 
                } else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                //Make sure the title and slug are unique

                if (db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "That title or slug already exist");
                    return View(model);
                }

                //DTO rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;

                //save db

                db.Pages.Add(dto);
                db.SaveChanges();
            }
            //Set data transfer message
            TempData["SM"] = "You have added a new page successfully";

            //redirect
            return RedirectToAction("AddPage");
        }

        [HttpGet]
        public ActionResult EditPage(int id)
        {
            //Declare PageVM
            PageVM model;
            using (Db db = new Db())
            {
                //Get the page
                PagesDTO dto = db.Pages.Find(id);

                //Check page exists
                if (dto==null)
                {
                    return Content("The pages does not exists");
                }

                //init pagevm
                model = new PageVM(dto);
            }
            //return view
            return View(model);
        }

        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            //check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
           {

                //get page id 
                int id = model.Id;
                //declare slug
                string slug = "home";
                //init dto
                PagesDTO dto = db.Pages.Find(id);
                //dto title
                dto.Title = model.Title;

                //check and set slug
                if (model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    } else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }

                //check uniqueness of slug and title
                if (db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title) ||
                    db.Pages.Where(x => x.Id !=id).Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "That title or slug already exists");
                    return View(model);
                }

                //dto rest
                dto.Slug = slug;
                dto.HasSidebar = model.HasSidebar;
                dto.Body = model.Body;

                //save db
                db.SaveChanges();
            }
            //redirect
            TempData["SM"] = "You have successfully edited the page";
            return RedirectToAction("EditPage");
        }


    }
}