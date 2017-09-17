using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StConlethsBookSystem_v2._1.Models.BookModels
{
    public class UnassignBookViewModel
    {
        public string ID { get; set; }
        public int BookID { get; set; }
        public string BookName { get; set; }
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        
    }
}