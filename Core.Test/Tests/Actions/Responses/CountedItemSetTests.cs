using Core.Model.Actions.Responses.ItemSet;
using Xunit;
using System.Collections.Generic;

namespace Core.Test.Tests.Actions.Responses 
{
    public class CountedItemSetTests
    {
        [Fact]
        public void CountItemsThree()
        {
            var strings = new List<string>() {"", "", ""};

            var set = new CountedItemSet<string>(strings);

            Assert.Equal(3, set.Count);
        }

        [Fact]
        public void CountItemsZero()
        {
            var strings = new List<string>();

            var set = new CountedItemSet<string>(strings);

            Assert.Equal(0, set.Count);
        }

        [Fact]
        public void CountItemsNull()
        {
            var set = new CountedItemSet<string>();

            Assert.Equal(0, set.Count);
        }
    }
}