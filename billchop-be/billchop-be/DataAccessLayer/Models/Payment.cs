using System;
using System.ComponentModel.DataAnnotations;
using BillChopBE.DataAccessLayer.Models.Interfaces;
using BillChopBE.DataAccessLayer.Models.Validation;
using Newtonsoft.Json;

namespace BillChopBE.DataAccessLayer.Models
{
    public class Payment : ValidatableModel, IDbModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [JsonIgnore]
        public Guid PayerId { get; set; }

        [Required]
        public virtual User Payer { get; set; } = null!;

        [Required]
        [JsonIgnore]
        public Guid ReceiverId { get; set; }

        [Required]
        public virtual User Receiver { get; set; } = null!;

        [Required]
        public Guid GroupContextId { get; set; }

        [JsonIgnore]
        public virtual Group GroupContext { get; set; } = null!;
    }
}