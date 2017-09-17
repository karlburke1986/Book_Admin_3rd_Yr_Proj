using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StConlethsBookSystem_v2._1.Models.StudentModels
{
    public class AddStudentViewModel
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        public string Surname { get; set; }                
        public string Year  { get; set; }        
        public string Class { get; set; }        
    }
}