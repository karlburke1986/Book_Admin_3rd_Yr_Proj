using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StConlethsBookSystem_v2._1.Models;
using StConlethsBookSystem_v2._1.Models.AdminModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StConlethsBookSystem_v2._1.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        [Authorize(Roles = "Admin")]
        public ActionResult Home()
        {
            return View();
        }
        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var allUsers = context.Users.ToList();            

            List<ViewUsersModel> users = new List<ViewUsersModel>();

            UserManager<ApplicationUser> userManager;
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            foreach (var item in allUsers)
            {
                ViewUsersModel temp = new ViewUsersModel();

                var role = userManager.GetRoles(item.Id);

                temp.Email = item.Email;
                temp.ID = item.Id;
                temp.Role = role.FirstOrDefault().ToString();           
                               
                users.Add(temp);
            }

            return View(users);
        }

        [Authorize(Roles = "Admin")]
        //Get Register Screen
        public ActionResult Register()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        //Post Result of the Register Screen input
        [HttpPost]
        public ActionResult Register(Register model)
        {
            bool result = false;

            try {
                if (ModelState.IsValid && Admin_Tools.AdminTools.passwordValid(model.Password).Count == 0)
                {
                    result = Admin_Tools.UserControls.CreateUser(model.Email, model.Password, model.Role);

                    if (result != false)
                    {
                        Admin_Tools.LogControls.createLog(User.Identity.Name, "Register New User",
                            ("User : " + model.Email + "was created successfully with a " + model.Role + " role"));
                    }

                    else if (result == false)
                    {
                        Admin_Tools.LogControls.createLog(User.Identity.Name, "Register New User",
                            ("User : " + model.Email + "was unsuccessfully attempted to be created with a " + model.Role + " role"));
                    }

                    return RedirectToAction("Index");
                }

                else if (!ModelState.IsValid)
                {
                    Admin_Tools.LogControls.createLog(User.Identity.Name, "Register New User",
                        ("User : " + model.Email + "was unsuccessfully attempted to be created with a " + model.Role + " role"));
                }


                foreach(var item in Admin_Tools.AdminTools.passwordValid(model.Password))
                {
                    ModelState.AddModelError("Validation Error", item);
                }

                return View();
            }

            catch(Exception ex)
            {
                return View("Error");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult ResetPassword(string id = null)
        {
            try {
                var user = context.Users.Find(id);

                if (user != null && user.Email != User.Identity.Name)
                {
                    ResetPassword resetUser = new ResetPassword();

                    resetUser.ID = id;

                    TempData["IDIn"] = id;

                    return View(resetUser);
                }

                return View();
            }

            catch(Exception ex)
            {
                return View("Error");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPassword model) 
        {
            bool result = false;

            try {
                if (ModelState.IsValid && Admin_Tools.AdminTools.passwordValid(model.Password).Count == 0)
                {
                    result = Admin_Tools.UserControls.AdminPasswordReset(model.ID, model.Password);

                    var user = context.Users.Find(model.ID);

                    if (result == true)
                    {
                        Admin_Tools.LogControls.createLog(User.Identity.Name, "Password Reset", User.Identity.Name + " successfully reset "
                            + user.Email + " password");

                        return RedirectToAction("Index");
                    }

                    else
                    {
                        Admin_Tools.LogControls.createLog(User.Identity.Name, "Password Reset", User.Identity.Name + " unsuccessfully reset "
                            + user.Email + " password");

                        return View();
                    }
                }

                foreach (var item in Admin_Tools.AdminTools.passwordValid(model.Password))
                {
                    ModelState.AddModelError("Validation Error", item);
                }

                return View();
            }

            catch(Exception ex)
            {
                return View("Error");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult RemoveUser(string id = null)
        {
            try {

                var user = context.Users.Find(id);

                UserManager<ApplicationUser> userManager;
                userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

                RemoveUserViewModel remove = new RemoveUserViewModel();

                var userRole = userManager.GetRoles(user.Id);

                remove.ID = user.Id;
                remove.Email = user.Email;
                remove.Role = userRole.FirstOrDefault().ToString();
                TempData["makeSureIdIsSame"] = id;
                return View(remove);
            }

            catch(Exception ex)
            {
                return View("Error");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult RemoveUser(RemoveUserViewModel model)
        {
            if (TempData["makeSureIdIsSame"].ToString() == model.ID)
            {
                bool result = false;

                if (ModelState.IsValid)
                {
                    UserManager<ApplicationUser> userManager;
                    userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

                    var toBeRomoved = context.Users.Find(model.ID);

                    model.Role = toBeRomoved.Roles.ToString();
                    model.Email = toBeRomoved.Email.ToString();

                    result = Admin_Tools.UserControls.RemoveUser(model.ID, model.Role);

                    if (result != false)
                    {
                        Admin_Tools.LogControls.createLog(User.Identity.Name, "Delete User",
                            ("User : " + model.Email + "was deleted successfully with a " + model.Role + " role"));
                    }

                    else if (result == false)
                    {
                        Admin_Tools.LogControls.createLog(User.Identity.Name, "Delete User",
                            ("User : " + model.Email + "was unsuccessfully attempted to be deleted"));
                    }

                    return RedirectToAction("Index");
                }

                else if (!ModelState.IsValid)
                {
                    Admin_Tools.LogControls.createLog(User.Identity.Name, "Delete User",
                            ("User : " + model.Email + "was unsuccessfully attempted to be deleted"));
                }

                return View();
            }
            else
            {
                return View();
            }
        }
        
        public ActionResult ChangeUserPassword()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeUserPassword(ChangeUserPassword model)
        {
            bool result = false;

            if (ModelState.IsValid)
            {
                UserManager<ApplicationUser> userManager;
                userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

                var user = userManager.Find(User.Identity.Name, model.oldPassword);

                if (user != null)
                {
                    if (ModelState.IsValid && Admin_Tools.AdminTools.passwordValid(model.newPassword).Count == 0)
                    {
                        result = Admin_Tools.UserControls.AdminPasswordReset(user.Id, model.newPassword);

                        if (result == true)
                        {
                            Admin_Tools.LogControls.createLog(User.Identity.Name, "Change Password", User.Identity.Name + " successfully changed there password");

                            return RedirectToAction("Index");
                        }

                        else
                        {
                            Admin_Tools.LogControls.createLog(User.Identity.Name, "Change Password", User.Identity.Name + " unsuccessfully changed there password");

                            return View();
                        }
                    }

                    foreach (var item in Admin_Tools.AdminTools.passwordValid(model.newPassword))
                    {
                        ModelState.AddModelError("Validation Error", item);
                    }

                    return View();
                }
                else
                {
                    List<string> errorList = new List<string>();

                    errorList.Add("Old Password: Old Password is incorrect");

                    ModelState.AddModelError("Validation Error", errorList[0]);
                }
            }


            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}