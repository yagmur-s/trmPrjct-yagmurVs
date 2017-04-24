using MShop.Models.Data;
using MShop.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity.Infrastructure;
using System.Net;

namespace MShop.Controllers
{
    //vdersnoup
    public class ShopController : Controller
    {
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
        public ActionResult AddProduct(int? page, int? catId)
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
        public ActionResult AddProduct(ProductVM model, HttpPostedFileBase file)
        {
            // Check model state
            if (!ModelState.IsValid)
            {
                using (Db db = new Db())
                {
                    model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                    model.TimePeriods = new SelectList(db.TimePeriods.ToList(), "Id", "TimePeriod");
                    model.Cities = new SelectList(db.Cities.ToList(), "Id", "CityName");
                    return View(model);
                }
            }

            // Make sure product name is unique
            using (Db db = new Db())
            {
                if (db.Products.Any(x => x.Name == model.Name))
                {
                    model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                    model.TimePeriods = new SelectList(db.TimePeriods.ToList(), "Id", "TimePeriod");
                    model.Cities = new SelectList(db.Cities.ToList(), "Id", "CityName");
                    ModelState.AddModelError("", "That product name is taken!");
                    return View(model);
                }
            }

            // Declare product id
            int id;

            // Init and save productDTO
            using (Db db = new Db())
            {
                ProductDTO product = new ProductDTO();

                product.Name = model.Name;
                product.Description = model.Description;
                product.Price = model.Price;
                product.Date = model.Date;

                product.CategoryId = model.CategoryId;
                CategoryDTO catDTO = db.Categories.FirstOrDefault(x => x.Id == model.CategoryId);
                product.CategoryName = catDTO.Name;

                product.TimePeriodId = model.TimePeriodId;
                TimePeriodDTO timePeriodDTO = db.TimePeriods.FirstOrDefault(x => x.Id == model.TimePeriodId);
                product.TimePeriodName = timePeriodDTO.TimePeriod;

                product.PlaceId = model.PlaceId;
                CityDTO cityDTO= db.Cities.FirstOrDefault(x => x.Id == model.PlaceId);
                product.Place = cityDTO.CityName;

                db.Products.Add(product);
                db.SaveChanges();

                // Get the id
                id = product.Id;
            }

            // Set TempData message
            TempData["SM"] = "You have added a product!";

            #region Upload Image

            // Create necessary directories
            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

            var pathString1 = Path.Combine(originalDirectory.ToString(), "Products");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
            var pathString3 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");
            var pathString4 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
            var pathString5 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");

            if (!Directory.Exists(pathString1))
                Directory.CreateDirectory(pathString1);

            if (!Directory.Exists(pathString2))
                Directory.CreateDirectory(pathString2);

            if (!Directory.Exists(pathString3))
                Directory.CreateDirectory(pathString3);

            if (!Directory.Exists(pathString4))
                Directory.CreateDirectory(pathString4);

            if (!Directory.Exists(pathString5))
                Directory.CreateDirectory(pathString5);

            // Check if a file was uploaded
            if (file != null && file.ContentLength > 0)
            {
                // Get file extension
                string ext = file.ContentType.ToLower();

                // Verify extension
                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png")
                {
                    using (Db db = new Db())
                    {
                        model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                        ModelState.AddModelError("", "The image was not uploaded - wrong image extension.");
                        return View(model);
                    }
                }

                // Init image name
                string imageName = file.FileName;

                // Save image name to DTO
                using (Db db = new Db())
                {
                    ProductDTO dto = db.Products.Find(id);
                    dto.ImageName = imageName;

                    db.SaveChanges();
                }

                // Set original and thumb image paths
                var path = string.Format("{0}\\{1}", pathString2, imageName);
                var path2 = string.Format("{0}\\{1}", pathString3, imageName);

                // Save original
                file.SaveAs(path);

                // Create and save thumb
                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);
            }

            #endregion

            // Redirect
            return RedirectToAction("AddProduct");
        }
        public ActionResult Products(int? page, int? catId,int? timePeriodId,int? cityId)
        {
            // Declare a list of ProductVM
            List<ProductVM> listOfProductVM;

            // Set page number
            var pageNumber = page ?? 1;

            using (Db db = new Db())
            {
                    listOfProductVM = db.Products.ToArray()
                                  .Where(x => (cityId==null || cityId==0 || x.PlaceId==cityId)&&
                                  (timePeriodId==null || timePeriodId==0 || x.TimePeriodId==timePeriodId )&& (catId == null||catId == 0 || x.CategoryId == catId))
                                  .Select(x => new ProductVM(x))
                                  .ToList();
                
                // Populate categories select list
                ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                ViewBag.TimePeriods = new SelectList(db.TimePeriods.ToList(), "Id", "TimePeriod");
                ViewBag.Cities = new SelectList(db.Cities.ToList(), "Id", "CityName");
                // Set selected category
                ViewBag.SelectedCat = catId.ToString();
                ViewBag.SelectedTimePeriod = timePeriodId.ToString();
                ViewBag.SelectedCity = cityId.ToString();
                
            }

            // Set pagination
            var onePageOfProducts = listOfProductVM.ToPagedList(pageNumber, 3);
            ViewBag.OnePageOfProducts = onePageOfProducts;

            // Return view with list
            return View(listOfProductVM);
        }
      
        // GET: Admin/Shop/EditProduct
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            // Init model
            ProductVM prod = new ProductVM();
           
            using (Db db = new Db())
            {
                ProductDTO product = db.Products.Where(x => x.Id == id).FirstOrDefault();
                prod.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                prod.CategoryId = product.CategoryId;
                prod.Id = product.Id;
                prod.CategoryName = product.CategoryName;
                prod.Description = product.Description;
                prod.ImageName = product.ImageName;
                prod.Name = product.Name;
                prod.Price = product.Price;
                prod.TimePeriodName = product.TimePeriodName;
                prod.TimePeriodId = product.TimePeriodId;

            }
            // Return view with model
            return View(prod);
        }
        // POST: Admin/Shop/EditProduct
        [HttpPost]
        public ActionResult EditProduct(ProductDTO product)
        {
            // Make sure product name is unique
            using (Db db = new Db())
            {
                ProductDTO old_product = db.Products.Where(x => x.Id == product.Id).FirstOrDefault();
                if (old_product != null)
                {
                    old_product.CategoryName = product.CategoryName;
                    old_product.Description = product.Description;
                    old_product.ImageName = product.ImageName;
                    old_product.Name = product.Name;
                    old_product.Price = product.Price;
                    old_product.TimePeriodName = product.TimePeriodName;
                    old_product.TimePeriodId = product.TimePeriodId;
                }
                if (ModelState.IsValid)
                {
                    db.SaveChanges();
                }
            }


            // Return view with model
            return RedirectToAction("Products");
        }
        public ActionResult DeleteProduct(int ID)
        {
            ProductDTO product;
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using(Db db=new Db())
            {
                product = db.Products.Find(ID);
                if (product == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    db.Products.Remove(product);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        ModelState.AddModelError("",
                          String.Format("Product with id {0} no longer exists in the database.", product.Id));
                    }
                }
            }
            
            return RedirectToAction("Products");
        }
        // POST: Admin/Shop/DeleteProduct
        [HttpPost]
        public ActionResult DeleteProduct(ProductDTO product)
        {
            
            using (Db db = new Db())
            {
                if (db.Products.Contains(product))
                {
                    db.Products.Remove(product);
                }
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("",
                      String.Format("Product with id {0} no longer exists in the database.", product.Id));
                }

            }
            // Return view with model
            return RedirectToAction("Products");
        }
    }
}