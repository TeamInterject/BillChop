using BillChopBE.DataAccessLayer.Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BillChopBE.Services.Models
{
    public class LoginDetails : ValidatableModel
    {
        [Required]
        [RegularExpression(@"^[\w_+-\.]+@([\w-]+\.)+[\w-]{2,}$")]
        public string Email { get; set; } = null!;
    }
}
