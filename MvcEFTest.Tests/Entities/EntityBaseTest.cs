using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcEFTest.Entities;
using Xunit;

namespace MvcEFTest.Tests.Entities
{
    public class EntityBaseTest
    {
        [Fact]
        public void Equals_BothParamsAreDerivedFromEntityBaseWithSameIds_ReturnsTrue()
        {
            var first = new User { Id = 1 };
            var second = new User { Id = 1 };

            Assert.False(ReferenceEquals(first, second));
            Assert.True(first.Equals(second));
            Assert.True(first == second);
            Assert.False(first != second);
            Assert.Equal(first.GetHashCode(), second.GetHashCode());
        }

        [Fact]
        public void Equals_BothParamsAreDerivedFromEntityBaseWithDifferentIds_ReturnsFalse()
        {
            var first = new User { Id = 1 };
            var second = new User { Id = 2 };

            Assert.False(ReferenceEquals(first, second));
            Assert.False(first.Equals(second));
            Assert.False(first == second);
            Assert.True(first != second);
            Assert.NotEqual(first.GetHashCode(), second.GetHashCode());
        }

        [Fact]
        public void Equals_BothParamsAreDerivedFromEntityBaseButDifferentTypes_ReturnsFalse()
        {
            var first = new User { Id = 1 };
            var second = new Phone { Id = 1 };

            Assert.False(ReferenceEquals(first, second));
            Assert.False(first.Equals(second));

            Assert.Equal(first.GetHashCode(), second.GetHashCode());
        }

        [Fact]
        public void Equals_BothParamsAreDerivedFromEntityBaseAndIdsEqualZero_ReturnsFalse()
        {
            // Should be considered different as in EF
            var first = new User { Id = 0 };
            var second = new User { Id = 0 };

            Assert.False(ReferenceEquals(first, second));
            Assert.False(first.Equals(second));
            Assert.False(first == second);
            Assert.True(first != second);
            Assert.NotEqual(first.GetHashCode(), second.GetHashCode());
        }
    }
}
