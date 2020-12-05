using BillChopBE.DataAccessLayer.Models;
using BillChopBE.Validation;
using System.ComponentModel.DataAnnotations;

namespace BillChopBE.Services.Models
{
    public class CreateNewUser : ValidatableModel
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [RegularExpression(@"^[\w_+-\.]+@([\w-]+\.)+[\w-]{2,}$")]
        public string Email { get; set; } = null!;

        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$")]
        public string Password { get; set; } = null!;

        public User ToUser()
        {
            return new User() { Name = Name, Email = Email, Password = Password };
        }
    }
}
