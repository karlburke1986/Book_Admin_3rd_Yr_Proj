using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StConlethsBookSystem_v2._1.Models.BookRequestModels
{
    public class BookRequestCreateModel
    {
        public int ID { get; set; }    
        [Required]    
        public string BookName { get; set; }
        public string AltBookName { get; set; }
        [Required]
        public int Quantity { get; set; }
        
        
    }
}