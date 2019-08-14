using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace TitleCaseBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var summary = BenchmarkRunner.Run<TitleCases>();
        }
    }

    public class TitleCases
    {
        [Params("the last of the mohicans", "snow white and the seven dwarfs")]
        public string TitleCase;

        public CultureInfo CultureInfo;

        [GlobalSetup]
        public void Setup()
        {
            CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
        }

        [Benchmark]
        public string TextInfoToTitleCase()
        {
            return CultureInfo.TextInfo.ToTitleCase(TitleCase);
        }

        [Benchmark]
        public string WithChars()
        {
            var str = new char[TitleCase.Length];
            bool isPreviousSpace = true;
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = isPreviousSpace ? char.ToUpper(TitleCase[i]) : char.ToLower(TitleCase[i]);
                isPreviousSpace = false || TitleCase[i] == ' ';
            }

            return new string(str);
        }

        [Benchmark]
        public string WithCharsCheckingTolower()
        {
            var str = new char[TitleCase.Length];
            bool isPreviousSpace = false;
            for (int i = 0; i < str.Length; i++)
            {
                var currentChar = TitleCase[i];
                str[i] = isPreviousSpace
                    ? char.ToUpper(currentChar)
                    : currentChar > 'z' || currentChar < 'a' 
                        ? char.ToLower(currentChar)
                        : currentChar;
                isPreviousSpace = false || TitleCase[i] == ' ';
            }

            return new string(str);
        }

        /// <summary>
        /// Code from https://stackoverflow.com/a/38361008/97615
        /// </summary>
        /// <returns></returns>
        [Benchmark]
        public string WithSubString()
        {
            var tokens = TitleCase.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < tokens.Length; i++)
            {
                var token = tokens[i];
                tokens[i] = token == token.ToUpper()
                    ? token 
                    : token.Substring(0, 1).ToUpper() + token.Substring(1).ToLower();
            }

            return string.Join(" ", tokens);
        }

        /// <summary>
        /// Code From https://stackoverflow.com/a/40322543/97615
        /// </summary>
        /// <returns></returns>
        [Benchmark]
        public string WithRegexAndCultureInfo()
        {
            var tokens = Regex.Split(CultureInfo.TextInfo.ToLower(TitleCase), "([ -])");

            for (var i = 0; i < tokens.Length; i++)
            {
                if (!Regex.IsMatch(tokens[i], "^[ -]$"))
                {
                    tokens[i] = $"{CultureInfo.TextInfo.ToUpper(tokens[i].Substring(0, 1))}{tokens[i].Substring(1)}";
                }
            }

            return string.Join("", tokens);
        }

        /// <summary>
        /// Code from https://stackoverflow.com/a/45113271/97615
        /// Leaves acronyms
        /// </summary>
        /// <returns></returns>
        [Benchmark]
        public string WithCharsAndStringBuilderAndMore()
        {
            var tokens = TitleCase.Split(new[] { " " }, StringSplitOptions.None);
            var stringBuilder = new StringBuilder();
            for (var ti = 0; ti < tokens.Length; ti++)
            {
                var token = tokens[ti];
                if (token == token.ToUpper())
                    stringBuilder.Append(token + " ");
                else
                {
                    var previousWasSeperator = false;
                    var previousWasNumber = false;
                    var ignoreNumber = false;
                    for (var i = 0; i < token.Length; i++)
                    {

                        if (char.IsNumber(token[i]))
                        {
                            stringBuilder.Append(token[i]);
                            previousWasNumber = true;
                        }
                        else if (!char.IsLetter(token[i]))
                        {
                            stringBuilder.Append(token[i]);
                            previousWasSeperator = true;
                        }
                        else if ((previousWasNumber && !ignoreNumber) || previousWasSeperator)
                        {
                            stringBuilder.Append(char.ToUpper(token[i]));
                            previousWasSeperator = false;
                            previousWasNumber = false;
                        }
                        else if (i == 0)
                        {
                            ignoreNumber = true;
                            stringBuilder.Append(char.ToUpper(token[i]));
                        }
                        else
                        {
                            ignoreNumber = true;
                            stringBuilder.Append(char.ToLower(token[i]));
                        }
                    }
                    stringBuilder.Append(" ");
                }
            }
            return stringBuilder.ToString().TrimEnd();
        }
    }
}
