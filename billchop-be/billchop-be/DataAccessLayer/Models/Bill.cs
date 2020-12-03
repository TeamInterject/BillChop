using BillChopBE.DataAccessLayer.Models.Interfaces;
using BillChopBE.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BillChopBE.DataAccessLayer.Models
{
    public class Bill : ValidatableModel, IDbModel
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Total { get; set; }

        [Required]
        public DateTime CreationTime { get; set; } = DateTime.Now;

        [Required]
        public Guid LoanerId { get; set; }

        [Required]
        public virtual User Loaner { get; set; } = null!;

        public virtual List<Loan> Loans { get; set; } = new List<Loan>();

        [Required]
        public Guid GroupContextId { get; set; }

        public virtual Group GroupContext { get; set; } = null!;
    }
}
