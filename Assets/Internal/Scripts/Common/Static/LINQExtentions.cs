using System;
using System.Collections.Generic;

namespace Shakhtarsk.Static
{
    public static class LINQExtentions
    {
        public static void Foreach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }
    }
}