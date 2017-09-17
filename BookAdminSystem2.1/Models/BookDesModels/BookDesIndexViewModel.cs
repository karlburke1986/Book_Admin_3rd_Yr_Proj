using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StConlethsBookSystem_v2._1.Models.BookDesModels
{
    public class BookDesIndexViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Edition { get; set; }
        public string Subject { get; set; }
        [Display(Name = "Total Books")]
        public int inStock { get; set; }
        [Display(Name = "Free")]
        public int unassigned { get; set; }
        [Display(Name = "Rented")]
        public int rented { get; set; }
    }
}