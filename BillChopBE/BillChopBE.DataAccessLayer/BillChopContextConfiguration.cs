using BillChopBE.Validation;
using System.ComponentModel.DataAnnotations;

namespace BillChopBE.DataAccessLayer
{
    public class BillChopContextConfiguration : ValidatableModel
    {
        [Required]
        public string BillChopDb { get; set; } = null!;
    }
}
