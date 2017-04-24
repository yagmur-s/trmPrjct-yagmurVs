using MShop.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MShop.Models.ViewModels.Shop
{
    public class TimePeriodVM
    {
        public TimePeriodVM()
        {
            
        }

        public TimePeriodVM(TimePeriodDTO row)
        {
            Id = row.Id;
            TimePeriod = row.TimePeriod;
        }
        public int Id { get; set; }
        public string TimePeriod { get; set; }
    }
}