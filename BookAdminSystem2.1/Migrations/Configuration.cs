namespace StConlethsBookSystem_v2._1.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<StConlethsBookSystem_v2._1.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(StConlethsBookSystem_v2._1.Models.ApplicationDbContext context)
        {
            Admin_Tools.UserControls.SeedMethod(context);
        }
    }
}
