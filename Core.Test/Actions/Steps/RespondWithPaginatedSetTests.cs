using Core.Model.Actions.Responder;
using Core.Model.Actions.Responses.ItemSet;
using Core.Model.Actions.Steps;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Core.Test.Actions.Steps
{
    public class RespondWithPaginatedSetTests
    {
        [Theory]
        [InlineData(0, 1, 1, 0)]
        [InlineData(100, 10, 1, 10)]
        [InlineData(100, 10, 10, 10)]
        [InlineData(100, 10, 11, 0)]
        [InlineData(105, 10, 11, 5)]
        public void RespondWithPaginatedRecipes(int totalCount, int take, int page, int expectedCount)
        {
            ItemSetPage<string> itemSetPage = null;

            var mockResponder = new Mock<IActionResponder>();
            mockResponder
                .Setup(mock => mock.WithItem(It.IsAny<ItemSetPage<string>>(), It.IsAny<string>()))
                .Callback((IItemSetPage setPage, string log) =>
                {
                    itemSetPage = (ItemSetPage<string>)setPage;
                });

            var set = new List<string>();

            for (var i = 0; i < totalCount; i++)
            {
                set.Add(i.ToString());
            }

            new RespondWithPaginatedSet<string>(set, take, page, "log").Execute(mockResponder.Object);

            mockResponder.Verify(mock => mock.WithItem(It.IsAny<ItemSetPage<string>>(), It.IsAny<string>()), Times.Exactly(1));

            Assert.NotNull(itemSetPage);
            Assert.Equal(expectedCount, itemSetPage.Count);
            Assert.Equal(page, itemSetPage.Page);
            Assert.Equal(totalCount, itemSetPage.TotalCount);
        }
    }
}