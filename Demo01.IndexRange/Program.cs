using System;
using System.Linq;

/////////////////////////////////////////////////////////////////
// Probléma: szeretném a szöveg hátulról 3. karakterét,
// illetve az első és utolsó karaktert leszámítva a rész-szöveget
/////////////////////////////////////////////////////////////////
var myText = "the brown fox jumped over the lazy dog";

////////////////////////////
// Megoldás 1: Old-school C#
////////////////////////////
Console.WriteLine($"3rd character from the back: {myText[myText.Length - 3]}");
Console.WriteLine($"text without first and last chars: {myText.Substring(1, myText.Length - 2)}");

// Tipikus minta, .Count - X vagy .Length - X
// A Substring-ben extra logika van, azt specifikálom hogy hány karakter kell, nem azt, hogy hátulról hányadikig kell
// Allokál a Substring

///////////////////
// Megoldás 2: LINQ
///////////////////
Console.WriteLine($"3rd character from the back: {myText.SkipLast(2).Last()}");
Console.WriteLine($"text without first and last chars: {new string(myText.Skip(1).SkipLast(1).ToArray())}");

// Sokkal természetesebben írja le, amit szerettem volna
// De elég sokat fizetek LINQ-val

/////////////////////////////
// Megoldás 3: Index és Range
/////////////////////////////
Console.WriteLine($"3rd character from the back: {myText[^3]}");
Console.WriteLine($"text without first and last chars: {myText[1..(myText.Length - 1)]}");

// A ^ a hátulról indexelés, ahol a 0 az utolsó utáni elem, olyan mint a negatív index Python-ban
// .. az intervallum
// Eltörölte a zajos mintát
// Sok helyen a range Span-t ad vissza, átlagosan kevesebb allokáció

// Bónusz: A Range ötvözése indexekkel:
Console.WriteLine($"text without first and last chars: {myText[1..^1]}");
