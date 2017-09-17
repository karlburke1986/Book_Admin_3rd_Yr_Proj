using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace StConlethsBookSystem_v2._1.Models 
{
    public class BookSystemDBMain : DbContext
    {

        public BookSystemDBMain() : base("name=BooksDb")
        { }

        public DbSet<Student> Students { get; set; }

        public DbSet<BookDes> BookDess { get; set; }
        public DbSet<Book> Books { get; set; }

        public System.Data.Entity.DbSet<StConlethsBookSystem_v2._1.Models.BookModels.BookStudentViewModel> BookStudentViewModels { get; set; }

        public System.Data.Entity.DbSet<StConlethsBookSystem_v2._1.Models.BookModels.UnassignBookViewModel> UnassignBookViewModels { get; set; }

        public System.Data.Entity.DbSet<StConlethsBookSystem_v2._1.Models.BookModels.DeleteBookViewModel> DeleteBookViewModels { get; set; }

        public System.Data.Entity.DbSet<StConlethsBookSystem_v2._1.Models.StudentModels.StudentsBookViewModel> StudentsBookViewModels { get; set; }

        public System.Data.Entity.DbSet<StConlethsBookSystem_v2._1.Models.BookModels.BookStudentPartialViewModel> BookStudentPartialViewModels { get; set; }

        public System.Data.Entity.DbSet<StConlethsBookSystem_v2._1.Models.ReturnBooksModel.ReturnBookViewModel> ReturnBookViewModels { get; set; }

        public System.Data.Entity.DbSet<StConlethsBookSystem_v2._1.Models.BookDesModels.AddAdditionalBooks> AddAdditionalBooks { get; set; }

        public System.Data.Entity.DbSet<StConlethsBookSystem_v2._1.Models.StudentModels.CSVStudents> CSVStudents { get; set; }

        public System.Data.Entity.DbSet<StConlethsBookSystem_v2._1.Models.BookRequestModel> BookRequestModels { get; set; }

        public System.Data.Entity.DbSet<StConlethsBookSystem_v2._1.Models.BookRequestModels.BookRequestCreateModel> BookRequestCreateModels { get; set; }

        public System.Data.Entity.DbSet<StConlethsBookSystem_v2._1.Models.AdminModels.ChangeUserPassword> ChangeUserPasswords { get; set; }
    }
}