using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StConlethsBookSystem_v2._1.Models;
using StConlethsBookSystem_v2._1.Models.AdminModels;

namespace StConlethsBookSystem_v2._1.Admin_Tools
{
    public class LogControls
    {
        public static bool createLog(string userName, string eventType, string eventDetails)
        {
            try
            {
                BooksSystemDB _db = new BooksSystemDB();

                LogModel cLog = new LogModel();
                cLog.userName = userName;
                cLog.time = DateTime.UtcNow;
                cLog.eventType = eventType;
                cLog.eventDetails = eventDetails;

                _db.Logs.Add(cLog);
                _db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}