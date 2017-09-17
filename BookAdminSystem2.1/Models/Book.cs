using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StConlethsBookSystem_v2._1.Models
{
    public class Book
    {
        public int ID { get; set; }
        public int StudentID { get; set; }
        public int BookDesID { get; set; }
        public bool letOut { get; set; }
    }
}