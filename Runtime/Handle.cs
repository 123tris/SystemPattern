using System;

namespace System_Pattern
{
    public sealed class Handle<T> where T : class
    {
        public Handle(T value)
        {
            Value = value;
        }

        public T Value { get; }
    }
}