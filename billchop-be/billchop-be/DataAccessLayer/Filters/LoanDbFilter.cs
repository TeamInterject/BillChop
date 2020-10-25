using BillChopBE.DataAccessLayer.Models;
using System;

namespace BillChopBE.DataAccessLayer.Filters
{
    public class LoanFilterInfo 
    {
        /// <summary>
        /// Optional Id of user who borrowed money.
        /// </summary>
        public Guid? LoaneeId { get; set; }

        /// <summary>
        /// Optional Id of user who lent money.
        /// </summary>
        public Guid? LoanerId { get; set; }

        /// <summary>
        /// Optional Id of context group.
        /// </summary>
        public Guid? GroupId { get; set; }

        /// <summary>
        /// Optional id of bill loans were created from.
        /// </summary>
        public Guid? BillId { get; set; }
    }

    public class LoanDbFilter : AbstractDbFilter<Loan>
    {
        public LoanDbFilter(LoanFilterInfo loanFilterInfo) 
        {
            if (loanFilterInfo.GroupId.HasValue)
                AddGroupFilter(loanFilterInfo.GroupId.Value);

            if (loanFilterInfo.LoaneeId.HasValue)
                AddLoaneeFilter(loanFilterInfo.LoaneeId.Value);

            if (loanFilterInfo.LoanerId.HasValue)
                AddLoanerFilter(loanFilterInfo.LoanerId.Value);

            if (loanFilterInfo.BillId.HasValue)
                AddBillFilter(loanFilterInfo.BillId.Value);
        }

        public void AddGroupFilter(Guid groupId)
        {
            Filters.Add((loan) => loan.Bill.GroupContextId == groupId);
        }

        public void AddBillFilter(Guid billId)
        {
            Filters.Add((loan) => loan.BillId == billId);
        }

        public void AddLoaneeFilter(Guid loaneeId) 
        {
            Filters.Add((loan) => loan.LoaneeId == loaneeId);
        }

        public void AddLoanerFilter(Guid loanerId) 
        {
            Filters.Add((loan) => loan.Bill.LoanerId == loanerId);
        }
    }
}
