using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace StConlethsBookSystem_v2._1.Models.AdminModels
{
    public class Register //: IValidatableObject
    {
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Confirm Email is required")]
        [DataType(DataType.EmailAddress)]
        [Compare("Email")]
        [Display(Name = "Confirm Email")]
        public string Email2 { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }
        [Required(ErrorMessage ="Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string Password2 { get; set; }



        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    List<char> list = new List<char>(new char[] { '\\', '|', ',', '.', '<', '>', '/', '?', '#', '~', ':', ';', '{', '[', '}', ']', '=', '+'
        //        , '_', '-', ')', '(', '*', '&', '^', '%', '$', '"', '€', '£'});

        //    bool result = false; 

        //    if (Password.Any(char.IsUpper))
        //    {
        //        if(Password.Any(char.IsLower))
        //        {
        //            if(Password.Any(char.IsDigit))
        //            {
        //                for(int i = 0; i < list.Count && result == false; i++)
        //                {
        //                    char a = list[i];

        //                    for (int j = 0; j < Password.Length; j++)
        //                    {
        //                        if(Password.Contains(a))
        //                        {
        //                            result = true;
        //                        }

        //                        else
        //                        {
        //                            result = false;
        //                        }
        //                    }
        //                }                        

        //                if (result == true)
        //                {
        //                    //Do nothing
        //                }
        //                else
        //                {
        //                    yield return new ValidationResult("Non-numeric or alphabetic character must be used");
        //                }
        //            }
        //            else
        //            {
        //                yield return new ValidationResult("Password must contain at least one number");                        
        //            }
        //        }
        //        else
        //        {
        //            yield return new ValidationResult("Password must contain at least one lowercase letter");
        //        }
        //    }
        //    else
        //    {
        //        yield return new ValidationResult("Password must contain at least one uppercase letter");                
        //    }
            
        //}
    }
}