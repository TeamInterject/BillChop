using BillChopBE.DataAccessLayer.Models;

namespace BillChopBE.DataAccessLayer.Filters.Factories
{
    public interface IBillDbFilterFactory
    {
        IDbFilter<Bill> Create(BillFilterInfo billFilterInfo);
    }

    public class BillDbFilterFactory : IBillDbFilterFactory
    {
        public IDbFilter<Bill> Create(BillFilterInfo billFilterInfo)
        {
            return new BillDbFilter(billFilterInfo);
        }
    }
}
