using BillChopBE.DataAccessLayer.Models;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using BillChopBE.Services.Models;
using System;
using System.Threading.Tasks;

namespace BillChopBE.Services
{
    public interface IPaymentService
    {
        Task<Payment> AddPaymentAsync(CreateNewPayment newPaymentData);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        public Task<Payment> AddPaymentAsync(CreateNewPayment newPaymentData) 
        {
            newPaymentData.Validate();
            var paymentToAdd = newPaymentData.ToPayment();

            return paymentRepository.AddAsync(paymentToAdd);
        }
    }
}
