using BillChopBE.DataAccessLayer.Models.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace BillChopBE.DataAccessLayer.Models
{
    public class Expense : ValidatableModel
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public virtual User Loanee { get; set; } = null!;
    }
}
