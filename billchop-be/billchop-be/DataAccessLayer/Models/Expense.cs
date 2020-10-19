using BillChopBE.DataAccessLayer.Models.Interfaces;
using BillChopBE.DataAccessLayer.Models.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillChopBE.DataAccessLayer.Models
{
    public class Expense : ValidatableModel, IDbModel
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public virtual User Loanee { get; set; } = null!;

        [Required]
        public virtual Bill Bill { get; set; } = null!;
    }
}
