using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MShop.Models.Data
{
    [Table("tblCities")]
    public class CityDTO
    {
        [Key]
        public int Id { get; set; }
        public string CityName { get; set; }
    }
}