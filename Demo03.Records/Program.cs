using System;
using System.Collections.Generic;

// Implicit print, equality, hash

var myPerson = new Person("Alice", 23);
Console.WriteLine($"myPerson: {myPerson}");

var myPerson2 = new Person("Alice", 23);
var myPerson3 = new Person("Bob", 54);
Console.WriteLine($"myPerson2: {myPerson2}");
Console.WriteLine($"myPerson3: {myPerson3}");

Console.WriteLine($"myPerson == myPerson2: {myPerson == myPerson2}");
Console.WriteLine($"ReferenceEquals(myPerson, myPerson2): {ReferenceEquals(myPerson, myPerson2)}");

Console.WriteLine($"myPerson == myPerson3: {myPerson == myPerson3}");
Console.WriteLine($"ReferenceEquals(myPerson, myPerson3): {ReferenceEquals(myPerson, myPerson3)}");

// Immutable by default
// myPerson.Name = "Cecil";

// "Változtatás" a with kulcsszóval
var myYoungerAlice = myPerson with { Age = 15 };

// Oops
var s1 = new Set("primes-less-than-10", new[] { 2, 3, 5, 7 });
var s2 = new Set("primes-less-than-10", new[] { 2, 3, 5, 7 });
Console.WriteLine($"s1 == s2: {s1 == s2}");

// Auto-deconstruct
var (name, age) = myPerson;

// Polymorphism
Expr add = new Expr.Add(new Expr.Int(1), new Expr.Int(2));
Expr mul = new Expr.Mul(new Expr.Int(1), new Expr.Int(2));
Console.WriteLine($"add == mul: {add == mul}");

#if true

record Person(string Name, int Age);

#else

// Ezt mind kaptuk
class Person : IEquatable<Person>
{
    public string Name { get; init; }
    public int Age { get; init; }

    public Person(string Name, int Age)
    {
        this.Name = Name;
        this.Age = Age;
    }

    public override string ToString() => $"Person {{ Name = {Name}, Age = {Age} }}";
    
    public override bool Equals(object? obj) => Equals(obj as Person);

    public virtual bool Equals(Person? other) =>
           other is not null
        && Name.Equals(other.Name)
        && Age.Equals(other.Age);

    public override int GetHashCode() => HashCode.Combine(Name, Age);

    public static bool operator ==(Person p1, Person p2) => p1.Equals(p2);
    public static bool operator !=(Person p1, Person p2) => !p1.Equals(p2);

    public void Deconstruct(out string Name, out int Age)
    {
        Name = this.Name;
        Age = this.Age;
    }
}

#endif

record Set(string Name, IReadOnlyList<int> elements);

abstract record Expr
{
    public abstract int Evaluate();

    public record Int(int Value) : Expr
    {
        public override int Evaluate() => Value;
    }

    public record Neg(Expr Sub) : Expr
    {
        public override int Evaluate() => -Sub.Evaluate();
    }

    public record Add(Expr Left, Expr Right) : Expr
    {
        public override int Evaluate() => Left.Evaluate() + Right.Evaluate();
    }

    public record Mul(Expr Left, Expr Right) : Expr
    {
        public override int Evaluate() => Left.Evaluate() * Right.Evaluate();
    }
}
