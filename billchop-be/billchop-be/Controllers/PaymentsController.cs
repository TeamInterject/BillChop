using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BillChopBE.DataAccessLayer.Models;
using BillChopBE.Services;
using BillChopBE.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillChopBE.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        [HttpGet("/user/{userId}")]
        public async Task<ActionResult<IList<Payment>>> GetExpectedPayments(Guid userId, Guid? groupId)
        {
            return Ok(await paymentService.GetExpectedPaymentsForUserAsync(userId, groupId));
        }

        [HttpPost]
        public async Task<ActionResult<Payment>> CreatePayment([FromBody] CreateNewPayment newPaymentData)
        {
            return Ok(await paymentService.AddPaymentAsync(newPaymentData));
        }
    }
}
