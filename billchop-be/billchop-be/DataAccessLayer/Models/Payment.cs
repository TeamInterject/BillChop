using System;
using System.ComponentModel.DataAnnotations;
using BillChopBE.DataAccessLayer.Models.Interfaces;
using BillChopBE.Validation;

namespace BillChopBE.DataAccessLayer.Models
{
    public class Payment : ValidatableModel, IDbModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime CreationTime { get; set; } = DateTime.Now;

        [Required]
        public Guid PayerId { get; set; }

        [Required]
        public virtual User Payer { get; set; } = null!;

        [Required]
        public Guid ReceiverId { get; set; }

        [Required]
        public virtual User Receiver { get; set; } = null!;

        [Required]
        public Guid GroupContextId { get; set; }

        public virtual Group GroupContext { get; set; } = null!;
    }
}