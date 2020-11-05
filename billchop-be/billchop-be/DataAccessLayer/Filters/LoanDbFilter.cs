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
        /// Optional DateTime to find laons created after a certain time.
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Optional DateTime to find laons created before a certain time.
        /// </summary>
        public DateTime? EndTime { get; set; }
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

            if (loanFilterInfo.StartTime.HasValue)
                AddStartTimeFilter(loanFilterInfo.StartTime.Value);

            if (loanFilterInfo.EndTime.HasValue)
                AddEndTimeFilter(loanFilterInfo.EndTime.Value);
        }

        public void AddGroupFilter(Guid groupId)
        {
            Filters.Add((loan) => loan.Bill.GroupContextId == groupId);
        }

        public void AddLoaneeFilter(Guid loaneeId) 
        {
            Filters.Add((loan) => loan.LoaneeId == loaneeId);
        }

        public void AddLoanerFilter(Guid loanerId) 
        {
            Filters.Add((loan) => loan.Bill.LoanerId == loanerId);
        }

        public void AddStartTimeFilter(DateTime startTime)
        {
            Filters.Add((loan) => loan.Bill.CreationTime > startTime);
        }

        public void AddEndTimeFilter(DateTime endTime)
        {
            Filters.Add((loan) => loan.Bill.CreationTime < endTime);
        }
    }
}
