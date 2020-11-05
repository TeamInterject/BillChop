using BillChopBE.DataAccessLayer.Models;
using System;

namespace BillChopBE.DataAccessLayer.Filters
{
    public class BillFilterInfo 
    {
        /// <summary>
        /// Optional DateTime to find laons created after a certain time.
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Optional DateTime to find laons created before a certain time.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Optional Id of user who lent money.
        /// </summary>
        public Guid? LoanerId { get; set; }

        /// <summary>
        /// Optional Id of context group.
        /// </summary>
        public Guid? GroupId { get; set; }
    }

    public class BillDbFilter : AbstractDbFilter<Bill>
    {
        public BillDbFilter(BillFilterInfo billFilterInfo) 
        {
            if (billFilterInfo.StartTime.HasValue)
                AddStartTimeFilter(billFilterInfo.StartTime.Value);

            if (billFilterInfo.EndTime.HasValue)
                AddEndTimeFilter(billFilterInfo.EndTime.Value);

            if (billFilterInfo.LoanerId.HasValue)
                AddLoanerFilter(billFilterInfo.LoanerId.Value);

            if (billFilterInfo.GroupId.HasValue)
                AddGroupFilter(billFilterInfo.GroupId.Value);
        }

        public void AddStartTimeFilter(DateTime startTime)
        {
            Filters.Add((bill) => bill.CreationTime > startTime);
        }

        public void AddEndTimeFilter(DateTime endTime)
        {
            Filters.Add((bill) => bill.CreationTime < endTime);
        }

        public void AddLoanerFilter(Guid loanerId)
        {
            Filters.Add((bill) => bill.LoanerId == loanerId);
        }

        public void AddGroupFilter(Guid groupId) 
        {
            Filters.Add((bill) => bill.GroupContextId == groupId);
        }

        
    }
}
