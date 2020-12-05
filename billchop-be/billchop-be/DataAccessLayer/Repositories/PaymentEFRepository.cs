using BillChopBE.DataAccessLayer.Models;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BillChopBE.DataAccessLayer.Repositories
{
    public class PaymentEFRepository : AbstractEFRepository<Payment>, IPaymentRepository
    {
        private readonly BillChopContext context;
        protected override DbContext DbContext => context;
        protected override DbSet<Payment> DbSet => context.Payments;

        public PaymentEFRepository(BillChopContext context)
        {
            this.context = context;
        }
    }
}