using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcEFTest.Entities
{
    public class Phone
    {
        private ICollection<User> _users;

        public Phone()
        {
            _users = new HashSet<User>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string AndroidVersion { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }

        public virtual ICollection<User> Users
        {
            get { return _users; }
            set { _users = value; }
        }
    }
}
