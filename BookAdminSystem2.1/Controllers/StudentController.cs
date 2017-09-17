using StConlethsBookSystem_v2._1.Models;
using StConlethsBookSystem_v2._1.Models.StudentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using StConlethsBookSystem_v2._1.Models.BookModels;
using System.Data.Entity;
using System.Web;
using System.IO;

namespace StConlethsBookSystem_v2._1.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        BookSystemDBMain _db = new BookSystemDBMain();
        BooksSystemDB _db2 = new BooksSystemDB();
        // GET: Student
        public ActionResult Index(int page = 1, string fname = null, string sname = null, int? year = null, string showall = null)
        {
            
            try
            {
                if (showall != "yes")
                {
                    string tempfName = TempData["fname"].ToString();
                    string tempsName = TempData["sname"].ToString();
                    string tempyear = TempData["year"].ToString();

                    if (fname == null && tempfName != null)
                    {
                        fname = tempfName;
                    }
                    if (sname == null && tempsName != null)
                    {
                        sname = tempsName;
                    }
                    if (year == null && tempyear != null)
                    {
                        year = Convert.ToInt32(tempyear);
                    }
                }
            }
            catch { }

            if(showall == "yes")
            {
                fname = null;
                sname = null;
                year = null;

            }
          

            var students = from item in _db.Students
                           where (String.IsNullOrEmpty(fname) || item.FirstName.StartsWith(fname)) && 
                           (String.IsNullOrEmpty(sname) || item.LastName.StartsWith(sname)) &&
                           (year == null || item.Year == year)
                           orderby item.LastName
                           select item;

            int pageSize = 20;

            TempData["fname"] = fname;
            TempData["sname"] = sname;
            TempData["year"] = year;

            ViewBag.fname = fname;
            ViewBag.sname = sname;
            ViewBag.year = year;

            return View(students.ToPagedList(page, pageSize));
        }

        [Authorize(Roles ="Admin")]
        public ActionResult AddStudent(string type = null)
        {

            TempData["Type"] = type;
            try
            {
                if (type != null)
                {                    
                    List<Classes> allClasses = _db2.classes.ToList();

                    ViewBag.Classes = allClasses;
                    ViewBag.Type = type;                 
                    AddStudentViewModel teacher = new AddStudentViewModel();

                    if (type == "Teacher")
                    {

                        teacher.Class = ViewBag.Type.ToString();
                        teacher.Year = "1";
                    }

                    return View(teacher);
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
        [Authorize(Roles = "Admin")]
        public ActionResult AddStudent(AddStudentViewModel model)
        {
            string type = TempData["Type"].ToString();

            try
            {
                bool resultClass = false;
                bool resultYear = false;
                List<string> errorList = new List<string>();

                if (model.Class != "Teacher")
                {

                    if (model.Class != null)
                    {
                        List<Classes> classList = _db2.classes.ToList();
                        List<string> classListin = new List<string>();

                        for (int i = 0; i < classList.Count; i++)
                        {                           

                            string classIn = classList[i].className.ToString();

                            classListin.Add(classIn);
                        }

                        for (int i = 0; i < classListin.Count(); i++)
                        {
                            if (classListin[i] == model.Class)
                            {
                                resultClass = true;
                            }
                        }

                        if (resultClass != true)
                        {
                            errorList.Add("Class Error: Class name must be selected from the list provided");
                        }
                    }
                    else
                    {
                        errorList.Add("Class Error: Class name must be selected from the list provided");
                    }

                }
                else
                {
                    resultClass = true;
                }

                if (model.Year == null)
                {
                    errorList.Add("Year Error: Year must be from list provided");
                }
                 
                else
                {
                    resultYear = true;
                }

                if (ModelState.IsValid && errorList.Count == 0)
                {

                    Student stu = new Student();

                    string firstL = model.FirstName.Substring(0, 1);
                    firstL = firstL.ToUpper();

                    string rest = model.FirstName.Substring(1);
                    rest = rest.ToLower();
                    model.FirstName = firstL + rest;

                    firstL = "";
                    rest = "";

                    firstL = model.Surname.Substring(0, 1);
                    firstL = firstL.ToUpper();

                    rest = model.Surname.Substring(1);
                    rest = rest.ToLower();
                    model.Surname = firstL + rest;

                    stu.FirstName = model.FirstName;
                    stu.LastName = model.Surname;
                    stu.Class = model.Class;
                    stu.Year = Convert.ToInt32(model.Year);
                    stu.ID = model.ID;

                    _db.Students.Add(stu);
                    _db.SaveChanges();

                    if(model.Class == "Teacher")
                    {
                        Admin_Tools.LogControls.createLog(User.Identity.Name, "Created Teacher", "Created " + model.FirstName +
                        " " + model.Surname);
                    }

                    else
                    {
                        Admin_Tools.LogControls.createLog(User.Identity.Name, "Created Student", "Created " + model.FirstName +
                        " " + model.Surname);
                    }

                    

                    return RedirectToAction("Index");


                }
                else if (!ModelState.IsValid)
                {
                    return View("Error");
                }
                else if (resultClass == false || resultYear == false)
                {
                    for (int i = 0; i < errorList.Count; i++)
                    {

                        ModelState.AddModelError("Validation Error", errorList[i]);
                    }
                }
                ViewBag.Type = type;
                TempData["Type"] = type;
                List<Classes> allClasses = _db2.classes.ToList();

                ViewBag.Classes = allClasses;


                return View();
            }

            catch
            {
                return View("Error");
            }

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string ID = null)
        {
            try
            {
                if (ID != null)
                {
                    int id = Convert.ToInt32(ID);

                    var stu = _db.Students.Find(id);

                    var booksCount = (from item in _db.Books
                                      where item.StudentID.Equals(id)
                                      select item).Count();

                    ViewBag.BookCount = booksCount;

                    DeleteStudentViewModel delStudent = new DeleteStudentViewModel();

                    delStudent.ID = stu.ID.ToString();
                    delStudent.studentName = stu.FirstName + " " + stu.LastName;

                    return View(delStudent);
                }
            }
            catch
            {
                return View("Error");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(DeleteStudentViewModel model)
        {            
            try
            {
                if(ModelState.IsValid && model.ID != null)
                {
                    Student stu = _db.Students.Find(Convert.ToInt32(model.ID));

                    _db.Students.Remove(stu);
                    _db.SaveChanges();

                    if(stu.Class == "Teacher")
                    {
                        Admin_Tools.LogControls.createLog(User.Identity.Name, "Delete Teacher", stu.FirstName + " "
                        + stu.LastName + " was successfully deleted from the system");
                    }
                    else
                    {
                        Admin_Tools.LogControls.createLog(User.Identity.Name, "Delete Student", stu.FirstName + " "
                        + stu.LastName + " was successfully deleted from the system");
                    }

                   var books = from item in _db.Books
                                where item.StudentID.Equals(stu.ID)
                                select item;
                

                    BookSystemDBMain _dbUn = new BookSystemDBMain();
                    foreach (var item in books)
                    {
                        try
                        {


                            item.letOut = false;
                            item.StudentID = 0;
                            _dbUn.Entry(item).State = EntityState.Modified;
                            _dbUn.SaveChanges();

                            Admin_Tools.LogControls.createLog(User.Identity.Name, "Book Unassigned", item.ID + " was removed from " + stu.FirstName +
                            " " + stu.LastName + "'s account");
                        }
                        catch(Exception ex)
                        {
                            Admin_Tools.LogControls.createLog(User.Identity.Name, "Book ERROR", item.ID + " could not be removed from " + stu.FirstName +
                            " " + stu.LastName + "'s account : " + ex.Message);
                        }
                    }

                    return RedirectToAction("Index");
                }
            }

            catch
            {
                return View("Error");
            }

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string ID = null)
        {
            try
            {
                if (ID != null)
                {

                    Student stu = _db.Students.Find(Convert.ToInt32(ID));

                    var ClassesIn = _db2.classes.ToList();

                    List<Classes> allClasses = new List<Classes>();

                    Classes temp = new Classes();

                    temp.className = "Not Selected";

                    allClasses.Add(temp);

                    foreach(var item in ClassesIn)
                    {
                        Classes newTemp = new Classes();
                        newTemp.ID = item.ID;
                        newTemp.className = item.className;
                        allClasses.Add(newTemp);
                    }


                    ViewBag.Classes = allClasses;

                    if(stu.Class == "Teacher")
                    {
                        ViewBag.Type = "Teacher";
                    }

                    return View(stu);
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
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Student model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(model.Class == "Not Selected")
                    {
                        model.Class = "";
                    }

                    var stu = _db.Students.Find(model.ID);
                    stu.FirstName = model.FirstName;
                    stu.LastName = model.LastName;
                    stu.Class = model.Class;
                    stu.Year = model.Year;

                    _db.SaveChanges();

                    if(model.Class == "Teacher")
                    {
                        Admin_Tools.LogControls.createLog(User.Identity.Name, "Edit Teacher", "Edited " + model.FirstName +
                        " " + model.LastName);
                    }

                    else
                    {
                        Admin_Tools.LogControls.createLog(User.Identity.Name, "Edit Student", "Edited " + model.FirstName +
                        " " + model.LastName);
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
        public ActionResult StudentsBooks(int? id)
        {
            if(id != null)
            {
                int ID = Convert.ToInt32(id);

                var stu = _db.Students.Find(ID);

                StudentsBookViewModel student = new StudentsBookViewModel();

                student.ID = stu.ID;
                student.StudentName = stu.FirstName + " " + stu.LastName;
                student.Class = stu.Class;
                student.Year = stu.Year;

                return View(student);
            }

            return View();
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult _Books(string studentID = null, string barcode = null)
        {
            
            string barcodeIn = barcode;
            Student student = new Student();
            List<BookDes> bookDes = _db.BookDess.ToList();        

            if (studentID != null)
            {
                int stuID = Convert.ToInt32(studentID);

                student = _db.Students.Find(stuID);
            }

            if(student != null && barcode != null)
            {
                if (barcode.Length == 13)
                {

                    barcode = barcode.Substring(5, (barcode.Length - 5));
                    barcode = barcode.Remove(barcode.Length - 1);

                    int code = 0;

                    bool result = Int32.TryParse(barcode, out code);

                    Book book = new Book();

                    if (result == true)
                    {
                        book = _db.Books.Find(code);

                        if (book != null)
                        {
                            if (book.letOut == false)
                            {
                                book.letOut = true;
                                book.StudentID = student.ID;
                                ViewBag.Result = "Success";
                                ViewBag.Message = "The book has been successfully added to the students account";
                                Admin_Tools.LogControls.createLog(User.Identity.Name, "Book Added", book.ID + " was added to " + student.FirstName +
                                " " + student.LastName + "'s account");
                                _db.SaveChanges();
                            }
                            else if (book.letOut == true)
                            {
                                ViewBag.Result = "Failure";
                                ViewBag.Message = "Book is already assigned. The books must be unassigned before it can be added to a new students account";
                            }
                        }
                        else
                        {
                            ViewBag.Result = "Failure";
                            ViewBag.Message = "The entered Barcode could not be found on the database";
                        }
                    }
                    else
                    {
                        ViewBag.Result = "Failure";
                        ViewBag.Message = "Only Numbers can be entered in this section";
                    }
                }
                else
                {
                    ViewBag.Result = "Failure";
                    ViewBag.Message = "The entered barcode does not contain enough numbers";
                }                 
            }

            if(student != null)
            {
                var books = from item in _db.Books
                            where item.StudentID.Equals(student.ID)
                            orderby item.ID
                            select item;

                List<BookStudentPartialViewModel> model = new List<BookStudentPartialViewModel>();
                
                foreach(var item in books)
                {
                    BookStudentPartialViewModel temp = new BookStudentPartialViewModel();
                    temp.Barcode = "70680" + item.ID.ToString();
                    temp.ID = item.ID;
                    var details = from item1 in bookDes
                                  where item1.ID.Equals(item.BookDesID)
                                  select item1;
                    temp.Subject = details.First().Subject.ToString();
                    temp.Title = details.First().Title.ToString();

                    model.Add(temp);
                }

                return PartialView("_Books", model);
            }

            return View();
        }

        public ActionResult CSVUpload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CSVUpload(HttpPostedFileBase file, string year)
        {
            string extension = Path.GetExtension(file.FileName);

            if(file != null && file.ContentLength > 0 && extension == ".csv")
            {
                var fileName = Path.GetFileName(file.FileName);

                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);

                file.SaveAs(path);

                StreamReader reader = new StreamReader(path);

                bool secondLine = false;

                while(!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    if(secondLine == true)
                    {
                        Student temp = new Student();

                        temp.FirstName = values[0];
                        temp.LastName = values[1];
                        if(year != null)
                        {
                            temp.Year = Convert.ToInt32(year);

                        }

                        _db.Students.Add(temp);
                        _db.SaveChanges();
                    }
                    else
                    {
                        secondLine = true;
                    }
                }

                Admin_Tools.LogControls.createLog(User.Identity.Name, "CSV Upload", "Students were bulk uploaded");

                return RedirectToAction("Index");
            }

            return View("Error"); 
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