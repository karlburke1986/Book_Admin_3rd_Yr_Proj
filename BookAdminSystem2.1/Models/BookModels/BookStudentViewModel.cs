using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StConlethsBookSystem_v2._1.Models.BookModels
{
    public class BookStudentViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Barcode")]
        public double BookID { get; set; }
        [Display(Name = "Book Name")]
        public string BookName { get; set; }
        public int StudentID { get; set; }
        [Display(Name = "Retail Detail")]
        public string Detail { get; set; }
    }
}