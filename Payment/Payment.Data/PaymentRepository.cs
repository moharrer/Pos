using System;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Payment.Data
{
    public interface IPaymentRepository : IRepository<Domain.Payment>
    {
        Domain.Payment Add(Domain.Payment payment);

        void Update(Domain.Payment payment);

        Task<Domain.Payment> GetAsync(int paymentId);
    }

    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentContext context;

        public IUnitOfWork UnitOfWrok { get { return context; } }

        public PaymentRepository(PaymentContext dbContext)
        {
            this.context = dbContext;
        }


        public Domain.Payment Add(Domain.Payment payment)
        {
            return context.Payments.Add(payment).Entity;
        }

        public Task<Domain.Payment> GetAsync(int paymentId)
        {
            var result = context.Payments.SingleOrDefaultAsync(a => a.Key == paymentId);
            return result;
        }

        public Task<Domain.Payment> GetAsync(Guid paymentId)
        {
            var result = context.Payments.SingleOrDefaultAsync(a => a.Id == paymentId);
            return result;
        }

        public void Update(Domain.Payment payment)
        {
            context.Entry(payment).State = EntityState.Modified;
        }
    }
}
