using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BillChopBE.Validation;

namespace BillChopBE.Controllers.Models
{
    public class ApiBill : ValidatableModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Total { get; set; }

        [Required]
        public DateTime CreationTime { get; set; }

        [Required]
        public virtual ApiUser Loaner { get; set; } = null!;

        public virtual List<ApiLoan> Loans { get; set; } = new List<ApiLoan>();

        [Required]
        public Guid GroupContextId { get; set; }
    }
}