using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BillChopBE.Controllers.Models;
using BillChopBE.DataAccessLayer.Filters;
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
        private readonly IMapper mapper;

        public PaymentsController(IPaymentService paymentService, IMapper mapper)
        {
            this.mapper = mapper;
            this.paymentService = paymentService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<ApiPayment>>> GetAllPayments([FromQuery] PaymentFilterInfo filterInfo) 
        {
            var payments = await paymentService.GetFilteredPaymentsAsync(filterInfo);
            return mapper.Map<List<ApiPayment>>(payments);
        }
        
        /// <summary>
        /// Generates payments to be received and paid by given user for each group.
        /// </summary>
        /// <param name="userId">User to generate expected payments for.</param>
        /// <param name="groupId">Optional group context for returning payments in a single group.</param>
        /// <returns>Expected payments to be done and received by this user.</returns>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IList<ApiPayment>>> GetExpectedPayments(Guid userId, Guid? groupId)
        {
            var payments = await paymentService.GetExpectedPaymentsForUserAsync(userId, groupId);
            return mapper.Map<List<ApiPayment>>(payments);
        }

        [HttpPost]
        public async Task<ActionResult<ApiPayment>> CreatePayment([FromBody] CreateNewPayment newPaymentData)
        {
            var payment = await paymentService.AddPaymentAsync(newPaymentData);
            return mapper.Map<ApiPayment>(payment);
        }
    }
}
