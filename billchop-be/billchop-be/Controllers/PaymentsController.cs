using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BillChopBE.Controllers.Models;
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
        private readonly IMapper mapper;

        public PaymentsController(IPaymentService paymentService, IMapper mapper)
        {
            this.mapper = mapper;
            this.paymentService = paymentService;
        }

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
