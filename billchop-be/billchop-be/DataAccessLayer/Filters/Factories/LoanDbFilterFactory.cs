using BillChopBE.DataAccessLayer.Models;

namespace BillChopBE.DataAccessLayer.Filters.Factories
{
    public interface ILoanDbFilterFactory
    {
        IDbFilter<Loan> Create(LoanFilterInfo loanFilterInfo);
    }

    public class LoanDbFilterFactory : ILoanDbFilterFactory
    {
        public IDbFilter<Loan> Create(LoanFilterInfo loanFilterInfo)
        {
            return new LoanDbFilter(loanFilterInfo);
        }
    }
}
