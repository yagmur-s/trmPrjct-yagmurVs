using MShop.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MShop.Models.ViewModels.Shop
{
    public class CityVM
    {
        public CityVM()
        {

        }
        public CityVM(CityDTO row)
        {
            Id = row.Id;
            CityName = row.CityName;
        }
        public int Id { get; set; }
        public string CityName { get; set; }
    }
    
}