using BillChopBE.DataAccessLayer.Models.Interfaces;
using BillChopBE.DataAccessLayer.Models.Validation;
using Newtonsoft.Json;
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
        [JsonIgnore]
        public virtual Bill Bill { get; set; } = null!;
    }
}
