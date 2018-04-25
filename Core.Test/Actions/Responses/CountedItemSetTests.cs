using Core.Model.Actions.Responses.ItemSet;
using System.Collections.Generic;
using Xunit;

namespace Core.Test.Actions.Responses
{
    public class CountedItemSetTests
    {
        [Fact]
        public void CountItemsNull()
        {
            var set = new CountedItemSet<string>();

            Assert.Equal(0, set.Count);
        }

        [Fact]
        public void CountItemsThree()
        {
            var set = new CountedItemSet<string>(new List<string>() { "", "", "" });

            Assert.Equal(3, set.Count);
        }

        [Fact]
        public void CountItemsZero()
        {
            var set = new CountedItemSet<string>(new List<string>());

            Assert.Equal(0, set.Count);
        }
    }
}