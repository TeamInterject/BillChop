using System;
using System.Collections.Generic;

namespace ProjectPortableTools.Extensions
{
    public static class DecimalExtensions
    {
        public static IEnumerable<decimal> SplitEqually(this decimal total, int splits, int afterDecimal = 2)
        {
            var subTotals = new List<decimal>();
            splits.ForEach((index) =>
            {
                var splitsLeft = splits - index;
                decimal subTotal = Math.Round(total / splitsLeft, afterDecimal);

                subTotals.Add(subTotal);
                total -= subTotal;
            });

            return subTotals;
        }
    }
}
