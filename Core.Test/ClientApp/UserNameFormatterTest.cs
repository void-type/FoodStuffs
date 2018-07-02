using Core.Services.ClientApp;
using Xunit;

namespace Core.Test.ClientApp
{
    public class UserNameFormatterTests
    {
        [Theory]
        [InlineData("Domain1\\Name", "Name")]
        [InlineData("Name", "Name")]
        [InlineData(null, "Unknown")]
        public void FormatNameFromAdLogin(string input, string expected)
        {
            var formatter = new UserNameFormatter();

            var userName = formatter.FromAdLogin(input);

            Assert.Equal(expected, userName);
        }
    }
}
