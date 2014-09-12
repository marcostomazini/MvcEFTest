using System.Collections.Generic;
using MvcEFTest.Models;

namespace MvcEFTest.Views
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<PhoneViewModel> Phones { get; set; }
    }
}