using StConlethsBookSystem_v2._1.Models;
using StConlethsBookSystem_v2._1.Models.HomeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StConlethsBookSystem_v2._1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        BookSystemDBMain _db = new BookSystemDBMain();
        BooksSystemDB db = new BooksSystemDB();

        public ActionResult Index()
        {
            BookRequestAccess access = new BookRequestAccess();

            try
            {
                access = db.BookRequestAccess.First();
            }
            catch
            {
                access.Access = false;
                db.BookRequestAccess.Add(access);
                db.SaveChanges();

                access = db.BookRequestAccess.First();
            }

            ViewBag.Access = access.Access;

            return View(); 
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact() 
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult FindBook()
        {
            return PartialView("_FindBook");
        }

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult _FindBook(string BookID = null)
        {

            List<string> errorList = new List<string>();

            if (BookID != null)
            {
                SearchBookViewModel model = new SearchBookViewModel();

                if(BookID.Length == 13)
                {
                    string barcodeIn = BookID;
                   

                    int bookID = 0;

                    BookID = BookID.Substring(5, (BookID.Length - 5));
                    BookID = BookID.Remove(BookID.Length - 1);

                    bool result = Int32.TryParse(BookID, out bookID);

                    if (result == true)
                    {
                        Book book = _db.Books.Find(bookID);

                        if(book == null)
                        {
                            errorList.Add("Book could not be found");
                        }
                        else
                        {
                            BookDes bookdes = _db.BookDess.Find(book.BookDesID);

                            if (book.letOut == false)
                            {
                                
                                model.BookID = book.ID;
                                model.BookName = bookdes.Title;
                                model.StudentID = book.StudentID;
                                model.StudentClass = "N/A";
                                model.StudentName = "N/A";
                                model.StudentYear = "N/A";
                            }
                            else
                            {
                                Student student = _db.Students.Find(book.StudentID);

                                model.Barcode = barcodeIn;
                                model.BookName = bookdes.Title;
                                model.StudentID = student.ID;
                                model.StudentName = student.FirstName + " " + student.LastName;
                                model.StudentYear = student.Year.ToString();
                                model.StudentClass = student.Class;

                            }
                        }                
            
                        return PartialView("_FindBook", model);
                    }
                    else
                    {
                        errorList.Add("Only Numbers can be entered in this section");
                    }                

                    
                }

                else
                {
                    errorList.Add("Entered Value Must be 13 Numbers Long");                    
                }
                
            }
            if(errorList.Count != 0)
            {
                for(int i = 0; i < errorList.Count; i++)
                {
                    ModelState.AddModelError("Validation Error", errorList[i]);
                }
            }
            return View();     
        }
    }
}