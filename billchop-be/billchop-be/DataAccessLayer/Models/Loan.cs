using BillChopBE.DataAccessLayer.Models.Interfaces;
using BillChopBE.DataAccessLayer.Models.Validation;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace BillChopBE.DataAccessLayer.Models
{
  public class Loan : ValidatableModel, IDbModel
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [JsonIgnore]
        public Guid LoaneeId { get; set; }

        public virtual User Loanee { get; set; } = null!;

        [Required]
        public Guid BillId { get; set; }

        [JsonIgnore]
        public virtual Bill Bill { get; set; } = null!;

        public User Loaner
        {
            get => Bill.Loaner;
        }
    }
}
