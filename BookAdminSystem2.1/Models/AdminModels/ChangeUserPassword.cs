using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StConlethsBookSystem_v2._1.Models.AdminModels
{
    public class ChangeUserPassword
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Old Password is required")]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string oldPassword { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string newPassword { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("newPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string confirmPassword { get; set; }
    }
}