using AutoMapper;
using BillChopBE.Controllers.Models;
using BillChopBE.DataAccessLayer.Filters;
using BillChopBE.Services;
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
    [Route("api/loans")]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService loanService;
        private readonly IMapper mapper;

        public LoansController(ILoanService loanService, IMapper mapper) 
        {
            this.mapper = mapper;
            this.loanService = loanService;
        }

        /// <summary>
        /// Get all loans that need to be paid back to specific user. AKA the loans the user provided.
        /// </summary>
        /// <param name="loanerId">Id of user who lent money.</param>
        /// <param name="groupId">Optional Id of context group.</param>
        /// <param name="startTime">Optional DateTime for filtering loans created after a certain time</param>
        /// <param name="endTime">Optional DateTime for filtering loans created before a certain time</param>
        /// <returns></returns>
        [HttpGet("provided-loans/{loanerId}")]
        public async Task<ActionResult<IList<ApiLoan>>> GetProvidedLoans(Guid loanerId, Guid? groupId, DateTime? startTime, DateTime? endTime)
        {
            var loans = await loanService.GetProvidedLoansAsync(loanerId, groupId, startTime, endTime);
            return mapper.Map<List<ApiLoan>>(loans);
        }

        /// <summary>
        /// Get all loans of a specific user that need to be paid back.
        /// If group ID is specified, get all loans of specific user in a specific group.
        /// </summary>
        /// <param name="loaneeId">Id of user who borrowed money.</param>
        /// <param name="groupId">Optional Id of context group.</param>
        /// <param name="startTime">Optional DateTime for filtering loans created after a certain time</param>
        /// <param name="endTime">Optional DateTime for filtering loans created before a certain time</param>
        /// <returns></returns>
        [HttpGet("received-loans/{loaneeId}")]
        public async Task<ActionResult<IList<ApiLoan>>> GetReceivedLoans(Guid loaneeId, Guid? groupId, DateTime? startTime, DateTime? endTime)
        {
            var loans = await loanService.GetReceivedLoansAsync(loaneeId, groupId, startTime, endTime);
            return mapper.Map<List<ApiLoan>>(loans);
        }

        /// <summary>
        /// Gets all loans user owns to himself, or in other words their own expenses.
        /// </summary>
        /// <param name="loanerAndLoaneeId">Id of user who paid for himself, making it an expense.</param>
        /// <param name="groupId">Optional Id of context group.</param>
        /// <param name="startTime">Optional DateTime for filtering loans created after a certain time</param>
        /// <param name="endTime">Optional DateTime for filtering loans created before a certain time</param>
        /// <returns></returns>
        [HttpGet("self-loans/{loanerAndLoaneeId}")]
        public async Task<ActionResult<IList<ApiLoan>>> GetSelfLoans(Guid loanerAndLoaneeId, Guid? groupId, DateTime? startTime, DateTime? endTime)
        {
            var loans = await loanService.GetSelfLoansAsync(loanerAndLoaneeId, groupId, startTime, endTime);
            return mapper.Map<List<ApiLoan>>(loans);
        }

        /// <summary>
        /// Gets all loans that can optionally be filterd.
        /// </summary>
        /// <param name="loanFilterInfo"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IList<ApiLoan>>> GetLoans([FromQuery] LoanFilterInfo loanFilterInfo) 
        {
            var loans = await loanService.GetFilteredLoansAsync(loanFilterInfo);
            return mapper.Map<List<ApiLoan>>(loans);
        }
    }
}
