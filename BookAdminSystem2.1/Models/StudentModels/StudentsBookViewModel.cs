using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StConlethsBookSystem_v2._1.Models.StudentModels
{
    public class StudentsBookViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Student Name")]
        public string StudentName { get; set; }        
        public int Year { get; set; }
        public string Class { get; set; }
    }
}