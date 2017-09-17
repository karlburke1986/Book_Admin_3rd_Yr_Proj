using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StConlethsBookSystem_v2._1.Models
{
    public class Classes
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Class Name")]        
        public string className { get; set; }

        public static explicit operator string(Classes v)
        {
            throw new NotImplementedException();
        }
    }  

    
}