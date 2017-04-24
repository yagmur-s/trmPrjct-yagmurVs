using MShop.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MShop.Models.ViewModels.Users
{
    public class UsersVM
    {
        public UsersVM()
        {

        }
        public UsersVM(UsersDTO row)
        {
            Id = row.Id;
            UserName = row.UserName;
            Email = row.Email;
            Password = row.Password;
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter a valid username")]
        [StringLength(50, MinimumLength = 6)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter a valid email address")]
        [StringLength(50, MinimumLength = 10)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter a valid password")]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }
    }
}