using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MShop.Models.Data
{
    [Table("tblProducts")]
    public class ProductDTO
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public int PlaceId { get; set; }
        public DateTime Date { get; set; }
        public int TimePeriodId { get; set; }
        public string TimePeriodName { get; set; }

        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public string ImageName { get; set; }

        [ForeignKey("CategoryId")]
        public virtual CategoryDTO Category { get; set; }

        [ForeignKey("TimePeriodId")]
        public virtual TimePeriodDTO TimePeriod { get; set; }
        [ForeignKey("PlaceId")]
        public virtual CityDTO City { get; set; }

    }
}