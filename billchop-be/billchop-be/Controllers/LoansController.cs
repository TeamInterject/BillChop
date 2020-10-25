﻿using BillChopBE.DataAccessLayer.Filters;
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
    [Route("api/loans")]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService loanService;

        public LoansController(ILoanService loanService) 
        {
            this.loanService = loanService;
        }

        /// <summary>
        /// Get all loans that need to be paid back to specific user. AKA the loans the user provided.
        /// </summary>
        /// <param name="loanerId">Id of user who lent money.</param>
        /// <param name="groupId">Optional Id of context group.</param>
        /// <returns></returns>
        [HttpGet("provided-loans/{loanerId}")]
        public async Task<ActionResult<IList<Loan>>> GetProvidedLoans(Guid loanerId, Guid? groupId)
        {
            return Ok(await loanService.GetProvidedLoansAsync(loanerId, groupId));
        }

        /// <summary>
        /// Get all loans of a specific user that need to be paid back.
        /// If group ID is specified, get all loans of specific user in a specific group.
        /// </summary>
        /// <param name="loaneeId">Id of user who borrowed money.</param>
        /// <param name="groupId">Optional Id of context group.</param>
        /// <returns></returns>
        [HttpGet("received-loans/{loaneeId}")]
        public async Task<ActionResult<IList<Loan>>> GetReceivedLoans(Guid loaneeId, Guid? groupId)
        {
            return Ok(await loanService.GetReceivedLoansAsync(loaneeId, groupId));
        }

        /// <summary>
        /// Gets all loans user owns to himself, or in other words their own expenses.
        /// </summary>
        /// <param name="loanerAndLoaneeId">Id of user who paid for himself, making it an expense.</param>
        /// <param name="groupId">Optional Id of context group.</param>
        /// <returns></returns>
        [HttpGet("self-loans/{loanerAndLoaneeId}")]
        public async Task<ActionResult<IList<Loan>>> GetSelfLoans(Guid loanerAndLoaneeId, Guid? groupId)
        {
            return Ok(await loanService.GetSelfLoansAsync(loanerAndLoaneeId, groupId));
        }

        /// <summary>
        /// Gets all loans that can optionally be filterd.
        /// </summary>
        /// <param name="loanFilterInfo"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IList<Loan>>> GetLoans([FromQuery] LoanFilterInfo loanFilterInfo) 
        {
            return Ok(await loanService.GetFilteredLoansAsync(loanFilterInfo));
        }
    }
}