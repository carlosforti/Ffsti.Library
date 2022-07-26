using FluentAssertions;

using Xunit;

namespace Ffsti.Library.Tests.ExtensionMethods
{
    public class StringExtensionMethodsTests
    {
        [Theory]
        [InlineData("68752076000107")]
        [InlineData("68.752.076/0001-07")]
        public void ValidateCnpj_ShouldBeTrue(string value)
        {
            var isValid = value.ValidateCnpj();
            isValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("68752076000108")]
        [InlineData("68.752.076/0001-08")]
        public void ValidateCnpj_ShouldBeFalse(string value)
        {
            var isValid = value.ValidateCnpj();
            isValid.Should().BeFalse();
        }

        [Theory]
        [InlineData("42027660089")]
        [InlineData("420.276.600-89")]
        public void ValidateCpf_ShouldBeTrue(string value)
        {
            var isValid = value.ValidateCpf();
            isValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("96687505008")]
        [InlineData("966.875.050-08")]
        public void ValidateCpf_ShouldBeFalse(string value)
        {
            var isValid = value.ValidateCpf();
            isValid.Should().BeFalse();
        }
    }
}
