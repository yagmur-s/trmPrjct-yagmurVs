using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MShop.Models.Data
{
    public class Db : DbContext
    {
        public DbSet<PagesDTO> Pages { get; set; }
        public DbSet<CategoryDTO> Categories { get; set; }
        public DbSet<ProductDTO> Products { get; set; }
        public DbSet<TimePeriodDTO> TimePeriods { get; set; }
        public DbSet<CityDTO> Cities { get; set; }
        public DbSet<UsersDTO> Users { get; set; }

        public System.Data.Entity.DbSet<MShop.Models.ViewModels.Shop.CategoryVM> CategoryVMs { get; set; }
    }
}