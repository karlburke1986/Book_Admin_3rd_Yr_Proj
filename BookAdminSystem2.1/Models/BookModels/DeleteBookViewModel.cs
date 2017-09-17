using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StConlethsBookSystem_v2._1.Models.BookModels
{
    public class DeleteBookViewModel
    {
        public string ID { get; set; }
        public string BarCode { get; set; }
        public string BookName { get; set; }
        [Display(Name = "Assigned To")]
        public string StudentName { get; set; }
    }
}