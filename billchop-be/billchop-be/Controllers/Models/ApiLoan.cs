using System;
using System.ComponentModel.DataAnnotations;
using BillChopBE.Validation;

namespace BillChopBE.Controllers.Models
{
    public class ApiLoan : ValidatableModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public virtual ApiUser Loanee { get; set; } = null!;
        
        [Required]
        public ApiUser Loaner { get; set; } = null!;

        [Required]
        public Guid BillId { get; set; }
    }
}