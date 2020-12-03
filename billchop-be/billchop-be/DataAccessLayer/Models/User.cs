using BillChopBE.DataAccessLayer.Models.Interfaces;
using BillChopBE.DataAccessLayer.Models.Validation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BillChopBE.DataAccessLayer.Models
{
    public class User : ValidatableModel, IDbModel
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [RegularExpression(@"^[\w_+-\.]+@([\w-]+\.)+[\w-]{2,}$")]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [JsonIgnore]
        public virtual List<Group> Groups { get; set; } = new List<Group>();

        [JsonIgnore]
        public virtual List<Loan> Loans { get; set; } = new List<Loan>();

        [JsonIgnore]
        public virtual List<Bill> Bills { get; set; } = new List<Bill>();

        [JsonIgnore]
        public virtual List<Payment> PaymentsMade { get; set; } = new List<Payment>();

        [JsonIgnore]
        public virtual List<Payment> PaymentsReceived { get; set; } = new List<Payment>();
    }
}
