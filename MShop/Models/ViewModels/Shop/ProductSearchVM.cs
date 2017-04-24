using MShop.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MShop.Models.ViewModels.Shop
{
    public class ProductSearchVM
    {
        public ProductSearchVM()
        {

        }
        public ProductSearchVM(ProductDTO row)
        {
            CategoryId = row.CategoryId;
            TimePeriodId = row.PlaceId;
            CityId = row.TimePeriodId;
        }
        public int CategoryId { get; set; }
        public int TimePeriodId { get; set; }
        public int CityId { get; set; }
    }
}