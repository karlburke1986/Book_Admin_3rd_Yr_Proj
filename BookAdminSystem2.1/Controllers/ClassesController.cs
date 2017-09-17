using StConlethsBookSystem_v2._1.Models;
using StConlethsBookSystem_v2._1.Models.ClassesModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StConlethsBookSystem_v2._1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClassesController : Controller
    {
        BooksSystemDB _db = new BooksSystemDB();
        // GET: Classes
        public ActionResult Index()
        {
            try
            {
                List<Classes> classes = _db.classes.ToList();
                return View(classes);
            }

            catch
            {
                return View("Error"); 
            }
        }

        public ActionResult CreateClass()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateClass(Classes model)
        {
            List<string> errorList = new List<string>();
            if (model.className == null || model.className.Length == 0)
            {
                errorList.Add("Please enter a value");
            }
            else if (model.className.Length == 1)
            {
                //Do nothing 
            }
            else
            {
                errorList.Add("Maximum length is 1 character");
            }

            try
            {
                if (ModelState.IsValid && errorList.Count == 0)
                {
                    model.className = model.className.ToUpper();
                    _db.classes.Add(model);
                    _db.SaveChanges();

                    Admin_Tools.LogControls.createLog(User.Identity.Name, "Create Class", "Created new class " + model.className);
                    return RedirectToAction("index");

                }
            }
            catch
            {
                Admin_Tools.LogControls.createLog(User.Identity.Name, "Create Class", "Failed to create new class ");
                return View("Error");
            }

            if (errorList.Count > 0)
            {
                for (int i = 0; i < errorList.Count; i++)
                {
                    ModelState.AddModelError("Validation Error", errorList[i]);
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult DeleteClass(string ID = null)
        {
            try
            {
                if(ID != null)
                {
                    int IDIn = Int32.Parse(ID);

                    var classIn = _db.classes.Find(IDIn);

                    RemoveClassModel rcm = new RemoveClassModel();

                    rcm.ID = classIn.ID;
                    rcm.ClassName = classIn.className;

                    return View(rcm);
                }

                else
                {
                    return View("Error");
                }
            }

            catch {
                return View("Error");
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteClass(RemoveClassModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Classes c1 = _db.classes.Find(model.ID);

                    Admin_Tools.LogControls.createLog(User.Identity.Name, "Delete Class", "Deleted class " + c1.className);

                    _db.classes.Remove(c1);
                    _db.SaveChanges();

                    return RedirectToAction("index");
                }
                else
                {
                    return View();
                }
            }

            catch(Exception ex)
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