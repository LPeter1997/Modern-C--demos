using System;

// Alappélda
var greeting = DateTime.Now.Hour switch
{
    6 or 7 or 8 or 9 or 10 => "good morning",
    11 or 12 or 13 or 14 or 15 or 16 or 17 => "good day",
    18 or 19 or 20 or 21 or 22 => "good evening",
    23 or 0 or 1 or 2 or 3 or 4 or 5 => "good night",
    _ => "leap second?",
};

Shape myShape = new Square(3);

// Type-matching patterns
var type = myShape switch
{
    Square s => $"Square with side length {s.Width}",
    Circle => "Circle",
    _ => "something else",
};

// Works with tuples
Shape s1 = new Square(10);
Shape s2 = new Circle(4);
var collision = (s1, s2) switch
{
    (Square a, Square b) => "Square vs Square",

    (Square, Circle) or (Circle, Square) => "Square vs Circle",

    (Circle, Circle) => "Circle vs Circle",

    (_, _) => throw new NotImplementedException(), // :(
};

var myPerson = new Person("Alice", 23);

// Destructuring patterns
var description = myPerson switch
{
    Person { Name: "Bob", Age: 21 } => "We got 21 years old Bob",
    Person { Name: "Bob" } b => $"We got Bob aged {b.Age}",
    _ => "some default",
};

// Relational patterns
var ageGroup = myPerson.Age switch
{
    < 14 => "young child",
    (>= 14) and (< 18) => "teen",
    >= 18 => "adult",
};

// Complex patterns
var description2 = myPerson switch
{
    Person { Name: "Bob", Age: >= 18 } => "Adult Bob",
    Person { Name: "Alice", Age: < 18 } a => $"Not adult Alice with age {a.Age}",
    _ => "some default",
};

record Person(string Name, int Age);

abstract record Shape;
record Square(int Width) : Shape;
record Circle(int Radius) : Shape;
