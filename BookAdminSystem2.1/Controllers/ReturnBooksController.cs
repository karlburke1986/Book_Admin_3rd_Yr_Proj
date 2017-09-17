using StConlethsBookSystem_v2._1.Models;
using StConlethsBookSystem_v2._1.Models.ReturnBooksModel;
using System;
using System.Web.Mvc;

namespace StConlethsBookSystem_v2._1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReturnBooksController : Controller
    {
        BookSystemDBMain _db = new BookSystemDBMain();
        // GET: ReturnBooks
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string code = null )
        {
            if(code.Length == 13)
            {
                code = code.Substring(5, (code.Length - 5));
                code = code.Remove(code.Length - 1);

                int barcode = 0;

                bool result = Int32.TryParse(code, out barcode);
                ReturnBookViewModel model = new ReturnBookViewModel();

                if(result == true)
                {
                    var books = _db.Books.Find(barcode);
                    if(books != null)
                    {
                        if (books.BookDesID != 0)
                        {
                            var bookDes = _db.BookDess.Find(books.BookDesID);
                        }
                        else
                        {
                            model.Title = "ERROR!!!!";
                        }
                        if (books.StudentID != 0)
                        {
                            var student = _db.Students.Find(books.StudentID);
                            model.Student = student.FirstName + " " + student.LastName;
                            books.letOut = false;
                            books.StudentID = 0;
                            _db.SaveChanges();

                            Admin_Tools.LogControls.createLog(User.Identity.Name, "Book Unassigned", books.ID + " was removed from " + student.FirstName +
                        " " + student.LastName + "'s account");

                            ViewBag.Message = "Success: Book has been successfully unassigned";
                        }
                        else
                        {
                            ViewBag.Message = "ERROR: Book is already unassigned";
                        }
                    }
                    else
                    {
                        ViewBag.Message = "ERROR: Book could not be found";
                    }                 
                    
                }
                else
                {
                    ViewBag.Message = "ERROR: Invalid input. Entries must be only contain numbers and must be 13 numbers long";
                }
            }
            else
            {
                ViewBag.Message = "ERROR: Entries must be 13 numbers long";
            }
                      

            return View();
        }
    }
}