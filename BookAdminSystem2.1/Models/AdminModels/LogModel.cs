using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StConlethsBookSystem_v2._1.Models.AdminModels
{
    public class LogModel
    {
        public int ID { get; set; }
        public string userName { get; set; }
        public DateTime time { get; set; }
        public string eventType { get; set; }
        public string eventDetails { get; set; }
    }
}