using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace MvcEFTest.Tests.Helpers
{
    internal class MvcEFTestDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
    {
        public MvcEFTestDbAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable)
        {
        }

        public MvcEFTestDbAsyncEnumerable(Expression expression) : base(expression)
        {
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new MvcEFTestDbAsyncQueryProvider<T>(this); }
        }

        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new MvcEFTestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }
    }
}