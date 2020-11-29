using System;
using System.Collections.Generic;

namespace ProjectPortableTools.Extensions
{
    public static class IntExtensions
    {
        /// <summary>
        /// Ruby-like ForEach extension that will call given callback number of given times
        /// Note: negative numbers will not call callback
        /// </summary>
        public static void ForEach(this int times, Action callback)
        {
            for (var index = 0; index < times; index++)
                callback();
        }

        /// <summary>
        /// Ruby-like times extension that will call given callback number of given times with index
        /// Note: negative numbers will not call callback
        /// </summary>
        public static void ForEach(this int times, Action<int> callback) 
        {
            for (var index = 0;  index < times; index++) 
                callback(index);
        }

        /// <summary>
        /// Ruby-like times extension to convert generate objects of given type
        /// Note: negative numbers will return an empty enumerable
        /// </summary>
        public static IEnumerable<TMapped> Select<TMapped>(this int times, Func<int, TMapped> callback)
        {
            for (var index = 0; index < times; index++)
                yield return callback(index);
        }
    }
}
