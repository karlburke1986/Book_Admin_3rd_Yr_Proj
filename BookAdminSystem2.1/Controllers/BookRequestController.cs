using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StConlethsBookSystem_v2._1.Models;
using StConlethsBookSystem_v2._1.Models.BookRequestModels;

namespace StConlethsBookSystem_v2._1.Controllers
{
    [Authorize]
    public class BookRequestController : Controller
    {
        private BooksSystemDB db = new BooksSystemDB();
        private BookSystemDBMain _db = new BookSystemDBMain();

        // GET: BookRequest
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

            List<BookRequestModel> books = db.BookRequestModels.ToList();

            books = (from item in books
                     where item.Resolved.Equals(false)
                     select item).ToList();

            return View(books);
        }

        // GET: BookRequest/Create
        public ActionResult Create()
        {
            var books = (from item in _db.BookDess
                        orderby item.Title
                        select item).ToList();

            ViewBag.Books = books;

            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookRequestCreateModel model)
        {
            if (ModelState.IsValid)
            {
                BookRequestModel book = new BookRequestModel();

                if(model.BookName != null && model.BookName != "0" && model.BookName != "")
                {
                    book.BookName = model.BookName;

                }
                else
                {
                    book.BookName = model.AltBookName;
                }

                book.Quantity = model.Quantity;
                book.RequestedUser = User.Identity.Name;
                book.Resolved = false;                      
                

                db.BookRequestModels.Add(book);
                db.SaveChanges();

                Admin_Tools.LogControls.createLog(User.Identity.Name, "Book Request", book.BookName + " was requested");
                                
                return RedirectToAction("Index", "Home");
            }

            var books = (from item in _db.BookDess
                         orderby item.Title
                         select item).ToList();

            ViewBag.Books = books;

            return View();
        }        

        [HttpGet]
        public ActionResult Resolved(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookRequestModel bookRequestModel = db.BookRequestModels.Find(id);
            if (bookRequestModel == null)
            {
                return HttpNotFound();
            }
            return View(bookRequestModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Resolved(int id)
        {
            if(id != 0)
            {
                BookRequestModel bookRequestModel = db.BookRequestModels.Find(id);
                bookRequestModel.ResolvedUser = User.Identity.Name;
                bookRequestModel.Resolved = true;
                db.SaveChanges();
                Admin_Tools.LogControls.createLog(User.Identity.Name, "Book Request",  bookRequestModel.RequestedUser + "'s requested for "
                    + bookRequestModel.Quantity + " " + bookRequestModel.BookName + " was resolved");
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error");
            }
            
        }

        public ActionResult ChangeAccess(bool option)
        {

            BookRequestAccess temp = db.BookRequestAccess.First();

            temp.Access = option;
            db.SaveChanges();

            if(option == true)
            {
                Admin_Tools.LogControls.createLog(User.Identity.Name, "Book Request Access", "Access was opened");
            }
            else if(option == false)
            {
                Admin_Tools.LogControls.createLog(User.Identity.Name, "Book Request Access", "Access was closed");
            }


            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
