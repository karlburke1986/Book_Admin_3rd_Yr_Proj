using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StConlethsBookSystem_v2._1.Models.BookModels
{
    public class AddBookViewModel
    {
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }   
        [Required]     
        public string Author { get; set; }
        [Required]
        public string Edition { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}