using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StConlethsBookSystem_v2._1.Models
{
    public class BookRequestModel
    {
        public int ID { get; set; }
        public string RequestedUser { get; set; }
        public string BookName { get; set; }
        public int Quantity { get; set; }
        public string ResolvedUser { get; set; }
        public bool Resolved { get; set; }

    }
}