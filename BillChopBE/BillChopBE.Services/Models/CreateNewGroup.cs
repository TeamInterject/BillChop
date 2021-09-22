using BillChopBE.DataAccessLayer.Models;
using BillChopBE.Validation;
using System.ComponentModel.DataAnnotations;

namespace BillChopBE.Services.Models
{
    public class CreateNewGroup : ValidatableModel
    {
        [Required]
        public string Name { get; set; } = null!;

        public Group ToGroup()
        {
            return new Group() { Name = Name };
        }
    }
}
