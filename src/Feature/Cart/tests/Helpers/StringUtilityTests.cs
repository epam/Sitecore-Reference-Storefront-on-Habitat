namespace Sitecore.Feature.Cart.Tests.Extensions
{
    using System;
    using FluentAssertions;
    using Helpers;
    using Xunit;

    public class StringUtilityTests
    {
        [Fact]
        public void RemoveCurlyBrackets_ShoudReturnStringWithoutCurlyBrackets()
        {
            var expectedResult = "13ji21p3n12412";
            var inputString = "{" + expectedResult + "}";

            var result = StringUtility.RemoveCurlyBrackets(inputString);
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void RemoveCurlyBrackets_ShoudThrowNullReferenceException()
        {
            try
            {
                StringUtility.RemoveCurlyBrackets(null);
            }
            catch (Exception ex)
            {
                ex.Should().BeOfType(typeof(ArgumentNullException));
            }
        }
    }
}
