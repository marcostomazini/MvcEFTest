using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcEFTest.Utilities
{
    public static class EnumerableHelpers
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            source.ToList().ForEach(action);
        }
    }
}