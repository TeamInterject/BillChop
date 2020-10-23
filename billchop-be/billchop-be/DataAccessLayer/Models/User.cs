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

        [JsonIgnore]
        public virtual IEnumerable<Group> Groups { get; set; } = new List<Group>();

        public virtual IEnumerable<Loan> Loans { get; set; } = new List<Loan>();

        public virtual IEnumerable<Bill> Bills { get; set; } = new List<Bill>();
    }
}
