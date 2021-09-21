using BillChopBE.DataAccessLayer.Models;

namespace BillChopBE.DataAccessLayer.Filters.Factories
{
    public interface IPaymentDbFilterFactory
    {
        IDbFilter<Payment> Create(PaymentFilterInfo loanFilterInfo);
    }

    public class PaymentDbFilterFactory : IPaymentDbFilterFactory
    {
        public IDbFilter<Payment> Create(PaymentFilterInfo loanFilterInfo)
        {
            return new PaymentDbFilter(loanFilterInfo);
        }
    }
}
