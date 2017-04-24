using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MShop.Models.Data;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MShop.Models.ViewModels.Shop
{
    public class ProductVM
    {
        public ProductVM()
        {
        }

        public ProductVM(ProductDTO row)
        {
            Id = row.Id;
            Name = row.Name;
            Description = row.Description;
            Price = row.Price;
            CategoryName = row.CategoryName;
            CategoryId = row.CategoryId;
            ImageName = row.ImageName;
            Place = row.Place;
            Date = row.Date;
            TimePeriodName = row.TimePeriodName;
            TimePeriodId = row.TimePeriodId;
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Place { get; set; }
        [Required]

        public int PlaceId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string TimePeriodName { get; set; }
        public int TimePeriodId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string ImageName { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> TimePeriods { get; set; }
        public IEnumerable<SelectListItem> Cities { get; set; }
        public IEnumerable<string> GalleryImages { get; set; }
    }
}