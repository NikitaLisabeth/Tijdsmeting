namespace Tijdsmeting.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Tijdsmeting.Model;

    internal sealed class Configuration : DbMigrationsConfiguration<Tijdsmeting.DAL.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Tijdsmeting.DAL.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Runners.AddOrUpdate(
                r => r.Name,
                    new Runner { Name = "Skiridov", Firstname = "Sergey" },
                    new Runner { Name = "Lisabeth", Firstname = "Nikita" },
                    new Runner { Name = "Cruise", Firstname = "Tom" },
                    new Runner { Name = "Hanks", Firstname = "Tom" },
                    new Runner { Name = "Everdeen", Firstname = "Katniss" },
                    new Runner { Name = "Jolie", Firstname = "Angelina" },
                    new Runner { Name = "Barack", Firstname = "Obama" },
                    new Runner { Name = "Lemmens", Firstname = "Ann" }
            );

            context.SaveChanges();
        }
    }
}
