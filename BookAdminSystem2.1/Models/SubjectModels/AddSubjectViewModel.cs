using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StConlethsBookSystem_v2._1.Models.SubjectModels
{
    public class AddSubjectViewModel
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
    }
}