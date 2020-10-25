using BillChopBE.DataAccessLayer.Models.Interfaces;
using BillChopBE.DataAccessLayer.Models.Validation;
using Newtonsoft.Json;
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
        public Guid LoanerId { get; set; }

        [Required]
        public virtual User Loaner { get; set; } = null!;

        public virtual IEnumerable<Loan> Loans { get; set; } = new List<Loan>();

        [Required]
        public Guid GroupContextId { get; set; }

        [Required]
        public virtual Guid GroupContextId { get; set; }

        [JsonIgnore]
        public virtual Group GroupContext { get; set; } = null!;
    }
}
