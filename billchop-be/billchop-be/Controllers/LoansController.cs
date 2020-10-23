using BillChopBE.DataAccessLayer.Models;
using BillChopBE.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BillChopBE.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService loanService;

        public LoansController(ILoanService loanService) 
        {
            this.loanService = loanService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<Loan>>> GetBillLoans(Guid billId) 
        {
            return Ok(await loanService.GetBillLoans(billId));
        }

        public async Task<ActionResult<IList<Loan>>> GetLentUserLoansInGroup(Guid loanerId, Guid groupId) 
        {
            return Ok(await loanService.GetLentUserLoansInGroup(loanerId, groupId));
        }

        public async Task<ActionResult<IList<Loan>>> GetTakenUserLoans(Guid userId) 
        {
            return Ok(await loanService.GetTakenUserLoans(userId));
        }

        public async Task<ActionResult<IList<Loan>>> GetTakenUserLoansInGroup(Guid loaneeId, Guid groupId) 
        {
            return Ok(await loanService.GetTakenUserLoansInGroup(loaneeId, groupId));
        }
    }
}
