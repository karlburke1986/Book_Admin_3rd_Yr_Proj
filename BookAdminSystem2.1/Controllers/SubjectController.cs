using StConlethsBookSystem_v2._1.Models;
using StConlethsBookSystem_v2._1.Models.SubjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StConlethsBookSystem_v2._1.Controllers
{
    [Authorize(Roles ="Admin")]
    public class SubjectController : Controller
    {
        BooksSystemDB _db = new BooksSystemDB();
        // GET: Subject
        public ActionResult Index()
        {
            var subjects = _db.Subjects.ToList();

            return View(subjects);
        }

        public ActionResult AddSubject()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSubject(AddSubjectViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    Subject subject = new Subject();

                    subject.ID = model.ID;
                    subject.Name = model.Name;
                    subject.Name = subject.Name.First().ToString().ToUpper() + subject.Name.Substring(1);
                    _db.Subjects.Add(subject);
                    _db.SaveChanges();

                    Admin_Tools.LogControls.createLog(User.Identity.Name, "Subject Added", subject.Name + " was added to the DB");

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

        
        public ActionResult DeleteSubject(int ID)
        {
            try
            {
                var subject = _db.Subjects.Find(ID);

                DeleteSubjectViewModel delSubject = new DeleteSubjectViewModel();

                delSubject.ID = subject.ID;
                delSubject.Name = subject.Name;

                return View(delSubject);
            }

            catch
            {
                return View("Error");
            }              
                        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSubject(DeleteSubjectViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    Subject subject = _db.Subjects.Find(model.ID);

                    _db.Subjects.Remove(subject);
                    _db.SaveChanges();

                    Admin_Tools.LogControls.createLog(User.Identity.Name, "Subject Removed", subject.Name + " was removed from the DB");

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
    }
}