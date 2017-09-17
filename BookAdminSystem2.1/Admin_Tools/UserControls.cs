using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StConlethsBookSystem_v2._1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StConlethsBookSystem_v2._1.Admin_Tools
{
    public class UserControls
    {
        public static void SeedMethod(StConlethsBookSystem_v2._1.Models.ApplicationDbContext context)
        {
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Teacher"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Teacher" };

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "karl@gmail.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "karl@gmail.com", Email = "karl@gmail.com" };

                manager.Create(user, "MyNewPassword_10");
                manager.AddToRole(user.Id, "Admin");
            }

        }


        public static bool CreateUser(string email, string password, string role)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            bool option = false;


            var newUser = new ApplicationUser();
            newUser.UserName = email;
            newUser.Email = email;

            string roleIn = role;

            var createdUser = UserManager.Create(newUser, password);

            if (createdUser.Succeeded)
            {
                var roleUser = UserManager.AddToRole(newUser.Id, role);

                if (roleUser.Succeeded)
                {
                    option = true;
                }
            }

            else
            {
                option = false;
            }

            return option;
        }

        public static void UpdateEmail(string id, string email)
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var user = context.Users.Single(u => u.Id == id);
            user.Email = email;
            context.SaveChanges();
        }

        public static bool AdminPasswordReset(string id, string password)
        {
            bool option = false;

            UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());

            userManager.RemovePassword(id);

            var add = userManager.AddPassword(id, password);

            if (add.Succeeded)
            {
                option = true;
            }
            else
            {
                option = false;
            }

            return option;
        }

        public static bool RemoveUser(string id, string role)
        {
            bool option = false;

            UserManager<ApplicationUser> userManager;
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var user = userManager.FindById(id);

            var removeRole = userManager.RemoveFromRole(id, role);
            var removeUser = userManager.Delete(user);

            if (removeRole.Succeeded && removeUser.Succeeded)
            {
                option = true;
            }
            else
            {
                option = false;
            }

            return option;
        }
    }
}