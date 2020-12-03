using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPortableTools.Extensions
{
    public static class MoreLinqExtensions
    {
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> enumerable) where T : class
        {
            return enumerable.Where(e => e != null).Select(e => e!);
        }

        public static async Task<IEnumerable<T>> WhenAll<T>(this IEnumerable<Task<T>> enumerable) 
        {
            return await Task.WhenAll(enumerable);
        }
    }
}