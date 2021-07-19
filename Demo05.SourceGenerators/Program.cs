using System;
using System.Diagnostics;
using System.Numerics;

namespace Demo05.SourceGenerators
{
    public partial class MyAlgos
    {
        [Cached]
        public static bool IsPrime(BigInteger n)
        {
            if (n == 2) return true;
            for (var i = 2; i <= n / 2; ++i)
            {
                if (n % i == 0) return false;
            }
            return true;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            const int Min = 1_000_000;
            const int Max = 1_004_000;

            var timer = Stopwatch.StartNew();
            for (var i = Min; i < Max; ++i)
            {
                if (MyAlgos.IsPrime(i)) Console.Write($"{i}, ");
            }
            Console.WriteLine("\n=======================");
            Console.WriteLine($"without cache took {timer.ElapsedMilliseconds} ms");
            Console.WriteLine("=======================");

            timer = Stopwatch.StartNew();
            for (var i = Min; i < Max; ++i)
            {
                if (MyAlgos.CachedIsPrime(i)) Console.Write($"{i}, ");
            }
            Console.WriteLine("\n=======================");
            Console.WriteLine($"with cache (1st pass) took {timer.ElapsedMilliseconds} ms");
            Console.WriteLine("=======================");

            timer = Stopwatch.StartNew();
            for (var i = Min; i < Max; ++i)
            {
                if (MyAlgos.CachedIsPrime(i)) Console.Write($"{i}, ");
            }
            Console.WriteLine("\n=======================");
            Console.WriteLine($"with cache (2nd pass) took {timer.ElapsedMilliseconds} ms");
            Console.WriteLine("=======================");
        }
    }
}
