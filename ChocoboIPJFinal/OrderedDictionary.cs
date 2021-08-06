using SFML.Graphics;
using System;
using System.Collections.Generic;

internal class OrderedDictionary<T1, T2>
{
    public static implicit operator OrderedDictionary<T1, T2>(Dictionary<IntRect, int> v)
    {
        throw new NotImplementedException();
    }
}