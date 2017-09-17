using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StConlethsBookSystem_v2._1.Models.ReturnBooksModel
{
    public class ReturnBookViewModel
    {
        public int ID { get; set; }
        public string Barcode { get; set; }
        public string Student { get; set; }
        public string Title { get; set; }
    }
}