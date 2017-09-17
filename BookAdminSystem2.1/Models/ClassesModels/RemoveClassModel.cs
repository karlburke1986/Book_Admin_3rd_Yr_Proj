using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StConlethsBookSystem_v2._1.Models.ClassesModels
{
    public class RemoveClassModel
    {
        [Key]
        public int ID { get; set; }
        [Display(Name ="Class Name")]
        public string ClassName { get; set; }
    }
}