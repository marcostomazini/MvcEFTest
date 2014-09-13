using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace MvcEFTest.Entities
{
    public class MvcEFDropCreateAlwaysInitializer : DropCreateDatabaseAlways<MvcEFTestContext>
    {
        protected override void Seed(MvcEFTestContext context)
        {
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

            context.Users.AddOrUpdate(u => u.Id, users.ToArray());
            context.Manufacturers.AddOrUpdate(u => u.Id, manufacturers.ToArray());
            context.Phones.AddOrUpdate(u => u.Id, phones.ToArray());
        }
    }
}
