using System.Collections.Generic;
using MvcEFTest.Models;

namespace MvcEFTest.Views
{
    public class UserViewModel : ViewModelBase
    {
        public string Name { get; set; }

        public IEnumerable<PhoneViewModel> Phones { get; set; }
    }
}