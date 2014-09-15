using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.Expressions;
using MvcEFTest.Views;

namespace MvcEFTest.Models
{
    public class PhoneViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string AndroidVersion { get; set; }
    }
}