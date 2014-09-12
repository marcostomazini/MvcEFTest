using System.Collections.Generic;

namespace MvcEFTest.Entities.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<MvcEFTestContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MvcEFTestContext context)
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

            var phones = new List<Phone>
            {
                new Phone { AndroidVersion = "4.0", Name = "Nexus 5" },
                new Phone { AndroidVersion = "5.0", Name = "Nexus X" },
                new Phone { AndroidVersion = "4.0", Name = "Samsung S4" }
            };

            var users = new List<User>
            {
                new User { Income = 1.0M, Name = "Rex" },
                new User { Income = 2.0M, Name = "Billy" }
            };

            var manufacturers = new List<Manufacturer>
            {
                new Manufacturer { Name = "Samsung" },
                new Manufacturer { Name = "Google" }
            };

            phones[0].Manufacturer = manufacturers[1];
            phones[1].Manufacturer = manufacturers[1];
            phones[2].Manufacturer = manufacturers[0];

            phones[0].Users.Add(users[0]);
            phones[0].Users.Add(users[1]);

            phones[1].Users.Add(users[0]);

            phones[2].Users.Add(users[1]);

            context.Users.AddOrUpdate(u => u.Name, users.ToArray());
            context.Manufacturers.AddOrUpdate(u => u.Name, manufacturers.ToArray());
            context.Phones.AddOrUpdate(u => u.Name, phones.ToArray());
        }
    }
}
