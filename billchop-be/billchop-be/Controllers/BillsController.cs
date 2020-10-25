using BillChopBE.DataAccessLayer.Models;
using BillChopBE.Services;
using BillChopBE.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BillChopBE.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/bills")]
    public class BillsController : ControllerBase
    {
        private readonly IBillService billService;

        public BillsController(IBillService billService)
        {
            this.billService = billService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<Bill>>> GetBills(Guid? groupId)
        {
            return Ok(await billService.GetBillsAsync(groupId));
        }

        [HttpPost]
        public async Task<ActionResult<Bill>> CreateBill([FromBody] CreateNewBill newBill) 
        {
            return Ok(await billService.CreateAndSplitBillAsync(newBill));
        }
    }
}
