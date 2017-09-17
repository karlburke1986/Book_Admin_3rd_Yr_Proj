using StConlethsBookSystem_v2._1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace StConlethsBookSystem_v2._1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            runSeed();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }

        private void runSeed()
        {
            Database.SetInitializer<StConlethsBookSystem_v2._1.Models.ApplicationDbContext>(
            new MigrateDatabaseToLatestVersion<ApplicationDbContext,
            StConlethsBookSystem_v2._1.Migrations.Configuration>());

        }
    }
}