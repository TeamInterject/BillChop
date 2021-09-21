using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace BillChopBE.Middleware
{
    public class TransactionMiddleware
    {
        private readonly RequestDelegate next;
        public TransactionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, DbTransaction transaction)
        {
            try
            {
                var connection = transaction.Connection;
                if (connection == null || connection.State != ConnectionState.Open)
                    throw new Exceptions.DbException("It seems our servers are down right now");

                await next(context);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
