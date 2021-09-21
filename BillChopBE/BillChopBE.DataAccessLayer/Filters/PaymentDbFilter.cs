using BillChopBE.DataAccessLayer.Models;
using System;

namespace BillChopBE.DataAccessLayer.Filters
{
    public class PaymentFilterInfo 
    {
        /// <summary>
        /// Optional Id of user who payed money back.
        /// </summary>
        public Guid? PayerId { get; set; }

        /// <summary>
        /// Optional Id of user who received money back.
        /// </summary>
        public Guid? ReceiverId { get; set; }

        /// <summary>
        /// Optional Id of context group.
        /// </summary>
        public Guid? GroupId { get; set; }

        /// <summary>
        /// Optional DateTime to find payments created after a certain time.
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Optional DateTime to find payments created before a certain time.
        /// </summary>
        public DateTime? EndTime { get; set; }
    }

    public class PaymentDbFilter : AbstractDbFilter<Payment>
    {
        public PaymentDbFilter(PaymentFilterInfo paymentFilterInfo) 
        {
            if (paymentFilterInfo.GroupId.HasValue)
                AddGroupFilter(paymentFilterInfo.GroupId.Value);

            if (paymentFilterInfo.PayerId.HasValue)
                AddPayerFilter(paymentFilterInfo.PayerId.Value);

            if (paymentFilterInfo.ReceiverId.HasValue)
                AddReceiverFilter(paymentFilterInfo.ReceiverId.Value);

            if (paymentFilterInfo.StartTime.HasValue)
                AddStartTimeFilter(paymentFilterInfo.StartTime.Value);

            if (paymentFilterInfo.EndTime.HasValue)
                AddEndTimeFilter(paymentFilterInfo.EndTime.Value);
        }

        public void AddGroupFilter(Guid groupId)
        {
            Filters.Add((payment) => payment.GroupContextId == groupId);
        }

        public void AddPayerFilter(Guid payerId) 
        {
            Filters.Add((payment) => payment.PayerId == payerId);
        }

        public void AddReceiverFilter(Guid receiverId) 
        {
            Filters.Add((payment) => payment.ReceiverId == receiverId);
        }

        public void AddStartTimeFilter(DateTime startTime)
        {
            Filters.Add((payment) => payment.CreationTime >= startTime);
        }

        public void AddEndTimeFilter(DateTime endTime)
        {
            Filters.Add((payment) => payment.CreationTime <= endTime);
        }
    }
}
