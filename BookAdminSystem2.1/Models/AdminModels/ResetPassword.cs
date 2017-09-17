using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StConlethsBookSystem_v2._1.Models.AdminModels
{
    public class ResetPassword : IValidatableObject
    {
        [Key]
        public string ID { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string Password2 { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {


            if (Password.Any(char.IsUpper))
            {
                if (Password.Any(char.IsLower))
                {
                    if (Password.Any(char.IsDigit))
                    {
                        //if (!Password.Any(char.IsLetterOrDigit))
                        //{

                        //}
                        //else
                        //{
                        //    yield return new ValidationResult("Non-numeric or alphabetic character must be used");
                        //}
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
}