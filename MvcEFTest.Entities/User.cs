using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcEFTest.Entities
{
    public class User : EntityBase
    {
        [Required]
        public string Name { get; set; }

        public decimal Income { get; set; }

        public virtual ICollection<Phone> Phones { get; set; } 
    }
}
