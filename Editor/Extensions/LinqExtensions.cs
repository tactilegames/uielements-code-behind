using System;
using System.Collections.Generic;
using System.Linq;

namespace TactileModules.UIElementsCodeBehind
{
    internal static class LinqExtensions
    {
        public static IEnumerable<TValue> FlattenTree<TValue>(this IEnumerable<TValue> source, Func<TValue, IEnumerable<TValue>> selector) {
            var enumerable = source.ToList();
            return enumerable.SelectMany(x => selector(x).FlattenTree(selector)).Concat(enumerable);
        }
    }
}