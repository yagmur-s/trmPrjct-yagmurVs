using MShop.Models.Data;
using MShop.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
namespace MShop.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Categories()
        {
            // Declare a list of models
            List<CategoryVM> categoryVMList;

            using (Db db = new Db())
            {
                // Init the list
                categoryVMList = db.Categories
                                .ToArray()
                                .OrderBy(x => x.Sorting)
                                .Select(x => new CategoryVM(x))
                                .ToList();
            }
            // Return view with list
            return View(categoryVMList);
        }



        // GET: Admin/Shop/AddProduct
        [HttpGet]
        public ActionResult AddCategory(int? page, int? catId)
        {
            ProductVM model = new ProductVM();
            using (Db db = new Db())
            {
                model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                model.TimePeriods = new SelectList(db.TimePeriods.ToList(), "Id", "TimePeriod");
                model.Cities = new SelectList(db.Cities.ToList(), "Id", "CityName");

                return View(model);
            }
            return View(model);
        }

        // POST: Admin/Shop/AddProduct
        [HttpPost]
        public ActionResult AddCategory(CategoryVM model, HttpPostedFileBase file)
        {
            // Declare product id
            int id;

            // Init and save productDTO
            using (Db db = new Db())
            {
                CategoryDTO category = new CategoryDTO();

                category.Name = model.Name;
                category.Id = model.Id;
                category.Slug = model.Slug;
                category.Sorting = model.Sorting;
                
                db.Categories.Add(category);
                db.SaveChanges();

                // Get the id
                id = category.Id;
            }

            // Set TempData message
            TempData["SM"] = "You have added a category!";

            return RedirectToAction("AddCategory");
        }
        public ActionResult Categories(int? page, int? catId)
        {
            // Declare a list of ProductVM
            List<CategoryVM> listOfCategoryVM;

            // Set page number
            var pageNumber = page ?? 1;

            using (Db db = new Db())
            {
            listOfCategoryVM = db.Categories.ToArray()
                          .Where(x => ((catId == null || catId == 0 || x.Id == catId)))
                          .Select(x => new CategoryVM(x))
                          .ToList();
                              

                // Populate categories select list
                ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                ViewBag.TimePeriods = new SelectList(db.TimePeriods.ToList(), "Id", "TimePeriod");
                ViewBag.Cities = new SelectList(db.Cities.ToList(), "Id", "CityName");
                

            }
            

            // Return view with list
            return View(listOfCategoryVM);
        }

        // GET: Admin/Shop/EditProduct
        [HttpGet]
        public ActionResult EditCategory(int id)
        {
            // Init model
            CategoryVM cat = new CategoryVM();

            using (Db db = new Db())
            {
                CategoryDTO category = db.Categories.Where(x => x.Id == id).FirstOrDefault();
                
                cat.Id = category.Id;
                cat.Name = category.Name;
                cat.Slug = category.Slug;
                cat.Sorting = category.Sorting;

            }
            // Return view with model
            return View(cat);
        }
        // POST: Admin/Shop/EditProduct
        [HttpPost]
        public ActionResult EditProduct(CategoryDTO category)
        {
            // Make sure product name is unique
            using (Db db = new Db())
            {
                CategoryDTO old_category = db.Categories.Where(x => x.Id == category.Id).FirstOrDefault();
                if (old_category != null)
                {
                    old_category.Name = category.Name;
                    old_category.Slug = category.Slug;
                    old_category.Sorting = category.Sorting;
                    
                }
                if (ModelState.IsValid)
                {
                    db.SaveChanges();
                }
            }


            // Return view with model
            return RedirectToAction("Categories");
        }
        public ActionResult DeleteProduct(int ID)
        {
            CategoryDTO category;
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (Db db = new Db())
            {
                category = db.Categories.Find(ID);
                if (category == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    db.Categories.Remove(category);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        ModelState.AddModelError("",
                          String.Format("Category with id {0} no longer exists in the database.", category.Id));
                    }
                }
            }

            return RedirectToAction("Categories");
        }
        // POST: Admin/Shop/DeleteProduct
        [HttpPost]
        public ActionResult DeleteProduct(CategoryDTO category)
        {

            using (Db db = new Db())
            {
                if (db.Categories.Contains(category))
                {
                    db.Categories.Remove(category);
                }
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("",
                      String.Format("Category with id {0} no longer exists in the database.", category.Id));
                }

            }
            // Return view with model
            return RedirectToAction("Categories");
        }
    }
}
}