using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcEFTest.Entities
{
    public class Phone : EntityBase
    {
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; }

        public string AndroidVersion { get; set; }

        public virtual User User { get; set; }
    }
}
