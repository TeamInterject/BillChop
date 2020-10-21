using BillChopBE.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillChopBE.Extensions
{
    public static class DecimalExtensions
    {
        public static IEnumerable<decimal> SplitEqually(this decimal total, int splits, int afterDecimal = 2)
        {
            if (splits <= 0)
                throw new BadRequestException("Cannot split value to 0 or less parts");

            var subTotals = new List<decimal>();
            for (; splits > 0; splits--)
            {
                decimal subTotal = Math.Round(total / splits, afterDecimal);
                subTotals.Add(subTotal);

                total -= subTotal;
            }

            return subTotals;
        }
    }
}
