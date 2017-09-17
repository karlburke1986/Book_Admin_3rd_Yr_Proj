using StConlethsBookSystem_v2._1.Models.AdminModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace StConlethsBookSystem_v2._1.Models
{
    public class BooksSystemDB : DbContext
    {
        public BooksSystemDB() : base("name=DefaultConnection")
        {}

        public DbSet<LogModel> Logs { get; set; }
        
        public DbSet<Classes> classes { get; set; }        
        public DbSet<Subject>Subjects { get; set; }
        public DbSet<BookRequestModel>BookRequestModels { get; set; }
        public DbSet<BookRequestAccess>BookRequestAccess { get; set; }

    }
}