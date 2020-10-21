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
    [Route("api/[controller]")]
    public class BillsController : ControllerBase
    {
        private readonly IBillService billService;

        public BillsController(IBillService billService)
        {
            this.billService = billService;
        }

        [HttpGet("group/${groupId}")]
        public async Task<ActionResult<IList<Bill>>> GetBillsByGroupId(Guid groupId)
        {
            return Ok(await billService.GetGroupBillsAsync(groupId));
        }

        public async Task<ActionResult<Bill>> CreateBill(CreateNewBill newBill) 
        {
            return Ok(await billService.CreateAndSplitBillAsync(newBill));
        }
    }
}
