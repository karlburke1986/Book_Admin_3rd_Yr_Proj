using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StConlethsBookSystem_v2._1.Models
{
    public class Student
    {
        [Key]
        public int ID { get; set; } 
        [Display(Name ="First Name")]     
        public string FirstName { get; set; }
        [Display(Name ="Surname")]     
        public string LastName { get; set; }   
        public int Year { get; set; }
        public string Class { get; set; }
    }
}