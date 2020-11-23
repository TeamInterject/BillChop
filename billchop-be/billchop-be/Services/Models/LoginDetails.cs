using BillChopBE.DataAccessLayer.Models.Validation;
using System.ComponentModel.DataAnnotations;

namespace BillChopBE.Services.Models
{
  public class LoginDetails : ValidatableModel
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
