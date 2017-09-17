using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Linq;

namespace StConlethsBookSystem_v2._1.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel : IValidatableObject
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<char> list = new List<char>(new char[] { '\\', '|', ',', '.', '<', '>', '/', '?', '#', '~', ':', ';', '{', '[', '}', ']', '=', '+'
                , '_', '-', ')', '(', '*', '&', '^', '%', '$', '"', '€', '£'});

            bool result = false; 

            if (NewPassword.Any(char.IsUpper))
            {
                if(NewPassword.Any(char.IsLower))
                {
                    if(NewPassword.Any(char.IsDigit))
                    {
                        for(int i = 0; i < list.Count && result == false; i++)
                        {
                            char a = list[i];

                            for (int j = 0; j < NewPassword.Length; j++)
                            {
                                if(NewPassword.Contains(a))
                                {
                                    result = true;
                                }

                                else
                                {
                                    result = false;
                                }
                            }
                        }                        

                        if (result == true)
                        {
                            //Do nothing
                        }
                        else
                        {
                            yield return new ValidationResult("Non-numeric or alphabetic character must be used");
                        }
                    }
                    else
                    {
                        yield return new ValidationResult("Password must contain at least one number");                        
                    }
                }
                else
                {
                    yield return new ValidationResult("Password must contain at least one lowercase letter");
                }
            }
            else
            {
                yield return new ValidationResult("Password must contain at least one uppercase letter");                
            }

        }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}