using System;
using System.Globalization;
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
    }
}
