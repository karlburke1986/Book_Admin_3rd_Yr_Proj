using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList;
using Spire.Barcode;
using iTextSharp;
using iTextSharp.text;
using StConlethsBookSystem_v2._1.Models;
using StConlethsBookSystem_v2._1.Models.BookDesModels;
using StConlethsBookSystem_v2._1.Models.BookModels;
using System.IO;
using iTextSharp.text.pdf;
using System.Drawing;
using System.Drawing.Imaging;

namespace StConlethsBookSystem_v2._1.Controllers
{
    public class BookDesController : Controller
    {
        private BookSystemDBMain db = new BookSystemDBMain();
        BooksSystemDB _db = new BooksSystemDB();

        // GET: BookDes
        public ActionResult Index(int page = 1, string bookName = null, string subject = null, string showall = null)
        {
            try
            {
                List<Subject> subjects = _db.Subjects.ToList();

                var bookDes = from item in db.BookDess
                                        orderby item.Title
                                        select item;

                ViewBag.Subjects = subjects;
                ViewBag.Books = bookDes;

                if (showall == null)
                {
                    ViewBag.BookName = bookName;
                    ViewBag.Subject = subject;
                }

                else
                {
                    ViewBag.BookName = null;
                    ViewBag.Subject = null;
                    bookName = null;
                    subject = null;
                    page = 1;
                }

                bookDes = from item in bookDes
                            where (String.IsNullOrEmpty(bookName) || item.Title.StartsWith(bookName)) &&
                            (String.IsNullOrEmpty(subject) || item.Subject.StartsWith(subject))
                            orderby item.Title
                            select item;

                List<BookDesIndexViewModel> models = new List<BookDesIndexViewModel>();
                
                if(bookDes != null)
                {                   
                    foreach(var item in bookDes)
                    {
                        BookDesIndexViewModel temp = new BookDesIndexViewModel();

                        temp.ID = item.ID;
                        temp.Title = item.Title;
                        temp.Subject = item.Subject;
                        temp.Edition = item.Edition;
                        temp.Author = item.Author;

                        models.Add(temp);
                    }
                } 

                if(models != null)
                {
                    foreach(var item in models)
                    {
                        var books = from items in db.Books
                                    select items;

                        int bookTotal = (from item1 in books
                                         where item1.BookDesID.Equals(item.ID)
                                         select item1).Count();

                        int totalFree = (from item2 in books
                                         where (item2.BookDesID.Equals(item.ID) && item2.letOut.Equals(false))
                                         select item2).Count();
                        int totalRented = (from item2 in books
                                           where (item2.BookDesID.Equals(item.ID) && item2.letOut.Equals(true))
                                           select item2).Count();

                        item.inStock = bookTotal;
                        item.unassigned = totalFree;
                        item.rented = totalRented;
                    }

                    
                }


                int pagesize = 40;

                return View(models.ToPagedList(page, pagesize));
            }

            catch
            {
                return View("Error");
            }
        }
                
        // GET: BookDes/Create
        public ActionResult Create()
        {
            try
            {
                List<Subject> subjects = _db.Subjects.ToList();

                ViewBag.Subjects = subjects;

                return View();
            }
            catch
            {
                return View("Error");
            }
        }

        // POST: BookDes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddBookViewModel model)
        {
            List<string> errorList = new List<string>();

            try
            {
                if (model.Title == null || model.Title.Length < 1)
                {
                    errorList.Add("Title Error: Book Title must be provided");
                }

                if (model.Subject == null || model.Subject.Length < 1)
                {
                    errorList.Add("Subject Error: Book Subject must be provided");
                }

                if (ModelState.IsValid && errorList.Count == 0)
                {

                    BookDes book = new BookDes();

                    book.Author = model.Author;
                    book.Edition = model.Edition;
                    book.ID = model.ID;
                    book.Subject = model.Subject;
                    book.Title = model.Title;
                    db.BookDess.Add(book);
                    db.SaveChanges();



                    Admin_Tools.LogControls.createLog(User.Identity.Name, "New Book Created", book.Title + " was added to the system");

                    var lastBook = (from item in db.Books
                                    orderby item.ID descending
                                    select item).FirstOrDefault();

                    BookDes tmepBook = (from item in db.BookDess
                                        where item.Title.Equals(model.Title)
                                        select item).First();

                    string id = "";

                    if (tmepBook != null && model.Quantity != 0)
                    {
                        for (int i = 1; i <= model.Quantity; i++)
                        {
                            Book temp = new Book();

                            temp.ID = lastBook.ID + i;
                            temp.letOut = false;
                            temp.StudentID = 0;
                            temp.BookDesID = tmepBook.ID;
                            db.Books.Add(temp);
                            db.SaveChanges();
                            id = temp.ID.ToString();
                        }

                        return View("Print", book);
                    }   
                    else
                    {
                        return RedirectToAction("Index");
                    }                

                    
                }
                else if (!ModelState.IsValid)
                {
                    return View("Error");
                }
                else if (errorList.Count > 0) 
                {
                    for (int i = 0; i < errorList.Count; i++)
                    {
                        ModelState.AddModelError("Validation Error", errorList[i]);
                    }
                }

                List<Subject> subjects = _db.Subjects.ToList();

                ViewBag.Subjects = subjects;

                return View();
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: BookDes/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                BookDes bookDes = db.BookDess.Find(id);

                List<Subject> subjects = _db.Subjects.ToList();

                ViewBag.Subjects = subjects;
                if (bookDes == null)
                {
                    return HttpNotFound();
                }
                return View(bookDes);
            }
            catch
            {
                return View("Error");
            }
        }

