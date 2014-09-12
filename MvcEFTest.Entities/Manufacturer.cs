using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcEFTest.Entities
{
    public class Manufacturer
    {
        private ICollection<Phone> _phones;

        public Manufacturer()
        {
            _phones = new HashSet<Phone>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DbGeography HeadquartersLocation { get; set; }

        public virtual ICollection<Phone> Phones
        {
            get { return _phones; }
            set { _phones = value; }
        }
    }
}
