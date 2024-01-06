using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Vic3ModManager.Essentials
{
    internal static class StringHelpers
    {
        private static readonly Dictionary<char, string> CyrillicToLatinMap = new Dictionary<char, string>
        {
            // Uppercase letters
            {'А', "A"}, {'Б', "B"}, {'В', "V"}, {'Г', "G"}, {'Д', "D"},
            {'Е', "E"}, {'Ё', "Yo"}, {'Ж', "Zh"}, {'З', "Z"}, {'И', "I"},
            {'Й', "Y"}, {'К', "K"}, {'Л', "L"}, {'М', "M"}, {'Н', "N"},
            {'О', "O"}, {'П', "P"}, {'Р', "R"}, {'С', "S"}, {'Т', "T"},
            {'У', "U"}, {'Ф', "F"}, {'Х', "Kh"}, {'Ц', "Ts"}, {'Ч', "Ch"},
            {'Ш', "Sh"}, {'Щ', "Shch"}, {'Ъ', "Ie"}, {'Ы', "Y"}, {'Ь', ""},
            {'Э', "E"}, {'Ю', "Yu"}, {'Я', "Ya"},

            // Lowercase letters
            {'а', "a"}, {'б', "b"}, {'в', "v"}, {'г', "g"}, {'д', "d"},
            {'е', "e"}, {'ё', "yo"}, {'ж', "zh"}, {'з', "z"}, {'и', "i"},
            {'й', "y"}, {'к', "k"}, {'л', "l"}, {'м', "m"}, {'н', "n"},
            {'о', "o"}, {'п', "p"}, {'р', "r"}, {'с', "s"}, {'т', "t"},
            {'у', "u"}, {'ф', "f"}, {'х', "kh"}, {'ц', "ts"}, {'ч', "ch"},
            {'ш', "sh"}, {'щ', "shch"}, {'ъ', "ie"}, {'ы', "y"}, {'ь', ""},
            {'э', "e"}, {'ю', "yu"}, {'я', "ya"}
        };

        public static string ReplaceSpaces(string input)
        {
            string output = "";

            for (int i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case '-':
                        break;
                    case ' ':
                        output += "_";
                        break;
                    default:
                        output += input[i];
                        break;
                }
            }

            return output;
        }

        public static string TransliterateCyrillicToLatin(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            string output = input.Select(c => CyrillicToLatinMap.ContainsKey(c) ? CyrillicToLatinMap[c] : c.ToString())
                                 .Aggregate((current, next) => current + next);
            return output;
        }
    }
}