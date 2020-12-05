using AutoMapper;
using BillChopBE.Controllers.Models;
using BillChopBE.Services;
using BillChopBE.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BillChopBE.Controllers
{
    [Authorize]    
    [ApiController]
    [Produces("application/json")]
    [Route("api/bills")]
    public class BillsController : ControllerBase
    {
        private readonly IBillService billService;
        private readonly IMapper mapper;

        public BillsController(IBillService billService, IMapper mapper)
        {
            this.mapper = mapper;
            this.billService = billService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<ApiBill>>> GetBills(Guid? groupId, DateTime? startTime, DateTime? endTime)
        {
            var bills = await billService.GetBillsAsync(groupId, startTime, endTime);
            return mapper.Map<List<ApiBill>>(bills);
        }

        [HttpPost]
        public async Task<ActionResult<ApiBill>> CreateBill([FromBody] CreateNewBill newBillData) 
        {
            var bill = await billService.CreateAndSplitBillAsync(newBillData);
            return mapper.Map<ApiBill>(bill);
        }
    }
}
