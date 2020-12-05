using System;
using System.ComponentModel.DataAnnotations;
using BillChopBE.DataAccessLayer.Models;
using BillChopBE.Validation;

namespace BillChopBE.Services.Models
{
    public class CreateNewPayment : ValidatableModel
    {
        [Required]
        public decimal Amount { get; set; }
        
        [Required]
        public Guid PayerId { get; set; }
        
        [Required]
        public Guid ReceiverId { get; set; }
        
        [Required]
        public Guid GroupContextId { get; set; }

        public Payment ToPayment () 
        {
            return new Payment()
                {
                    Amount = Amount,
                    PayerId = PayerId,
                    ReceiverId = ReceiverId,
                    GroupContextId = GroupContextId,
                };
        }
    }
}