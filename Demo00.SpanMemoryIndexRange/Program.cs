using System;
using System.Collections.Generic;
using System.Linq;

//////////////////////////////////////////////////////////////////
// Probléma: össze akarom adni a számokat a tömböm egy részletében
//////////////////////////////////////////////////////////////////
var myNumbers = new[] { 1, 8, 4, 6, 2, 8, 2, 5, 7, 1, 6, 4, 6 };
//                            ^-- innentől (inclusive)   ^-- mondjuk idáig (exclusive)

/////////////////////////////////
// Megoldás 1: Új tárolót képzünk
/////////////////////////////////
int Sum_Solution1(IReadOnlyList<int> numbers)
{
    var sum = 0;
    foreach (var number in numbers) sum += number;
    return sum;
}

var subarray = new int[9];
Array.Copy(myNumbers, 2, subarray, 0, 9);
Console.WriteLine($"Sum_Solution1: {Sum_Solution1(subarray)}");

// Az API jó
// Allokáció szempontjából borzalmas

//////////////////////////////////////////
// Megoldás 2: Indexek! indexek mindenütt!
//////////////////////////////////////////
int Sum_Solution2(IReadOnlyList<int> numbers, int start, int length)
{
    var sum = 0;
    for (var i = 0; i < length; ++i) sum += numbers[start + i];
    return sum;
}

Console.WriteLine($"Sum_Solution2: {Sum_Solution2(myNumbers, 2, 9)}");

// Az API borzalmas
// Allokáció szempontjából viszont nagyon jó

/////////////////////////////////////
// Megoldás 3: LINQ és IEnumerable<T>
/////////////////////////////////////
int Sum_Solution3(IEnumerable<int> numbers)
{
    // MEGJ.: A valóságban elég lenne egy numbers.Sum()
    var sum = 0;
    foreach (var number in numbers) sum += number;
    return sum;
}

Console.WriteLine($"Sum_Solution3: {Sum_Solution3(myNumbers.Skip(2).Take(9))}");

// Az API egész jó, bármilyen LINQ leképzést megeszik
// Allokáció és teljesítmény szempontjából pedig nem
// Elvesztettük azt az információt, hogy ez szekvenciális memória, nincs vektorizációra lehetőségünk

//////////////////////
// Megoldás 4: Span<T>
//////////////////////
int Sum_Solution4(ReadOnlySpan<int> numbers)
{
    var sum = 0;
    foreach (var number in numbers) sum += number;
    return sum;
}

Console.WriteLine($"Sum_Solution4: {Sum_Solution4(myNumbers.AsSpan(2, 9))}");

// Az API tulajdonképpen majdnem olyan jó, mint a LINQs
// Elvesztettük a képességet, hogy bármilyen LINQ szekvenciát szummázzunk
// Allokáció szempontjából nagyon jó, illetve ide már ismét írhatunk SIMD-gyorsítást
