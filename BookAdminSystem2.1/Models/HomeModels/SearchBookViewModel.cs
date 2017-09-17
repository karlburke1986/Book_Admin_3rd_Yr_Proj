using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StConlethsBookSystem_v2._1.Models.HomeModels
{
    public class SearchBookViewModel
    {
        public int BookID { get; set; }
        public string Barcode { get; set; }
        public string BookName { get; set; }
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public string StudentYear { get; set; }
        public string StudentClass { get; set; }

    }
}