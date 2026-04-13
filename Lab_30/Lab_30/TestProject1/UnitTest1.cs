using System;
using Xunit;
using ClassLibrary1; // <-- Підключаємо твою бібліотеку

namespace Lab30.Tests
{
    public class StringHelperTests
    {
        private readonly StringHelper _helper = new StringHelper();

        [Fact]
        public void Reverse_ValidString_ReturnsReversedString()
        {
            Assert.Equal("olleh", _helper.Reverse("hello"));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("a", "a")]
        [InlineData("123 456", "654 321")]
        public void Reverse_EdgeCases_ReturnsCorrectly(string input, string expected)
        {
            Assert.Equal(expected, _helper.Reverse(input));
        }

        [Fact]
        public void Reverse_NullInput_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _helper.Reverse(null));
        }

        [Theory]
        [InlineData("radar")]
        [InlineData("Level")]
        [InlineData("A nut for a jar of tuna")]
        [InlineData("a")]
        public void IsPalindrome_ValidPalindromes_ReturnsTrue(string input)
        {
            Assert.True(_helper.IsPalindrome(input));
        }

        [Theory]
        [InlineData("hello")]
        [InlineData("radars")]
        public void IsPalindrome_InvalidPalindromes_ReturnsFalse(string input)
        {
            Assert.False(_helper.IsPalindrome(input));
        }

        [Fact]
        public void IsPalindrome_NullInput_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _helper.IsPalindrome(null));
        }

        [Theory]
        [InlineData("Hello world", 2)]
        [InlineData("One two three four", 4)]
        [InlineData("JustOneWord", 1)]
        public void WordCount_NormalSentences_ReturnsCorrectCount(string input, int expectedCount)
        {
            Assert.Equal(expectedCount, _helper.WordCount(input));
        }

        [Fact]
        public void WordCount_MultipleSpaces_IgnoresExtraSpaces()
        {
            var input = "Hello    world   with  spaces";
            Assert.Equal(4, _helper.WordCount(input));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\n \t")]
        public void WordCount_EmptyOrWhitespace_ReturnsZero(string input)
        {
            Assert.Equal(0, _helper.WordCount(input));
        }

        [Fact]
        public void WordCount_NullInput_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _helper.WordCount(null));
        }
    }
}