using System;
using System.Collections.Generic;

namespace BotCore
{
    internal static class CollectionExtensions
    {
        private static readonly Random Random = new Random();

        // Fisher–Yates shuffle
        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = Random.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}
