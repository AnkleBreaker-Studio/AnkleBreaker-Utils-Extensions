using System;
using System.Collections.Generic;
using System.Linq;

namespace AnkleBreaker.Utils.Extensions
{
	public static class EnumerableExtensions
    {
        /// <summary>
        /// Returns whether the enumerable is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of enumerable.</typeparam>
        /// <param name="enumerable">The enumerable to check.</param>
        /// <returns>Whether the enumerable is null or empty.</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) => enumerable == null || !enumerable.Any();

        /// <summary>
        /// Executes an action on each element of the enumerable.
        /// Avoids the need for .ToList().ForEach().
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable == null) return;
            foreach (T item in enumerable)
                action(item);
        }
    }
}
