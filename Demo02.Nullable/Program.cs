using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

// Nullable contract attributes ////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////////
// Probléma: A függvényem csak akkor ír ki nekem null-t, ha hamissal tért vissza
// Tipikus Try... pattern
////////////////////////////////////////////////////////////////////////////////
var people = new List<Person>
{ 
    new() { Name = "Alice", Age = 23 },
    new() { Name = "Bob", Age = 57 },
    new() { Name = "Cecil", Age = 14 },
    new() { Name = "Daniel", Age = 32 },
};

// Megoldás: MaybeNullWhen
if (people.TryGetFirst(p => p.Age > 30, out var person))
{
    Console.WriteLine(person.Name);
}

Console.WriteLine(person.Name);

//////////////////////////////////////////////////////////////////////////////
// Probléma: Az analyzer sem átverhetetlen, néha meg kell győzni az igazunkról
//////////////////////////////////////////////////////////////////////////////

Person? myPerson = null;
var i = 3;

if (i >= 3) myPerson = new();
if (i < 3) myPerson = new();

// 2 lehetséges megoldás: Debug.Assert vs !

// Debug.Assert(myPerson is not null, "If branches must have covered all possible values of i");
Console.WriteLine(myPerson.Name);

public static class TryGetPatternExtensions
{
    public static bool TryGetFirst<T>(
        this IEnumerable<T> enumerable,
        Predicate<T> predicate,
        /* [MaybeNullWhen(false)] */ out T? value)
    {
        foreach (var item in enumerable)
        {
            if (predicate(item))
            {
                value = item;
                return true;
            }
        }
        value = default;
        return false;
    }
}

class Person
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}
