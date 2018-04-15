using Core.Model.Actions.Chain;
using Core.Model.Actions.Responder;
using Core.Model.Actions.Responses.ItemSet;
using Core.Model.Actions.Steps;
using System.Collections.Generic;
using Xunit;

namespace Core.Test.Tests.Actions
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
            var responder = new SimpleActionResponder();

            var set = new List<string>();

            for (var i = 0; i < totalCount; i++)
            {
                set.Add(i.ToString());
            }

            new ActionChain(responder)
                .Execute(new RespondWithPaginatedSet<string>(set, take, page));

            var response = responder.Response.Item as ItemSetPage<string>;

            Assert.NotNull(response);
            Assert.True(responder.ResponseCreated);
            Assert.Equal(expectedCount, response.Count);
            Assert.Equal(page, response.Page);
            Assert.Equal(totalCount, response.TotalCount);
        }
    }
}