using System;
using System.Linq;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace IsPalindromeBenchmark
{
    class Program
    {
        public static void Main()
        {
            string[] str =
            {
                "Usa ma bin la dene dalni bam ASU",
                "Monki flip",
                "Red rum, sir, is murder",
                "saippuakivikauppias",
                "12.02.2021"
            };
            foreach (var s in str)
            {
                Console.WriteLine($"{s}:");
                foreach (var methodInfo in typeof(Palindrome).GetMethods(BindingFlags.Public | BindingFlags.Static))
                {
                    Console.WriteLine($"\t{methodInfo.Name,-25} - {methodInfo.Invoke(null, new[] { s })}");
                }
                Console.WriteLine();
            }

            BenchmarkRunner.Run<PalindromeBenchmark>();
        }
    }

    public static class Palindrome
    {
        public static bool Method0(string str)
        {
            str = new string(str.Where(char.IsLetter).ToArray()).ToLower();
            return str.SequenceEqual(str.Reverse());
        }
        public static bool Method1(string str)
        {
            str = new string(str.Where(char.IsLetter).ToArray()).ToLower();
            for (int i = 0; i < str.Length - i; i++)
            {
                if (str[i] != str[^(i + 1)])
                {
                    return false;
                }
            }
            return true;
        }
        public static bool Method2(string str)
        {
            string newStr = string.Empty;
            foreach (var c in str.Where(char.IsLetter))
            {
                newStr += c;
            }
            newStr = newStr.ToLower();

            for (int i = 0; i < newStr.Length - i; ++i)
            {
                if (newStr[i] != newStr[^(i + 1)])
                {
                    return false;
                }
            }
            return true;
        }
        public static bool Method3(string str)
        {
            string newStr = string.Empty;
            foreach (var c in str)
            {
                if (char.IsLetter(c))
                {
                    newStr += c;
                }
            }
            newStr = newStr.ToLower();

            for (int i = 0; i < newStr.Length - i; ++i)
            {
                if (newStr[i] != newStr[^(i + 1)])
                {
                    return false;
                }
            }
            return true;
        }
        public static bool Method4(string str)
        {
            str = str.Where(char.IsLetter).Aggregate(string.Empty, (current, c) => current + c).ToLower();

            for (int i = 0; i < str.Length - i; ++i)
            {
                if (str[i] != str[^(i + 1)])
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class PalindromeBenchmark
    {
        private readonly string _str = "Usa ma bin la dene dalni bam ASU";

        [Benchmark]
        public bool Method0() => Palindrome.Method0(_str);

        [Benchmark]
        public bool Method1() => Palindrome.Method1(_str);

        [Benchmark]
        public bool Method2() => Palindrome.Method2(_str);

        [Benchmark]
        public bool Method3() => Palindrome.Method3(_str);

        [Benchmark]
        public bool Method4() => Palindrome.Method4(_str);
    }
}
