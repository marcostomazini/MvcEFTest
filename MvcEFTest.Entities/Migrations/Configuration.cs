using System.Collections.Generic;

namespace MvcEFTest.Entities.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MvcEFTest.Entities.MvcEFTestContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MvcEFTest.Entities.MvcEFTestContext context)
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
                new User { Income = 1.0M, Name = "Foo" },
                new User { Income = 2.0M, Name = "Bar" }
            };

            phones[0].User = users[0];
            phones[1].User = users[0];
            phones[2].User = users[1];

            context.Users.AddOrUpdate(u => u.Name, users.ToArray());
            context.Phones.AddOrUpdate(u => u.Name, phones.ToArray());
        }
    }
}
