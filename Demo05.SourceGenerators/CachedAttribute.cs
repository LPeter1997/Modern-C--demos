using System;

namespace Demo05.SourceGenerators
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CachedAttribute : Attribute
    {
    }
}
