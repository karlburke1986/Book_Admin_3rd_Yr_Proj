using StConlethsBookSystem_v2._1.Models;
using StConlethsBookSystem_v2._1.Models.BookModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Spire.Barcode;
using System.Drawing;

namespace StConlethsBookSystem_v2._1.Controllers
{
    public class BookController : Controller
    {
        BookSystemDBMain _db = new BookSystemDBMain();
        // GET: Book
        public ActionResult Index(int? id, int page = 1, string bookName = null, string status = null, string barcode = null, string showall = null)
        {
            try
            {
                ViewData.Clear();

                //Barcode books
                List<Book> books = (from item in _db.Books
                                    orderby item.ID
                                    select item).ToList();

                if ((bookName == "" || bookName == null) && (status == "" || status == null) && (barcode == "" || barcode == null) && (showall == "" || showall == null))
                {
                    int totalBooksIn = (from item in books
                                        where item.letOut.Equals(false)
                                        select item).Count();

                    int totalBooksOut = (from item in books
                                         where item.letOut.Equals(true)
                                         select item).Count();

                    ViewBag.TotalBooksIn = totalBooksIn;
                    ViewBag.TotalBooksOut = totalBooksOut;
                }

                //Outer books
                List<BookDes> bookDes = (from item in _db.BookDess
                                         orderby item.Title
                                         select item).ToList();
                List<Student> students = (from item in _db.Students
                                          orderby item.FirstName
                                          select item).ToList();

                if (showall == null)
                {
                    if (bookName != "" && bookName != null)
                    {
                        int bookDesid = 0;
                        var bookDesSearch = from item in bookDes
                                            where item.Title.Equals(bookName)
                                            select item;
                        bookDesid = bookDesSearch.First().ID;

                        books = (from item in books
                                 where item.BookDesID.Equals(bookDesid)
                                 orderby item.ID
                                 select item).ToList();
                    }

                    if (status != "" && status != null)
                    {
                        if (status == "true")
                        {
                            books = (from item in books
                                     where item.letOut.Equals(true)
                                     orderby item.ID
                                     select item).ToList();

                            int totalOut = books.Count;
                            ViewBag.TotalBooksOut = totalOut;
                            ViewBag.TotalBooks = totalOut;
                        }

                        if (status == "false")
                        {
                            books = (from item in books
                                     where item.letOut.Equals(false)
                                     orderby item.ID
                                     select item).ToList();

                            int totalIn = books.Count;
                            ViewBag.TotalBooksIn = totalIn;
                            ViewBag.TotalBooks = totalIn;
                        }

                        ViewBag.Status = status;
                    }

                    if (barcode != null && barcode != "")
                    {
                        string barcodeViewBag = barcode;

                        barcode = barcode.Substring(5, (barcode.Length - 5));
                        barcode = barcode.Remove(barcode.Length - 1);
                        int barcodeIn = Convert.ToInt32(barcode);

                        books = (from item in books
                                 where item.ID == (barcodeIn)
                                 orderby item.ID
                                 select item).ToList();

                        ViewBag.Barcode = barcodeViewBag;
                    }
                }

                ViewBag.BookNames = bookDes;

                if (id != null)
                {
                    int idIn = Convert.ToInt32(id);

                    books = (from item in books
                             where item.BookDesID.Equals(idIn)
                             orderby item.ID
                             select item).ToList();
                }


                List<BookStudentViewModel> bookList = new List<BookStudentViewModel>();

                //All books with bcode or selecetd based on outer book
                foreach (var item in books)
                {
                    try
                    {
                        BookStudentViewModel temp = new BookStudentViewModel();

                        //Barcode only for visual
                        temp.BookID = Double.Parse(70680.ToString() + item.ID.ToString());

                        foreach (var ob in bookDes)
                        {
                            try
                            {
                                if (ob.ID == item.BookDesID)
                                {
                                    temp.BookName = ob.Subject + " - " + ob.Title;
                                    break;
                                }
                                else
                                {
                                    temp.BookName = "ERROR!!!";
                                }
                            }
                            catch
                            {

                            }

                        }

                        if (item.StudentID != 0)
                        {
                            temp.StudentID = item.StudentID;

                            var stu = from items in students
                                      where items.ID.Equals(item.StudentID)
                                      select items;

                            //Add breakpoint if only one entry
                            foreach (var i in stu)
                            {
                                if (i.Class == "Teacher")
                                {
                                    temp.Detail = "Teacher - " + i.FirstName + " " +
                                        i.LastName;
                                }
                                else
                                {
                                    temp.Detail = "Student - " + i.FirstName + " " +
                                        i.LastName;
                                }

                                bookList.Add(temp);
                            }
                        }
                        else
                        {
                            temp.StudentID = 0;
                            temp.Detail = "Unassigned";

                            bookList.Add(temp);
                        }
                    }
                    catch { }

                }

                if (books != null)
                {
                    int total = books.Count;
                    ViewBag.TotalBooks = total;

                    int totalIn = (from item in books
                                   where item.letOut.Equals(false)
                                   select item).Count();
                    int totalOut = (from item in books
                                    where item.letOut.Equals(true)
                                    select item).Count();

                    ViewBag.TotalBooksIn = totalIn;
                    ViewBag.TotalBooksOut = totalOut;
                }

                int pageSize = 150;

                return View(bookList.ToPagedList(page, pageSize));
            }
            catch
            {
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult UnassignBook(string id, string viewName = null)
        {
            try
            {
                if (id != null)
                {                   

                    if(id.Length > 7)
                    {
                        ViewBag.ID = id;
                        id = id.Substring(5, (id.Length - 5));                        
                    }
                    else
                    {
                        ViewBag.ID = 70680.ToString() + id;
                    }

                    
                    
                    int ID = Convert.ToInt32(id);

                    var book = _db.Books.Find(ID);

                    var bookDes = _db.BookDess.Find(book.BookDesID);

                    var student = _db.Students.Find(book.StudentID);

                    if (viewName != null)
                    {
                        TempData["stuID"] = book.StudentID;
                    }



                    UnassignBookViewModel unassBook = new UnassignBookViewModel();

                    unassBook.ID = book.ID.ToString();
                    unassBook.BookID = book.BookDesID;
                    unassBook.BookName = bookDes.Title;
                    unassBook.StudentID = student.ID;
                    unassBook.StudentName = student.FirstName + " " + student.LastName;

                    return View(unassBook);
                }
                else
                {
                    return View("Error");
                }
            }

            catch
            {
                  return View("Error");
            }
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnassignBook(UnassignBookViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string id = model.ID;
                    if (id.Length > 7)
                    {
                        id = id.Substring(5, (id.Length - 5));
                    }

                    Book book = new Book();


                    int ID = Convert.ToInt32(id);
                    book = _db.Books.Find(ID);
                    int stuID = book.StudentID;
                    book.letOut = false;
                    int? studentsID = book.StudentID;
                    book.StudentID = 0;
                    _db.SaveChanges();

                    var student = from item in _db.Students
                                  where item.ID.Equals(stuID)
                                  select item;

                    Admin_Tools.LogControls.createLog(User.Identity.Name, "Book Unassigned", book.ID + " was removed from " + student.First().FirstName +
                        " " + student.First().LastName + "'s account");


                    try
                    {
                        if (TempData["stuID"].ToString() != null)
                        {
                            return RedirectToAction("StudentsBooks", "Student", new { id = studentsID });
                        }
                    }
                    catch
                    {
                        return RedirectToAction("Index");
                    }


                    return RedirectToAction("Index");

                }
                else
                {
                    return View("Error");
                }
            }
            catch
            {
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult DeleteBook(string ID)
        {
            if(ID != null)
            {
                string barcode = ID;

                ID = ID.Substring(5, (ID.Length - 5));
                int IDIn = Convert.ToInt32(ID);

                var book = _db.Books.Find(IDIn);

                DeleteBookViewModel delBook = new DeleteBookViewModel();

                if(book.StudentID != 0)
                {
                    var student = from item in _db.Students
                                  where item.ID.Equals(book.StudentID)
                                  select item;

                    delBook.StudentName = student.First().FirstName + " " + student.First().LastName;
                }
                else
                {
                    delBook.StudentName = "This book is not assigned";
                }

                if(book.BookDesID != 0)
                {
                    var bookName = from item in _db.BookDess
                                   where item.ID.Equals(book.BookDesID)
                                   select item;

                    delBook.BookName = bookName.First().Title;
                }
                else
                {
                    delBook.BookName = "ERROR!!!";
                }
                               

                delBook.ID = book.ID.ToString();           
                delBook.BarCode = barcode;

                return View(delBook);
            }
            else
            {
                return View("Error");
            }

            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteBook(DeleteBookViewModel model)
        {
            if(ModelState.IsValid)
            {
                string ID = model.ID;
                ID = ID.Substring(5, (ID.Length - 5));
                int IDIn = Convert.ToInt32(ID);
                string bookName = "";
                string stuName = "";

                Book delBook = _db.Books.Find(IDIn);
                BookDes book = new BookDes();
                Student stu = new Student();
                if(delBook.BookDesID != 0)
                {
                    book = _db.BookDess.Find(delBook.BookDesID);
                    bookName = book.Title;
                }
                else
                {
                    bookName = "ERROR!!!";
                }
                if(delBook.StudentID != 0)
                {
                    stu = _db.Students.Find(delBook.StudentID);
                    stuName = stu.FirstName + " " + stu.LastName;
                }
                  
                _db.Books.Remove(delBook);
                _db.SaveChanges();

                if(delBook.StudentID == 0)
                {
                    Admin_Tools.LogControls.createLog(User.Identity.Name, "Book Deleted -Individual", bookName + " was deleted from the system" +
                            "the ID for this book is " + delBook.ID + " and was unassigned");
                }
                else
                {
                    Admin_Tools.LogControls.createLog(User.Identity.Name, "Book Deleted -Individual", book.Title + " was deleted from the system " +
                            "the ID for this book is " + delBook.ID + " and was assigned too " + stuName);
                }
                

                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult PrintLabels(double? ID = null)
        {
            if (ID != 0 && ID != null)
            {
                string idIn = ID.ToString();
                idIn = idIn.Substring(5, (idIn.Length - 5));
                int id = Convert.ToInt32(idIn);

                var books = from item in _db.Books
                            where item.ID.Equals(id)
                            select item;

                List<BookDes> bookDes = new List<BookDes>();

                if (books != null)
                {
                    int bookID = books.FirstOrDefault().BookDesID;

                    bookDes = (from item in _db.BookDess
                               where item.ID.Equals(bookID)
                               select item).ToList();
                }

                

                MemoryStream workStream = new MemoryStream();
                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, workStream);
                writer.CloseStream = false;

                document.Open();

                PdfContentByte cb = writer.DirectContent;


                // we tell the ContentByte we're ready to draw text
                cb.BeginText();

                BaseFont f_cn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb.SetFontAndSize(f_cn, 12);

                // we draw some text on a certain position
                cb.SetTextMatrix(220, 820);
                cb.ShowText(bookDes.FirstOrDefault().Title.ToString() + " - " + bookDes.FirstOrDefault().Subject.ToString() + " - " + bookDes.FirstOrDefault().Author.ToString());


                // we tell the contentByte, we've finished drawing text
                cb.EndText();

                Random rnd = new Random();
                int num = rnd.Next(0, 9);

                BarcodeSettings bar = new BarcodeSettings();
                bar.Data = "70680" + id + num.ToString();
                bar.Type = BarCodeType.EAN13;
                BarCodeGenerator bargen = new BarCodeGenerator(bar);

                System.Drawing.Image img = bargen.GenerateImage();

                BaseColor colour = new BaseColor(255, 0, 0);
                Bitmap bmp = img as Bitmap;
                string heading = "St. Conleths C.C. Book Scheme";
                PointF headingLocation = new PointF(15f, 0f);
                Bitmap cropBmp = bmp.Clone(new System.Drawing.Rectangle(0, 20, img.Width, img.Height - 20), bmp.PixelFormat);

                using (Graphics graphics = Graphics.FromImage(cropBmp))
                {
                    using (System.Drawing.Font arial = new System.Drawing.Font("Arial", 10))
                    {
                        graphics.DrawString(heading, arial, Brushes.Black, headingLocation);
                    }
                }

                iTextSharp.text.Image img1 = iTextSharp.text.Image.GetInstance(cropBmp, colour);
                img1.ScalePercent(80f);
                img1.SpacingAfter = 2f;

                int[] xLine = { 10, 210, 400 };                
                int yLine = 730;

                for (int z = 0; z < xLine.Length; z++)
                {                  
                    img1.SetAbsolutePosition(xLine[z], yLine);
                    document.Add(img1);
                }              

                document.Close();

                byte[] byteInfo = workStream.ToArray();
                workStream.Write(byteInfo, 0, byteInfo.Length);
                workStream.Position = 0;

                Response.AppendHeader("Content-Disposition", "inline;test.pdf");
                return new FileStreamResult(workStream, "application/pdf");

            }
            else
            {
                return View("Error");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}