        // POST: BookDes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Author,Edition,Subject")] BookDes bookDes)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(bookDes).State = EntityState.Modified;
                    db.SaveChanges();

                    Admin_Tools.LogControls.createLog(User.Identity.Name, "Book Edited", bookDes.Title + " was edited");

                    return RedirectToAction("Index");
                }
                return View(bookDes);
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: BookDes/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                BookDes bookDes = db.BookDess.Find(id);
                if (bookDes == null)
                {
                    return HttpNotFound();
                }
                return View(bookDes);
            }
            catch
            {
                return View("Error");
            }
        }

        // POST: BookDes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                BookDes bookDes = db.BookDess.Find(id);
                if(bookDes != null)
                {
                    var books = from item in db.Books
                                where item.BookDesID.Equals(id)
                                orderby item.ID
                                select item;
                    var students = from item in db.Students
                                   select item;

                    foreach(var item in books.ToList())
                    {
                        Book b1 = db.Books.Find(item.ID);
                        db.Books.Remove(b1);
                        db.SaveChanges();

                        if(b1.letOut == false)
                        {
                            Admin_Tools.LogControls.createLog(User.Identity.Name, "Book Deleted -Individual", bookDes.Title + " was deleted from the system" +
                            "the ID for this book is " + b1.ID + " and was unassigned");
                        }
                        if(b1.letOut == true)
                        {
                            var name = from stu in students
                                       where stu.ID.Equals(item.StudentID)
                                       select stu;

                            string studentName = name.First().FirstName + " " + name.First().LastName;

                            Admin_Tools.LogControls.createLog(User.Identity.Name, "Book Deleted -Individual", bookDes.Title + " was deleted from the system" +
                            "the ID for this book is " + b1.ID + " and was assigned too " + studentName);
                        }

                    }
                }
                db.BookDess.Remove(bookDes);
                db.SaveChanges();

                Admin_Tools.LogControls.createLog(User.Identity.Name, "Book Deleted -Bulk", bookDes.Title + " was deleted from the system");

                return RedirectToAction("Index");
            }

            catch
            {
                return View("Error");
            }
        }

        
        public ActionResult Print(BookDes model)
        {
            return View(model);
        }
        
        public ActionResult PrintLabels(int ID, int? Quantity)
        {
            
            if (ID != 0)
            {
                List<Book> books = (from item in db.Books
                                    where item.BookDesID.Equals(ID)
                                    orderby item.ID
                                    select item).ToList();
                if(Quantity != null && Quantity != 0)
                {
                    int quantity = Convert.ToInt32(Quantity);

                    books = (from item in books
                             orderby item.ID descending
                             select item).Take(quantity).ToList();
                    books = (from item in books
                             orderby item.ID ascending
                             select item).ToList();
                }

                int bookID = books.FirstOrDefault().BookDesID;

                var bookDes = (from item in db.BookDess
                              where item.ID.Equals(bookID)
                              select item).ToList();

                MemoryStream workStream = new MemoryStream();
                Document document = new Document();

                //PdfWriter.GetInstance(document, workStream).CloseStream = false;
                PdfWriter writer = PdfWriter.GetInstance(document, workStream);
                writer.CloseStream = false;

                document.Open();               


                // step 4: we grab the ContentByte and do some stuff with it
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


                int counter = 0;
                if (books != null)
                {

                    for (int i = 0; i < books.Count; i++)
                    {
                        Random rnd = new Random();
                        int num = rnd.Next(0, 9);

                        BarcodeSettings bar = new BarcodeSettings();
                        bar.Data = "70680" + books[i].ID + num.ToString();
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

                        int[] xLine = { 10,210, 400 };
                        int[] yLine = { 730, 620, 500, 390, 280, 170, 60 };

                        for (int z = 0; z < xLine.Length; z++)
                        {
                            int yPos = 0;

                            if (i != 0)
                            {
                                yPos = i % 7;
                            }

                            img1.SetAbsolutePosition(xLine[z], yLine[yPos]);
                            document.Add(img1);
                        }

                        counter++;

                        if (counter == 7)
                        {
                            document.NewPage();
                            counter = 0;

                            cb.BeginText();
                                                        
                            cb.SetFontAndSize(f_cn, 12);

                            // we draw some text on a certain position
                            cb.SetTextMatrix(260, 820);
                            cb.ShowText(bookDes.FirstOrDefault().Title.ToString() + " - " + bookDes.FirstOrDefault().Subject.ToString() + " - " + bookDes.FirstOrDefault().Author.ToString());


                            // we tell the contentByte, we've finished drawing text
                            cb.EndText();
                        }
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
            else
            {
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult AddAdditionalBooks(int ID)
        {
            if(ID != 0)
            {
                BookDes book = db.BookDess.Find(ID);

                AddAdditionalBooks AddBook = new AddAdditionalBooks();
                AddBook.ID = book.ID;
                AddBook.Title = book.Title;

                return View(AddBook);
            }           

            return View("Error");
        }

        [HttpPost]
        public ActionResult AddAdditionalBooks(AddAdditionalBooks model)
        {
            if(ModelState.IsValid)
            {
                var lastBook = (from item in db.Books
                                orderby item.ID descending
                                select item).FirstOrDefault();

                BookDes tmepBook = (from item in db.BookDess
                                    where item.ID.Equals(model.ID)
                                    select item).First();

                string id = "";

                int? quantity = model.Quantity;

                ViewBag.Quantity = quantity;

                if (tmepBook != null && model.Quantity != 0)
                {
                    for (int i = 1; i <= model.Quantity; i++)
                    {
                        Book temp = new Book();

                        temp.ID = lastBook.ID + i;
                        temp.letOut = false;
                        temp.StudentID = 0;
                        temp.BookDesID = tmepBook.ID;
                        db.Books.Add(temp);
                        db.SaveChanges();
                        id = temp.ID.ToString();
                    }

                    Admin_Tools.LogControls.createLog(User.Identity.Name, "Added Additional Books", model.Quantity + " was added to the system");

                    return View("Print", tmepBook);
                }

                return View(model);
            }
            return View("Error");
        }

        public ActionResult StockReport()
        {
            List<BookDes> booksDes = db.BookDess.ToList();
            List<Book> books = db.Books.ToList();

            MemoryStream workStream = new MemoryStream();
            Document document = new Document();
            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            PdfPTable table = new PdfPTable(6);

            table.WidthPercentage = 98.0f;

            PdfPCell cell = new PdfPCell(new Phrase("Title"));
            cell.Colspan = 2;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);           

            cell = new PdfPCell(new Phrase("Subject"));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Rented"));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Free"));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Total"));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);            

            foreach(var item in booksDes)
            {
                string rented = "";
                string free = "";
                string total = "";

                var rentedIn = from item1 in books
                               where item1.BookDesID.Equals(item.ID) && item1.letOut.Equals(true)
                               select item1;
                var freeIn = from item1 in books
                             where item1.BookDesID.Equals(item.ID) && item1.letOut.Equals(false)
                             select item1;
                var totalIn = from item1 in books
                              where item1.BookDesID.Equals(item.ID)
                              select item1;

                rented = rentedIn.Count().ToString();
                free = freeIn.Count().ToString();
                total = totalIn.Count().ToString();

                cell = new PdfPCell(new Phrase(item.Title));
                cell.Colspan = 2;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(item.Subject));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(rented));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(free));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(total));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);               

            }
            document.Open();

            document.Add(table);

            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            Response.AppendHeader("Content-Disposition", "inline;test.pdf");
            return new FileStreamResult(workStream, "application/pdf");            
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
