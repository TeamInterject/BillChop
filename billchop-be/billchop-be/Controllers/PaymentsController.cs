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
    [Route("api/groups")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        [HttpPost]
        public async Task<ActionResult<Group>> CreateGroup([FromBody] CreateNewPayment newPaymentData)
        {
            return Ok(await paymentService.AddPaymentAsync(newPaymentData));
        }
    }
}
