using System;
using System.ComponentModel.DataAnnotations;
using BillChopBE.Validation;

namespace BillChopBE.Controllers.Models
{
    public class ApiUser : ValidatableModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [RegularExpression(@"^[\w_+-\.]+@([\w-]+\.)+[\w-]{2,}$")]
        public string Email { get; set; } = null!;
    }

    public class ApiUserWithToken : ValidatableModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [RegularExpression(@"^[\w_+-\.]+@([\w-]+\.)+[\w-]{2,}$")]
        public string Email { get; set; } = null!;

        [Required]
        public string Token { get; set; } = null!;
    }
}