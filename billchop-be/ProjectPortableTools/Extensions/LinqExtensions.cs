using System.Collections.Generic;
using System.Linq;

namespace ProjectPortableTools.Extensions
{
    public static class MoreLinqExtensions
    {
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> enumerable) where T : class
        {
            return enumerable.Where(e => e != null).Select(e => e!);
        }
    }
}