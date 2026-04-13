using System;
using System.Linq;

namespace ClassLibrary1 // <-- Зверни увагу, тут тепер твоя назва проєкту
{
    public class StringHelper
    {
        public string Reverse(string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input), "Рядок не може бути null.");
            char[] charArray = input.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public bool IsPalindrome(string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input), "Рядок не може бути null.");
            var cleaned = new string(input.Where(c => !char.IsWhiteSpace(c)).ToArray()).ToLower();
            var reversed = Reverse(cleaned);
            return cleaned == reversed;
        }

        public int WordCount(string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input), "Рядок не може бути null.");
            if (string.IsNullOrWhiteSpace(input)) return 0;
            return input.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }
}